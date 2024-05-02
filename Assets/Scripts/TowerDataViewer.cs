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

    public void OnPanel(Transform towerWeapon) {                    //타워정보 panel on
        currentTower = towerWeapon.GetComponent<TowerWeapon>();     //출력해야하는 타워 정보 가져오기
        gameObject.SetActive(true);     //타워 패널 on
        UpdateTowerData();              //타워정보 갱신
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);    //타워 오브젝트 주변에 표시되는 공격범위 스프라이트 On
    }
    public void OffPanel() {    //타워정보 panel off
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData() {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;

        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;     //타워가 만랩이라 업그레이드 불가시 버튼 비활성화
    }

    public void OnClickEventTowerUpgrade() {
        bool isSuccess = currentTower.Upgrade();

        if(isSuccess == true) {
            UpdateTowerData();      //타워 업그레이드 했기에 타워 데이터 업데이트
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);    //공격범위도 같이 갱신
        }
        else {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell() {
        currentTower.Sell();    //타워 판매
        OffPanel();     //선택한 타워가 사라져 Panel, 공격범위 off
    }
}
