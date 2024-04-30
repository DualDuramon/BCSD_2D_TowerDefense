using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner;      //���� �ʿ� �����ϴ� �� ����Ʈ ����

    public void SpawnTower(Transform tileTransform) {
        Tile tile = tileTransform.GetComponent<Tile>();

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ �Ǽ� x
        if (tile.IsBuildTower == true) {
            return;
        }

        tile.IsBuildTower = true;   //Ÿ���� �Ǽ��Ǿ� ������ ����.
        GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity); //�������� ��ġ�� Ÿ�� ���� �� Ŭ������
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);      //Ÿ�� ���⿡ enemySpawner ����

    }
}
/*
 * Dest
 *  Ÿ�� ���� ����
 */
