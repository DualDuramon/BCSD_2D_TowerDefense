using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;               //현재 스테이지의 모든 정보
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;  //현재 웨이브 인덱스

    //웨이브 정보 출력을 위한 Get 프로퍼티(현재 웨이브, 총 웨이브)
    public int CurrentWave => currentWaveIndex + 1;     //현재 웨이브 값. + 1
    public int MaxWave => waves.Length;

    public void StartWave() {
        //현재 맵에 적이 없고, wave가 남아있으면
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) {
            currentWaveIndex++; //인덱스가 -1이라 wave 증가먼저 함
            enemySpawner.StartWave(waves[currentWaveIndex]); //EnemySpawner의 StartWave()호출. 현재 웨이브 정보 제공
        }
    }
}

[System.Serializable]
public struct Wave {

    public float spawnTime;             //현재 웨이브 적 생성 주기
    public int maxEnemyCount;           //적 등장 숫자
    public GameObject[] enemyPrefabs;   //적 등장 종류
}

/*
 * Desc
 *  : 현재 스테이지의 웨이브 관리
 */