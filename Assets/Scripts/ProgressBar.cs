using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ProgressBar : MonoBehaviour
{
    public Slider progressBar;

    private void Start()
    {
        progressBar.gameObject.SetActive(false); // Hide progress bar initially
    }

    // Show and update the progress bar based on power-up duration
    public void ActivateProgressBar(float duration)
    {
        progressBar.gameObject.SetActive(true);
        StartCoroutine(FillProgressBar(duration));
    }

    private IEnumerator FillProgressBar(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            progressBar.value = Mathf.Clamp01(elapsed / duration); // Fill progress bar
            yield return null;
        }

        DeactivateProgressBar(); // Hide progress bar after duration
    }

    // Hide and reset the progress bar
    public void DeactivateProgressBar()
    {
        progressBar.value = 0f;
        progressBar.gameObject.SetActive(false);
    }
}
