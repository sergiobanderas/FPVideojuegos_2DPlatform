using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    enum PickupType
    {
        Sword,
        Arrows
    }

    [SerializeField] private PickupType pickupType = PickupType.Sword;
    [SerializeField] private int arrowAmount = 5; // Only used by arrows

    [Header("Floating effect")]
    [SerializeField] private float floatHeight = 0.10f; // Height of the floating     effect
    [SerializeField] private float floatSpeed = 5f; // Speed of the floating effect

    Vector3 startPosition;
    bool collected = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        FloatEffect();
    }

    /// <summary>
    /// Creates a floating effect for the pickup item.
    /// </summary>
    void FloatEffect()
    {
        // Floating effect
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPosition + new Vector3(0, offset, 0);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return; // Prevent multiple colliders ; only collect one 

        if (!other.CompareTag("Player")) return;

        PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
        if (playerCombat == null) return;

        collected = true; 
        switch (pickupType)
        {
            case PickupType.Sword:
                playerCombat.AddSword();
                break;
            case PickupType.Arrows:
                playerCombat.AddArrows(arrowAmount);
                break;
        }

        //TODO Play pickup sound effect here
        Destroy(gameObject); 


    }



}
