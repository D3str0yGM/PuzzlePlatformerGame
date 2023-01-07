using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    //******* Blade Puzzle *************
    [SerializeField] GameObject Blade;
    Sequence sequenceBladeMove;
    Sequence sequenceBlade;


    //******* Button Stone Puzzle *************
    bool button1 = false;
    bool button2 = false;
    bool button3 = false;
    int PressCount = 3;
    [SerializeField] private GameObject GlassPlatform;


    //******* Elevator Puzzle ************
    public bool isElevatorMoving = false;
    [SerializeField] private GameObject Elevator;
    public bool elUp;

    // ******* Wall Puzzle ***************
    [SerializeField] private GameObject PuzzleWall;
    [HideInInspector]
    public bool stoneDragBool1, stoneDragBool2, stoneDragBool3, stoneDragBool4 = false;
 
    public bool item1, item2, item3, item4, item5 = false;
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
        BladeStart();
    }
    public void BladeStart()
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
        Button.gameObject.layer = 0;
        PressCount--;
        Button.transform.DOMoveY(Button.transform.position.y - .12f, .5f);
        if (Button.gameObject.name == "Stone Button 1")
        {
            button1 = true;
        }
        if (Button.gameObject.name == "Stone Button 3" && button1)
        {
            button3 = true;
        }
        if (Button.gameObject.name == "Stone Button 2" && button3)
        {
            button2 = true;
        }
        if (button1 && button2 && button3)
        {
            GlassPlatform.transform.DOMoveY(GlassPlatform.transform.position.y + 2f, 2f);
            GlassPlatform.transform.DORotate(new Vector3(0, 180, 0), 3f);
        }
        if (PressCount <= 0 && !button2)
        {
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
                    ButtonsInList.gameObject.layer = 7;
                }
            });
        }
    }
    public void ElevatorUp()
    {
        if (!elUp)
        {
            elUp = true;
            isElevatorMoving = true;
            Sequence sequence = DOTween.Sequence();

            float Pos = Elevator.transform.position.y;

            sequence.AppendInterval(2f).Insert(2f, Elevator.transform.DOMoveY(Pos + 8f, 1f))
            .Insert(3f, Elevator.transform.DOMoveX(Elevator.transform.position.x + 3.5f, 1f));
            sequence.Append(Elevator.transform.DOMoveY(7.8f, 1f)).
            OnComplete(() =>
            {
                isElevatorMoving = false;
            });
        }
    }
    public void ElevatorDown()
    {
        if (elUp)
        {
            isElevatorMoving = true;
            Sequence sequence = DOTween.Sequence();

            float Pos = Elevator.transform.position.y;
            sequence.AppendInterval(2f).Insert(2f, Elevator.transform.DOMoveY(9f, 1f))
            .Insert(3f, Elevator.transform.DOMoveX(Elevator.transform.position.x - 4f, 1f))
            .Insert(4f, Elevator.transform.DOMoveY(1.5f, 1f)).OnComplete(() =>
            {
                isElevatorMoving = false;

            });
        }
    }
    public void DragStonePuzzleCheck()
    {
        if (stoneDragBool1 && stoneDragBool2 && stoneDragBool3 && stoneDragBool4)
        {
            Debug.Log("Unlocked");
        }
    }
    public void Wallin()
    {
        Sequence Wallin = DOTween.Sequence();
        Wallin.AppendInterval(1f).Insert(1f, PuzzleWall.transform.DOLocalMoveZ(PuzzleWall.transform.position.z + 65f, 1f));

    }




}

