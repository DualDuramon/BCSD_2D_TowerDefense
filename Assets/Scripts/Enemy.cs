using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int             wayPointCount;      //�̵� ��� ����
    private Transform[]     wayPoints;          //�̵� ��� ����
    private int             currentIdx = 0;     // ���� ��ǥ���� �ε���
    private Movement2D      movement2D;         // ������Ʈ �̵� ����

    public void Setup(Transform[] wayPoints) {
        movement2D = GetComponent<Movement2D>();

        //�� �̵� ��� WayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        //���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIdx].position;

        //�� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove() {
        NextMoveTo();

        while (true) {
            //�� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);

            //���� ������ġ -> ��ǥ��ġ �Ÿ��� 0.02f * movement~~ ���� ������ if ����
            // movement2D.MoveSpeed�� �����ִ� ���� : �ӵ��� �ʹ� ������ �� �����ӿ� 0.02f����ũ�� ������ -> if���ǹ��� �Ȱɸ��� ��� Ż���ϴ� ������Ʈ �߻�
            if (Vector3.Distance(transform.position, wayPoints[currentIdx].position) < 0.02f * movement2D.MoveSpeed) {
                
                NextMoveTo();   //���� �̵����� ����
            }

            yield return null;
        }

    }

    private void NextMoveTo() {

        if (currentIdx < wayPointCount - 1) {   //�̵������� wayPoints�� ����������
            transform.position = wayPoints[currentIdx].position;        //�̵����� ���� -> ���� ��ǥ����(wayPoints)��
            currentIdx++;
            Vector3 direction = (wayPoints[currentIdx].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else {  //������ ��ǥ���� (wayPoints)�� ����
            Destroy(gameObject);
        }
    }
}

/*
 * 
 */