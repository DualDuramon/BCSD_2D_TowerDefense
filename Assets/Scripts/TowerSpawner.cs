using System.Collections;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[]   towerTemplate;            //타워정보
    [SerializeField]
    private EnemySpawner    enemySpawner;               //현재 맵에 존재하는 적 리스트 정보
    [SerializeField]
    private PlayerGold      playerGold;                 //타워 건설 시 골드 감소를 위함.
    [SerializeField]
    private SystemTextViewer systemTextViewer;          //돈 부족, 건설 불가와 같은 시스템 메세지 출력
    private bool             isOnTowerButton = false;   //타워 건설 버튼 눌렀는지 체크
    private GameObject       followTowerClone = null;   //임시타워 사용 완료 시 삭제를 위해 임시타워를 저장하는 변수
    private int              towerType;                 //타워속성


    public void ReadyToSpawnTower(int type) {           //타워 건설 여부 확인

        towerType = type;
         
        if (isOnTowerButton == true) return;    //중복클릭 방지

        //타워를 건설할 만큼 돈 없으면 건설 X
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold) {
            systemTextViewer.PrintText(SystemType.Money);                   //골드가 부족해 타워 건설 불가능하다를 출력
            return;
        }

        isOnTowerButton = true;                                             //타워 건설 버튼을 눌렀다고 설정
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);    //마우스를 따라다니는 임시 타워 생성

        StartCoroutine("OnTowerCancelSystem");                              //타워 건설을 취소할 수 있는 코루틴 함수 시작
    }
    
    public void SpawnTower(Transform tileTransform) {

        if (isOnTowerButton == false) {                                     //타워 건설 버튼을 눌렀을 때만 타워 건설 가능
            return;
        }

        // 타워 건설 가능 여부 확인
        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 건설 x
        if (tile.IsBuildTower == true) {
            systemTextViewer.PrintText(SystemType.Build);       //현재위치에서 타워 건설 불가함을 출력
            return;
        }
        isOnTowerButton = false;                                            //다시 타워 건설 버튼을 눌러 타워를 건설하도록 변수 설정

        tile.IsBuildTower = true;                                           //타워가 건설되어 있음을 설정.
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;             //타워 건설에 필요한 만큼의 골드 감소
        Vector3 position = tileTransform.position + Vector3.back;           //선택한 타일의 위치에 타워 건설 (타일보다 Z축 -1 위치에 배치)
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity); //제공받은 위치에 타일 생성 및 클론저장
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);      //타워 무기에 enemySpawner 전달

        Destroy(followTowerClone);                                          //타워 배치 후 마우스를 따라다니는 임시 타워 삭제
        StopCoroutine("OnTowerCancelSystem");                               //타워 건설을 취소할 수 있는 코루틴 함수 중지.
    }

    private IEnumerator OnTowerCancelSystem() {
        while (true) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {  //esc키 클릭 또는 그냥 우클릭시 타워 건설 취소
                isOnTowerButton = false;
                Destroy(followTowerClone);  //마우스를 따라다니는 임시 타워 삭제
                break;
            }

            yield return null;
        }
    }
}
/*
 * Dest
 *  타워 생성 제어
 */
