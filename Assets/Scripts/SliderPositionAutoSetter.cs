using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;
    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target) {
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate() {
        if (targetTransform == null) {  //쫒아다니던 대상이 파괴되 null이되면 이 UI도 삭제
            Destroy(gameObject);
            return;
        }

        //오브젝트의 위치가 갱신된 이후에 Slider UI도 함께 위치를 설정하도록 하기 위해
        // LateUpdate()에서 호출

        //오브젝트의 월드좌표를 기준으로 화면에서의 좌표 값을 구함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance; //화면내 좌표 + distance 만큼 떨어진 위치에 slider UI를 설정
    }
}
