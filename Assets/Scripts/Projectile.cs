using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;

    public void Setup(Transform target, float damage) {
        movement2D = GetComponent<Movement2D>();
        this.target = target;       //타워가 설정해준 target
        this.damage = damage;
    }

    private void Update() {
        if (target != null) {   //target이 존재하면
            //발사체를 target의 위치로 날라가게함.
            Vector3 direction = (target.position - transform.position).normalized;      //이동방향 설정
            movement2D.MoveTo(direction);
        }
        else {  //target이 사라지면 해당 발사체 같이 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy")) return;     //적 아닌거 부딪힐때
        if (collision.transform != target) return;      // 현재 target이 아닌 적과 부딪힐때

        //collision.GetComponent<Enemy>().OnDie();    //적 사망 함수 호출
        collision.GetComponent<EnemyHP>().TakeDamage(damage);   //적 체력 데미지만큼감소
        Destroy(gameObject);                        // 발사체 오브젝트 삭제
    }
}

/*
 * Dest
 *  : 타워가 발사하는 기본 발사체에 부착
 *  
 *  function
 *      update() : 타겟이 존재하면 타겟방향으로 이동, 타겟이 존재 안하면 발사체 삭제
 *      OnTriggerEnter2D() : 타겟으로 설정한 적과 부딪혔으면 둘다 삭제
 */
