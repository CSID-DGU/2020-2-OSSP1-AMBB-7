#include <process.h>
#include <iostream>
#include <functional>
#include <vector>
#include <string>
#include <fstream>
#include <map>
#include <tuple>
#include <limits>
using namespace std;
typedef tuple<double, double, double> xyz;
enum TYPE		//도면 타입
{
	FRONT, REAR, RIGHT, LEFT, ROOF, FLOOR
};

class Point;
class Polyline;
void readCSV(vector<Polyline>& line, string csv, TYPE T);
void printType(TYPE t);
bool Check(const map<xyz, vector<reference_wrapper<Polyline>>>::iterator& iter);
xyz find_on_normal_line(map<xyz, vector<reference_wrapper<Polyline>>>& coordinate_system, TYPE t, const xyz& p);
void PointShift(map<xyz, vector<reference_wrapper<Polyline>>>& coordinate_system, map<xyz, vector<reference_wrapper<Polyline>>>::iterator iter);
void PolylineShift(map<xyz, vector<reference_wrapper<Polyline>>>& coordinate_system, Polyline &p, TYPE t);
bool is_in(vector<Polyline> vec1, vector<Polyline> vec2, Point a, Point b, TYPE t);

class Point			//공간 좌표 클래스
{		
private:
	double x;
	double y;
	double z;
	TYPE T;
public:
	friend class Polyline;
	Point(double a,double b, double c, TYPE t)	//생성자
		:x(a),y(b),z(c),T(t)
	{}
	bool operator ==(Point &p)					//==연산자 오버로딩
	{
		if ((x == p.x&&y == p.y&&z == p.z))
			return true;
		else
			return false;
	}
	bool operator ==(xyz &p)					//==xyz연산자 오버로딩 
	{
		if (x == get<0>(p) && y == get<1>(p) && z == get<2>(p))
			return true;
		else return false;
	}
	pair<double, double> xy() const				//xy평면 정사영 좌표 반환
	{
		return pair<double, double>(x, y);
	}
	pair<double, double> xz() const				//xz평면 정사영 좌표 반환
	{
		return pair<double, double>(x, z);
	}
	pair<double, double> yz() const				//yz평면 정사영 좌표 반환
	{
		return pair<double, double>(y, z);
	}
	void set(double a, double b, double c)		//좌표 변경
	{
		x = a;
		y = b;
		z = c;
	}
	TYPE getType()								//타입 반환
	{
		return this->T;
	}
	double getX() const							//x좌표 반환
	{
		return this->x;
	}
	double getY() const							//y좌표 반환
	{
		return this->y;
	}
	double getZ() const							//z좌표 반환
	{
		return this->z;
	}

	friend ostream& operator<<(ostream& out,const Point& p)		//디버깅용 출력 연산자 오버로딩
	{
		out << "x : " << p.x << " y : " << p.y << " z : " << p.z << endl;
		return out;
	}
	bool operator <(const Point &p)				//<연산자 오버로딩
	{
		if (x < p.x)
			return true;
		else
			return false;
	}
};
class Polyline									//공간좌표 상 선분 클래스
{
private:
	Point a;
	Point b;
	TYPE T;
public:
	friend class Point;
	Polyline(Point x, Point y)					//생성자
		:a(x), b(y)
	{
		T = x.getType();
	}
	bool operator ==(Polyline &p)				//==연산자 오버로딩
	{
		if ((a == p.a&&b == p.b) || (a == p.b&&b == p.a))
			return true;
		else
			return false;
	}
	Point& getA()								//점 반환
	{
		return this->a;
	}
	Point& getB()								//점 반환
	{
		return this->b;
	}
	TYPE getType()								//타입 반환
	{
		return this->T;
	}
	void replaceA(const Point &p)				//점 변환
	{
		a = p;
	}
	void replaceB(const Point &p)				//점 변환
	{
		b = p;
	}
	friend ostream& operator<<(ostream& out, const Polyline& p)		//디버깅용 출력 연산자 오버로딩
	{
		out << p.a.getX() << ", " << p.a.getY() << ", " << p.a.getZ() << " to " << p.b.getX() << ", " << p.b.getY() << ", " << p.b.getZ() << endl;
		return out;
	}
};

int main() {
	map < xyz, vector<reference_wrapper<Polyline>>> coordinate_system;	//공간좌표 생성
	//system("python ./PolyLine_Extraction.py");						//PolyLine_Extraction.py 실행
	//system("pause");
	vector<Polyline>  front, rear, right, left, roof, floor;			//각 도면 선분 배열

	readCSV(front, "front_view.csv",FRONT);								//csv파일에서 선분 읽어들이기
	readCSV(rear, "rear_view.csv",REAR);
	readCSV(right, "right_view.csv",RIGHT);
	readCSV(left, "left_view.csv",LEFT);
	readCSV(roof, "roof_view.csv",ROOF);
	readCSV(floor, "floor_view.csv",FLOOR);

	for (auto & p : front)												//공간좌표에 점 생성
	{			
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : rear)
	{
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : right)
	{
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : left)
	{
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : roof) 
	{
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	for (auto & p : floor)
	{
		coordinate_system[xyz(p.getA().getX(), p.getA().getY(), p.getA().getZ())].push_back(p);
		coordinate_system[xyz(p.getB().getX(), p.getB().getY(), p.getB().getZ())].push_back(p);
	}
	


																							//공간좌표에 생성된 모든 점에 대해, 
																							//1. 최소 2개 이상의 도면의 선분들이 해당 점을 지나는지
																							//2. 1의 조건을 만족시키지 못했을 경우 해당 점을 지나는 선분의 개수 확인
																							//조건을 만족하지 못한 점을, 법선 방향의 점으로 치환
	TYPE t;
	for (auto iter = coordinate_system.begin(); iter != coordinate_system.end(); iter++)	
	{
		if (!Check(iter)) {
			PointShift(coordinate_system, iter);
		}
	}

																							//공간좌표 상의 모든 선분에 대해,
																							//수직하는 평면에 정사영된 결과가 
																							//해당 도면에 존재하는지 확인 후 이동
	bool is_in_axis_1,is_in_axis_2;
	for (int i = 0; i < front.size(); i++) 
	{
		is_in_axis_1 = is_in(right, left, front[i].getA(), front[i].getB(), RIGHT);
		is_in_axis_2 = is_in(roof, floor, front[i].getA(), front[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, front[i], FRONT);
			i = -1;
		}
	}
	for (int i = 0; i < rear.size(); i++) 
	{
		is_in_axis_1 = is_in(right, left, rear[i].getA(), rear[i].getB(), RIGHT);
		is_in_axis_2 = is_in(roof, floor, rear[i].getA(), rear[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, rear[i], REAR);
			i = -1;
		}
	}
	for (int i = 0; i < right.size(); i++)
	{
		is_in_axis_1 = is_in(front, rear, right[i].getA(), right[i].getB(), FRONT);
		is_in_axis_2 = is_in(roof, floor, right[i].getA(), right[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, right[i], RIGHT);
			i = -1;
		}
	}
	for (int i = 0; i < left.size(); i++)
	{
		is_in_axis_1 = is_in(front, rear, left[i].getA(), left[i].getB(), FRONT);
		is_in_axis_2 = is_in(roof, floor, left[i].getA(), left[i].getB(), ROOF);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, left[i], LEFT);
			i = -1;
		}
	}
	for (int i = 0; i < roof.size(); i++) 
	{
		is_in_axis_1 = is_in(right, left, roof[i].getA(), roof[i].getB(), RIGHT);
		is_in_axis_2 = is_in(front, rear, roof[i].getA(), roof[i].getB(), FRONT);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, roof[i], ROOF);
			i = -1;
		}
	}
	for (int i = 0; i < floor.size(); i++)
	{
		is_in_axis_1 = is_in(right, left, floor[i].getA(), floor[i].getB(), RIGHT);
		is_in_axis_2 = is_in(front, rear, floor[i].getA(), floor[i].getB(), FRONT);
		if (!(is_in_axis_1&&is_in_axis_2)) {
			PolylineShift(coordinate_system, floor[i], FLOOR);
			i = -1;
		}
	}
	map < xyz, vector<reference_wrapper<Polyline>>> copy;
	xyz temp,temp2;
	bool overlap;
	for (int i = 0; i < front.size(); i++) {
		temp = xyz(front[i].getA().getX(), front[i].getA().getY(), front[i].getA().getZ());
		temp2 = xyz(front[i].getB().getX(), front[i].getB().getY(), front[i].getB().getZ());
		if (copy[temp].size() == 0) {
			copy[temp].push_back(front[i]);
			copy[temp2].push_back(front[i]);
		}
		else {
			overlap = false;
			for (int j = 0; j < copy[temp].size();j++) {
				if (front[i] == copy[temp][j].get()) {
					overlap = true;
					break;
				}
				
			}
			if (!overlap) {
				copy[temp].push_back(front[i]);
				copy[temp2].push_back(front[i]);
			}
		}

	}
	for (int i = 0; i < rear.size(); i++) {
		temp = xyz(rear[i].getA().getX(), rear[i].getA().getY(), rear[i].getA().getZ());
		temp2 = xyz(rear[i].getB().getX(), rear[i].getB().getY(), rear[i].getB().getZ());
		if (copy[temp].size() == 0) {
			copy[temp].push_back(rear[i]);
			copy[temp2].push_back(rear[i]);
		}
		else {
			overlap = false;
			for (int j = 0; j < copy[temp].size(); j++) {
				if (rear[i] == copy[temp][j].get()) {
					overlap = true;
					break;
				}

			}
			if (!overlap) {
				copy[temp].push_back(rear[i]);
				copy[temp2].push_back(rear[i]);
			}
		}

	}
	for (int i = 0; i < right.size(); i++) {
		temp = xyz(right[i].getA().getX(), right[i].getA().getY(), right[i].getA().getZ());
		temp2 = xyz(right[i].getB().getX(), right[i].getB().getY(), right[i].getB().getZ());
		if (copy[temp].size() == 0) {
			copy[temp].push_back(right[i]);
			copy[temp2].push_back(right[i]);
		}
		else {
			overlap = false;
			for (int j = 0; j < copy[temp].size(); j++) {
				if (right[i] == copy[temp][j].get()) {
					overlap = true;
					break;
				}

			}
			if (!overlap) {
				copy[temp].push_back(right[i]);
				copy[temp2].push_back(right[i]);
			}
		}

	}
	for (int i = 0; i < left.size(); i++) {
		temp = xyz(left[i].getA().getX(), left[i].getA().getY(), left[i].getA().getZ());
		temp2 = xyz(left[i].getB().getX(), left[i].getB().getY(), left[i].getB().getZ());
		if (copy[temp].size() == 0) {
			copy[temp].push_back(left[i]);
			copy[temp2].push_back(left[i]);
		}
		else {
			overlap = false;
			for (int j = 0; j < copy[temp].size(); j++) {
				if (left[i] == copy[temp][j].get()) {
					overlap = true;
					break;
				}

			}
			if (!overlap) {
				copy[temp].push_back(left[i]);
				copy[temp2].push_back(left[i]);
			}
		}

	}
	for (int i = 0; i < floor.size(); i++) {
		temp = xyz(floor[i].getA().getX(), floor[i].getA().getY(), floor[i].getA().getZ());
		temp2 = xyz(floor[i].getB().getX(), floor[i].getB().getY(), floor[i].getB().getZ());
		if (copy[temp].size() == 0) {
			copy[temp].push_back(floor[i]);
			copy[temp2].push_back(floor[i]);
		}
		else {
			overlap = false;
			for (int j = 0; j < copy[temp].size(); j++) {
				if (floor[i] == copy[temp][j].get()) {
					overlap = true;
					break;
				}

			}
			if (!overlap) {
				copy[temp].push_back(floor[i]);
				copy[temp2].push_back(floor[i]);
			}
		}

	}
	for (int i = 0; i < roof.size(); i++) {
		temp = xyz(roof[i].getA().getX(), roof[i].getA().getY(), roof[i].getA().getZ());
		temp2 = xyz(roof[i].getB().getX(), roof[i].getB().getY(), roof[i].getB().getZ());
		if (copy[temp].size() == 0) {
			copy[temp].push_back(roof[i]);
			copy[temp2].push_back(roof[i]);
		}
		else {
			overlap = false;
			for (int j = 0; j < copy[temp].size(); j++) {
				if (roof[i] == copy[temp][j].get()) {
					overlap = true;
					break;
				}

			}
			if (!overlap) {
				copy[temp].push_back(roof[i]);
				copy[temp2].push_back(roof[i]);
			}
		}
	}
	vector<Polyline> result;
	bool is_in;
	for (auto iter = copy.begin(); iter != copy.end(); iter++) {
		
		for (int i = 0; i < iter->second.size();i++) {
			is_in = false;
			for (int j = 0; j < result.size(); j++) {
				if (result[j] == iter->second[i].get()) {
					is_in = true;
					break;
				}
			}
			if (!is_in)
				result.push_back(iter->second[i].get());
		}
	}
	ofstream os("3d.txt");
	for (int i = 0; i < result.size(); i++) {
		os << result[i].getA().getX() << " " << result[i].getA().getY() << " " << result[i].getA().getZ() << " ";
		os << result[i].getB().getX() << " " << result[i].getB().getY() << " " << result[i].getB().getZ() << endl;
	}
	os.close();
	return 0;
}

void PolylineShift(map<xyz, vector<reference_wrapper<Polyline>>>& coordinate_system, Polyline &p, TYPE t) //특정 선분을 이동
{
	Point a = p.getA();
	Point b = p.getB();
	xyz destination_a = find_on_normal_line(coordinate_system, t, xyz(a.getX(), a.getY(), a.getZ()));
	xyz destination_b = find_on_normal_line(coordinate_system, t, xyz(b.getX(), b.getY(), b.getZ()));
	bool case1=false, case2=false, case3=false;			//case 1: ab->a'b, case 2: ab->ab', case 3: ab->a'b'
	Polyline a_b = Polyline(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t), Point(b.getX(), b.getY(), b.getZ(), t));
	Polyline ab_ = Polyline(Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t), Point(a.getX(), a.getY(), a.getZ(), t));
	Polyline a_b_= Polyline(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t), Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t));
	for (int i = 0; i < coordinate_system[destination_a].size(); i++) {
		if (a_b == coordinate_system[destination_a][i].get())	//선분 a_b가 존재하는 경우
			case1 = true;
		if (a_b_ == coordinate_system[destination_a][i].get())	//선분a_b_가 존재하는 경우
			case3 = true;
	}
	for (int i = 0; i < coordinate_system[destination_b].size(); i++) //선분 ab_가 존재하는 경우
	{
		if (ab_ == coordinate_system[destination_b][i].get())
			case2 = true;
	}
	if (case1)			//case1, case2, case3 세 개의 경우 중 두 가지 이상이 동시에 만족되고 그 중 case3가 포함될 때, case3는 배제한다
	{
		xyz current_a = xyz(a.getX(), a.getY(), a.getZ());
		for (auto iter = coordinate_system[current_a].begin(); iter != coordinate_system[current_a].end(); iter++) {
			if (iter->get() == p) {
				coordinate_system[current_a].erase(iter);
				break;
			}
		}
		p.replaceA(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t));
		coordinate_system[destination_a].push_back(p);
	}
	else if (case2) 
	{
		xyz current_b = xyz(b.getX(), b.getY(), b.getZ());
		for (auto iter = coordinate_system[current_b].begin(); iter != coordinate_system[current_b].end(); iter++) {
			if (iter->get() == p) {
				coordinate_system[current_b].erase(iter);
				break;
			}
		}
		p.replaceB(Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t));
		coordinate_system[destination_b].push_back(p);
	}
	else if (case3) {
		xyz current_a = xyz(a.getX(), a.getY(), a.getZ());
		for (auto iter = coordinate_system[current_a].begin(); iter != coordinate_system[current_a].end(); iter++) {
			if (iter->get() == p) {
				coordinate_system[current_a].erase(iter);
				break;
			}
		}
		p.replaceA(Point(get<0>(destination_a), get<1>(destination_a), get<2>(destination_a), t));
		coordinate_system[destination_a].push_back(p);

		xyz current_b = xyz(b.getX(), b.getY(), b.getZ());
		for (auto iter = coordinate_system[current_b].begin(); iter != coordinate_system[current_b].end(); iter++) {
			if (iter->get() == p) {
				coordinate_system[current_b].erase(iter);
				break;
			}
		}
		p.replaceB(Point(get<0>(destination_b), get<1>(destination_b), get<2>(destination_b), t));
		coordinate_system[destination_b].push_back(p);
	}
}

bool is_in(vector<Polyline> vec1, vector<Polyline> vec2, Point a, Point b, TYPE t) //선분ab를 t 타입 도면의 법선 방향으로 정사영한 결과가 vec1,vec2에 존재하는지 확인		Type t : type of vector
{
	pair<double, double> projection_a, projection_b;	//정사영 a, b
	bool is_in_a_1 = false , is_in_b_1 = false, is_in_a_2 = false, is_in_b_2 = false;	//점a->vec1,vec2 점b->vec1,vec2
	if (t == FRONT || t == REAR)		//정면도, 배면도
	{
		projection_a = a.xz();			//정사영 좌표
		projection_b = b.xz();
		if (projection_a == projection_b)	//xz평면에 수직하는 경우
		{
			for (int i = 0; i < vec1.size(); i++) {
				if (projection_a == vec1[i].getA().xz() || projection_a == vec1[i].getB().xz()) {
					is_in_a_1 = true;
					is_in_b_1 = true;
					break;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if (projection_a == vec2[i].getA().xz() || projection_a == vec2[i].getB().xz()) {
					is_in_a_2 = true;
					is_in_b_2 = true;
					break;
				}
			}
		}
		else {
			for (int i = 0; i < vec1.size(); i++) {
				if ((projection_a == vec1[i].getA().xz() && projection_b == vec1[i].getB().xz())|| (projection_a == vec1[i].getB().xz() && projection_b == vec1[i].getA().xz())) {
					is_in_a_1 = true;
					is_in_b_1 = true;
					break;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if ((projection_a == vec1[i].getA().xz() && projection_b == vec1[i].getB().xz()) || (projection_a == vec1[i].getB().xz() && projection_b == vec1[i].getA().xz())) {
					is_in_a_2 = true;
					is_in_b_2 = true;
					break;
				}
			}
		}
	}
	else if (t == RIGHT || t == LEFT)		//우측면도, 좌측면도
	{
		projection_a = a.yz();
		projection_b = b.yz();
		if (projection_a == projection_b)	//yz평면에 수직하는 경우
		{
			for (int i = 0; i < vec1.size(); i++) {
				if (projection_a == vec1[i].getA().yz() || projection_a == vec1[i].getB().yz()) {
					is_in_a_1 = true;
					is_in_b_1 = true;
					break;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if (projection_a == vec2[i].getA().yz() || projection_a == vec2[i].getB().yz()) {
					is_in_a_2 = true;
					is_in_b_2 = true;
					break;
				}
			}
		}
		else {
			for (int i = 0; i < vec1.size(); i++) {
				if ((projection_a == vec1[i].getA().yz() && projection_b == vec1[i].getB().yz()) || (projection_a == vec1[i].getB().yz() && projection_b == vec1[i].getA().yz())) {
					is_in_a_1 = true;
					is_in_b_1 = true;
					break;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if ((projection_a == vec2[i].getA().yz() && projection_b == vec2[i].getB().yz()) || (projection_a == vec2[i].getB().yz() && projection_b == vec2[i].getA().yz())) {
					is_in_a_2 = true;
					is_in_b_2 = true;
					break;
				}
			}
		}
	}
	else		//지붕도면, 평면도
	{
		projection_a = a.xy();
		projection_b = b.xy();
		if (projection_a == projection_b)	//xy평면에 수직하는 경우
		{
			for (int i = 0; i < vec1.size(); i++) {
				if (projection_a == vec1[i].getA().xy() || projection_a == vec1[i].getB().xy()) {
					is_in_a_1 = true;
					is_in_b_1 = true;
					break;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if (projection_a == vec2[i].getA().xy() || projection_a == vec2[i].getB().xy()) {
					is_in_a_2 = true;
					is_in_b_2 = true;
					break;
				}
			}
		}
		else {
			for (int i = 0; i < vec1.size(); i++) {
				if ((projection_a == vec1[i].getA().xy() && projection_b == vec1[i].getB().xy()) || (projection_a == vec1[i].getB().xy() && projection_b == vec1[i].getA().xy())) {
					is_in_a_1 = true;
					is_in_b_1 = true;
					break;
				}

			}
			for (int i = 0; i < vec2.size(); i++) {
				if ((projection_a == vec2[i].getA().xy() && projection_b == vec2[i].getB().xy()) || (projection_a == vec2[i].getB().xy() && projection_b == vec2[i].getA().xy())) {
					is_in_a_2 = true;
					is_in_b_2 = true;
					break;
				}
			}
		}
	}
	return (is_in_a_1&&is_in_b_1) && (is_in_a_2&&is_in_b_2);
}

void PointShift(map<xyz,vector<reference_wrapper<Polyline>>>& coordinate_system, map<xyz, vector<reference_wrapper<Polyline>>>::iterator iter) //특정 점을 지나는 선분들에 대해, 해당 점 치환
{
	TYPE t = iter->second[0].get().getType();
	xyz destination = find_on_normal_line(coordinate_system, t, iter->first);	//점을 지나는 법선 상의 최소 거리 좌표 탐색
	if (!(destination == iter->first)) //좌표가 탐색 되었을 경우
	{
		xyz currunt_location = iter->first;	//현재 위치 저장
		for (int i = 0; i < iter->second.size(); i++) 
		{
			if (iter->second[i].get().getA() == currunt_location) //좌표가 선분의 구성 성분 A인 경우
			{
				iter->second[i].get().replaceA(Point(get<0>(destination), get<1>(destination), get<2>(destination), iter->second[i].get().getType()));	//좌표 변경
				coordinate_system[destination].push_back(iter->second[i]);			//해당 좌표 해쉬맵에 삽입
			}
			else												//좌표가 선분의 구성 성분 B인 경우
			{
				iter->second[i].get().replaceB(Point(get<0>(destination), get<1>(destination), get<2>(destination), iter->second[i].get().getType()));
				coordinate_system[destination].push_back(iter->second[i]);
			}
		}
		iter->second.clear();
	}
}

xyz find_on_normal_line(map<xyz, vector<reference_wrapper<Polyline>>>& coordinate_system, TYPE t, const xyz& p) //특정 점에 대해, 해당 점을 지나는 도면의 법선 상 점 탐색
{
	function < tuple<double,double>(xyz)> func;	//함수 선언
	double distance = numeric_limits<double>::max();
	double temp;
	xyz temp_point = xyz(0, 0, 0);
	if(t==FRONT||t==REAR)	//xz평면
		func = [](xyz x)->tuple<double, double> {return tuple<double,double>(get<0>(x), get<2>(x)); };
	else if(t==RIGHT||t==LEFT)	//yz평면
		func = [](xyz x)->tuple<double, double> {return tuple<double, double>(get<1>(x), get<2>(x)); };
	else//xy평면
		func = [](xyz x)->tuple<double, double> {return tuple<double, double>(get<0>(x), get<1>(x)); };

	auto func2 = std::bind(func, p);		//점 p를 func함수에 바인딩

	for (auto iter = coordinate_system.begin(); iter != coordinate_system.end(); iter++) {
		if (func(iter->first) == func2()) //법선 상에 위치하는 점 탐색
		{
			temp = get<0>(iter->first) - get<0>(p) + get<1>(iter->first) - get<1>(p) + get<2>(iter->first) - get<2>(p);	//해당 점과 점 p의 좌표 값의 차이
			if (temp < 0 && (t == REAR || t == RIGHT || t == ROOF))	//점 p가 배면도, 우측면도, 지붕도면이고 좌표값의 차이가 음수인 경우
				temp = abs(temp);
			else if (!(temp > 0 && (t == FRONT || t == LEFT || t == FLOOR)))	//점 p가 정면도, 좌측면도, 평면도이고 좌표값의 차이가 양수인 경우를 만족하지 못할 때
				continue;

			if (temp < distance) //촤소 거리의 좌표 탐색
			{
				distance = temp;
				temp_point = iter->first;
			}
		}
	}
	if (temp_point == xyz(0, 0, 0))	//조건을 만족하는 좌표가 탐색되지 않았을 경우
		return p;
	else
		return temp_point;
}

bool Check(const map<xyz, vector<reference_wrapper<Polyline>>>::iterator& iter)	//특정한 좌표에 대해, 해당 좌표를 지나는 선분들이 속한 도면의 종류 확인
{
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
		i >> str;
		b_z = stod(str);
		line.push_back(Polyline(Point(a_x, a_y, a_z, T), Point(b_x, b_y, b_z, T)));
		getline(i, str);
	}
}