using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Hit Effect Settings")]
    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private float shakeDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        originalPosition = transform.localPosition;
    }

    public void OnHit()
    {
        if (!isShaking)
        {
            StartCoroutine(HitEffect());
        }
    }

    private IEnumerator HitEffect()
    {
        isShaking = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        isShaking = false;
    }
}
