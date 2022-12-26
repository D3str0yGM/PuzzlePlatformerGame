using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject SettingsMenu;


    public void OpenPauseMenu()
    {
        PausePanel.SetActive(true);
        PauseMenu.transform.DOScale(1f,0.1f);

    }


}
