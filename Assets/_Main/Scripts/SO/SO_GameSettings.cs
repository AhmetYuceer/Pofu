using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings" , order = 0)]
public class SO_GameSettings : ScriptableObject
{
    public Level CurrentLevel;
}