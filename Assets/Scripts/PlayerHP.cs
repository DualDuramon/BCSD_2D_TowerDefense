using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;  //�ǰݽ� ǥ���Ǵ� �������
    [SerializeField]
    private float maxHP = 20;   //�ִ�ü��
    private float currentHP;    //���� ü��

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake() {
        currentHP = maxHP;  //���� �� �ִ�ü������ �ڽ��� ü�� ����
    }

    public void TakeDamage(float damage) {
        currentHP -= damage;    //ü�°���
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) {
            //ü���� 0�� �Ǹ� ���� ����

        }
    }

    private IEnumerator HitAlphaAnimation() {
        //imageScreen �� ������ 40%�� ����
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        while (color.a >= 0.0f) {   //������ 0�� �� �� ���� ����
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}
