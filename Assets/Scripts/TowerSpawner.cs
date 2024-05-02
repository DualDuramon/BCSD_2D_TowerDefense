using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate   towerTemplate;
    [SerializeField]
    private EnemySpawner    enemySpawner;      //���� �ʿ� �����ϴ� �� ����Ʈ ����
    [SerializeField]
    private PlayerGold      playerGold;  //Ÿ�� �Ǽ� �� ��� ���Ҹ� ����.
    [SerializeField]
    private SystemTextViewer systemTextViewer;  //�� ����, �Ǽ� �Ұ��� ���� �ý��� �޼��� ���


    public void SpawnTower(Transform tileTransform) {

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� x
        if (towerTemplate.weapon[0].cost > playerGold.CurrentGold) {

            systemTextViewer.PrintText(SystemType.Money);       //��� ������ �� ��� Ÿ�� �Ǽ� �Ұ����� ���
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ �Ǽ� x
        if (tile.IsBuildTower == true) {
            systemTextViewer.PrintText(SystemType.Build);       //������ġ���� Ÿ�� �Ǽ� �Ұ����� ���
            return;
        }

        tile.IsBuildTower = true;   //Ÿ���� �Ǽ��Ǿ� ������ ����.
        playerGold.CurrentGold -= towerTemplate.weapon[0].cost;   //Ÿ�� �Ǽ��� �ʿ��� ��ŭ�� ��� ����
        Vector3 position = tileTransform.position + Vector3.back;       //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ� (Ÿ�Ϻ��� Z�� -1 ��ġ�� ��ġ)
        GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity); //�������� ��ġ�� Ÿ�� ���� �� Ŭ������
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);      //Ÿ�� ���⿡ enemySpawner ����

    }
}
/*
 * Dest
 *  Ÿ�� ���� ����
 */
