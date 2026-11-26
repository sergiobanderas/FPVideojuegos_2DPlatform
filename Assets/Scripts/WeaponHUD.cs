using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{

    [Header("Sword slot")]
    [SerializeField] private GameObject swordSlot;
    [SerializeField] private Image swordBackground;

    [Header("Arrow slot")]
    [SerializeField] private GameObject arrowSlot;
    [SerializeField] private Image arrowBackground;
    [SerializeField] private TextMeshProUGUI arrowCountText;

    [Header("Weapon slot colors")]
    [SerializeField] private Color activeColor = Color.yellow;
    [SerializeField] private Color inactiveColor = new Color(0f, 0f, 0f, 0.5f); // Semi-transparent black


    PlayerCombat playerCombat;

    void Awake()
    {
        playerCombat = FindAnyObjectByType<PlayerCombat>();
    }

    void OnEnable()
    {
        // Subscribe to the event when the HUD is enabled
        playerCombat.InventoryChanged += RefreshHUD;         
        RefreshHUD(); // Initial refresh when the HUD is enabled
    }

    void OnDisable()
    {
        // Unsubscribe from the event when the HUD is disabled
        playerCombat.InventoryChanged -= RefreshHUD; 
    }

    void RefreshHUD()
    {
        //Show only the weapons the player has in their inventory
        swordSlot.SetActive(playerCombat.HasSword);
        arrowSlot.SetActive(playerCombat.Arrows > 0);

        arrowCountText.text = "x" + playerCombat.Arrows;

        //Highlight the currently equipped weapon
        swordBackground.color = (playerCombat.CurrentWeapon == WeaponType.Sword) ? activeColor : inactiveColor;
        arrowBackground.color = (playerCombat.CurrentWeapon == WeaponType.Bow) ? activeColor : inactiveColor;
    }

}     
