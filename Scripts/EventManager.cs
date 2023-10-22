using UnityEngine.Events;

public static class EventManager
{
    public static readonly UnityEvent OnEnemyDeath = new UnityEvent();
    public static readonly UnityEvent OnPlayerDeath = new UnityEvent();
}
