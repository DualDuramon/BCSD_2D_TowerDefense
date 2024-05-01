using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject      enemyPrefab;     //적 프리펩
    [SerializeField]
    private GameObject      enemyHPSliderPrefab;     //적의 체력을 나타내는 SliderUI프리펩
    [SerializeField]
    private Transform       canvasTransform;      //UI를 표현하는 Canvas 오브젝트의 Transform
    [SerializeField]
    private float           spawnTime;       // 적 생성 주기
    [SerializeField]
    private Transform[]     wayPoints;      //현재 스테이지의 이동 경로
    [SerializeField]
    private PlayerHP        playerHP;       //플레이어의 체력 컴포넌트
    [SerializeField]
    private PlayerGold      playerGold;     //플레이어의 골드 컴포넌트
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

            SpawnEnemyHPSlider(clone);  //적 체력 UI를 생성 및 설정하는 함수
            yield return new WaitForSeconds(spawnTime);     //spawnTime 시간동안 대기 
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int gold) {
        if(type == EnemyDestroyType.Arrive) {
            playerHP.TakeDamage(1);     //적이 골인지점에 도착했으면 체력 -1
        }
        else if(type == EnemyDestroyType.Kill) {
            playerGold.CurrentGold += gold;
        }
        
        enemyList.Remove(enemy);    //리스트에서 사망하는 적 정보 삭제
        Destroy(enemy.gameObject);  //해당 오브젝트 같이 삭제
    }

    private void SpawnEnemyHPSlider(GameObject enemy) {
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);  //체력 슬라이더 UI 생성
        sliderClone.transform.SetParent(canvasTransform); //Slider UI를 Canvas 오브젝트의 자식으로 설정 -> 캔버스의 자식으로 있어야 화면에 UI로 보임.
        sliderClone.transform.localScale = Vector3.one; //계층설정으로 바뀐 scale를 (1,1,1)로 바꿈

        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);    //slider UI가 따라다닐 대상을 본인으로 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>()); //slider UI가 자신의 체력 정보를 표시하도록 설정.
    }
}
