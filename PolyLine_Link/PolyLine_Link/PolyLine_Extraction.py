import pandas as pd
from math import trunc
import csv


def extract(exel):  # 엑셀로부터 선분 리스트 생성
    list_polyline = exel[exel['Name'] == 'Line']
    list_a_x = list_polyline['Start X'].tolist()
    list_a_y = list_polyline['Start Y'].tolist()
    for x in range(0, len(list_a_x)):
        list_a_x[x] = trunc(list_a_x[x])
    for x in range(0, len(list_a_y)):
        list_a_y[x] = trunc(list_a_y[x])
    list_b_x = list_polyline['Delta X'].tolist()
    list_b_y = list_polyline['Delta Y'].tolist()
    for x in range(0, len(list_b_x)):
        list_b_x[x] = list_a_x[x] + list_b_x[x]
    for x in range(0, len(list_a_y)):
        list_b_y[x] = list_a_y[x] + list_b_y[x]
    list_line = []
    for x in range(0, len(list_a_x)):
        list_line.append(((list_a_x[x], list_a_y[x], 0), (list_b_x[x], list_b_y[x], 0)))
    return list_line


def extract_point(list_line):  # 선분 리스트로부터 점 리스트 생성
    list_point = []
    for x in range(0, len(list_line)):
        list_point.append(list_line[x][0])
        list_point.append(list_line[x][1])
    return list_point


def min_xy(list_point):  # 최소 좌표 탐색
    min_x = list_point[0][0]
    min_y = list_point[0][1]
    for x in range(1, len(list_point)):
        if list_point[x][0] < min_x:
            min_x = list_point[x][0]
        if list_point[x][1] < min_y:
            min_y = list_point[x][1]
    return min_x, min_y


def min_xy_symmetry_x_axis(list_point):  # x축 대칭방향 최소 좌표 탐색
    min_x = list_point[0][0]
    min_y = list_point[0][1]
    for x in range(1, len(list_point)):
        if list_point[x][0] > min_x:
            min_x = list_point[x][0]
        if list_point[x][1] < min_y:
            min_y = list_point[x][1]
    return min_x, min_y


def max_xyz(front, rear, right, left, roof, floor):
    max_x = front[0][0]
    max_y = front[0][1]
    max_z = front[0][2]
    for x in range(1, len(front)):
        if front[x][0] > max_x:
            max_x = front[x][0]
            print("front[{}][0]={}".format(x, front[x][0]))
            print(max_x)
        if front[x][2] > max_z:
            max_z = front[x][2]
            print("front[{}][2]={}".format(x, front[x][2]))
            print(max_z)

    for x in range(0, len(rear)):
        if rear[x][0] > max_x:
            max_x = rear[x][0]
            print("rear[{}][0]={}".format(x, rear[x][0]))
            print(max_x)
        if rear[x][2] > max_z:
            max_z = rear[x][2]
            print("rear[{}][2]={}".format(x, rear[x][2]))
            print(max_z)

    for x in range(0, len(right)):
        if right[x][0] > max_x:
            max_y = right[x][1]
            print("right[{}][1]={}".format(x, right[x][1]))
            print(max_y)
        if right[x][2] > max_z:
            max_z = right[x][2]
            print("right[{}][2]={}".format(x, right[x][2]))
            print(max_z)

    for x in range(0, len(left)):
        if left[x][1] > max_y:
            max_y = left[x][1]
            print("left[{}][1]={}".format(x, left[x][1]))
            print(max_y)
        if left[x][2] > max_z:
            max_z = left[x][2]
            print("left[{}][2]={}".format(x, left[x][1]))
            print(max_z)

    for x in range(0, len(roof)):
        if roof[x][0] > max_x:
            max_x = roof[x][0]
            print("roof[{}][0]={}".format(x, roof[x][0]))
            print(max_x)
        if roof[x][1] > max_y:
            max_y = roof[x][1]
            print("roof[{}][1]={}".format(x, roof[x][1]))
            print(max_y)

    for x in range(0, len(floor)):
        if floor[x][0] > max_x:
            max_x = floor[x][0]
            print("floor[{}][0]={}".format(x, floor[x][0]))
            print(max_x)
        if floor[x][1] > max_y:
            max_y = floor[x][1]
            print("floor[{}][1]={}".format(x, floor[x][1]))
            print(max_y)

    print("max_x = {}, max_y={}, max_z={}".format(max_x, max_y, max_z))
    return max_x, max_y, max_z


def normalize(list_line, min_x, min_y):  # 표준화
    for x in range(0, len(list_line)):
        list_line[x] = ((int(list_line[x][0][0]) - int(min_x), int(list_line[x][0][1]) - int(min_y), 0),
                        (int(list_line[x][1][0]) - int(min_x), int(list_line[x][1][1]) - int(min_y), 0))
    return list_line


def normalize_symmetry_x_axis(list_line, min_x, min_y):  # x축 대칭 방향 표준화
    for x in range(0, len(list_line)):
        list_line[x] = ((int(min_x) - int(list_line[x][0][0]), int(list_line[x][0][1]) - int(min_y), 0),
                        (int(min_x) - int(list_line[x][1][0]), int(list_line[x][1][1]) - int(min_y), 0))
    return list_line


def xyz_front_view(list_line):  # 정면도 배치
    for x in range(0, len(list_line)):
        list_line[x] = ((list_line[x][0][0], 0, list_line[x][0][1]), (list_line[x][1][0], 0, list_line[x][1][1]))
    return list_line


def xyz_rear_view(list_line):  # 배면도 배치
    for x in range(0, len(list_line)):
        list_line[x] = (
            (list_line[x][0][0], 0, list_line[x][0][1]), (list_line[x][1][0], 0, list_line[x][1][1]))
    return list_line


def xyz_right_side_view(list_line):  # 우측면도 배치
    for x in range(0, len(list_line)):
        list_line[x] = (
            (0, list_line[x][0][0], list_line[x][0][1]), (0, list_line[x][1][0], list_line[x][1][1]))
    return list_line


def xyz_left_side_view(list_line):  # 좌측면도 배치
    for x in range(0, len(list_line)):
        list_line[x] = ((0, list_line[x][0][0], list_line[x][0][1]), (0, list_line[x][1][0], list_line[x][1][1]))
    return list_line


def xyz_roof_floor_view(list_line):  # 지붕도면 배치
    for x in range(0, len(list_line)):
        list_line[x] = (
            (list_line[x][0][0], list_line[x][0][1], 0), (list_line[x][1][0], list_line[x][1][1], 0))
    return list_line


def xyz_right_side_view_shift(list_line, max_x):
    for x in range(0, len(list_line)):
        list_line[x] = (
            (max_x, list_line[x][0][1], list_line[x][0][2]), (max_x, list_line[x][1][1], list_line[x][1][2]))
    return list_line


def xyz_rear_view_shift(list_line, max_y):
    for x in range(0, len(list_line)):
        list_line[x] = (
            (list_line[x][0][0], max_y, list_line[x][0][2]), (list_line[x][1][0], max_y, list_line[x][1][2]))
    return list_line


def xyz_roof_floor_view_shift(list_line, max_z):
    for x in range(0, len(list_line)):
        list_line[x] = (
            (list_line[x][0][0], list_line[x][0][1], max_z), (list_line[x][1][0], list_line[x][1][1], max_z))
    return list_line


front_view_view_xls = pd.read_excel("front_view.xls")  # 각 도면의 엑셀 파일 읽어오기
rear_view_view_xls = pd.read_excel("rear_view.xls")
right_side_view_xls = pd.read_excel("right_side_view.xls")
left_side_view_xls = pd.read_excel("left_side_view.xls")
floor_view_xls = pd.read_excel("floor_view.xls")
roof_floor_view_xls = pd.read_excel("roof_floor_view.xls")

front_view_line = extract(front_view_view_xls)  # 각 도면의 엑셀 파일로부터 선분 리스트 생성
rear_view_line = extract(rear_view_view_xls)
right_side_view_line = extract(right_side_view_xls)
left_side_view_line = extract(left_side_view_xls)
floor_view_line = extract(floor_view_xls)
roof_floor_view_line = extract(roof_floor_view_xls)

front_view_points = extract_point(front_view_line)  # 각 도면의 선분 리스트로부터 모든 점의 리스트 생성
rear_view_points = extract_point(rear_view_line)
right_side_points = extract_point(right_side_view_line)
left_side_points = extract_point(left_side_view_line)
floor_points = extract_point(floor_view_line)
roof_floor_points = extract_point(roof_floor_view_line)

print("size={}\n front point = {}".format(len(front_view_points), front_view_points))
print("rear point = {}".format(rear_view_points))
print("right point = {}".format(right_side_points))
print("left point = {}".format(left_side_points))
print("roof point = {}".format(roof_floor_points))
print("floor point = {}".format(floor_points))

front_view_min_x, front_view_min_y = min_xy(front_view_points)  # x좌표와 y좌표 최소값 탐색
rear_view_min_x, rear_view_min_y = min_xy_symmetry_x_axis(rear_view_points)
right_side_min_x, right_side_min_y = min_xy(right_side_points)
left_side_min_x, left_side_min_y = min_xy_symmetry_x_axis(left_side_points)
floor_min_x, floor_min_y = min_xy(floor_points)
roof_floor_min_x, roof_floor_min_y = min_xy(roof_floor_points)

front_view_line = normalize(front_view_line, front_view_min_x, front_view_min_y)  # 최소 x, y값을 기준으로 좌표 변환
rear_view_line = normalize_symmetry_x_axis(rear_view_line, rear_view_min_x, rear_view_min_y)
right_side_view_line = normalize(right_side_view_line, right_side_min_x, right_side_min_y)
left_side_view_line = normalize_symmetry_x_axis(left_side_view_line, left_side_min_x, left_side_min_y)
floor_view_line = normalize(floor_view_line, floor_min_x, floor_min_y)
roof_floor_view_line = normalize(roof_floor_view_line, roof_floor_min_x, roof_floor_min_y)

print("size={}\n front point = {}".format(len(front_view_points), front_view_points))
print("rear point = {}".format(rear_view_points))
print("right point = {}".format(right_side_points))
print("left point = {}".format(left_side_points))
print("roof point = {}".format(roof_floor_points))
print("floor point = {}".format(floor_points))

front_view_line = xyz_front_view(front_view_line)  # 3차원 공간좌표로 변환
rear_view_line = xyz_rear_view(rear_view_line)
right_side_view_line = xyz_right_side_view(right_side_view_line)
left_side_view_line = xyz_left_side_view(left_side_view_line)
roof_floor_view_line = xyz_roof_floor_view(roof_floor_view_line)

front_view_points = extract_point(front_view_line)  # 변환된 각 도면의 선분 리스트로부터 모든 점의 리스트 재생성
rear_view_points = extract_point(rear_view_line)
right_side_points = extract_point(right_side_view_line)
left_side_points = extract_point(left_side_view_line)
floor_points = extract_point(floor_view_line)
roof_floor_points = extract_point(roof_floor_view_line)

max_x, max_y, max_z = max_xyz(  # 변환한 공간 좌표 중 x, y, z 최대값 탐색
    front_view_points,
    rear_view_points,
    right_side_points,
    left_side_points,
    roof_floor_points,
    floor_points)

rear_view_line = xyz_rear_view_shift(rear_view_line, max_y)  # 탐색한 최대값을 이용하여 배면도, 우측면도, 지붕도면 평행 이동
right_side_view_line = xyz_right_side_view_shift(right_side_view_line, max_x)
roof_floor_view_line = xyz_roof_floor_view_shift(roof_floor_view_line, max_z)

print("front")
for x in front_view_line:
    print("{} to {}".format(x[0], x[1]))
print("rear")
for x in rear_view_line:
    print("{} to {}".format(x[0], x[1]))
print("right side")
for x in right_side_view_line:
    print("{} to {}".format(x[0], x[1]))
print("left side")
for x in left_side_view_line:
    print("{} to {}".format(x[0], x[1]))
print("roof floor")
for x in roof_floor_view_line:
    print("{} to {}".format(x[0], x[1]))
print("floor")
for x in floor_view_line:
    print("{} to {}".format(x[0], x[1]))

front = open("front_view.csv", "w", newline="")
wr_front = csv.writer(front)
for x in range(0, len(front_view_line)):
    wr_front.writerow(
        [front_view_line[x][0][0], front_view_line[x][0][1], front_view_line[x][0][2],
         front_view_line[x][1][0] ,
         front_view_line[x][1][1] , front_view_line[x][1][2] ])
front.close()

rear = open("rear_view.csv", "w", newline="")
wr_rear = csv.writer(rear)
for x in range(0, len(rear_view_line)):
    wr_rear.writerow(
        [rear_view_line[x][0][0] , rear_view_line[x][0][1] , rear_view_line[x][0][2] ,
         rear_view_line[x][1][0] ,
         rear_view_line[x][1][1] , rear_view_line[x][1][2]])
rear.close()

right = open("right_view.csv", "w", newline="")
wr_right = csv.writer(right)
for x in range(0, len(right_side_view_line)):
    wr_right.writerow([right_side_view_line[x][0][0], right_side_view_line[x][0][1],
                       right_side_view_line[x][0][2] ,
                       right_side_view_line[x][1][0] ,
                       right_side_view_line[x][1][1] , right_side_view_line[x][1][2] ])
right.close()

left = open("left_view.csv", "w", newline="")
wr_left = csv.writer(left)
for x in range(0, len(left_side_view_line)):
    wr_left.writerow(
        [left_side_view_line[x][0][0] , left_side_view_line[x][0][1] , left_side_view_line[x][0][2],
         left_side_view_line[x][1][0] ,
         left_side_view_line[x][1][1] , left_side_view_line[x][1][2] ])
left.close()

roof = open("roof_view.csv", "w", newline="")
wr_roof = csv.writer(roof)
for x in range(0, len(roof_floor_view_line)):
    wr_roof.writerow([roof_floor_view_line[x][0][0] , roof_floor_view_line[x][0][1] ,
                      roof_floor_view_line[x][0][2] ,
                      roof_floor_view_line[x][1][0] ,
                      roof_floor_view_line[x][1][1] , roof_floor_view_line[x][1][2] ])
roof.close()

floor = open("floor_view.csv", "w", newline="")
wr_floor = csv.writer(floor)
for x in range(0, len(floor_view_line)):
    wr_floor.writerow(
        [floor_view_line[x][0][0] , floor_view_line[x][0][1] , floor_view_line[x][0][2] ,
         floor_view_line[x][1][0] ,
         floor_view_line[x][1][1] , floor_view_line[x][1][2] ])
floor.close()
