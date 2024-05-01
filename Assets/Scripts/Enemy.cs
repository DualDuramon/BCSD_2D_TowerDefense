using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDestroyType { Kill = 0, Arrive }

public class Enemy : MonoBehaviour
{
    private int             wayPointCount;      //이동 경로 개수
    private Transform[]     wayPoints;          //이동 경로 정보
    private int             currentIdx = 0;     // 현재 목표지점 인덱스
    private Movement2D      movement2D;         // 오브젝트 이동 제어
    private EnemySpawner    enemySpawner;          // 적을 삭제 할 때 EnemySpawner에 알려서 삭제함
    [SerializeField]
    private int gold = 10;  //적 사망시 획득 가능한 골드

    public void Setup(EnemySpawner enemySpawner , Transform[] wayPoints) {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //적 이동 경로 WayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //적의 위치를 첫번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIdx].position;

        //적 이동/목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove() {
        NextMoveTo();

        while (true) {
            //적 오브젝트 회전
            transform.Rotate(Vector3.forward * 10);

            //적의 현재위치 -> 목표위치 거리가 0.02f * movement~~ 보다 작을때 if 실행
            // movement2D.MoveSpeed를 곱해주는 이유 : 속도가 너무 빠르면 한 프레임에 0.02f보다크게 움직임 -> if조건문에 안걸리고 경로 탈주하는 오브젝트 발생
            if (Vector3.Distance(transform.position, wayPoints[currentIdx].position) < 0.02f * movement2D.MoveSpeed) {
                
                NextMoveTo();   //다음 이동방향 설정
            }

            yield return null;
        }

    }

    private void NextMoveTo() {

        if (currentIdx < wayPointCount - 1) {   //이동가능한 wayPoints가 남아있으면
            transform.position = wayPoints[currentIdx].position;        //이동방향 설정 -> 다음 목표지점(wayPoints)로
            currentIdx++;
            Vector3 direction = (wayPoints[currentIdx].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else {  //마지막 목표지점 (wayPoints)면 삭제
            //Destroy(gameObject);
            gold = 0;   //goal에 도착해서 죽는거면 돈 안주도록
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type) {
        //본인이 삭제될 때 enemySpawner에게 자신을 넘겨 삭제함.
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}

/*
 * 
 */