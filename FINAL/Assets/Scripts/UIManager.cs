using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
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




    //******** PUZZLE PIECES *****************
    [SerializeField] List<Image> emptyPuzzlePieces;
    [SerializeField] List<Sprite> fullPuzzlePieces;
    [SerializeField] GameObject PiecePanel;



    [SerializeField] GameObject PlayPanel;
    [SerializeField] GameObject LoadingPanel;
    [SerializeField] Slider LoadingSlider;



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

    public void StatusText(string Mode, string Wall)
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


    public void PuzzlePieces(string itemName)
    {
        PiecePanel.SetActive(true);
        if (itemName == "item 1")
        {
            emptyPuzzlePieces[0].sprite = fullPuzzlePieces[0];
        }
        if (itemName == "item 2")
        {
            emptyPuzzlePieces[1].sprite = fullPuzzlePieces[1];
        }
        if (itemName == "item 3")
        {
            emptyPuzzlePieces[2].sprite = fullPuzzlePieces[2];
        }
        if (itemName == "item 4")
        {
            emptyPuzzlePieces[3].sprite = fullPuzzlePieces[3];
        }


    }

    public void Play()
    {
        PlayPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        DOTween.To(() => LoadingSlider.value, x => LoadingSlider.value = x, 100, 2f).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }



}
