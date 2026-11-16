using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;    
    
    [Header("Blink Settings")]
    [SerializeField] private float blinkDuration = 0.2f;
    [SerializeField] private Color blinkColor = Color.red;

    private SpriteRenderer spriteRenderer;


    private bool isVulnerable = true; 
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Reduces the player's health by 1 and triggers the blinking effect if the player is vulnerable. 
    /// If health reaches 0, the player dies. 
    /// </summary>
    public void TakeDamage()
    {
        if (isVulnerable)
        {
            currentHealth --;
            if (currentHealth <= 0)
            {
                //TODO: Implement die logic 
                Debug.Log("Player has died");              
            }
            else
            {
                StartCoroutine(Blink(4));
            }
        }
    }

    /// <summary>
    /// Makes the player blink for a specified number of times to indicate invulnerability.
    /// During this time, the player cannot take damage.
    /// </summary>
    /// <param name="numberOfBlinks"></param>
    /// <returns></returns>
    private IEnumerator Blink(int numberOfBlinks)
    {
        isVulnerable = false; // Make the player invulnerable during blinking

        do
        {
            spriteRenderer.color = blinkColor; // Change to blink color
            yield return new WaitForSeconds(blinkDuration);
            spriteRenderer.color = Color.white; // Change back to original color
            yield return new WaitForSeconds(blinkDuration);
            numberOfBlinks--;
        } while (numberOfBlinks > 0);

        isVulnerable = true; // Make the player vulnerable again after blinking
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    

}
