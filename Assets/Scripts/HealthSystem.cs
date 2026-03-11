using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HealthSystem : MonoBehaviour
{
    [SerializeField] private bool isFriendly;
    [SerializeField] private int maxHealth;

    [SerializeField] private GameObject spawnOnDeath;
    [SerializeField] private Transform spawnOnDeathLocation;

    [SerializeField] private Transform[] movedByDotween;

    [SerializeField] private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void GetHit(int pDamage)
    {
        // THIS IS WHERE THE HIT TAKES PLACE!!! PUT ANIMATION STUFF HERE
        currentHealth -= pDamage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        for (int i = 0; i < movedByDotween.Length; i++)
        {
            // If any DOTween movement is still happening, kill it (to prevent possible memory leaks)
            movedByDotween[i].DOKill();
        }

        if (spawnOnDeath != null)
        {
            // If there's an object to spawn on death, spawn it at designated position and default rotation
            Instantiate(spawnOnDeath, spawnOnDeathLocation.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.attachedRigidbody.TryGetComponent(out Danger danger))
        {
            // Friendly doesn't hit friendly (preventing player from hurting themselves with own bullets)
            if (isFriendly && danger.IsFriendly()) return;

            // Hit takes place: one of either is not friendly
            GetHit(danger.GetDamage());
        }
    }
}
