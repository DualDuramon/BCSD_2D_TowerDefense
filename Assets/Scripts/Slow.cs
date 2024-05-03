using UnityEngine;

public class Slow : MonoBehaviour
{
    //slow Tower를 위한 스크립트임.
    private TowerWeapon towerWeapon;

    private void Awake() {
        towerWeapon = GetComponentInParent<TowerWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy")) return;

        Movement2D movement2D = collision.GetComponent<Movement2D>();
        movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;
        //movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;    //타워 감속률 만큼 충돌한놈 속도 감소.
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy")) return;
        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
