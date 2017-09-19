using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainbowRotate : MonoBehaviour
{
    public enum Type
    {
        Clockwise,
        CounterClockwise
    }

    int index = -1;
    public float time = .2f;
    public Type movesType;
    public Vector3[] moves;
    public Ease easeType;
    

    void OnEnable()
    {
        Next();
        //transform.DOMoveY(LocalY, 0);
        //Debug.Log(LocalY);
    }

    void Next()
    {
        index = (index + 1) % moves.Length;

        if (movesType == Type.Clockwise)
            transform.DORotate(moves[index], time, RotateMode.FastBeyond360).SetEase(easeType).OnComplete(() => Next());

    }

    void OnDisable()
    {
            transform.DOKill();
    }
}