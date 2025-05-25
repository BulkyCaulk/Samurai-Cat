using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEnlarge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scaleMultiplier = 1.2f;
    public float transitionSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isHovered;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleMultiplier;
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        isHovered = false;
    }
}