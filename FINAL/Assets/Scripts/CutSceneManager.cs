using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CASP.CameraManager;

public class CutSceneManager : MonoBehaviour
{

    private void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.instance.OpenCamera("Cutscene2", 3f, CameraEaseStates.Linear);
            StartCoroutine(Back2PlayerCam());
        }



        IEnumerator Back2PlayerCam()
        {
            yield return new WaitForSeconds(4f);
            CameraManager.instance.OpenCamera("3D Cam", 1f, CameraEaseStates.Linear);

        }
    }
}
