using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CASP.SoundManager;
using Particle;
public class TransformCorrection : MonoBehaviour
{
    [SerializeField] LayerMask GroundLayer;
    public PlayerController PlayerScript;
    public void Correction()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        PlayerScript.lockMovement = false;
    }
    public void CorrectionMain()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }
    public void StepSoundAndParticle()
    {
        ParticleManager.instance.Play("Step");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5f, GroundLayer))
        {
            if (hit.transform.gameObject.layer == 11)
            {
                SoundManager.instance.PlaySandStepSound();
                Debug.Log(hit.transform.gameObject.layer);
            }
            if (hit.transform.gameObject.layer == 12 || hit.transform.gameObject.layer == 10)
            {
                SoundManager.instance.PlayStoneStepSound();
                Debug.Log(hit.transform.gameObject.layer);
            }

        }
    }




}
