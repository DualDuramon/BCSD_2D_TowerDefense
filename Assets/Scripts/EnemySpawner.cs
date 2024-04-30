using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject      enemyPrefab;     //�� ������
    [SerializeField]
    private float           spawnTime;       // �� ���� �ֱ�
    [SerializeField]
    private Transform[]     wayPoints;      //���� ���������� �̵� ���
    private List<Enemy>     enemyList;      //���� �����ϴ� ��� ���� ����

    public List<Enemy> EnemyList => enemyList;  //���� ���� �� ������ EnemySpawner���� �ϹǷ� Set �ʿ� x

    private void Awake() {
        enemyList = new List<Enemy>();      //�� ����Ʈ ���� ����
        StartCoroutine("SpawnEnemy");       //�� ���� �ڷ�ƾ
    }

    private IEnumerator SpawnEnemy() {
        while (true) {
            GameObject  clone = Instantiate(enemyPrefab);       //�� ������Ʈ ����
            Enemy       enemy = clone.GetComponent<Enemy>();    //������ ���� Enemy ������Ʈ
            
            enemy.Setup(this, wayPoints);     //wayPoints ������ �Ű������� Setup()ȣ��
            enemyList.Add(enemy);

            yield return new WaitForSeconds(spawnTime);     //spawnTime �ð����� ��� 
        }
    }

    public void DestroyEnemy(Enemy enemy) {
        enemyList.Remove(enemy);    //����Ʈ���� ����ϴ� �� ���� ����
        Destroy(enemy.gameObject);  //�ش� ������Ʈ ���� ����
    }
}
