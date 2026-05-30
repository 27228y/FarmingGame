using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    private Vector3 originalScale;
    private bool isAnimating = false;

    private void Start()
    {
        originalScale = transform.localScale;
    }
    
    public void PlayBounceAnimation()
    {
        isAnimating = true;
        
        Vector3 squashScale =  new Vector3(originalScale.x * 1.2f, originalScale.y * 0.8f, originalScale.z * 1.2f);
        
        Sequence bounceSequence = DOTween.Sequence();
        bounceSequence.Append(transform.DOScale(squashScale, 0.1f).SetEase(Ease.OutQuad));
        bounceSequence.Append(transform.DOScale(originalScale, 0.4f).SetEase(Ease.OutBack));
        
        bounceSequence.onComplete += () => isAnimating = false;
    }
}
