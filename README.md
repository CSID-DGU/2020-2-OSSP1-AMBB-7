# 2020-2-OSSP1-AMBB-7
Auto 3D Modeling for 2D Building Blueprints

# INSTALL #
1. install unity hub (here : https://unity3d.com/get-unity/download)
2. install unityEditor(Recommended Version: 2019.4.13f1)
3. add Project `part_unity`

# 도면 제약 조건 #
1. 각 Line의 Delta X, Delta Y 값은 모두 정수여야합니다.
2. 선분 AB위의 점 C에 대해, 점 C를 양 끝점 중 하나로 갖는 선분이 존재한다면 AB는 반드시 AC, CB로 표현되어야 합니다.
3. Auto CAD가 한글버전일 경우, DATA EXTRACTION을 수행한 엑셀(.xls)파일의 칼럼 명을 다음과 같이 변경해야 합니다.
- 델타 X -> Delta X
- 델타 Y -> Delta Y
- 시작 X -> Start X
- 시작 Y -> Start Y
- 이름 -> Name ( 이름 칼럼에 속하는 모든 '선' 값을 갖는 데이터에 대해 Line으로 데이터 값을 변경해야합니다.)

# 빌드파일 실행 시 참고 사항 #
- 윈도우 전용 빌드로, 운영체제는 윈도우여야 합니다.
- 파이썬과 Pandas 모듈이 설치된 PC여야 합니다.
- c, c++가 설치된 PC여야 합니다.
- github의 BuildFile 폴더와, ExcelFiles 폴더를 다운받아야 합니다.

# 빌드 파일 실행 방법 #
1. BuildFile 폴더 안의 part_Unity.exe를 실행한다.
(1-1. 창모드로 실행이 되었다면, alt + enter 키를 눌러 전체화면으로 전환해 줍니다.)
2. 각 순서대로
- Floor View (평면도)
- Front View (정면도)
- Left Side View (좌측면도)
- Right Side View (우측면도)
- Roof Floor View (지붕 평면도)
- Rear View (배면도)
에 속하는 파일들을 ExcelFiles 폴더 내에서 번호하나를 선택해 같은 번호들을 넣어 준다.
3. Build 버튼을 눌러 3D 도면을 생성한다.
4. 이후, 다른 도면을 보고 싶다면 Load Files 버튼을 눌러 다시 다른 도면 파일들을 불러와준다.

# 기능 설명 #
위의 과정을 거치고 난 뒤의 프로그램에서 확인해볼 수 있는 기능들입니다.

- 우측 상단의 Image 버튼을 누르면, 자신이 현재 보고 있는 장면이 BuildFile/part_Unity_Data 폴더 속에 3dModelCapture 시간-분-초_년월일.jpg 형식으로 파일이 저장됩니다.

- 만들어진 도면의 철제 빔들에 마우스를 가져다 대면, 해당 기둥이나 철제 빔의 정보를 왼쪽 아래에서 확인할 수 있습니다.
 
- 우측 상단의 보고서 버튼을 누르게 되면, 3D 모델링에 들어간 철제 빔들에 대한 3D 모델링에 들어간 철제 빔들의 요약 정보 표, 개수, 소비 예상 비용 등을 문서화하고, 정면도, 평면도, 좌측면도, 우측면도, 배면도, 저면도 등을 이미지로 저장해 문서화하여 pdf파일로 출력이 되게 됩니다. 해당 pdf파일은 BuilFile 폴더 내에 pdfExtraction.pdf 파일로 저장이 되어 확인해볼 수 있습니다.

- 좌클릭을 통해 3D 모델을 기준으로 구조물을 회전하며 둘러볼 수  있습니다.
- 우클릭을 통해 3D 모델을 이동시켜 볼 수 있습니다.
- 마우스 휠을 사용하여 3D 모델을 확대, 축소시켜 볼  수 있습니다.
- F키를 누르게 되면, 이동되었던 카메라가 다시 구조물의 위치로 재조정됩니다.

- 우측 하단의 원근 무시 토글을 껐다 키는 방식으로 모델을 입체감이 들어간 모습, 입체감이 들어가지 않은 모습으로 다양한 모습을 확인해 볼 수 있습니다.

- Load Files 버튼을 누르면, 다른 도면들을 다시 불러 새로운 구조물들을 확인해볼 수 있습니다.
