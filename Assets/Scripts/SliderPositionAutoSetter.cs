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
        if (targetTransform == null) {  //�i�ƴٴϴ� ����� �ı��� null�̵Ǹ� �� UI�� ����
            Destroy(gameObject);
            return;
        }

        //������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider UI�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        // LateUpdate()���� ȣ��

        //������Ʈ�� ������ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPosition + distance; //ȭ�鳻 ��ǥ + distance ��ŭ ������ ��ġ�� slider UI�� ����
    }
}
