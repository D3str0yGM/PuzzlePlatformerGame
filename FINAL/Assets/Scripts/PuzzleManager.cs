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


    //*** Button ***
    bool button1 = false;
    bool button2 = false;
    bool button3 = false;
    int PressCount = 3;





    public List<GameObject> buttonList;


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
        buttonList.Add(Button);
        Button.gameObject.layer = 1;
        PressCount--;
        Button.transform.DOMoveY(Button.transform.position.y - .12f, .5f);
        if (Button.gameObject.name == "Stone Button 1")
        {
            Debug.Log(Button.gameObject.name);
            button1 = true;
        }

        if (Button.gameObject.name == "Stone Button 3" && button1)
        {
            Debug.Log(Button.gameObject.name);
            button3 = true;
        }

        if (Button.gameObject.name == "Stone Button 2" && button3)
        {
            Debug.Log(Button.gameObject.name);
            button2 = true;
        }
        if (button1 && button2 && button3)
        {
            Debug.Log("UNLOCKED");
        }

        if (PressCount <= 0 && !button2)
        {
            Debug.Log("LOCKED");
            button1 = false;
            button2 = false;
            button3 = false;

            var seq = DOTween.Sequence();
            seq.AppendInterval(1f).OnComplete(() =>
            {
                PressCount = 3;



                foreach (GameObject ButtonsInList in buttonList)
                {
                    ButtonsInList.transform.DOMoveY(Button.transform.position.y + .12f, .5f);
                }

            });

        }
    }

}
