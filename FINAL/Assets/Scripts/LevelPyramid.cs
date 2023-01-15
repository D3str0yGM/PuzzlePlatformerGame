using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CASP.CameraManager;

public class LevelPyramid : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] private Transform TeleportDungeonTransform;
    [SerializeField] GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PuzzleManager.instance.item1 && PuzzleManager.instance.item2 && PuzzleManager.instance.item3)
        {
            anim.SetBool("FadeIn", true);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1f).OnComplete(() =>
            {
                CameraManager.instance.OpenCamera("Dungeon1Cam1", 1f, CameraEaseStates.Linear);
                anim.SetBool("FadeIn", false);

                Player.transform.position = TeleportDungeonTransform.position;

                sequence.Kill();
            });
        }

        if (other.CompareTag("Player") && transform.gameObject.name == "Dungeon1Cam2")
        {
            CameraManager.instance.OpenCamera("Dungeon1Cam2", 0f, CameraEaseStates.Linear);
            transform.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
