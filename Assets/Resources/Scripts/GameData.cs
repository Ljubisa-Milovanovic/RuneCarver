using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    public string lastLoadedScene = "bug";
    public int currentHP;
    public int MaxHP =11;
    public int playerMana;


}