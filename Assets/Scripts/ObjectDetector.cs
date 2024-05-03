using UnityEngine.EventSystems;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;  //마우스 픽킹으로 선택한 오브젝트 임시 저장

    private void Awake() {

        //"MaintCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() 와 동일.
        mainCamera = Camera.main;
    }

    private void Update() {

        if (EventSystem.current.IsPointerOverGameObject() == true) //마우스가 UI에 머물러 있을 떈 아래 코드 실행 안되게 함.
            return;

        if (Input.GetMouseButtonDown(0)) {   //좌클릭 시
            //카메라위치에서 화면의 마우스 위치를 관통하는 광선 생성
            // ray.origin : 광선의 시작위치 (=카메라 위치)
            // ray.diriection : 광선의 진행방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D 모니터를 향해 3D월드의 오브젝트를 마우스로 선택하는 방법
            // 광선에 부딪힌 3D콜라이더를 가진 오브젝트를 검출해 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

                hitTransform = hit.transform;

                if (hit.transform.CompareTag("Tile")) { //태그 비교 및 SpawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower")) {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            //마우스 눌렀다 땠을 때 선택한 오브젝트가 없거나 그 오브젝트가 Tower이면
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false) {
                towerDataViewer.OffPanel(); //타워정보 패널 비활성화. 즉 빈 공간 선택시 UI 종료
            }
            hitTransform = null;
        }
    }
}
