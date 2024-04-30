using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject      enemyPrefab;     //적 프리펩
    [SerializeField]
    private float           spawnTime;       // 적 생성 주기
    [SerializeField]
    private Transform[]     wayPoints;      //현재 스테이지의 이동 경로
    private List<Enemy>     enemyList;      //현재 존재하는 모든 적의 정보

    public List<Enemy> EnemyList => enemyList;  //적의 생성 및 삭제는 EnemySpawner에서 하므로 Set 필요 x

    private void Awake() {
        enemyList = new List<Enemy>();      //적 리스트 동적 생성
        StartCoroutine("SpawnEnemy");       //적 생성 코루틴
    }

    private IEnumerator SpawnEnemy() {
        while (true) {
            GameObject  clone = Instantiate(enemyPrefab);       //적 오브젝트 생성
            Enemy       enemy = clone.GetComponent<Enemy>();    //생성한 적의 Enemy 컴포넌트
            
            enemy.Setup(this, wayPoints);     //wayPoints 정보를 매개변수로 Setup()호출
            enemyList.Add(enemy);

            yield return new WaitForSeconds(spawnTime);     //spawnTime 시간동안 대기 
        }
    }

    public void DestroyEnemy(Enemy enemy) {
        enemyList.Remove(enemy);    //리스트에서 사망하는 적 정보 삭제
        Destroy(enemy.gameObject);  //해당 오브젝트 같이 삭제
    }
}
