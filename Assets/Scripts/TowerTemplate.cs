using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;      //타워 프리팹
    public GameObject followTowerPrefab;    //임시타워프리팹
    public Weapon[] weapon;             //레벨별 타워 (무기) 정보

    [System.Serializable]
    public struct Weapon {
        public Sprite sprite;   //타워 이미지(UI)
        public float damage;    // 공격력
        public float slow;      //감속퍼센트(0.2 = 20%)
        public float rate;      //공격속도
        public float range;     //공격범위
        public int cost;        //필요 골드(0레벨 : 건설 / 1~레벨 : 업그레이드)
        public int sell;         //타워 판매시 획득 골드
    }
}