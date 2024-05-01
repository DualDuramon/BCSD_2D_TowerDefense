using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;               //���� ���������� ��� ����
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;  //���� ���̺� �ε���

    //���̺� ���� ����� ���� Get ������Ƽ(���� ���̺�, �� ���̺�)
    public int CurrentWave => currentWaveIndex + 1;     //���� ���̺� ��. + 1
    public int MaxWave => waves.Length;

    public void StartWave() {
        //���� �ʿ� ���� ����, wave�� ����������
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) {
            currentWaveIndex++; //�ε����� -1�̶� wave �������� ��
            enemySpawner.StartWave(waves[currentWaveIndex]); //EnemySpawner�� StartWave()ȣ��. ���� ���̺� ���� ����
        }
    }
}

[System.Serializable]
public struct Wave {

    public float spawnTime;             //���� ���̺� �� ���� �ֱ�
    public int maxEnemyCount;           //�� ���� ����
    public GameObject[] enemyPrefabs;   //�� ���� ����
}

/*
 * Desc
 *  : ���� ���������� ���̺� ����
 */