using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Particle;
using CASP.SoundManager;
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    GameObject Player;
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
    [SerializeField] private GameObject ElevatorModel;



    public bool elUp;
    [SerializeField] Material withChar;
    [SerializeField] Material withoutChar;

    // ******* Wall Puzzle ***************
    [SerializeField] private GameObject PuzzleWall;
    [HideInInspector]
    public bool stoneDragBool1, stoneDragBool2, stoneDragBool3, stoneDragBool4 = false;

    public bool item1, item2, item3, item4 = false;
    public List<GameObject> buttonList;
    //*********** Drag Stone Puzzle ***************
    [SerializeField] GameObject[] CollectableItems;
    [SerializeField] GameObject StoneGO;

    //************ DUNGEON ***************************
    [SerializeField] List<GameObject> CollectedItems;
    [SerializeField] GameObject pisaGO;
    [SerializeField] GameObject ritualGO;

    [SerializeField] Transform[] RitualTransform;







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
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public void BladeStart()
    {
        sequenceBladeMove = DOTween.Sequence();
        // sequenceBlade = DOTween.Sequence();
        sequenceBladeMove.Append(Blade.transform.DORotate(new Vector3(180, 0, 0), 0.5f)).
        SetLoops(-1, LoopType.Restart);
        //sequenceBlade.Append(Blade.transform.DOMoveZ(Blade.transform.position.z - 4.4f, 1.2f)).SetLoops(-1, LoopType.Yoyo);
    }
    public void BladeKill()
    {
        Blade.GetComponentInParent<BoxCollider>().enabled = false;
        var seq = DOTween.Sequence();
        seq.AppendInterval(1f).OnComplete(() =>
        {
            sequenceBladeMove.Kill();
            // sequenceBlade.Kill();
            seq.Kill();
        });
    }
    public void ButtonPress(GameObject Button)
    {
        Button.gameObject.layer = 0;
        PressCount--;
        Button.transform.DOMoveY(Button.transform.position.y - .09f, .5f);
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
            GlassPlatform.transform.DOMoveY(GlassPlatform.transform.position.y - 1.6f, 1f);
            GlassPlatform.transform.DOLocalRotate(new Vector3(0, -124f, 0), 1.5f).OnComplete(() =>
            {
                GlassPlatform.transform.DOLocalMove(new Vector3(2.706399f, 2.443f, -3.416861f), 0.8f);
            });
            CollectableItems[2].GetComponent<BoxCollider>().enabled = true;
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
                    ButtonsInList.transform.DOMoveY(Button.transform.position.y + .09f, .5f);
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

            sequence.AppendInterval(3f).Insert(2f, Elevator.transform.DOMoveY(Pos + 4.5f, 1f))
            .Insert(3f, Elevator.transform.DOMoveX(Elevator.transform.position.x + 2.2f, 1f));
            sequence.Append(Elevator.transform.DOMoveY(4.9f, 1f)).
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
            sequence.AppendInterval(3f).Insert(2f, Elevator.transform.DOMoveY(5.2f, 1f))
            .Insert(3f, Elevator.transform.DOMoveX(Elevator.transform.position.x - 2.2f, 1f))
            .Insert(4f, Elevator.transform.DOMoveY(1.1f, 1f)).OnComplete(() =>
            {
                isElevatorMoving = false;

            });
        }
    }
    public void DragStonePuzzleCheck()
    {
        if (stoneDragBool1 && stoneDragBool2 && stoneDragBool3 && stoneDragBool4)
        {
            Debug.Log("UNLOCKED ITEM1");
            CollectableItems[0].GetComponent<BoxCollider>().enabled = true;
            StoneGO.transform.DOMoveY(StoneGO.transform.position.y + 1f, 1f); //stone button scriptinden bool gelir

        }
    }
    public void ItemUnlocked(GameObject item) //umumi collectable item 
    {
        if (item.gameObject.name == "item 1")
        {
            item1 = true;
        }
        if (item.gameObject.name == "item 2")
        {
            item2 = true;
        }
        if (item.gameObject.name == "item 3")
        {
            item3 = true;
        }
        if (item.gameObject.name == "item 4")
        {
            item4 = true;
        }
    }
    public void Wallin()
    {
        Sequence Wallin = DOTween.Sequence();
        Wallin.AppendInterval(1f).Insert(1f, PuzzleWall.transform.DOLocalMoveZ(PuzzleWall.transform.position.z + 10.25f, 3f));

    }

    public void ElevatorwithCharacter()
    {
        ElevatorModel.GetComponent<Renderer>().material = withChar;
        Debug.Log("with");
    }
    public void ElevatorwithoutCharacter()
    {
        Debug.Log("without");
        ElevatorModel.GetComponent<Renderer>().material = withoutChar;
    }

    public void Ritual()
    {
        int i = 0;
        ritualGO.gameObject.GetComponent<BoxCollider>().enabled = false;
        ParticleManager.instance.Play("Ritual");
        Sequence RitualSequence = DOTween.Sequence();
        Sequence PisaSpawn = DOTween.Sequence();
        foreach (var item in CollectedItems)
        {
            item.SetActive(true);
            RitualSequence.Append(item.transform.DOJump(RitualTransform[0 + i].position, 1, 1, 0.6f)).Join(item.transform.DOScale(new Vector3(35f, 35f, 35f), 0.6f));
            item.transform.parent = null;
            i++;
        }
        PisaSpawn.AppendInterval(6f).OnComplete(() =>
        {
            ParticleManager.instance.Play("Pisa");
            SoundManager.instance.Play("Portal",true);
            
            pisaGO.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(2f).OnComplete(() =>
            {
                pisaGO.GetComponent<BoxCollider>().enabled = true;

            });

            foreach (var item in CollectedItems)
            {
                item.SetActive(false);
            }
        });

    }

}

