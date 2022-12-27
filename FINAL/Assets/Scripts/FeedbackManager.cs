using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    public MMFeedbacks ModeChanged;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
