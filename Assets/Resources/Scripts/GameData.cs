using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{
    public string lastLoadedScene = "bug";
    public int currentHP;

    public int MaxHP = 11;
    public int currentMana;

    public string currentStage = "bug-bug-bug"; //pretpostavljam da ce stage da izgleda kao 2-3-5 (regija - stage - fight)

}