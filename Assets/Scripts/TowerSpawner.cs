using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner;      //현재 맵에 존재하는 적 리스트 정보

    public void SpawnTower(Transform tileTransform) {
        Tile tile = tileTransform.GetComponent<Tile>();

        // 타워 건설 가능 여부 확인
        // 1. 현재 타일의 위치에 이미 타워가 건설되어 있으면 건설 x
        if (tile.IsBuildTower == true) {
            return;
        }

        tile.IsBuildTower = true;   //타워가 건설되어 있음을 설정.
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity); //제공받은 위치에 타일 생성 및 클론저장
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);      //타워 무기에 enemySpawner 전달

    }
}
/*
 * Dest
 *  타워 생성 제어
 */
