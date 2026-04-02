using UnityEngine;

public class PowerUpCollectable : MonoBehaviour
{
    [SerializeField] private PlayerController.PowerUp powerUpType = PlayerController.PowerUp.Movement;
    [SerializeField] private float multilpierAmount = 0;
    [SerializeField] private int cooldown = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().ApplyPowerUp(powerUpType, multilpierAmount, cooldown);

            Destroy(gameObject);
        }
    }
}
