using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class HealthSystem : MonoBehaviour
{
    [SerializeField] private bool isFriendly;
    [SerializeField] private int maxHealth;

    [Tooltip("What, if anything, should this object spawn upon death?")]
    [SerializeField] private GameObject spawnOnDeath;

    [Header("Other")]
    [Tooltip("Place where the prefab spawns upon death")]
    [SerializeField] private Transform spawnOnDeathLocation;

    [Tooltip("If any child is moved by DOTween, put it in this array")]
    [SerializeField] private Transform[] movedByDotween;

    [Header("Other health stuff")]
    [SerializeField] private float healthMoveDuration;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void GetHit(int pDamage)
    {
        // THIS IS WHERE THE HIT TAKES PLACE!!! PUT ANIMATION STUFF HERE
        currentHealth -= pDamage;

        if (healthSlider != null)
        {
            healthSlider.DOValue((float)currentHealth / maxHealth, healthMoveDuration);
        }

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        for (int i = 0; i < movedByDotween.Length; i++)
        {
            // If any DOTween movement is still happening, kill it (to prevent possible memory leaks)
            movedByDotween[i].DOKill();
        }
        if (healthSlider != null)
        {
            healthSlider.DOKill();
            healthSlider.value = 0;
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
        if (other.attachedRigidbody == null) return;
        if (other.attachedRigidbody.TryGetComponent(out Danger danger))
        {
            // Friendly doesn't hit friendly (preventing player from hurting themselves with own bullets)
            if (isFriendly && danger.IsFriendly()) return;

            // Hit takes place: one of either is not friendly
            GetHit(danger.GetDamage());
        }
    }
}
