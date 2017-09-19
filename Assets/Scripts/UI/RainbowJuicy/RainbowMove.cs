using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RainbowMove : MonoBehaviour
{
    public enum Type{
        LocalVertical,
        LocalHorizontal,
        GlobalVertical,
        GlobalHorizontal
    }

    int index = -1;
    public float time = .2f;
    public Type movesType;
    public float[] moves;
    public Ease easeType;

    private float LocalY;

    void OnEnable()
    {
        Next();
        //transform.DOMoveY(LocalY, 0);
        //Debug.Log(LocalY);
    }

    void Next()
    {
        index = (index + 1) % moves.Length;

        if (movesType == Type.LocalHorizontal)
            transform.DOLocalMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.LocalVertical)
            transform.DOLocalMoveY(transform.localPosition.y + moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.GlobalHorizontal)
            transform.DOMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.GlobalVertical)
            transform.DOMoveY(moves[index], time).SetEase(easeType).OnComplete(() => Next());

    }

    void OnDisable()
    {
        if (movesType == Type.LocalHorizontal)
            transform.DOLocalMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.LocalVertical)
        {

            //transform.DOMoveY(LocalY, 0);
            //Debug.Log("Disable " + LocalY);

            transform.DOKill();

            //LocalY = transform.position.y;
        }

        if (movesType == Type.GlobalHorizontal)
            transform.DOMoveX(moves[index], time).SetEase(easeType).OnComplete(() => Next());

        if (movesType == Type.GlobalVertical)
            transform.DOMoveY(moves[index], time).SetEase(easeType).OnComplete(() => Next());

    }
}