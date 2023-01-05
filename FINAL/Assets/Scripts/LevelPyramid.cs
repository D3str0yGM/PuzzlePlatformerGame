using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelPyramid : MonoBehaviour
{
    [SerializeField] Animator anim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Fadein");
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1.2f).OnComplete(() =>
            {
                sequence.Kill();
                SceneManager.LoadScene(1);
            });
        }
    }
}
