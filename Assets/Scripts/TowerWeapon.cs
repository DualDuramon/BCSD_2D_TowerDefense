using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate   towerTemplate;
    [SerializeField]
    private GameObject      projectilePrefab;   //�߻�ü ������
    [SerializeField]
    private Transform       spawnPoint;         //�߻�ü ���� ��ġ

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
            float closestDistSqr = Mathf.Infinity;  //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
            
            for (int i = 0; i < enemySpawner.EnemyList.Count; i++) {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                //��� ���� ���������ϴ� �߿� ������ �Ÿ��� ���ݹ��� ���� �ְ� �˻��� ������ �� �Ÿ��� ������
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
            //1.target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ����, goal�� ������ ���� ��)
            if (attackTarget == null) {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 2. target�� ���ݹ��� �ȿ� �ִ��� �˻�. (������ ���ο� �� Ž��)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance > towerTemplate.weapon[level].range) {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 3. attackRate �ð���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            //4. ������ (�߻�ü ����)
            SpawnProjectile();
        }
    }

    private void SpawnProjectile() {    //�߻�ü ����
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);   //������ �߻�ü���� ���ݴ�� ���� ����
    }

    public bool Upgrade() {
        if (playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost) {
            return false;
        }

        level++;        //Ÿ�� ���� ����
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;     //Ÿ�� ���� ����
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;     //��� ����

        return true;
    }

    public void Sell() {
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;      //��� ����
        ownerTile.IsBuildTower = false;     //���� Ÿ�Ͽ� �ٽ� Ÿ���� �Ǽ� �� �� �ֵ��� ����
        Destroy(gameObject);    //Ÿ���ı�
    }
}
