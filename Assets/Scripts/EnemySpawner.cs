using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject      enemyPrefab;     //적 프리펩
    [SerializeField]
    private float           spawnTime;       // 적 생성 주기
    
    [SerializeField]
    private Transform[]     wayPoints;      //현재 스테이지의 이동 경로


    private void Awake() {
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy() {
        while (true) {
            GameObject  clone = Instantiate(enemyPrefab);       //적 오브젝트 생성
            Enemy       enemy = clone.GetComponent<Enemy>();    //생성한 적의 Enemy 컴포넌트
            enemy.Setup(wayPoints);     //wayPoints 정보를 매개변수로 Setup()호출

            yield return new WaitForSeconds(spawnTime);     //spawnTime 시간동안 대기 
        }
    }
}
