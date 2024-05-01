using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;    //최대체력
    private float currentHP;    //현재체력
    private bool isDie = false; //적의 사망 여부
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;        //외부클레스에서 체력정보를 확인하도록 프로퍼티 생성
    public float CurrentHP => currentHP;

    private void Awake() {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage) {
        //Tip. 적의 체력이 damage만큼 감소해서 죽을 상황일때 여러 타워의 공격을 동시에 받으면
        // enemy.OnDie()가 여러번 실행될 수 있음.

        if (isDie == true) return;  //적이 사망상태였으면 return
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) {   //체력이 0 이하면 사망상태로 처리
            isDie = true;
            enemy.OnDie(EnemyDestroyType.Kill);
        }

    }

    private IEnumerator HitAlphaAnimation() {
        Color color = spriteRenderer.color;
        color.a = 0.4f; //알파값(투명도) 40%로 조정
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);

        color.a = 1.0f; //투명도 100퍼로 조정
        spriteRenderer.color = color;

    }
}
