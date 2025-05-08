using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/NoteSO")]
public class NoteSO : ScriptableObject
{
    public string noteTitle;
    [TextArea]
    public string noteText;
}
