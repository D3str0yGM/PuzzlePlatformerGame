using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject Blade;
    void Start()
    {
        Blade.transform.DORotate(new Vector3(180,0,0),0.5f).
        SetLoops(-1,LoopType.Restart);
        Blade.transform.DOMoveZ(transform.position.z-3.63f,1f).SetLoops(-1,LoopType.Yoyo);
    }

    
}
