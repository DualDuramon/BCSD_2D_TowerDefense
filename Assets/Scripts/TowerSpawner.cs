using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildGold = 50;    //Ÿ�� �Ǽ��� ����ϴ� ���
    [SerializeField]
    private EnemySpawner enemySpawner;      //���� �ʿ� �����ϴ� �� ����Ʈ ����
    [SerializeField]
    private PlayerGold playerGold;  //Ÿ�� �Ǽ� �� ��� ���Ҹ� ����.


    public void SpawnTower(Transform tileTransform) {

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� x
        if (towerBuildGold > playerGold.CurrentGold) {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ �Ǽ� x
        if (tile.IsBuildTower == true) {
            return;
        }

        tile.IsBuildTower = true;   //Ÿ���� �Ǽ��Ǿ� ������ ����.
        playerGold.CurrentGold -= towerBuildGold;
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity); //�������� ��ġ�� Ÿ�� ���� �� Ŭ������
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);      //Ÿ�� ���⿡ enemySpawner ����

    }
}
/*
 * Dest
 *  Ÿ�� ���� ����
 */
