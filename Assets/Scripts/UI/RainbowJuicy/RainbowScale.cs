using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainbowScale : MonoBehaviour
{
    int index = -1;
    public float time = .2f;
    public Vector3[] scales;
    public Ease easeType;


    void OnEnable()
    {
        Next();
    }

    void Next()
    {
        index = (index + 1) % scales.Length;


        transform.DOScale(scales[index], time).SetEase(easeType).OnComplete(() => Next());
        
    }

    void OnDisable()
    {
        transform.DOScale(Vector3.one, 0);
    }
}
