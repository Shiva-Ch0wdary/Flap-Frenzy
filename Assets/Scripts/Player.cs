using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
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
    private Vector3 direction;
    private int spriteIndex;

    private float normalSpeed = 5f;
    private float speedBoostMultiplier = 2f;
    private bool isInvisible = false;
    private bool hasShield = false;
    private bool isSliding = false;
    private float lastClickTime = 0.0f;

    private float originalSize;
    public GameObject shieldObject;
    public GameObject invisibilityEffect;
    public GameObject invisibleBirdPrefab; // Reference to the invisible bird prefab

    private Collider2D playerCollider;
    private GameObject invisibleBirdInstance;
    private bool isGrowActive = false;

    public float powerUpMaxHeight = 4f; // Max height when power-ups are active
    public float powerUpMinHeight = -2f; // Min height when power-ups are active
    private bool isPowerUpActive = false; // To track if any power-up is currently active

    //declaration of variables of the Coins Spawn
    public int coinCount = 0;
    public Text coinTextNow; 

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
            GameManager.Instance.GameOver();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (isGrowActive)
            {
                Destroy(other.gameObject); // Destroy the obstacle if Grow power-up is active
            }
            else if (!hasShield)
            {
                GameManager.Instance.GameOver();
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

            // Destroy the coin object
            Destroy(other.gameObject);
        }
    }
    public void ActivatePowerUp(PowerUpType powerUpType, float duration)
    {
        isPowerUpActive = true; // Mark power-up as active
        StartCoroutine(GameManager.Instance.DisplayPowerUpTimer(duration));

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
            case PowerUpType.CameraZoomOut:
                StartCoroutine(HandleCameraZoomOut(duration));
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
    }

    private IEnumerator HandleInvisibility(float duration)
    {
        isInvisible = true;

        // Disable the player’s sprite and collider
        spriteRenderer.enabled = false;
        if (playerCollider != null) playerCollider.enabled = false;

        // Instantiate the invisible bird at the player’s position
        if (invisibleBirdPrefab != null)
        {
            invisibleBirdInstance = Instantiate(invisibleBirdPrefab, transform.position, Quaternion.identity);
            invisibleBirdInstance.transform.parent = transform; // Optional: Parent it to the player if it should follow
        }

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

        yield return new WaitForSeconds(duration);

        normalSpeed = originalSpeed;
    }

    private IEnumerator HandleShield(float duration)
    {
        hasShield = true;
        if (shieldObject != null) shieldObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        hasShield = false;
        if (shieldObject != null) shieldObject.SetActive(false);
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

    private IEnumerator HandleCameraZoomOut(float duration)
    {
        float originalFOV = Camera.main.fieldOfView; // Store the original FOV
        float targetFOV = 160f; // Set your desired zoomed-out FOV

        // Gradually zoom out by increasing the FOV
        float elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            Camera.main.fieldOfView = Mathf.Lerp(originalFOV, targetFOV, elapsedTime / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Wait for the duration of the power-up effect
        yield return new WaitForSeconds(duration / 2);

        // Gradually zoom back in to the original FOV
        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            Camera.main.fieldOfView = Mathf.Lerp(targetFOV, originalFOV, elapsedTime / (duration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.fieldOfView = originalFOV; // Ensure the FOV is reset
    }
}
