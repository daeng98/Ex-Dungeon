# **스파르타 던전 탐험 만들기**

이번 과제는 강의를 통해 Unity3D에 대해 학습하고 
Unity3D의 캐릭터 이동과 물리 처리를 직접 구현하는 과제를 만들어 보겠습니다.


**이번에도 역시나 필수 기능 구현을 목표로 하였습니다**


# 구현된 필수 기능
 
### 1. 기본 이동 및 점프
- InputSystem 에셋을 사용하여서 이동과 점프 등 입력처리를 구현함
- 이동 : WASD  점프 : Space  달리기 : Shift  상호작용 : F  아이템사용 : E, R
- Rigidbody ForceMode를 활용하여 이동 점프 달리기를 구현함
- 인벤토리 기능을 사용하라는 가이드가 없었기에 슬롯만 만들고 키 입력 시 해당 아이템을 사용하게 구현함

![이동](https://github.com/user-attachments/assets/b43c001f-6ece-4543-80e1-1c2602754d83)
![점프](https://github.com/user-attachments/assets/45f1f4ac-52f5-4496-ac3d-0f2f86b09be3)


### 2. 체력바 UI
- UI에 체력을 추가하고 플레이어의 체력을 나타내도록 설정함
- 시간이 지남에 따라 체력이 깍이도록 설정

![image](https://github.com/user-attachments/assets/38f598fb-5ce6-49e2-b173-c904facfbb64)


### 3. 동적 환경 조사
- Raycast를 통해서 플레이어가 아이템의 정보를 확인 할 수 있게함
- 3인칭으로 변경 함에 따라 Raycast > OnTriggerEnter 변경
- 트리거 되면 아이템의 정보 확인 가능 (키 입력으로 획득)

![상호작용](https://github.com/user-attachments/assets/3e93fdf7-82b3-4543-ad7f-c17a70ff5560)


### 4. 점프대 
- 캐릭터가 밟을 때 위로 높이 튀어 오르는 점프대 구현
- ForceMode를 사용하였고 Impulse로 힘을 한번에 빡 줘서 점프 시켰음
- 해당 오브젝트 OnTriggerEnter를 통해 플레이어를 점프 시킴

![제목-없는-동영상-Clipchamp로-제작-_1_](https://github.com/user-attachments/assets/b40b51da-b67e-4f38-b62b-7801c98cbc3c)


### 5. 아이템 데이터
- ScriptableObject를 통해 아이템 데이터 관리
- 이름, 설명, 속성 등을 관리할 수 있음
- 강의에서 사용한 방식을 따라서 사용했음

![image](https://github.com/user-attachments/assets/cb3735af-13b4-4e10-8ca9-7989e44fdaec)


### 6. 아이템 사용
- 체력 회복, 속도 증가 포션을 구현
- 플레이어의 체력을 일정량 회복 시켜줌
- 코루틴을 통해 지속시간 동안만 속도를 증가 시켜줌

![image](https://github.com/user-attachments/assets/393a6e26-ef92-4f54-8350-3a1a06805d89)


# 구현된 추가 기능


### 1. 추가 UI
- 스태미나를 추가하고 달리기, 점프 시 소비하게 했음
- 간단한 타이머를 추가함

### 2. 3인칭 시점
- 1인칭 시점으로 만들었다가 3인칭 시점으로 변경함
- 그에 맞게 이동과 회전도 변경했음
- 추가로 줌 인, 줌 아웃도 구현함

![제목-없는-동영상-Clipchamp로-제작](https://github.com/user-attachments/assets/851e75d1-8f69-4251-950c-3a90de7e5e79)

