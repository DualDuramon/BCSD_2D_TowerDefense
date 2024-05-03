using System.Collections;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[]   towerTemplate;            //Ÿ������
    [SerializeField]
    private EnemySpawner    enemySpawner;               //���� �ʿ� �����ϴ� �� ����Ʈ ����
    [SerializeField]
    private PlayerGold      playerGold;                 //Ÿ�� �Ǽ� �� ��� ���Ҹ� ����.
    [SerializeField]
    private SystemTextViewer systemTextViewer;          //�� ����, �Ǽ� �Ұ��� ���� �ý��� �޼��� ���
    private bool             isOnTowerButton = false;   //Ÿ�� �Ǽ� ��ư �������� üũ
    private GameObject       followTowerClone = null;   //�ӽ�Ÿ�� ��� �Ϸ� �� ������ ���� �ӽ�Ÿ���� �����ϴ� ����
    private int              towerType;                 //Ÿ���Ӽ�


    public void ReadyToSpawnTower(int type) {           //Ÿ�� �Ǽ� ���� Ȯ��

        towerType = type;
         
        if (isOnTowerButton == true) return;    //�ߺ�Ŭ�� ����

        //Ÿ���� �Ǽ��� ��ŭ �� ������ �Ǽ� X
        if (towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold) {
            systemTextViewer.PrintText(SystemType.Money);                   //��尡 ������ Ÿ�� �Ǽ� �Ұ����ϴٸ� ���
            return;
        }

        isOnTowerButton = true;                                             //Ÿ�� �Ǽ� ��ư�� �����ٰ� ����
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);    //���콺�� ����ٴϴ� �ӽ� Ÿ�� ����

        StartCoroutine("OnTowerCancelSystem");                              //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
    }
    
    public void SpawnTower(Transform tileTransform) {

        if (isOnTowerButton == false) {                                     //Ÿ�� �Ǽ� ��ư�� ������ ���� Ÿ�� �Ǽ� ����
            return;
        }

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �Ǽ��Ǿ� ������ �Ǽ� x
        if (tile.IsBuildTower == true) {
            systemTextViewer.PrintText(SystemType.Build);       //������ġ���� Ÿ�� �Ǽ� �Ұ����� ���
            return;
        }
        isOnTowerButton = false;                                            //�ٽ� Ÿ�� �Ǽ� ��ư�� ���� Ÿ���� �Ǽ��ϵ��� ���� ����

        tile.IsBuildTower = true;                                           //Ÿ���� �Ǽ��Ǿ� ������ ����.
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;             //Ÿ�� �Ǽ��� �ʿ��� ��ŭ�� ��� ����
        Vector3 position = tileTransform.position + Vector3.back;           //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ� (Ÿ�Ϻ��� Z�� -1 ��ġ�� ��ġ)
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity); //�������� ��ġ�� Ÿ�� ���� �� Ŭ������
        clone.GetComponent<TowerWeapon>().Setup(this, enemySpawner, playerGold, tile);      //Ÿ�� ���⿡ TowerSpawner, enemySpawner, playerGold, tile ����

        OnBuffAllBuffTowers();      //���� ��ġ�Ǵ� Ÿ���� ����Ÿ�� �ֺ��� ��ġ�Ǹ� ����ȿ���� ���� �� �ֵ��� ��� ���� Ÿ���� ����ȿ�� ����.

        Destroy(followTowerClone);                                          //Ÿ�� ��ġ �� ���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        StopCoroutine("OnTowerCancelSystem");                               //Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����.
    }

    private IEnumerator OnTowerCancelSystem() {
        while (true) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {  //escŰ Ŭ�� �Ǵ� �׳� ��Ŭ���� Ÿ�� �Ǽ� ���
                isOnTowerButton = false;
                Destroy(followTowerClone);  //���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
                break;
            }

            yield return null;
        }
    }

    public void OnBuffAllBuffTowers() { //�� ���� ��� ����Ÿ���� ����ȿ���� �����ϴ� �Լ�.
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i = 0; i < towers.Length; i++) {
            TowerWeapon weapon = towers[i].GetComponentInParent<TowerWeapon>();

            if(weapon.WeaponType == WeaponType.Buff) {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
/*
 * Dest
 *  Ÿ�� ���� ����
 */
