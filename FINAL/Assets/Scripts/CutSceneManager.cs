using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CASP.CameraManager;

public class CutSceneManager : MonoBehaviour
{

  


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transform.CompareTag("Cutscene2"))
        {
            CameraManager.instance.OpenCamera("Cutscene2", 3f, CameraEaseStates.Linear);
            transform.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Back2PlayerCam());
        }

        if (other.CompareTag("Player") && transform.CompareTag("Cutscene3"))
        {
            CameraManager.instance.OpenCamera("Cutscene3", 3f, CameraEaseStates.Linear);
            transform.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Back2PlayerCam());
        }



        IEnumerator Back2PlayerCam()
        {
            yield return new WaitForSeconds(4f);
            CameraManager.instance.OpenCamera("3D Cam", 1f, CameraEaseStates.Linear);

        }
    }
}
