using QFSW.QC;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public GameData GameData;

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
        GameData.currentHP = GameData.MaxHP;
    }

    public int loseHPAmount(int amount)
    {
        GameData.currentHP -= amount;
        if (GameData.currentHP <= 0)
        {
            Debug.Log("mrtav si");
            GameData.currentHP = 0;
        }
        if (GameData.currentHP > GameData.MaxHP)
        {
            Debug.Log("neko me troluje CurrentHP > MaxHP");
            GameData.currentHP = GameData.MaxHP;
        }
        return GameData.currentHP;
    }

    [Command]
    private void ShowCurrentHP()
    {
        Debug.Log(GameData.currentHP.ToString());
    }
}
