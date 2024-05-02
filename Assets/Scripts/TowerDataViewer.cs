using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image               imageTower;
    [SerializeField]
    private TextMeshProUGUI     textDamage;
    [SerializeField]
    private TextMeshProUGUI     textRate;
    [SerializeField]
    private TextMeshProUGUI     textRange;
    [SerializeField]
    private TextMeshProUGUI     textLevel;
    [SerializeField]
    private TowerAttackRange    towerAttackRange;
    [SerializeField]
    private Button              buttonUpgrade;
    [SerializeField]
    private SystemTextViewer    systemTextViewer;

    private TowerWeapon         currentTower;

    private void Awake() {
        OffPanel();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon) {                    //Ÿ������ panel on
        currentTower = towerWeapon.GetComponent<TowerWeapon>();     //����ؾ��ϴ� Ÿ�� ���� ��������
        gameObject.SetActive(true);     //Ÿ�� �г� on
        UpdateTowerData();              //Ÿ������ ����
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);    //Ÿ�� ������Ʈ �ֺ��� ǥ�õǴ� ���ݹ��� ��������Ʈ On
    }
    public void OffPanel() {    //Ÿ������ panel off
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData() {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;

        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;     //Ÿ���� �����̶� ���׷��̵� �Ұ��� ��ư ��Ȱ��ȭ
    }

    public void OnClickEventTowerUpgrade() {
        bool isSuccess = currentTower.Upgrade();

        if(isSuccess == true) {
            UpdateTowerData();      //Ÿ�� ���׷��̵� �߱⿡ Ÿ�� ������ ������Ʈ
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);    //���ݹ����� ���� ����
        }
        else {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell() {
        currentTower.Sell();    //Ÿ�� �Ǹ�
        OffPanel();     //������ Ÿ���� ����� Panel, ���ݹ��� off
    }
}
