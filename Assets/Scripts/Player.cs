using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class Player : MonoBehaviour
{
    private bool isInvincible = false; 
    public Sprite[] sprites;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;
    public float maxHeight = 4f;

    public float slideHeight = -3f; // Height for sliding
    public float slideSpeed = 5f; // Speed for sliding
    public float groundLevel = -3f; // Ground level
    public float doubleClickThreshold = 0.25f; // Time within which a double click is detected

    private SpriteRenderer spriteRenderer;
    public Vector3 direction;
    private int spriteIndex;

    private float normalSpeed = 5f;
    private float speedBoostMultiplier = 2f;
    private bool isInvisible = false;
    private bool hasShield = false;
    private bool isSliding = false;
    private float lastClickTime = 0.0f;

    private float originalSize;
   // public GameObject shieldObject;
    public GameObject invisibilityEffect;
   // public GameObject invisibleBirdPrefab; // Reference to the invisible bird prefab

    private Collider2D playerCollider;
    private GameObject invisibleBirdInstance;
    private bool isGrowActive = false;

    public float powerUpMaxHeight = 4f; // Max height when power-ups are active
    public float powerUpMinHeight = -2f; // Min height when power-ups are active
    private bool isPowerUpActive = false; // To track if any power-up is currently active

    //declaration of variables of the Coins Spawn
    public int coinCount = 0;
    public Text coinTextNow;

    // NEW: Add Particle System for Speed Boost
    public GameObject speedBoostEffect; // Reference to Speed Boost particle effect
    public List<GameObject> shieldEffects; // Add all your shield effect prefabs here
    private List<GameObject> activeShieldEffects = new List<GameObject>();

    public GameObject destroyEffectPrefab; // Particle effect prefab for obstacle destruction

    public ProgressBar powerUpProgressBar; // Reference to the power-up progress bar


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSize = transform.localScale.x;
        playerCollider = GetComponent<Collider2D>();
        //ResetCoinCount();
        // Load the saved coin count from PlayerPrefs
        coinCount = PlayerPrefs.GetInt("TotalCoins", 0); // Ensure we use the correct key "TotalCoins"
        UpdateCoinUI();
    }
    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        // Load the saved coin count from PlayerPrefs
        coinCount = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateCoinUI();
    }
    public void UpdateCoinUI()
    {
        if (coinTextNow != null)
        {
            coinTextNow.text = coinCount.ToString();
        }
        PlayerPrefs.SetInt("TotalCoins", coinCount);
        PlayerPrefs.Save();
    }//code to reset the coins to zero call in the awake method

    //private void ResetCoinCount()
    //{
    //    PlayerPrefs.SetInt("TotalCoins", 0); // Set the coin count to zero
    //    PlayerPrefs.Save(); // Save the changes
    //}

    private void Update()
    {
        // Detect single click or long-press for sliding
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - lastClickTime;
            lastClickTime = Time.time;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                // Detected a double click, initiate sliding
                isSliding = true;
            }
            else
            {
                // Single click detected, move up
                direction = Vector3.up * strength;
                isSliding = false;
            }
        }

        // Check if the player is holding the button for sliding
        if (Input.GetMouseButton(0) && isSliding)
        {
            Slide();
        }
        else
        {
            ApplyGravity();
        }

        // Limit the player's height when a power-up is active
        if (isPowerUpActive)
        {
            Vector3 position = transform.position;
            position.y = Mathf.Clamp(position.y, powerUpMinHeight, powerUpMaxHeight);
            transform.position = position;
        }
        else
        {
            if (transform.position.y > maxHeight)
            {
                Vector3 position = transform.position;
                position.y = maxHeight;
                transform.position = position;
            }
        }

         transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3f, 7f), transform.position.z);

        // Tilt the player based on the direction
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
    }

    private void Slide()
    {
        // Smoothly transition to slide height and prevent game-over during sliding
        float currentY = transform.position.y;
        currentY = Mathf.MoveTowards(currentY, slideHeight, slideSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, currentY, transform.position.z);
        direction.y = 0; // Ensure no vertical movement while sliding
    }

    private void ApplyGravity()
    {
        // Apply gravity if not sliding
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // Trigger game-over if the player hits the ground when not sliding
        if (transform.position.y < groundLevel && !isSliding)
        {
            //adding if condition
            if (!isInvincible && !GameManager.Instance.IsInvincible)
            {
                GameManager.Instance.GameOver();
            }
            //adding if condition ends
            //GameManager.Instance.GameOver();
        }
    }


    private void AnimateSprite()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    //new method
    public void SetInvincibility(bool state)
    {
        isInvincible = state;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Obstacle") || other.CompareTag("Pipe"))
        {
            if (isGrowActive)
            {
                // If Grow power-up is active, destroy the obstacle
                Destroy(other.gameObject);
            }
            else if (!isInvincible && !GameManager.Instance.IsInvincible && !hasShield)
            {
                GameManager.Instance.GameOver();
            }
            else if (isInvincible)
            {
                // Optional: Handle obstacle interaction during invincibility, like bouncing or destroying
                Debug.Log("Obstacle ignored due to invincibility!");
            }
        }

        else if (other.gameObject.CompareTag("PowerUp"))
        {
            PowerUp powerUp = other.GetComponent<PowerUp>();
            if (powerUp != null)
            {
                ActivatePowerUp(powerUp.powerUpType, powerUp.duration);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Coin"))
        {
            // Increase the coin count
            coinCount++;

            // Save the updated coin count
            PlayerPrefs.SetInt("TotalCoins", coinCount);
            PlayerPrefs.Save();

            // Update the UI
            UpdateCoinUI();

            // Play the coin sound
            AudioManager.Instance.PlaySFX("coin");

            // Destroy the coin object
            Destroy(other.gameObject);
        }
    }
    public void ActivatePowerUp(PowerUpType powerUpType, float duration)
    {
        isPowerUpActive = true; // Mark power-up as active
        // StartCoroutine(GameManager.Instance.DisplayPowerUpTimer(duration));
        powerUpProgressBar.ActivateProgressBar(duration); // Activate the power-up progress bar


        switch (powerUpType)
        {
            case PowerUpType.Invisibility:
                StartCoroutine(HandleInvisibility(duration));
                AudioManager.Instance.PlaySFX("powerup");
                break;
            case PowerUpType.SpeedBoost:
                StartCoroutine(HandleSpeedBoost(duration));
                AudioManager.Instance.PlaySFX("powerup");
                break;
            case PowerUpType.Shield:
                StartCoroutine(HandleShield(duration));
                AudioManager.Instance.PlaySFX("powerup");
                break;
            case PowerUpType.Shrink:
                StartCoroutine(HandleShrink(duration));
                AudioManager.Instance.PlaySFX("powerup");
                break;
            case PowerUpType.Grow:
                StartCoroutine(HandleGrow(duration));
                AudioManager.Instance.PlaySFX("powerup");
                break;

        }

        // Reset the flag after the power-up duration
        StartCoroutine(DeactivatePowerUpAfterDuration(duration));
    }

    private IEnumerator DeactivatePowerUpAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPowerUpActive = false;

        powerUpProgressBar.DeactivateProgressBar(); // Hide progress bar when power-up ends

    }

    private IEnumerator HandleInvisibility(float duration)
    {
        isInvisible = true;

        // Disable the player’s sprite and collider
        spriteRenderer.enabled = false;
        if (playerCollider != null) playerCollider.enabled = false;

        if (invisibilityEffect != null) invisibilityEffect.SetActive(true);

        yield return new WaitForSeconds(duration);

        // Re-enable the player’s sprite and collider
        spriteRenderer.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;

        // Destroy the invisible bird instance after the effect ends
        if (invisibleBirdInstance != null) Destroy(invisibleBirdInstance);

        if (invisibilityEffect != null) invisibilityEffect.SetActive(false);
        isInvisible = false;
    }

    private IEnumerator HandleSpeedBoost(float duration)
    {
        float originalSpeed = normalSpeed;
        normalSpeed *= speedBoostMultiplier;

        // Activate and play the Speed Boost Particle Effect
        if (speedBoostEffect != null)
        {
            speedBoostEffect.SetActive(true);

            // Ensure the particle system starts playing
            ParticleSystem particleSystem = speedBoostEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null && !particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        else
        {
            Debug.LogWarning("Speed Boost effect is not assigned or missing!");
        }

        yield return new WaitForSeconds(duration);

        // Revert speed and stop the particle effect
        normalSpeed = originalSpeed;

        if (speedBoostEffect != null)
        {
            Debug.Log("Activating Speed Boost effect!");
            ParticleSystem particleSystem = speedBoostEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null && particleSystem.isPlaying)
            {
                particleSystem.Stop();
            }
            speedBoostEffect.SetActive(false);
        }
    }

    private IEnumerator HandleShield(float duration)
    {
        Debug.Log("Shield Activated"); // Debug to confirm this is called
        hasShield = true;

        // Instantiate and activate all shield effects
        foreach (GameObject shieldPrefab in shieldEffects)
        {
            if (shieldPrefab != null)
            {
                GameObject shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
                shieldInstance.transform.SetParent(transform); // Parent to the player
                activeShieldEffects.Add(shieldInstance); // Keep track of active shields
            }
            else
            {
                Debug.LogError("Shield prefab is null!");
            }
        }

        yield return new WaitForSeconds(duration);

        // Deactivate and destroy all shield effects
        hasShield = false;
        foreach (GameObject shieldInstance in activeShieldEffects)
        {
            if (shieldInstance != null)
            {
                Destroy(shieldInstance); // Destroy the shield instance
            }
        }

        activeShieldEffects.Clear(); // Clear the list of active shields
    }

    private IEnumerator HandleShrink(float duration)
    {
        transform.localScale = Vector3.one * 0.3f;

        yield return new WaitForSeconds(duration);

        transform.localScale = Vector3.one * originalSize;
    }

    private IEnumerator HandleGrow(float duration)
    {
        isGrowActive = true; // Set the Grow flag to true
        transform.localScale = Vector3.one * 0.5f;

        yield return new WaitForSeconds(duration);

        transform.localScale = Vector3.one * originalSize;
        isGrowActive = false; // Reset the Grow flag after the duration
    }

}
