using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StoneButton : MonoBehaviour
{
    bool button1 = false;
    private void OnTriggerEnter(Collider other)
    {
        if (this.transform.gameObject.name == "Button 1" && other.transform.gameObject.name == "Stone 1")
        {
            // transform.DOMoveY(transform.position.y - 0.1f, 1f);
            button1 = true;
        }
    }
    private void Update()
    {
        Debug.Log(button1);
    }
}
