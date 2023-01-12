using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CASP.CameraManager;
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
    [SerializeField] Image PausePanelImage;
    bool vibrationActive = false;
    [SerializeField] GameObject check;

    [SerializeField] GameObject VideoPlayer;

    [SerializeField] GameObject Skill2D;







    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        PausePanel.SetActive(true);
        DOTween.To(() => PausePanelImage.color, x => PausePanelImage.color = x, new Color32(255, 255, 255, 233), 0.2f);
        PauseMenu.transform.DOScale(1.13f, 0.15f);
    }

    public void Resume()
    {
        DOTween.To(() => PausePanelImage.color, x => PausePanelImage.color = x, new Color32(255, 255, 255, 0), 0.2f);
        PauseMenu.transform.DOScale(0f, 0.15f);
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
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
            CameraManager.instance.OpenCamera("3D Cam", 6f, CameraEaseStates.Linear);
            LoadingPanel.SetActive(false);


        });
    }

    public void Vibration()
    {
        vibrationActive = !vibrationActive;
        if (vibrationActive)
        {
            check.SetActive(true);
        }
        if (!vibrationActive)
        {
            check.SetActive(false);
        }
    }

    public void PlayVideo()
    {
        VideoPlayer.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(5f).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        });
    }

    public void SkillZoom()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Skill2D.transform.DOScale(0.2517883f,0.2f)).Insert(0.3f,Skill2D.transform.DOScale(0.22231f,0.2f));

    }

}
