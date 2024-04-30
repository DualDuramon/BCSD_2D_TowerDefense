using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.0f;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    public float MoveSpeed => moveSpeed;       //moveSpeed 변수의 프로퍼티임.

    private void Update() {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction) {
        moveDirection = direction;
    }
}
/*
 * Desc
 *  : 이동이 가능한 모든 오브젝트에게 부착
 *  
 *  Function 
 *   MoveTo() - 외부에서 호출해 이동방향 설정
 *   
 */