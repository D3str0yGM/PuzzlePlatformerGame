using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    [SerializeField] GameObject Blade;
    Sequence sequenceBladeMove;
    Sequence sequenceBlade;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    void Start()
    {
        sequenceBladeMove = DOTween.Sequence();
        sequenceBlade = DOTween.Sequence();



        sequenceBladeMove.Append(Blade.transform.DORotate(new Vector3(180, 0, 0), 0.5f)).
        SetLoops(-1, LoopType.Restart);
        sequenceBlade.Append(Blade.transform.DOMoveZ(Blade.transform.position.z - 6.49f, 1.2f)).SetLoops(-1, LoopType.Yoyo);
    }

    public void BladeKill()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(1f).OnComplete(() =>
        {
            sequenceBladeMove.Kill();
            sequenceBlade.Kill();
            seq.Kill();
        });
    }
    public void ButtonPress(GameObject Button)
    {
        Button.transform.DOMoveY(Button.transform.position.y - .12f, .5f);
    }

}
