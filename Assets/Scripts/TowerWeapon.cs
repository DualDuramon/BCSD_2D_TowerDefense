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
    private Transform spawnPoint;         //�߻�ü ���� ��ġ
    [SerializeField]
    private WeaponType weaponType;

    [Header("Cannon")]
    [SerializeField]
    private GameObject      projectilePrefab;   //�߻�ü ������

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;      //�������� ���Ǵ� ��(LineRenderer)
    [SerializeField]
    private Transform hitEffect;            //Ÿ��ȿ��
    [SerializeField]
    private LayerMask targetLayer;          //������ �ε����� ���̾� ����

    private int             level = 0;
    private WeaponState     weaponeState = WeaponState.SearchTarget;    //Ÿ�� ������ ����
    private Transform       attackTarget = null;    //���ݴ��
    private SpriteRenderer  spriteRenderer;     //Ÿ�� ������Ʈ �̹��� �����
    private EnemySpawner    enemySpawner;           // ���ӿ� �����ϴ� �� ���� ȹ���
    private PlayerGold      playerGold;         //�÷��̾��� ��� ���� ȹ�� �� ����
    private Tile            ownerTile;          //���� Ÿ���� ��ġ�Ǿ� �ִ� Ÿ��

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
        ChangeState(WeaponState.SearchTarget);  //���� ���¸� SearchTarget���� ����
    }

    public void ChangeState(WeaponState newState) {
        StopCoroutine(weaponeState.ToString());     //������ ������̴� ���� ����
        weaponeState = newState;
        StartCoroutine(weaponeState.ToString());
    }

    private void Update() {
        if (attackTarget != null) {
            RotateToTarget();
        }
    }

    private void RotateToTarget() { //Target�� �ٶ󺸴� �Լ�
        //�������κ����� �Ÿ�(����),���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
        //���� arctan(y/x)
        //������ ���ϱ�
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;     //dy/dx�� ���� ���� �� rad������ degree�� ��ȯ
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget() {
        while (true) {
            attackTarget = FindClosestAttackTarget();       //Ÿ������ ���� ����� ���ݴ����� Ž��
            if (attackTarget != null) {

                if (weaponType == WeaponType.Cannon)            //Ÿ���� ���� ���¿� ���� ���� ���¸� ����
                    ChangeState(WeaponState.TryAttackCannon);
                else if (weaponType == WeaponType.Laser)
                    ChangeState(WeaponState.TryAttackLaser);
            }

            yield return null;
        }
    }

    private IEnumerator TryAttackCannon() {  //Ÿ���� ���� �������� �˻��ϴ� �Լ�
        while (true) {
            if (IsPossibleToAttackTarget() == false) {  //Ÿ���� ������ �� ������ ���ο� Ÿ���� Ž��.
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);  //Ÿ���� ���� �ֱ⸸ŭ ���
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

    private Transform FindClosestAttackTarget() {       //���� ������ �ִ� �� ã�� �Լ�.
        float closestDistSqr = Mathf.Infinity;

        for (int i = 0; i < enemySpawner.EnemyList.Count; i++) {        //���� �����ϴ� EnemyList�� ��� ���� �˻���.
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr) {  //���ݹ��� �� �ְ� ������� �˻��� ������ �� ������
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;     //���ݴ������ ����
            }
        }

        return attackTarget;
    }

    private bool IsPossibleToAttackTarget() {       //���ݴ���� ���� �� �ִ��� �˻��ϴ� �Լ�
        if (attackTarget == null) return false;     //target�� �װų� goal�� ���� �����Ǹ� ���� false

        float distance = Vector3.Distance(attackTarget.position, transform.position);   //target�� ���ݹ����� ��� ��� ���� false
        if (distance > towerTemplate.weapon[level].range) {
            attackTarget = null;
            return false;
        }

        return true;
    }

    private void SpawnProjectile() {    //�߻�ü ����
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);   //������ �߻�ü���� ���ݴ�� ���� ����
    }

    private void EnableLaser() {        //������ Ű��
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    private void DisableLaser() {   //������ ����
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    private void SpawnLaser() {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        //RayCastAll(������ �߻���ġ����, direction���� ��������, tower�� range �Ÿ� ��ŭ ���� �ִ� �͵���, targetLayer�� �о����)
        //hit�� ������ �δ� ���� : ��ǥ�� Ÿ�� ���̿� �ٸ� ���� ������ ray������ �� ���� ���󰡼�.
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        for (int i = 0; i < hit.Length; i++) {
            if(hit[i].transform == attackTarget) {
                lineRenderer.SetPosition(0, spawnPoint.position);   //line�� ��������
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);  //���� ��ǥ����. target�� �Ѹ鿡 ����Ʈ ������� hit.point�� ��.
                hitEffect.position = hit[i].point;  //Ÿ�� ȿ�� ��ġ ����
                attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
            }
        }
    }

    public bool Upgrade() {
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost) {
            return false;
        }

        level++;        //Ÿ�� ���� ����
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;     //Ÿ�� ���� ����
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;     //��� ����

        if(weaponType == WeaponType.Laser) {
            lineRenderer.startWidth = 0.05f + level * 0.05f;    //������ ���� ���� ������ �κ��� ���� ����
            lineRenderer.endWidth = 0.05f;
        }
        return true;
    }

    public void Sell() {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;      //��� ����
        ownerTile.IsBuildTower = false;     //���� Ÿ�Ͽ� �ٽ� Ÿ���� �Ǽ� �� �� �ֵ��� ����
        Destroy(gameObject);    //Ÿ���ı�
    }
}
