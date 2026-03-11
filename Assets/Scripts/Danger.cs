using UnityEngine;

public class Danger : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private bool isFriendly;

    public int GetDamage()
    {
        // Destroy self?
        return damage;
    }
    public bool IsFriendly()
    {
        return isFriendly;
    }
}
