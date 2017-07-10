#ifndef INCLUDED_NETWORKSERVICES_H
#define INCLUDED_NETWORKSERVICES_H

#pragma warning (push)
#pragma warning (disable : 4005)
#include <winsock2.h>
#include <windows.h>
#include <ws2tcpip.h>
#include <iostream>
#include <map>
#include <string>
#pragma warning (pop)

#define DEFAULT_BUFLEN 512
#define MAX_PACKET_SIZE 1000000

class NetworkServices
{
    public:
        static int sendMessage(SOCKET curSocket, char * message, int messageSize)
        {
            return send(curSocket, message, messageSize, 0);
        }
        static int receiveMessage(SOCKET curSocket, char * buffer, int bufSize)
        {
            return recv(curSocket, buffer, bufSize, 0);
        }
};

class ClientNetwork
{
    public:
        int iResult;
        SOCKET ConnectSocket;
        int receivePackets(char * recvbuf)
        {
            iResult = NetworkServices::receiveMessage(ConnectSocket, recvbuf, MAX_PACKET_SIZE);
            if ( iResult == 0 )
            {
                printf("Connection closed\n");
                closesocket(ConnectSocket);
                WSACleanup();
                exit(1);
            }
            return iResult;
        }
        ClientNetwork(std::string ip,std::string port)
        {
            // create WSADATA object
            WSADATA wsaData;
            // socket
            ConnectSocket = INVALID_SOCKET;
            // holds address info for socket to connect to
            struct addrinfo *result = NULL,
                            *ptr = NULL,
                            hints;
            // Initialize Winsock
            iResult = WSAStartup(MAKEWORD(2,2), &wsaData);
            if (iResult != 0)
            {
                std::cout<<"WSAStartup failed with error: "<<iResult<<std::endl;
                exit(1);
            }
            // set address info
            ZeroMemory( &hints, sizeof(hints) );
            hints.ai_family = AF_UNSPEC;
            hints.ai_socktype = SOCK_STREAM;
            hints.ai_protocol = IPPROTO_TCP;  //TCP connection!!!

            //resolve server address and port
            iResult = getaddrinfo(ip.c_str(), port.c_str(), &hints, &result);

            if( iResult != 0 )
            {
                printf("getaddrinfo failed with error: %d\n", iResult);
                WSACleanup();
                exit(1);
            }

            // Attempt to connect to an address until one succeeds
            for(ptr=result; ptr != NULL ;ptr=ptr->ai_next) {

                // Create a SOCKET for connecting to server
                ConnectSocket = socket(ptr->ai_family, ptr->ai_socktype,
                    ptr->ai_protocol);

                if (ConnectSocket == INVALID_SOCKET) {
                    printf("socket failed with error: %ld\n", WSAGetLastError());
                    WSACleanup();
                    exit(1);
                }

                // Connect to server.
                iResult = connect( ConnectSocket, ptr->ai_addr, (int)ptr->ai_addrlen);

                if (iResult == SOCKET_ERROR)
                {
                    closesocket(ConnectSocket);
                    ConnectSocket = INVALID_SOCKET;
                    printf ("The server is down... did not connect");
                }
            }

            // no longer need address info for server
            freeaddrinfo(result);

            // if connection failed
            if (ConnectSocket == INVALID_SOCKET)
            {
                printf("Unable to connect to server!\n");
                WSACleanup();
                exit(1);
            }
            // Set the mode of the socket to be nonblocking
            u_long iMode = 1;

            iResult = ioctlsocket(ConnectSocket, FIONBIO, &iMode);
            if (iResult == SOCKET_ERROR)
            {
                printf("ioctlsocket failed with error: %d\n", WSAGetLastError());
                closesocket(ConnectSocket);
                WSACleanup();
                exit(1);
            }
            char value = 1;
            setsockopt( ConnectSocket, IPPROTO_TCP, TCP_NODELAY, &value, sizeof( value ) );
        }
        ~ClientNetwork(void)
        {
        }
};

class ServerNetwork
{
    public:
        ServerNetwork(std::string ip,std::string port)
        {
            // create WSADATA object
            WSADATA wsaData;

            // our sockets for the server
            ListenSocket = INVALID_SOCKET;
            ClientSocket = INVALID_SOCKET;



            // address info for the server to listen to
            struct addrinfo *result = NULL;
            struct addrinfo hints;

            // Initialize Winsock
            iResult = WSAStartup(MAKEWORD(2,2), &wsaData);
            if (iResult != 0) {
                printf("WSAStartup failed with error: %d\n", iResult);
                exit(1);
            }

            // set address information
            ZeroMemory(&hints, sizeof(hints));
            hints.ai_family = AF_INET;
            hints.ai_socktype = SOCK_STREAM;
            hints.ai_protocol = IPPROTO_TCP;    // TCP connection!!!
            hints.ai_flags = AI_PASSIVE;
            // Resolve the server address and port
            if(ip.size()==0)
                iResult = getaddrinfo(NULL, port.c_str(), &hints, &result);
            else
                iResult = getaddrinfo(ip.c_str(), port.c_str(), &hints, &result);

            if ( iResult != 0 ) {
                printf("getaddrinfo failed with error: %d\n", iResult);
                WSACleanup();
                exit(1);
            }

            // Create a SOCKET for connecting to server
            ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);

            if (ListenSocket == INVALID_SOCKET) {
                printf("socket failed with error: %ld\n", WSAGetLastError());
                freeaddrinfo(result);
                WSACleanup();
                exit(1);
            }

            // Set the mode of the socket to be nonblocking
            u_long iMode = 1;
            iResult = ioctlsocket(ListenSocket, FIONBIO, &iMode);

            if (iResult == SOCKET_ERROR) {
                printf("ioctlsocket failed with error: %d\n", WSAGetLastError());
                closesocket(ListenSocket);
                WSACleanup();
                exit(1);
            }

            // Setup the TCP listening socket
            iResult = bind( ListenSocket, result->ai_addr, (int)result->ai_addrlen);

            if (iResult == SOCKET_ERROR) {
                printf("bind failed with error: %d\n", WSAGetLastError());
                freeaddrinfo(result);
                closesocket(ListenSocket);
                WSACleanup();
                exit(1);
            }

            // no longer need address information
            freeaddrinfo(result);

            // start listening for new clients attempting to connect
            iResult = listen(ListenSocket, SOMAXCONN);

            if (iResult == SOCKET_ERROR) {
                printf("listen failed with error: %d\n", WSAGetLastError());
                closesocket(ListenSocket);
                WSACleanup();
                exit(1);
            }
        }
        ~ServerNetwork(void)
        {
        }

        // Socket to listen for new connections
        SOCKET ListenSocket;

        // Socket to give to the clients
        SOCKET ClientSocket;

        // for error checking return values
        int iResult;

        // table to keep track of each client's socket
        std::map<unsigned int, SOCKET> sessions;

        //Functions
        bool acceptNewClient(unsigned int & id)
        {
            // if client waiting, accept the connection and save the socket
            ClientSocket = accept(ListenSocket,NULL,NULL);
            if (ClientSocket != INVALID_SOCKET)
            {
                //disable nagle on the client's socket
                char value = 1;
                setsockopt( ClientSocket, IPPROTO_TCP, TCP_NODELAY, &value, sizeof( value ) );
                // insert new client into session id table
                sessions.insert( std::pair<unsigned int, SOCKET>(id, ClientSocket) );
                return true;
            }
            return false;
        }
        // receive incoming data
        int receiveData(unsigned int client_id, char * recvbuf)
        {
            if( sessions.find(client_id) != sessions.end() )
            {
                SOCKET currentSocket = sessions[client_id];
                iResult = NetworkServices::receiveMessage(currentSocket, recvbuf, MAX_PACKET_SIZE);
                if (iResult == 0)                {
                    printf("Connection closed\n");
                    closesocket(currentSocket);
                }
                return iResult;
            }
            return 0;
        }
        // send data to all clients
        void sendToAll(char * packets, int totalSize)
        {
            SOCKET currentSocket;
            std::map<unsigned int, SOCKET>::iterator iter;
            int iSendResult;

            for (iter = sessions.begin(); iter != sessions.end(); iter++)
            {
                currentSocket = iter->second;
                iSendResult = NetworkServices::sendMessage(currentSocket, packets, totalSize);

                if (iSendResult == SOCKET_ERROR)
                {
                    printf("send failed with error: %d\n", WSAGetLastError());
                    closesocket(currentSocket);
                }
            }
        }
};

enum PacketTypes
{
    INIT_CONNECTION = 0,
    HAND_EVENT = 1,
    CHANGE_EVENT = 2,
};

struct Packet
{
    std::string gesture_name;
    void serialize(char * data)
    {
        memcpy(data, this, sizeof(Packet));
    }
    void deserialize(char * data)
    {
        memcpy(this, data, sizeof(Packet));
    }
};

class ServerLeap
{
    private:
        unsigned int client_id;
        char network_data[MAX_PACKET_SIZE];
        ServerNetwork* server_network;
    public:
        void update()
        {
            // get new clients
            if(server_network->acceptNewClient(client_id))
            {
                printf("client %d has been connected to the server\n",client_id);
                ++client_id;
            }
            receiveFromClients();
        }
        void receiveFromClients()
        {
            Packet packet;
            std::map<unsigned int, SOCKET>::iterator iter;
            for(iter = server_network->sessions.begin(); iter != server_network->sessions.end(); iter++)
            {
                int data_length = server_network->receiveData(iter->first, network_data);
                if (data_length <= 0)
                {
                    continue;
                }
                int i = 0;
                while (i < (unsigned int)data_length)
                {
                    packet.deserialize(&(network_data[i]));
                    i += sizeof(Packet);
                }
            }
        }
        void sendActionPackets()
        {
            // send action packet
            std::cin>>actual_gesture;
            const unsigned int packet_size = sizeof(Packet);
            char *cstr = new char[actual_gesture.length() + 1];
            strcpy(cstr, actual_gesture.c_str());
            server_network->sendToAll(cstr,packet_size);
        }
        ServerLeap(std::string ip,std::string port)
        {
            client_id=0;
            server_network=new ServerNetwork(ip,port);
        }
        ~ServerLeap()
        {
            delete(server_network);
        }
};

class ClientLeap
{
    private:
        ClientNetwork* client_network;
        char network_data[MAX_PACKET_SIZE];
        std::stringstream streamer;
    public:
        std::string get_buffer()
        {
            std::string copy_buffer=streamer.str();
            streamer.str("");
            return copy_buffer;
        }
        ClientLeap(std::string ip,std::string port)
        {
            client_network=new ClientNetwork(ip,port);
            // send init packet
            const unsigned int packet_size = sizeof(Packet);
            char packet_data[packet_size];

            Packet packet;

            packet.serialize(packet_data);

            NetworkServices::sendMessage(client_network->ConnectSocket, packet_data, packet_size);
        }
        void sendActionPackets()
        {
            // send action packet
            const unsigned int packet_size = sizeof(Packet);
            char packet_data[packet_size];

            Packet packet;
            NetworkServices::sendMessage(client_network->ConnectSocket, packet_data, packet_size);
        }
        void update()
        {
            Packet packet;
            int data_length = client_network->receivePackets(network_data);
            if (data_length <= 0)
            {
                return;
            }
            int i = 0;
            while (i < (unsigned int)data_length)
            {
                packet.deserialize(&(network_data[i]));
                i += sizeof(Packet);
            }
        }
        ~ClientLeap()
        {
            delete(client_network);
        }
};

long long milliseconds_now() {
    static LARGE_INTEGER s_frequency;
    static BOOL s_use_qpc = QueryPerformanceFrequency(&s_frequency);
    if (s_use_qpc) {
        LARGE_INTEGER now;
        QueryPerformanceCounter(&now);
        return (1000LL * now.QuadPart) / s_frequency.QuadPart;
    } else {
        return GetTickCount();
    }
}

#endif // INCLUDED_NETWORKSERVICES_H
