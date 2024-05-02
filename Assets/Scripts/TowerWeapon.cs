using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate   towerTemplate;
    [SerializeField]
    private GameObject      projectilePrefab;   //발사체 프리팹
    [SerializeField]
    private Transform       spawnPoint;         //발사체 생성 위치

    private int             level = 0;
    private WeaponState     weaponeState = WeaponState.SearchTarget;    //타워 무기의 상태
    private Transform       attackTarget = null;    //공격대상
    private SpriteRenderer  spriteRenderer;     //타워 오브젝트 이미지 변경용
    private EnemySpawner    enemySpawner;           // 게임에 존재하는 적 정보 획득용
    private PlayerGold      playerGold;         //플레이어의 골드 정보 획득 및 설정
    private Tile            ownerTile;          //현재 타워가 배치되어 있는 타일

    public Sprite TowerSprite   => towerTemplate.weapon[level].sprite;
    public float Damage         => towerTemplate.weapon[level].damage;
    public float Rate           => towerTemplate.weapon[level].rate;
    public float Range          => towerTemplate.weapon[level].range;
    public int Level            => level + 1;
    public int MaxLevel         => towerTemplate.weapon.Length;

    public void Setup(EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile) {

        spriteRenderer = GetComponent<SpriteRenderer>();
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;
        ChangeState(WeaponState.SearchTarget);  //최초 상태를 SearchTarget으로 변경
    }

    public void ChangeState(WeaponState newState) {
        StopCoroutine(weaponeState.ToString());     //이전에 재생중이던 상태 종료
        weaponeState = newState;
        StartCoroutine(weaponeState.ToString());
    }

    private void Update() {
        if (attackTarget != null) {
            RotateToTarget();
        }
    }

    private void RotateToTarget() { //Target을 바라보는 함수
        //원점으로부터의 거리(변위),수평축으로부터의 각도를 이용해 위치를 구하는 극 좌표계 이용
        //각도 arctan(y/x)
        //변위값 구하기
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;     //dy/dx로 각도 구한 후 rad단위를 degree로 변환
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget() {
        while (true) {
            float closestDistSqr = Mathf.Infinity;  //제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
            
            for (int i = 0; i < enemySpawner.EnemyList.Count; i++) {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                //모든 적을 전수조사하는 중에 적과의 거리가 공격범위 내에 있고 검사한 적보다 더 거리가 가까우면
                if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr) {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }

            if(attackTarget != null) {
                ChangeState(WeaponState.AttackToTarget);

            }

            yield return null;
        }
    }

    private IEnumerator AttackToTarget() {
        while (true) {
            //1.target이 있는지 검사 (다른 발사체에 의한 제거, goal에 도착해 삭제 등)
            if (attackTarget == null) {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 2. target이 공격범위 안에 있는지 검사. (없으면 새로운 적 탐색)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance > towerTemplate.weapon[level].range) {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 3. attackRate 시간만큼 대기
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. 공격중 (발사체 생성)
            SpawnProjectile();
        }
    }

    private void SpawnProjectile() {    //발사체 생성
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);   //생성된 발사체에게 공격대상 정보 제공
    }

    public bool Upgrade() {
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost) {
            return false;
        }

        level++;        //타워 레벨 증가
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;     //타워 외형 변경
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;     //골드 차감

        return true;
    }

    public void Sell() {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;      //골드 증가
        ownerTile.IsBuildTower = false;     //현재 타일에 다시 타워가 건설 될 수 있도록 설정
        Destroy(gameObject);    //타워파괴
    }
}
