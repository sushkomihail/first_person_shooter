using UnityEngine;

[CreateAssetMenu(fileName = "_BulletSpec", menuName = "Specs/Bullet")]
public class BulletSpec : ScriptableObject
{
    public int Speed;
    public int FlightDistance;
    public int Damage;
}
