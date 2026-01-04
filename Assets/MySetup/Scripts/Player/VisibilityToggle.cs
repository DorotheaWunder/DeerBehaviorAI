using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VisibilityToggle : MonoBehaviour
{
    [Header("Marker UI")]
    public GameObject MarkerUI;
        
    [Header("Sense Materials")]
    public Material VisionMaterial;
    public Material HearingMaterial;

    [Header("Alpha")]
    public float VisionAlpha = 0.1f;
    public float HearingAlpha = 0.15f;
    public float FadeSpeed = 3f;

    private HunterSense input;

    private float targetVisionAlpha;
    private float targetHearingAlpha;

    private void Awake()
    {
        input = new HunterSense();
    }

    private void OnEnable()
    {
        input.PlayerControl.HunterSense.performed += OnHunterSensePressed;
        input.PlayerControl.HunterSense.canceled += OnHunterSenseReleased;
        input.PlayerControl.Enable();
    }

    private void OnDisable()
    {
        input.PlayerControl.HunterSense.performed -= OnHunterSensePressed;
        input.PlayerControl.HunterSense.canceled -= OnHunterSenseReleased;
        input.PlayerControl.Disable();
    }

    private void Update()
    {
        Fade(VisionMaterial, targetVisionAlpha);
        Fade(HearingMaterial, targetHearingAlpha);
    }

    private void OnHunterSensePressed(InputAction.CallbackContext ctx)
    {
        targetVisionAlpha = VisionAlpha;
        targetHearingAlpha = HearingAlpha;
        MarkerUI.SetActive(true);
    }

    private void OnHunterSenseReleased(InputAction.CallbackContext ctx)
    {
        targetVisionAlpha = 0f;
        targetHearingAlpha = 0f;
        MarkerUI.SetActive(false);
    }

    private void Fade(Material mat, float targetAlpha)
    {
        if (!mat) return;

        Color c = mat.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, FadeSpeed * Time.deltaTime);
        mat.color = c;
    }
}
