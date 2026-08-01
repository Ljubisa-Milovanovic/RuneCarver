using QFSW.QC;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    private int CurrentHP;
    private int MaxHP = 11;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        CurrentHP = MaxHP;
    }

    public int loseHPAmount(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            Debug.Log("mrtav si");
            CurrentHP = 0;
        }
        if (CurrentHP > MaxHP)
        {
            Debug.Log("neko me troluje CurrentHP > MaxHP");
            CurrentHP = MaxHP;
        }
        return CurrentHP;
    }

}
