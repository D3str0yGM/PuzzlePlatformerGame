using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class UIManager : MonoBehaviour
{

    [SerializeField] TMP_Text statusText;
    [SerializeField] TMP_Text horizontalText;
    [SerializeField] TMP_Text verticalText;
    [SerializeField] TMP_Text WallCheckerText;



    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject SettingsMenu;


    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void OpenPauseMenu()
    {
        PausePanel.SetActive(true);
        PauseMenu.transform.DOScale(1f, 0.1f);
    }

    public void StatusText(string Mode,string Wall)
    {
        statusText.text = "Mode: " + Mode;
        WallCheckerText.text = "Wall: " + Wall;
    }

    public void HorizontalText(float horizontal)
    {
        horizontalText.text = "SpeedH: " + horizontal;
    }
    public void VerticalText(float vertical)
    {
        verticalText.text = "SpeedV" + vertical;
    }


}
