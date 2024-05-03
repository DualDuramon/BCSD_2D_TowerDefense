using UnityEngine;

public class Slow : MonoBehaviour
{
    //slow Tower�� ���� ��ũ��Ʈ��.
    private TowerWeapon towerWeapon;

    private void Awake() {
        towerWeapon = GetComponentInParent<TowerWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy")) return;

        Movement2D movement2D = collision.GetComponent<Movement2D>();
        movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;
        //movement2D.MoveSpeed -= movement2D.MoveSpeed * towerWeapon.Slow;    //Ÿ�� ���ӷ� ��ŭ �浹�ѳ� �ӵ� ����.
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag("Enemy")) return;
        collision.GetComponent<Movement2D>().ResetMoveSpeed();
    }
}
