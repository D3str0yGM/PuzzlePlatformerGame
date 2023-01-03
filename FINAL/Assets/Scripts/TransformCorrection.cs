using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCorrection : MonoBehaviour
{
    public PlayerController PlayerScript;
    public void Correction()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        PlayerScript.lockMovement = false;
    }

    


}
