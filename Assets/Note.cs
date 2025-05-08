using UnityEngine;

public class Note : MonoBehaviour, IInteractbleItem,IToolTip
{
    [SerializeField] private NoteSO noteSO;
    public void OnPlayerInteract() {
        NoteUI.Instance.ShowNote(noteSO);
    }

    public void OnPlayerInteractAlternate() {
    }

    public void HasOwner() {
    }

    public string ShowToolTip() {
        return "PRESS E TO READ NOTE";
    }
}
