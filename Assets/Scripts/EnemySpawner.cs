using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject      enemyPrefab;     //�� ������
    [SerializeField]
    private GameObject      enemyHPSliderPrefab;     //���� ü���� ��Ÿ���� SliderUI������
    [SerializeField]
    private Transform       canvasTransform;      //UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    [SerializeField]
    private float           spawnTime;       // �� ���� �ֱ�
    [SerializeField]
    private Transform[]     wayPoints;      //���� ���������� �̵� ���
    [SerializeField]
    private PlayerHP        playerHP;       //�÷��̾��� ü�� ������Ʈ
    [SerializeField]
    private PlayerGold      playerGold;     //�÷��̾��� ��� ������Ʈ
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

            SpawnEnemyHPSlider(clone);  //�� ü�� UI�� ���� �� �����ϴ� �Լ�
            yield return new WaitForSeconds(spawnTime);     //spawnTime �ð����� ��� 
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold) {
        if(type == EnemyDestroyType.Arrive) {
            playerHP.TakeDamage(1);     //���� ���������� ���������� ü�� -1
        }
        else if(type == EnemyDestroyType.Kill) {
            playerGold.CurrentGold += gold;
        }
        
        enemyList.Remove(enemy);    //����Ʈ���� ����ϴ� �� ���� ����
        Destroy(enemy.gameObject);  //�ش� ������Ʈ ���� ����
    }

    private void SpawnEnemyHPSlider(GameObject enemy) {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);  //ü�� �����̴� UI ����
        sliderClone.transform.SetParent(canvasTransform); //Slider UI�� Canvas ������Ʈ�� �ڽ����� ���� -> ĵ������ �ڽ����� �־�� ȭ�鿡 UI�� ����.
        sliderClone.transform.localScale = Vector3.one; //������������ �ٲ� scale�� (1,1,1)�� �ٲ�

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);    //slider UI�� ����ٴ� ����� �������� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>()); //slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����.
    }
}
