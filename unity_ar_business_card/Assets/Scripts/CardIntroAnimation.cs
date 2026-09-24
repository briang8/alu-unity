using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardIntroAnimation : MonoBehaviour
{
    [Header("Drag your card elements here in the order you want them to appear")]
    public Transform[] popInElements; // buttons, icons, etc
    public CanvasGroup[] fadeElements; // name/title text, if using CanvasGroup

    [Header("Timing")]
    public float delayBetweenElements = 0.1f;
    public float animationDuration = 0.3f;

    private void OnEnable()
    {
        // runs every time this object gets activated (e.g. marker found again)
        StopAllCoroutines();
        PlayIntro();
    }

    private void PlayIntro()
    {
        StartCoroutine(AnimateIn());
    }

    private IEnumerator AnimateIn()
    {
        // reset everything to invisible/small first
        foreach (var el in popInElements)
            el.localScale = Vector3.zero;

        foreach (var cg in fadeElements)
            cg.alpha = 0f;

        // animate fade elements first (name, title)
        foreach (var cg in fadeElements)
        {
            StartCoroutine(FadeIn(cg));
            yield return new WaitForSeconds(delayBetweenElements);
        }

        // then pop in each button with a stagger
        foreach (var el in popInElements)
        {
            StartCoroutine(PopIn(el));
            yield return new WaitForSeconds(delayBetweenElements);
        }
    }

    private IEnumerator PopIn(Transform target)
    {
        float t = 0f;
        Vector3 targetScale = Vector3.one;

        while (t < animationDuration)
        {
            t += Time.deltaTime;
            float progress = t / animationDuration;
            // simple ease-out curve, starts fast then settles
            float eased = 1f - Mathf.Pow(1f - progress, 3f);
            target.localScale = Vector3.LerpUnclamped(Vector3.zero, targetScale, eased);
            yield return null;
        }

        target.localScale = targetScale;
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        float t = 0f;

        while (t < animationDuration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Clamp01(t / animationDuration);
            yield return null;
        }

        group.alpha = 1f;
    }
}