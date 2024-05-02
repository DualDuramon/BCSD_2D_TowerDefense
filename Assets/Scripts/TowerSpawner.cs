using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate   towerTemplate;
    [SerializeField]
    private EnemySpawner    enemySpawner;      //현재 맵에 존재하는 적 리스트 정보
    [SerializeField]
    private PlayerGold      playerGold;  //타워 건설 시 골드 감소를 위함.
    [SerializeField]
    private SystemTextViewer systemTextViewer;  //돈 부족, 건설 불가와 같은 시스템 메세지 출력


    public void SpawnTower(Transform tileTransform) {

        // 타워 건설 가능 여부 확인
        // 1. 타워를 건설할 만큼 돈이 없으면 타워 건설 x
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold) {

            systemTextViewer.PrintText(SystemType.Money);       //골드 부족시 돈 없어서 타워 건설 불가함을 출력
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 건설 x
        if (tile.IsBuildTower == true) {
            systemTextViewer.PrintText(SystemType.Build);       //현재위치에서 타워 건설 불가함을 출력
            return;
        }

        tile.IsBuildTower = true;   //타워가 건설되어 있음을 설정.
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;   //타워 건설에 필요한 만큼의 골드 감소
        Vector3 position = tileTransform.position + Vector3.back;       //선택한 타일의 위치에 타워 건설 (타일보다 Z축 -1 위치에 배치)
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity); //제공받은 위치에 타일 생성 및 클론저장
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);      //타워 무기에 enemySpawner 전달

    }
}
/*
 * Dest
 *  타워 생성 제어
 */
