#include <iostream>
#include <cstring>
#include "Leap.h"
#include <sstream>
#include <process.h>
#include <math.h>
#include "sqlite3.h"
//#include <boost/algorithm/string.hpp>

bool static real_read=false;
Leap::Vector static vectors[2][5][4];
Leap::Vector static ini_pos[2][5];
int finger_ord[10]={0,1,2,3,4,5,6,7,8,9};
std::string actual_gesture;

#include "include/NetworkServices.h"
#include <stdlib.h>

using namespace std;
using namespace Leap;

ServerLeap* server;
ClientLeap* client;

bool exit_leap=true;

long long miliseconds=1000;

static const double MATH_PI=3.14159265358979323846;

//
sqlite3 *db;
int total_gestures = 0;
std::vector<int> distances_finales;
std::vector<std::vector<float>> points_gestures;
std::vector<std::string> name_gestures;
std::vector<std::string> hands;

std::vector<Leap::Vector> ch_pos_left(6);
std::vector<Leap::Vector> ch_pos_right(6);

std::vector<Leap::Vector> pos_left(6);
std::vector<Leap::Vector> pos_right(6);

std::vector<float> distance_left(8);
std::vector<float> distance_right(8);

std::vector<std::string> split(std::string ges)
{
    stringstream ss(ges);
    std::vector<std::string> result;
    while( ss.good() )
    {
        std::string sub;
        getline( ss, sub, ',' );
        result.push_back( sub );
    }
    return result;
}
float calculated_distance(std::vector<float> x, std::vector<float> y)
{
	float a = pow(x[0]-y[0],2);
	float b = pow(x[1]-y[1],2);
	float c = pow(x[2]-y[2],2);
	return sqrt(a+b+c);
}
float calculated_distance_leap(Leap::Vector x, Leap::Vector y)
{
	float a = pow(x[0]-y[0],2);
	float b = pow(x[1]-y[1],2);
	float c = pow(x[2]-y[2],2);
	return sqrt(a+b+c);
}

void push_distances(std::vector<float>& distances, std::vector<std::vector<float>> mano)
{
	//std::cout<<"size: "<<ch_pos_right.size()<<std::endl;
		/*std::cout<<"1: "<<ch_pos_right[0]<<std::endl;
		std::cout<<"2: "<<ch_pos_right[1]<<std::endl;
		std::cout<<"3: "<<ch_pos_right[2]<<std::endl;
		std::cout<<"4: "<<ch_pos_right[3]<<std::endl;
		std::cout<<"5: "<<ch_pos_right[4]<<std::endl;
		std::cout<<"6: "<<ch_pos_right[5]<<std::endl;*/

	std::cout<<"1: "<<calculated_distance(mano[0],mano[1])<<std::endl;
	std::cout<<"2: "<<calculated_distance(mano[0],mano[2])<<std::endl;
	std::cout<<"3: "<<calculated_distance(mano[0],mano[3])<<std::endl;
	std::cout<<"4: "<<calculated_distance(mano[0],mano[4])<<std::endl;
	std::cout<<"5: "<<calculated_distance(mano[0],mano[5])<<std::endl;
	std::cout<<"6: "<<calculated_distance(mano[5],mano[4])<<std::endl;
	std::cout<<"7: "<<calculated_distance(mano[4],mano[3])<<std::endl;
	std::cout<<"8: "<<calculated_distance(mano[3],mano[2])<<std::endl;

	distances.push_back(calculated_distance(mano[0],mano[1]));
	distances.push_back(calculated_distance(mano[0],mano[2]));
	distances.push_back(calculated_distance(mano[0],mano[3]));
	distances.push_back(calculated_distance(mano[0],mano[4]));
	distances.push_back(calculated_distance(mano[0],mano[5]));
	distances.push_back(calculated_distance(mano[5],mano[4]));
	distances.push_back(calculated_distance(mano[4],mano[3]));
	distances.push_back(calculated_distance(mano[3],mano[2]));
}

static int all_gestures(void *data, int argc, char **argv, char **azColName)
{
	std::cout<<argv[0]<<std::endl;
	total_gestures = atoi(argv[0]);
	return 0;
}
void add_gestures(std::string gesture)
{
	std::vector<std::string> strs;
	std::vector<float> distances;
	strs = split(gesture);
	std::vector<std::vector<std::vector<float>>> new_gesture;
	if(strs.size()>19)
		hands.push_back("B");
	else
		hands.push_back(strs[0]);
	for(int i = 0;i<strs.size();++i)
	{
		std::cout<<strs.size()<<std::endl;
		if(strs[i%19]=="D")
		{
			std::vector<std::vector<float>> mano;
			++i;
			int j = 0;
			while(j<18)
			{
				std::vector<float> aux;
				float x =atof((strs[i]).c_str());
				float y =atof((strs[i+1]).c_str());
				float z =atof((strs[i+2]).c_str());
				aux.push_back(x);
				aux.push_back(y);
				aux.push_back(z);
				mano.push_back(aux);
				i+=3;
				j+=3;
				std::cout<<x<<" "<<y<<" "<<z<<std::endl;

			}
			hands.push_back("D");
			push_distances(distances,mano);
		}
		else if(strs[i%19]=="I")
		{
			std::vector<std::vector<float>> mano;
			++i;
			int j = 0;
			while(j<18)
			{
				std::vector<float> aux;
				float x =atof((strs[i]).c_str());
				float y =atof((strs[i+1]).c_str());
				float z =atof((strs[i+2]).c_str());
				aux.push_back(x);
				aux.push_back(y);
				aux.push_back(z);
				mano.push_back(aux);
				j+=3;
				i+=3;
				std::cout<<x<<" "<<y<<" "<<z<<std::endl;
			}
			hands.push_back("B");
			push_distances(distances,mano);
		}
	}
	points_gestures.push_back(distances);
	std::cout<<"TOTAL: "<<points_gestures.size()<<std::endl;
}

static int all_points_gestures(void *data, int argc, char **argv, char **azColName)
{
	//points_gestures.clear();
   	int i;
   	std::cout<<argc<<std::endl;
   	for(i = 0; i<argc; i+=3)
   	{
		std::string name(argv[i+1]);
		std::cout<<name<<std::endl;
		std::string gesture(argv[i+2]);
		std::cout<<"cargando: "<<gesture<<std::endl;
		name_gestures.push_back(name);
		add_gestures(gesture);
   	}
	return 0;
}
void print_d()
{
	for(int i=0;i<distance_right.size();++i)
	{
		std::cout<<distance_right[i]<< " ";
	}
	std::cout<<std::endl;
}

void print_de(std::vector<float> pos)
{
	for(int i=0;i<pos.size();++i)
	{
		std::cout<<pos[i]<< " ";
	}
	std::cout<<std::endl;
}
void print_gestures()
{
	for(int g = 0;g<points_gestures.size();++g)
	{
		std::cout<<name_gestures[g]<<std::endl;
		for (int d = 0; d< points_gestures[g].size(); ++d)
		{
			std::cout<<points_gestures[g][d]<<" ";
		}
		std::cout<<std::endl;
	}
}
float compare_distances(std::vector<float> dis)
{
	float value = 0;
	if(dis.size()<9)
	{
		for(int i=0;i<distance_right.size();++i)
		{
			value += pow(dis[i] - distance_right[i],2);
		}
	}
	else
	{
	    /*
		int i;
		for(i=0;i<distance_right.size();++i)
		{
			value += pow(dis[i] - distance_right[i],2);
		}
		for(int j=0;j<distance_left.size();++j)
		{
			value += pow(dis[i] - distance_left[j],2);
			++i;
		}
		*/
	}
	return sqrt(value);
}
void calculate_actual_distance()
{
	//std::cout<<"size: "<<ch_pos_right.size()<<std::endl;
	if(ch_pos_right.size()>2)
	{
		/*std::cout<<"1: "<<ch_pos_right[0]<<std::endl;
		std::cout<<"2: "<<ch_pos_right[1]<<std::endl;
		std::cout<<"3: "<<ch_pos_right[2]<<std::endl;
		std::cout<<"4: "<<ch_pos_right[3]<<std::endl;
		std::cout<<"5: "<<ch_pos_right[4]<<std::endl;
		std::cout<<"6: "<<ch_pos_right[5]<<std::endl;*/


		//std::cout<<"1: "<<calculated_distance_leap(ch_pos_right[0],ch_pos_right[1])<<std::endl;
		//std::cout<<"2: "<<calculated_distance_leap(ch_pos_right[0],ch_pos_right[2])<<std::endl;
		//std::cout<<"3: "<<calculated_distance_leap(ch_pos_right[0],ch_pos_right[3])<<std::endl;
		//std::cout<<"4: "<<calculated_distance_leap(ch_pos_right[0],ch_pos_right[4])<<std::endl;
		//std::cout<<"5: "<<calculated_distance_leap(ch_pos_right[0],ch_pos_right[5])<<std::endl;
		//std::cout<<"6: "<<calculated_distance_leap(ch_pos_right[5],ch_pos_right[4])<<std::endl;
		//std::cout<<"7: "<<calculated_distance_leap(ch_pos_right[4],ch_pos_right[3])<<std::endl;
		//std::cout<<"8: "<<calculated_distance_leap(ch_pos_right[3],ch_pos_right[2])<<std::endl;

		distance_right.push_back(calculated_distance_leap(ch_pos_right[0],ch_pos_right[1]));
		distance_right.push_back(calculated_distance_leap(ch_pos_right[0],ch_pos_right[2]));
		distance_right.push_back(calculated_distance_leap(ch_pos_right[0],ch_pos_right[3]));
		distance_right.push_back(calculated_distance_leap(ch_pos_right[0],ch_pos_right[4]));
		distance_right.push_back(calculated_distance_leap(ch_pos_right[0],ch_pos_right[5]));
		distance_right.push_back(calculated_distance_leap(ch_pos_right[5],ch_pos_right[4]));
		distance_right.push_back(calculated_distance_leap(ch_pos_right[4],ch_pos_right[3]));
		distance_right.push_back(calculated_distance_leap(ch_pos_right[3],ch_pos_right[2]));
	}
	//std::cout<<"size: "<<ch_pos_left.size()<<std::endl;
	if(ch_pos_left.size()>2)
	{
		//std::cout<<"1: "<<calculated_distance_leap(ch_pos_left[0],ch_pos_left[1])<<std::endl;
		//std::cout<<"2: "<<calculated_distance_leap(ch_pos_left[0],ch_pos_left[2])<<std::endl;
		//std::cout<<"3: "<<calculated_distance_leap(ch_pos_left[0],ch_pos_left[3])<<std::endl;
		//std::cout<<"4: "<<calculated_distance_leap(ch_pos_left[0],ch_pos_left[4])<<std::endl;
		//std::cout<<"5: "<<calculated_distance_leap(ch_pos_left[0],ch_pos_left[5])<<std::endl;
		//std::cout<<"6: "<<calculated_distance_leap(ch_pos_left[5],ch_pos_left[4])<<std::endl;
		//std::cout<<"7: "<<calculated_distance_leap(ch_pos_left[4],ch_pos_left[3])<<std::endl;
		//std::cout<<"8: "<<calculated_distance_leap(ch_pos_left[3],ch_pos_left[2])<<std::endl;

		/*distance_left.push_back(calculated_distance_leap(ch_pos_left[0],ch_pos_left[1]));
		distance_left.push_back(calculated_distance_leap(ch_pos_left[0],ch_pos_left[2]));
		distance_left.push_back(calculated_distance_leap(ch_pos_left[0],ch_pos_left[3]));
		distance_left.push_back(calculated_distance_leap(ch_pos_left[0],ch_pos_left[4]));
		distance_left.push_back(calculated_distance_leap(ch_pos_left[0],ch_pos_left[5]));
		distance_left.push_back(calculated_distance_leap(ch_pos_left[5],ch_pos_left[4]));
		distance_left.push_back(calculated_distance_leap(ch_pos_left[4],ch_pos_left[3]));
		distance_left.push_back(calculated_distance_leap(ch_pos_left[3],ch_pos_left[2]));
		*/
	}
}
float compare_gesture(int& pos)
{
	float max_value = 100000.00;
	float val = 0;
	for(int g = 0;g<points_gestures.size();++g)
	{
		//std::cout<<name_gestures[g]<<std::endl;
		//print_d();
		//print_de(points_gestures[g]);
		val = compare_distances(points_gestures[g]);
		//std::cout<<val<<std::endl;
		if(val<max_value){
			max_value = val;
			pos = g;
		}
	}
	return max_value;
}
float radian_to_sexagesimal(float radian)
{
    return (radian*180)/MATH_PI;
}

class SampleListener : public Leap::Listener {
    public:
        virtual void onConnect(const Controller&);
        virtual void onFrame(const Controller&);
};

void SampleListener::onConnect(const Controller& controller) {
    std::cout << "Connected" << std::endl;
}

int cont=0;
float angle=30;
float angle2=25;
int actual_hands;

void SampleListener::onFrame(const Controller& controller) {
    int pos_name;
	float value;
	std::vector<std::vector<Leap::Vector>> gesture;
	const Frame frame = controller.frame();
	HandList hands = frame.hands();
	actual_hands = frame.hands().count();
	//std::cout<<"TOTAL MANOS: "<<actual_hands<<std::endl;
	for (HandList::const_iterator hl = hands.begin(); hl != hands.end(); ++hl)
	{
		std::vector<Leap::Vector> buffer(6);
		const Hand hand = *hl;
		int handType = hand.isLeft() ? 0 : 1; // 0 mano izquierda 1 mano derecha
		buffer[0] = hand.palmPosition();
		//std::cout<<"mano: "<<buffer[0]<<" ";
		const FingerList fingers = hand.fingers();
		for (FingerList::const_iterator fl = fingers.begin(); fl != fingers.end(); ++fl)
		{
			const Finger finger = *fl;
            buffer[finger.type()+1] += finger.tipPosition();
			//std::cout<<buffer[finger.type()+1]<<" ";
		}
		if(handType){
			ch_pos_right= buffer;
			//std::cout<<"Derecha"<<std::endl;
		}
		else{
			ch_pos_left = buffer;
			//std::cout<<"Izquierda"<<std::endl;
		}
		if(total_gestures>0)
		{
			calculate_actual_distance();
			value = compare_gesture(pos_name);
			actual_gesture = name_gestures[pos_name];
			std::cout<<"GESTO : "<<name_gestures[pos_name]<<std::endl;
			std::cout<<"VALOR : "<<value<<std::endl;
		}
	}
	//print_distances();
    ch_pos_left.clear();
	ch_pos_right.clear();
    distance_left.clear();
    distance_right.clear();
}

void serverLoop(void * arg)
{
    long long start = milliseconds_now();
    long long elapsed;
    while(1)
    {
        elapsed = milliseconds_now()-start;
        server->update();
        if(elapsed>miliseconds)
        {
            start = milliseconds_now();
            server->sendActionPackets();
        }
    }
    _endthread();
}

void clientLoop(void * arg)
{
    while(exit_leap)
    {
        client->update();
    }
    _endthread();
}

int main()
{
    //Creating database sqlite
	const char* sql;
	int op;
	op = sqlite3_open("test.db", &db);
	if( op )
		std::cout<<"No se logro abrir la base de datos"<<std::endl;
   	else
      	std::cout<<"Se abrio la base de datos"<<std::endl;

	//Create date for gestures

    sql = "CREATE TABLE IF NOT EXISTS gestures("  \
         "ID 	INT PRIMARY        KEY      NOT NULL," \
         "name        CHAR(25)      NOT NULL," \
         "positions 	TEXT     NOT NULL);";

    op = sqlite3_exec(db, sql, NULL, 0, NULL);
    if( op != SQLITE_OK ){
   		std::cout<<"error"<<std::endl;
   	}else {
   		std::cout<<"Creada"<<std::endl;
   	}
    // Closing database

   	char *first_query;
   	first_query = "SELECT COUNT(*) from gestures";
   	op = sqlite3_exec(db, first_query, all_gestures, 0, NULL);

	if( op != SQLITE_OK ){
   		std::cout<<"Falla cargando el total de gestos"<<std::endl;
   	}else {
   		std::cout<<"total de gestos cargados"<<std::endl;
   	}
   	std::cout<<total_gestures<<std::endl;

   	char *second_query;
   	second_query = "SELECT * from gestures";
   	op = sqlite3_exec(db, second_query, all_points_gestures, 0, NULL);

	if( op != SQLITE_OK ){
   		std::cout<<"Falla cargando posiciones de gestos"<<std::endl;
   	}else {
   		std::cout<<"total de las posiciones de gestos cargados"<<std::endl;
   	}
   	//print_gestures();
   	sqlite3_close(db);

    std::string ip;
    std::cout<<"Ingrese IP: ";
    std::cin>>ip;
    int option=1;
    std::cout<<"1 Server, 0 Client: ";
    cin>>option;
    if(option)
    {
        std::cout<<"Milisegundos Server: ";
        std::cin>>miliseconds;
        //SampleListener listener;
        //Controller controller;
        //controller.addListener(listener);
        server=new ServerLeap(ip,"9011");
        HANDLE handle = (HANDLE)_beginthread( serverLoop, 0,NULL);
        system("pause");
        exit_leap=false;
        WaitForSingleObject( handle, INFINITE );
        //controller.removeListener(listener);
        delete(server);
    }
    else
    {
        client=new ClientLeap(ip,"9000");
        HANDLE handle = (HANDLE)_beginthread( clientLoop, 0,NULL);
        option=1;
        while(option)
        {
            std::cout<<"Opciones"<<std::endl;
            std::cout<<"1. Buffer"<<std::endl;
            std::cout<<"2. Cambiar Dedos"<<std::endl;
            std::cout<<"0. Salir"<<std::endl;
            std::cout<<"Opcion: ";
            std::cin>>option;
            switch(option)
            {
                case 1:
                    std::cout<<client->get_buffer()<<std::endl;
                    break;
                case 2:
                    client->sendActionPackets();
                    break;
            }
        }
        exit_leap=false;
        WaitForSingleObject( handle, INFINITE );
        delete(client);
    }
}
