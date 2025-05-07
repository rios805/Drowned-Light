using UnityEngine;

public interface IEnemy {
    public string GetEnemyType();
    public void SeenByPlayer(bool isSeen);
}
