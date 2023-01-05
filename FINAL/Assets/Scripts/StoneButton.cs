using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StoneButton : MonoBehaviour
{
    Collider[] colliders;
    [SerializeField] LayerMask puzzleLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (this.transform.gameObject.name == "Button 1" && other.transform.gameObject.name == "Stone 1")
        {
            transform.DOMoveY(transform.position.y - .05f, .5f);
            PuzzleManager.instance.stoneDragBool1 = true;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
        if (this.transform.gameObject.name == "Button 2" && other.transform.gameObject.name == "Stone 2")
        {
            transform.DOMoveY(transform.position.y - .05f, .5f);
            PuzzleManager.instance.stoneDragBool2 = true;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
        if (this.transform.gameObject.name == "Button 3" && other.transform.gameObject.name == "Stone 3")
        {
            transform.DOMoveY(transform.position.y - .05f, .5f);
            PuzzleManager.instance.stoneDragBool3 = true;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
        if (this.transform.gameObject.name == "Button 4" && other.transform.gameObject.name == "Stone 4")
        {
            transform.DOMoveY(transform.position.y - .05f, .5f);
            PuzzleManager.instance.stoneDragBool4 = true;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
    }

    private void OnTriggerExit(Collider other)

    {
        if (this.transform.gameObject.name == "Button 1" && other.transform.gameObject.name == "Stone 1")
        {
            transform.DOMoveY(transform.position.y + .05f, .5f);
            PuzzleManager.instance.stoneDragBool1 = false;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
        if (this.transform.gameObject.name == "Button 2" && other.transform.gameObject.name == "Stone 2")
        {
            transform.DOMoveY(transform.position.y + .05f, .5f);
            PuzzleManager.instance.stoneDragBool2 = false;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
        if (this.transform.gameObject.name == "Button 3" && other.transform.gameObject.name == "Stone 3")
        {
            transform.DOMoveY(transform.position.y + .05f, .5f);
            PuzzleManager.instance.stoneDragBool3 = false;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
        if (this.transform.gameObject.name == "Button 4" && other.transform.gameObject.name == "Stone 4")
        {
            transform.DOMoveY(transform.position.y + .05f, .5f);
            PuzzleManager.instance.stoneDragBool4 = false;
            PuzzleManager.instance.DragStonePuzzleCheck();
        }
    }
}
