using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Cannon = 0, Laser, }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser, }

public class TowerWeapon : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate   towerTemplate;
    [SerializeField]
    private Transform spawnPoint;         //발사체 생성 위치
    [SerializeField]
    private WeaponType weaponType;

    [Header("Cannon")]
    [SerializeField]
    private GameObject      projectilePrefab;   //발사체 프리팹

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;      //레이저로 사용되는 선(LineRenderer)
    [SerializeField]
    private Transform hitEffect;            //타격효과
    [SerializeField]
    private LayerMask targetLayer;          //광선에 부딪히는 레이어 설정

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
            attackTarget = FindClosestAttackTarget();       //타워에서 제일 가까운 공격대상들을 탐색
            if (attackTarget != null) {

                if (weaponType == WeaponType.Cannon)            //타워의 무기 상태에 따라 공격 상태를 변경
                    ChangeState(WeaponState.TryAttackCannon);
                else if (weaponType == WeaponType.Laser)
                    ChangeState(WeaponState.TryAttackLaser);
            }

            yield return null;
        }
    }

    private IEnumerator TryAttackCannon() {  //타워가 공격 중인지를 검사하는 함수
        while (true) {
            if (IsPossibleToAttackTarget() == false) {  //타겟을 공격할 수 없으면 새로운 타겟을 탐색.
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);  //타워의 공격 주기만큼 대기
        }
    }

    private IEnumerator TryAttackLaser() {
        EnableLaser();

        while (true) {
            if(IsPossibleToAttackTarget() == false) {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            SpawnLaser();

            yield return null;
        }
    }

    private Transform FindClosestAttackTarget() {       //제일 가까이 있는 적 찾는 함수.
        float closestDistSqr = Mathf.Infinity;

        for (int i = 0; i < enemySpawner.EnemyList.Count; i++) {        //현재 존재하는 EnemyList의 모든 적을 검사함.
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr) {  //공격범위 내 있고 현재까지 검사한 적보다 더 가까우면
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;     //공격대상으로 지정
            }
        }

        return attackTarget;
    }

    private bool IsPossibleToAttackTarget() {       //공격대상을 때릴 수 있는지 검사하는 함수
        if (attackTarget == null) return false;     //target이 죽거나 goal로 가서 삭제되면 리턴 false

        float distance = Vector3.Distance(attackTarget.position, transform.position);   //target이 공격범위를 벗어날 경우 리턴 false
        if (distance > towerTemplate.weapon[level].range) {
            attackTarget = null;
            return false;
        }

        return true;
    }

    private void SpawnProjectile() {    //발사체 생성
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);   //생성된 발사체에게 공격대상 정보 제공
    }

    private void EnableLaser() {        //레이저 키기
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    private void DisableLaser() {   //레이저 끄기
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    private void SpawnLaser() {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        //RayCastAll(레이저 발사위치에서, direction벡터 방향으로, tower의 range 거리 만큼 안의 있는 것들의, targetLayer를 읽어오기)
        //hit을 여러개 두는 이유 : 목표와 타워 사이에 다른 적이 있으면 ray광선이 그 놈을 따라가서.
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        for (int i = 0; i < hit.Length; i++) {
            if(hit[i].transform == attackTarget) {
                lineRenderer.SetPosition(0, spawnPoint.position);   //line의 시작지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);  //선의 목표지점. target의 겉면에 이팩트 만들려고 hit.point를 씀.
                hitEffect.position = hit[i].point;  //타격 효과 위치 설정
                attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
            }
        }
    }

    public bool Upgrade() {
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost) {
            return false;
        }

        level++;        //타워 레벨 증가
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;     //타워 외형 변경
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;     //골드 차감

        if(weaponType == WeaponType.Laser) {
            lineRenderer.startWidth = 0.05f + level * 0.05f;    //레벨에 따라 광선 나오는 부분의 굵기 조절
            lineRenderer.endWidth = 0.05f;
        }
        return true;
    }

    public void Sell() {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;      //골드 증가
        ownerTile.IsBuildTower = false;     //현재 타일에 다시 타워가 건설 될 수 있도록 설정
        Destroy(gameObject);    //타워파괴
    }
}
