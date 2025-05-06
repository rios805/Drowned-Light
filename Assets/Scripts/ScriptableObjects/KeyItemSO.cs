using UnityEngine;

[CreateAssetMenu()]
public class KeyItemSO : ScriptableObject
{
    public Transform prefab;
    public Sprite uIIcon;
    public string itemName;

    // For coin-based puzzles
    public string sideA_ID;
    public string sideB_ID;
    public Sprite sideA_Icon;
    public Sprite sideB_Icon;

    public bool IsCoin => !string.IsNullOrEmpty(sideA_ID) && !string.IsNullOrEmpty(sideB_ID);
}

