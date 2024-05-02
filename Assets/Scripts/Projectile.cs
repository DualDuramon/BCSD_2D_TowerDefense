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
        this.target = target;       //Ÿ���� �������� target
        this.damage = damage;
    }

    private void Update() {
        if (target != null) {   //target�� �����ϸ�
            //�߻�ü�� target�� ��ġ�� ���󰡰���.
            Vector3 direction = (target.position - transform.position).normalized;      //�̵����� ����
            movement2D.MoveTo(direction);
        }
        else {  //target�� ������� �ش� �߻�ü ���� ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy")) return;     //�� �ƴѰ� �ε�����
        if (collision.transform != target) return;      // ���� target�� �ƴ� ���� �ε�����

        //collision.GetComponent<Enemy>().OnDie();    //�� ��� �Լ� ȣ��
        collision.GetComponent<EnemyHP>().TakeDamage(damage);   //�� ü�� ��������ŭ����
        Destroy(gameObject);                        // �߻�ü ������Ʈ ����
    }
}

/*
 * Dest
 *  : Ÿ���� �߻��ϴ� �⺻ �߻�ü�� ����
 *  
 *  function
 *      update() : Ÿ���� �����ϸ� Ÿ�ٹ������� �̵�, Ÿ���� ���� ���ϸ� �߻�ü ����
 *      OnTriggerEnter2D() : Ÿ������ ������ ���� �ε������� �Ѵ� ����
 */
