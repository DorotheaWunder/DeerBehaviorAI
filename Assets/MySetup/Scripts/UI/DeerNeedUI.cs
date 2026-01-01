using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerNeedUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeerNeedController needs;
    [SerializeField] private CanvasGroup bubbleCanvas;
    [SerializeField] private UnityEngine.UI.Image iconImage;

    [Header("Icons")]
    public Sprite FoodIcon;
    public Sprite WaterIcon;
    public Sprite StaminaIcon;

    [Header("Timing")]
    public float showDuration = 2f;
    public float fadeSpeed = 6f;

    private Coroutine currentRoutine;

    private readonly Dictionary<NeedType, int> priority = new()
    {
        { NeedType.Water, 0 },
        { NeedType.Food, 1 },
        { NeedType.Stamina, 2 }
    };

    private void Awake()
    {
        bubbleCanvas.alpha = 0f;
    }

    private void OnEnable()
    {
        needs.OnNeedEvent += HandleNeedEvent;
    }

    private void OnDisable()
    {
        needs.OnNeedEvent -= HandleNeedEvent;
    }

    private void HandleNeedEvent(NeedEvent evt)
    {
        if (!evt.IsLow) return;

        ShowNeed(evt.NeedType);
    }

    private void ShowNeed(NeedType type)
    {
        iconImage.sprite = GetIcon(type);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        while (bubbleCanvas.alpha < 1f)
        {
            bubbleCanvas.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(showDuration);
        
        while (bubbleCanvas.alpha > 0f)
        {
            bubbleCanvas.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    private Sprite GetIcon(NeedType type)
    {
        return type switch
        {
            NeedType.Food => FoodIcon,
            NeedType.Water => WaterIcon,
            NeedType.Stamina => StaminaIcon,
            _ => null
        };
    }
}
