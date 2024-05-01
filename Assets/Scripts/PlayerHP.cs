using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;  //피격시 표현되는 빨간배경
    [SerializeField]
    private float maxHP = 20;   //최대체력
    private float currentHP;    //현재 체력

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake() {
        currentHP = maxHP;  //시작 시 최대체력으로 자신의 체력 설정
    }

    public void TakeDamage(float damage) {
        currentHP -= damage;    //체력감소
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0) {
            //체력이 0이 되면 게임 오버

        }
    }

    private IEnumerator HitAlphaAnimation() {
        //imageScreen 의 투명도를 40%로 설정
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        while (color.a >= 0.0f) {   //투명도가 0이 될 때 가지 감소
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}
