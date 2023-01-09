using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelPyramid : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] private Transform TeleportDungeonTransform;
    [SerializeField] GameObject Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PuzzleManager.instance.item1 && PuzzleManager.instance.item2 && PuzzleManager.instance.item3 && PuzzleManager.instance.item4)
        {
            anim.SetBool("FadeIn",true);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1f).OnComplete(() =>
            {
            anim.SetBool("FadeIn",false);

                Player.transform.position = TeleportDungeonTransform.position;

                sequence.Kill();
            });
        }
    }
}
