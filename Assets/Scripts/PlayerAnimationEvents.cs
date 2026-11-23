using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    PlayerCombat combat;

    void Awake()
    {
        combat = GetComponentInParent<PlayerCombat>();
    }

    // This method is called from the animation event in the "Shoot" animation clip
    public void ShootArrow()
    {
        combat.ShootArrow();
    }

    public void SwordAttack()
    {
        combat.SwordAttack();
    }

}
