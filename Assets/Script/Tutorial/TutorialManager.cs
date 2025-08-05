using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }


    [Header("Objects")]
    public GameObject tutorialCanvas;
    public GameObject tutorialHand;
    public TutorialHandAnimator tutorialHandAnimator;

    [Header("Tutorial Settings")]
    [SerializeField] private Vector3 ButtonClickOffset = new Vector3(280,70,0);
    [SerializeField] private Button EndTurnButton;
    [SerializeField] private Button StartButton;
    [SerializeField] private Button ShopButton;
    [SerializeField] private RectTransform EndTurnImage;
    private Tweener currentTween;
    public Cell cell;
    public bool IsTutorialActive = false;
    public bool IsTutorialActiveBuy = false;

    int tutorialLevel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        IsTutorialActive = SaveDataService.CurrentLevel == 1;
       
        //if (PlayerPrefs.GetInt("Tutorial2", 0) == 0)
        //{
        //    tutorialLevel = 0;
        //    StartButton.interactable = true;
        //    ShopButton.interactable = false;
        //    //EndTurnButton.interactable = false;
        //}
        //else if(PlayerPrefs.GetInt("Tutorial2", 0) == 1)
        //{
        //    tutorialHandAnimator.gameObject.SetActive(true);

        //    StartButton.interactable = false;
        //    ShopButton.interactable = true;
        //  //  tutorialHandAnimator.ShowTapAnimationUI(ShopButton.gameObject.GetComponent<RectTransform>(), new Vector3(-300, 0, 0));

        //}
        //else
        //{
        //    HideTutorialHand();
        //    IsTutorialActive = false;

        //}
    }
  

    public void TutorialStart()
    {
        if (IsTutorialActive)
        {
            EndTurnImage = GameManager.Instance.pieceSpawner.PlayerPieceParent[0].GetComponent<RectTransform>();

            tutorialHandAnimator.ShowMoveHandAnimationUI(EndTurnImage, new Vector3(0, 0, 0));
            //IsTutorialActive = false;
        }

    }

    public void TutorialEnd()
    {Debug.Log("Tutorial Ended");
        HideTutorialHand();
        IsTutorialActive = false;
    }
 
 
    public void HideHandTouchEndTurn()
    {
        currentTween?.Kill();
        currentTween = null;
       // EndTurnImage.gameObject.SetActive(false);

    }
   
    public void HideTutorialMoveHand()
    {
        if (!IsTutorialActive || tutorialLevel == 1 || tutorialLevel==4) return;
        tutorialHandAnimator.HideHandTouch();
    }
    public void HideTutorialHand()
    {
        Debug.Log("HideTutorialHand called");
        if (!IsTutorialActive) return;
        tutorialHandAnimator.HideHandTouch();
    }

   
}
