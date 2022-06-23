using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TMP_Text dialogText;
    public RectTransform startRect;
    public RectTransform endRect;
    public Image dialogBody;
    public CanvasGroup canvasGroup;

    private void OnEnable()
    {
        Instance = this;
    }

    public Tween FadeIn()
    {
        canvasGroup.DOKill();
        canvasGroup.blocksRaycasts = true;
        return canvasGroup.DOFade(1, 0.2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            canvasGroup.interactable = true;
        });
    }

    public Tween FadeOut()
    {
        canvasGroup.DOKill();
        canvasGroup.interactable = false;
        return canvasGroup.DOFade(0, 0.2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            canvasGroup.blocksRaycasts = false;
        });
    }

    public Tween Open()
    {
        dialogBody.rectTransform.DOKill();
        return dialogBody.rectTransform.DOMoveY(endRect.transform.position.y, 0.2f).OnStart(() =>
        {
            dialogBody.rectTransform.position = startRect.position;
        }).SetUpdate(true).SetEase(Ease.Linear);
    }
    
    public Tween Close()
    {
        dialogBody.rectTransform.DOKill();
        return dialogBody.rectTransform.DOMoveY(startRect.transform.position.y, 0.2f).OnStart(() =>
        {
            dialogBody.rectTransform.position = endRect.position;
        }).SetUpdate(true).SetEase(Ease.Linear);
    }

    public Tween SetText(string text)
    {
        dialogText.DOKill(true);
        dialogText.text = "";
        return dialogText.DOText(text, 0.4f).SetEase(Ease.Linear).SetUpdate(true);
    }
}