using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CASP.SoundManager;
using Particle;
public class TransformCorrection : MonoBehaviour
{
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
        SoundManager.instance.PlayStepSound();
        ParticleManager.instance.Play("Step");
    }




}
