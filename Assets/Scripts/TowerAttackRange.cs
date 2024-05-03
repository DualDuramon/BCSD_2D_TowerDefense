using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{

    public void OnAttackRange(Vector3 position, float range) {
        gameObject.SetActive(true);

        //공격범위크기 (range자체는 반지름이므로 2 곱해줌)
        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        //공격범위 배치
        transform.position = position;
    }

    public void OffAttackRange() {
        gameObject.SetActive(false);
    }
}
