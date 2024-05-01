using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;    //�ִ�ü��
    private float currentHP;    //����ü��
    private bool isDie = false; //���� ��� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;        //�ܺ�Ŭ�������� ü�������� Ȯ���ϵ��� ������Ƽ ����
    public float CurrentHP => currentHP;

    private void Awake() {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage) {
        //Tip. ���� ü���� damage��ŭ �����ؼ� ���� ��Ȳ�϶� ���� Ÿ���� ������ ���ÿ� ������
        // enemy.OnDie()�� ������ ����� �� ����.

        if (isDie == true) return;  //���� ������¿����� return
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) {   //ü���� 0 ���ϸ� ������·� ó��
            isDie = true;
            enemy.OnDie(EnemyDestroyType.Kill);
        }

    }

    private IEnumerator HitAlphaAnimation() {
        Color color = spriteRenderer.color;
        color.a = 0.4f; //���İ�(����) 40%�� ����
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);

        color.a = 1.0f; //���� 100�۷� ����
        spriteRenderer.color = color;

    }
}
