using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake() {

        //"MaintCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() 와 동일.
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)){   //좌클릭 시
            //카메라위치에서 화면의 마우스 위치를 관통하는 광선 생성
            // ray.origin : 광선의 시작위치 (=카메라 위치)
            // ray.diriection : 광선의 진행방향
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D 모니터를 향해 3D월드의 오브젝트를 마우스로 선택하는 방법
            // 광선에 부딪힌 3D콜라이더를 가진 오브젝트를 검출해 hit에 저장
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

                if (hit.transform.CompareTag("Tile")) { //태그 비교 및 SpawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);
                }
            }

        }
    }
}
