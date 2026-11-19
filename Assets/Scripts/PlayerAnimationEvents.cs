using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    PlayerMovement player;

    void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    // This method is called from the animation event in the "Shoot" animation clip
    public void ShootArrow()
    {
        player.ShootArrow();
    }
}
