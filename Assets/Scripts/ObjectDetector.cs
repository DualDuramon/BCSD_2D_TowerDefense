using UnityEngine.EventSystems;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;  //���콺 ��ŷ���� ������ ������Ʈ �ӽ� ����

    private void Awake() {

        //"MaintCamera" �±׸� ������ �ִ� ������Ʈ Ž�� �� Camera ������Ʈ ���� ����
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() �� ����.
        mainCamera = Camera.main;
    }

    private void Update() {

        if (EventSystem.current.IsPointerOverGameObject() == true) //���콺�� UI�� �ӹ��� ���� �� �Ʒ� �ڵ� ���� �ȵǰ� ��.
            return;

        if (Input.GetMouseButtonDown(0)) {   //��Ŭ�� ��
            //ī�޶���ġ���� ȭ���� ���콺 ��ġ�� �����ϴ� ���� ����
            // ray.origin : ������ ������ġ (=ī�޶� ��ġ)
            // ray.diriection : ������ �������
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D ����͸� ���� 3D������ ������Ʈ�� ���콺�� �����ϴ� ���
            // ������ �ε��� 3D�ݶ��̴��� ���� ������Ʈ�� ������ hit�� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {

                hitTransform = hit.transform;

                if (hit.transform.CompareTag("Tile")) { //�±� �� �� SpawnTower() ȣ��
                    towerSpawner.SpawnTower(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower")) {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            //���콺 ������ ���� �� ������ ������Ʈ�� ���ų� �� ������Ʈ�� Tower�̸�
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false) {
                towerDataViewer.OffPanel(); //Ÿ������ �г� ��Ȱ��ȭ. �� �� ���� ���ý� UI ����
            }
            hitTransform = null;
        }
    }
}
