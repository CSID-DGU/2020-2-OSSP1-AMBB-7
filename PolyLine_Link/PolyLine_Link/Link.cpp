#include <process.h>
#include <iostream>
#include <functional>
#include <vector>
#include <string>
#include <fstream>
#include <map>
#include <tuple>
using namespace std;
typedef tuple<double, double, double> xyz;
enum TYPE {
	FRONT, REAR, RIGHT, LEFT, ROOF, FLOOR
};

class Point;
class Polyline;
void readCSV(vector<Polyline>& line, string csv, TYPE T);
void printType(TYPE t);
bool Check(const map<xyz, vector<reference_wrapper<Polyline>>>::iterator& iter);
xyz find_on_normal_line(map<xyz, vector<reference_wrapper<Polyline>>> coordinate_system, TYPE t, const xyz& p);
void PointShift(map<xyz, vector<reference_wrapper<Polyline>>>& coordinate_system, map<xyz, vector<reference_wrapper<Polyline>>>::iterator iter);
void PolylineShift(map<xyz, vector<reference_wrapper<Polyline>>> coordinate_system, Polyline &p, TYPE t);
bool is_in(vector<Polyline> vec1, vector<Polyline> vec2, Point a, Point b, TYPE t);

class Point {
private:
	double x;
	double y;
	double z;
	TYPE T;
public:
	friend class Polyline;
	Point(double a,double b, double c, TYPE t)
		:x(a),y(b),z(c),T(t)
	{}
	bool operator ==(Point &p) {
		if ((x == p.x&&y == p.y&&z == p.z))
			return true;
		else
			return false;
	}
	bool operator ==(xyz &p) {
		if (x == get<0>(p) && y == get<1>(p) && z == get<2>(p))
			return true;
		else return false;
	}
	pair<double, double> xy() const {
		return pair<double, double>(x, y);
	}
	pair<double, double> xz() const {
		return pair<double, double>(x, z);
	}
	pair<double, double> yz() const {
		return pair<double, double>(y, z);
	}
	void set(double a, double b, double c){
		x = a;
		y = b;
		z = c;
	}
	TYPE getType() {
		return this->T;
	}
	double getX() const {
		return this->x;
	}
	double getY() const {
		return this->y;
	}
	double getZ() const {
		return this->z;
	}

	friend ostream& operator<<(ostream& out,const Point& p) {
		out << "x : " << p.x << " y : " << p.y << " z : " << p.z << endl;
		return out;
	}
	bool operator <(const Point &p) {
		if (x < p.x)
			return true;
		else
			return false;
	}
};
class Polyline {
private:
	Point a;
	Point b;
	TYPE T;
public:
	friend class Point;
	Polyline(Point x, Point y)
		:a(x), b(y)
	{
		T = x.getType();
	}
	bool operator ==(Polyline &p) {
		if ((a == p.a&&b == p.b) || (a == p.b&&b == p.a))
			return true;
		else
			return false;
	}
	Point& getA() {
		return this->a;
	}
	Point& getB() {
		return this->b;
	}
	TYPE getType() {
		return this->T;
	}
	void replaceA(const Point &p) {
		a = p;
	}
	void replaceB(const Point &p) {
		b = p;
	}
	Polyline&& operator&&(Polyline& p) {
		return move(*this);
	}
	friend ostream& operator<<(ostream& out, const Polyline& p) {
		out << p.a.getX() << ", " << p.a.getY() << ", " << p.a.getZ() << " to " << p.b.getX() << ", " << p.b.getY() << ", " << p.b.getZ() << endl;
		return out;
	}
};

int main() {
	map < xyz, vector<reference_wrapper<Polyline>>> coordinate_system;
	//system("python ./PolyLine_Extraction.py");
	//system("pause");
	vector<Polyline>  front, rear, right, left, roof, floor;
	readCSV(front, "front_view.csv",FRONT);
	readCSV(rear, "rear_view.csv",REAR);
	readCSV(right, "right_view.csv",RIGHT);
	readCSV(left, "left_view.csv",LEFT);
	readCSV(roof, "roof_view.csv",ROOF);
	readCSV(floor, "floor_view.csv",FLOOR);

	for (auto & p : front) {
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : rear) {
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : right) {
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : left) {
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : roof) {
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : floor) {
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	
	TYPE t;
	for (auto iter = coordinate_system.begin(); iter != coordinate_system.end(); iter++) 
	{
		if (!Check(iter)) {
			PointShift(coordinate_system, iter);
		}
	}
	bool is_in_axis_1,is_in_axis_2;
	for (int i = 0; i < front.size(); i++) {
		is_in_axis_1 = is_in(right, left, front[i].getA(), front[i].getB(), RIGHT);
		is_in_axis_2 = is_in(roof, floor, front[i].getA(), front[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, front[i], FRONT);
			i = 0;
		}
	}
	for (int i = 0; i < rear.size(); i++) {
		is_in_axis_1 = is_in(right, left, rear[i].getA(), rear[i].getB(), RIGHT);
		is_in_axis_2 = is_in(roof, floor, rear[i].getA(), rear[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, rear[i], REAR);
			i = 0;
		}
	}
	for (int i = 0; i < right.size(); i++) {
		is_in_axis_1 = is_in(front, rear, right[i].getA(), right[i].getB(), FRONT);
		is_in_axis_2 = is_in(roof, floor, right[i].getA(), right[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, right[i], RIGHT);
			i = 0;
		}
	}
	for (int i = 0; i < left.size(); i++) {
		is_in_axis_1 = is_in(front, rear, left[i].getA(), left[i].getB(), FRONT);
		is_in_axis_2 = is_in(roof, floor, left[i].getA(), left[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, left[i], LEFT);
			i = 0;
		}
	}
	for (int i = 0; i < roof.size(); i++) {
		is_in_axis_1 = is_in(right, left, roof[i].getA(), roof[i].getB(), RIGHT);
		is_in_axis_2 = is_in(front, rear, roof[i].getA(), roof[i].getB(), FRONT);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, roof[i], ROOF);
			i = 0;
		}
	}
	for (int i = 0; i < floor.size(); i++) {
		is_in_axis_1 = is_in(right, left, floor[i].getA(), floor[i].getB(), RIGHT);
		is_in_axis_2 = is_in(front, rear, floor[i].getA(), floor[i].getB(), FRONT);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, floor[i], FLOOR);
			i = 0;
		}
	}


	return 0;
}

void PolylineShift(map<xyz, vector<reference_wrapper<Polyline>>> coordinate_system, Polyline &p, TYPE t) {
	Point a = p.getA();
	Point b = p.getB();
	xyz destination_a = find_on_normal_line(coordinate_system, t, xyz(a.getX(), a.getY(), a.getZ()));
	xyz destination_b = find_on_normal_line(coordinate_system, t, xyz(b.getX(), b.getY(), b.getZ()));
	bool case1=false, case2=false, case3=false;																			//case 1: ab->a'b, case 2: ab->ab', case 3: ab->a'b'
	Polyline a_b = Polyline(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t), Point(b.getX(), b.getY(), b.getZ(), t));
	Polyline ab_ = Polyline(Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t), Point(a.getX(), a.getY(), a.getZ(), t));
	Polyline a_b_= Polyline(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t), Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t));
	for (int i = 0; i < coordinate_system[destination_a].size(); i++) {
		if (a_b == coordinate_system[destination_a][i].get())
			case1 = true;
		if (a_b_ == coordinate_system[destination_a][i].get())
			case3 = true;
	}
	for (int i = 0; i < coordinate_system[destination_b].size(); i++) {
		if (ab_ == coordinate_system[destination_b][i].get())
			case2 = true;
	}
	if (case1) {
		xyz current_a = xyz(a.getX(), a.getY(), a.getZ());
		for (auto iter = coordinate_system[current_a].begin(); iter != coordinate_system[current_a].end(); iter++) {
			if (iter->get() == p)
				coordinate_system[current_a].erase(iter);
		}
		p.replaceA(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t));
		coordinate_system[destination_a].push_back(p);
	}
	else if (case2) {
		xyz current_b = xyz(b.getX(), b.getY(), b.getZ());
		for (auto iter = coordinate_system[current_b].begin(); iter != coordinate_system[current_b].end(); iter++) {
			if (iter->get() == p)
				coordinate_system[current_b].erase(iter);
		}
		p.replaceB(Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t));
		coordinate_system[destination_b].push_back(p);
	}
	else if (case3) {
		xyz current_a = xyz(a.getX(), a.getY(), a.getZ());
		for (auto iter = coordinate_system[current_a].begin(); iter != coordinate_system[current_a].end(); iter++) {
			if (iter->get() == p)
				coordinate_system[current_a].erase(iter);
		}
		p.replaceA(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t));
		coordinate_system[destination_a].push_back(p);

		xyz current_b = xyz(b.getX(), b.getY(), b.getZ());
		for (auto iter = coordinate_system[current_b].begin(); iter != coordinate_system[current_b].end(); iter++) {
			if (iter->get() == p)
				coordinate_system[current_b].erase(iter);
		}
		p.replaceB(Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t));
		coordinate_system[destination_b].push_back(p);
	}
}

bool is_in(vector<Polyline> vec1, vector<Polyline> vec2, Point a, Point b, TYPE t) //type of vec
{
	pair<double, double> projection_a, projection_b;
	bool is_in_a_1 = false , is_in_b_1 = false, is_in_a_2 = false, is_in_b_2 = false;
	if (t == FRONT || t == REAR) {
		projection_a = a.xz();
		projection_b = b.xz();
		if (projection_a == projection_b) {
			for (int i = 0; i < vec1.size(); i++) {
				if (projection_a == vec1[i].getA().xz() || projection_a == vec1[i].getB().xz()) {
					is_in_a_1 = true;
					is_in_b_1 = true;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if (projection_a == vec2[i].getA().xz() || projection_a == vec2[i].getB().xz()) {
					is_in_a_2 = true;
					is_in_b_2 = true;
				}
			}
		}
		else {
			for (int i = 0; i < vec1.size(); i++) {
				if ((projection_a == vec1[i].getA().xz() && projection_b == vec1[i].getB().xz())|| (projection_a == vec1[i].getB().xz() && projection_b == vec1[i].getA().xz())) {
					is_in_a_1 = true;
					is_in_b_1 = true;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if ((projection_a == vec1[i].getA().xz() && projection_b == vec1[i].getB().xz()) || (projection_a == vec1[i].getB().xz() && projection_b == vec1[i].getA().xz())) {
					is_in_a_2 = true;
					is_in_b_2 = true;
				}
			}
		}
	}
	else if (t == RIGHT || t == LEFT) {
		projection_a = a.yz();
		projection_b = b.yz();
		if (projection_a == projection_b) {
			for (int i = 0; i < vec1.size(); i++) {
				if (projection_a == vec1[i].getA().yz() || projection_a == vec1[i].getB().yz()) {
					is_in_a_1 = true;
					is_in_b_1 = true;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if (projection_a == vec2[i].getA().yz() || projection_a == vec2[i].getB().yz()) {
					is_in_a_2 = true;
					is_in_b_2 = true;
				}
			}
		}
		else {
			for (int i = 0; i < vec1.size(); i++) {
				if ((projection_a == vec1[i].getA().yz() && projection_b == vec1[i].getB().yz()) || (projection_a == vec1[i].getB().yz() && projection_b == vec1[i].getA().yz())) {
					is_in_a_1 = true;
					is_in_b_1 = true;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if ((projection_a == vec1[i].getA().yz() && projection_b == vec1[i].getB().yz()) || (projection_a == vec1[i].getB().yz() && projection_b == vec1[i].getA().yz())) {
					is_in_a_2 = true;
					is_in_b_2 = true;
				}
			}
		}
	}
	else {
		projection_a = a.xy();
		projection_b = b.xy();
		if (projection_a == projection_b) {
			for (int i = 0; i < vec1.size(); i++) {
				if (projection_a == vec1[i].getA().xy() || projection_a == vec1[i].getB().xy()) {
					is_in_a_1 = true;
					is_in_b_1 = true;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if (projection_a == vec2[i].getA().xy() || projection_a == vec2[i].getB().xy()) {
					is_in_a_2 = true;
					is_in_b_2 = true;
				}
			}
		}
		else {
			for (int i = 0; i < vec1.size(); i++) {
				if ((projection_a == vec1[i].getA().xy() && projection_b == vec1[i].getB().xy()) || (projection_a == vec1[i].getB().xy() && projection_b == vec1[i].getA().xy())) {
					is_in_a_1 = true;
					is_in_b_1 = true;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if ((projection_a == vec1[i].getA().xy() && projection_b == vec1[i].getB().xy()) || (projection_a == vec1[i].getB().xy() && projection_b == vec1[i].getA().xy())) {
					is_in_a_2 = true;
					is_in_b_2 = true;
				}
			}
		}
	}
	return (is_in_a_1&&is_in_b_1) && (is_in_a_2&&is_in_b_2);
}

void PointShift(map<xyz,vector<reference_wrapper<Polyline>>>& coordinate_system, map<xyz, vector<reference_wrapper<Polyline>>>::iterator iter) {
	TYPE t = iter->second[0].get().getType();
	xyz destination = find_on_normal_line(coordinate_system, t, iter->first);
	if (!(destination == iter->first)) {
		xyz currunt_location = iter->first;
		for (int i = 0; i < iter->second.size(); i++) {
			if (iter->second[i].get().getA() == currunt_location) {
				iter->second[i].get().replaceA(Point(get<0>(destination), get<1>(destination), get<2>(destination), iter->second[i].get().getType()));
				coordinate_system[destination].push_back(iter->second[i]);
			}
			else {
				iter->second[i].get().replaceB(Point(get<0>(destination), get<1>(destination), get<2>(destination), iter->second[i].get().getType()));
				coordinate_system[destination].push_back(iter->second[i]);
			}
		}
		iter->second.clear();
	}
}

xyz find_on_normal_line(map<xyz, vector<reference_wrapper<Polyline>>> coordinate_system, TYPE t, const xyz& p) {
	function < tuple<double,double>(xyz)> func;
	double distance = numeric_limits<double>::max();
	double temp;
	if(t==FRONT&&t==REAR)
		func = [](xyz x)->tuple<double, double> {return tuple<double,double>(get<0>(x), get<2>(x)); };
	else if(t==RIGHT&&t==LEFT)
		func = [](xyz x)->tuple<double, double> {return tuple<double, double>(get<1>(x), get<2>(x)); };
	else
		func = [](xyz x)->tuple<double, double> {return tuple<double, double>(get<0>(x), get<1>(x)); };

	auto func2 = std::bind(func, p);

	for (auto iter = coordinate_system.begin(); iter != coordinate_system.end(); iter++) {
		if (func(iter->first) == func2()) {
			temp = abs(get<0>(iter->first) - get<0>(p) + get<1>(iter->first) - get<1>(p) + get<2>(iter->first) - get<2>(p));
			if (temp!=0&&temp < distance) {
				distance = temp;
				return iter->first;
			}
		}
	}
	return p;
}

bool Check(const map<xyz, vector<reference_wrapper<Polyline>>>::iterator& iter) {
	bool type=false;
	TYPE t = iter->second[0].get().getType();
	for (int i = 1; i < iter->second.size(); i++) {
		if (t != iter->second[i].get().getType())
			type = true;
	}
	return (type&&(iter->second.size()>2));
}

void printType(TYPE t) {
	switch (t)
	{
	case FRONT:
		cout << "FRONT" << endl;
		break;
	case REAR:
		cout << "REAR" << endl;
		break;
	case RIGHT:
		cout << "RIGHT" << endl;
		break;
	case LEFT:
		cout << "LEFT" << endl;
		break;
	case ROOF:
		cout << "ROOF" << endl;
		break;
	case FLOOR:
		cout << "FLOOR" << endl;
		break;
	}
}


void readCSV(vector<Polyline>& line, string csv,TYPE T) {
	ifstream i;
	string str;
	double a_x, a_y, a_z, b_x, b_y, b_z;
	i.open(csv);
	while (getline(i, str, ',')) {
		a_x = stod(str);
		getline(i, str, ',');
		a_y = stod(str);
		getline(i, str, ',');
		a_z = stod(str);
		getline(i, str, ',');
		b_x = stod(str);
		getline(i, str, ',');
		b_y = stod(str);
		getline(i, str, ',');
		b_z = stod(str);
		line.push_back(Polyline(Point(a_x, a_y, a_z, T), Point(b_x, b_y, b_z, T)));
		getline(i, str);
	}
}