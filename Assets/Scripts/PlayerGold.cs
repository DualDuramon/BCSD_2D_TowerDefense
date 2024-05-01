using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    private int currentGold = 100;

    public int CurrentGold {    //골드 관리 프로퍼티
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }
}
