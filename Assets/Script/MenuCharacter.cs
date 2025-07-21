using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MenuCharacter : MonoBehaviour
{
    public GameObject MainCharacter;
    [SerializeField] Animator characterAnimator;
    AnimatorOverrideController overrideController;


    public AnimationClip AnimationIdle;
    public AnimationClip AnimationAttack;
    public AnimationClip AnimationHit;

    public AnimationsEnum animationsEnum;
    private void Start()
    {
        characterAnimator = MainCharacter.GetComponent<Animator>();
       SetupSpecial();
    }
    public void SetupSpecial()
    {
      

        overrideController = new AnimatorOverrideController(characterAnimator.runtimeAnimatorController);
        characterAnimator.runtimeAnimatorController = overrideController;

        overrideController["_Idle"] = AnimationIdle;
        overrideController["_Attack"] = AnimationAttack;
        overrideController["_Hit"] = AnimationHit;
        characterAnimator.Rebind();

        AnimationStart(animationsEnum);
   
    }
    public void AttackButton()
    {
        if (MainCharacter != null )
        {
            AnimationStart(AnimationsEnum.Hit);
        }
    }
   
    public void AnimationStart(AnimationsEnum animation, RectTransform rectTransform = null)
    {
        switch (animation)
        {
            case AnimationsEnum.Idle:
                characterAnimator.SetInteger("AnimationInt", 0);
                break;
            case AnimationsEnum.Attack:
                characterAnimator.SetInteger("AnimationInt", 1);
               // StartCoroutine(ResetToIdleAfterAnimation());
                break;
            case AnimationsEnum.Hit:
                characterAnimator.SetInteger("AnimationInt", -1);
               // StartCoroutine(ResetToIdleAfterAnimation());
                break;
        }
        //characterAnimator.SetInteger("AnimationInt", 0);

    }
    //private IEnumerator ResetToIdleAfterAnimation()
    //{
    //    // Hal-hazırki state-i al
    //    AnimatorStateInfo info = characterAnimator.GetCurrentAnimatorStateInfo(0);

    //    // İndiki animasiya bitənə qədər gözlə
    //    yield return new WaitForSeconds(info.length);

    //    characterAnimator.SetInteger("AnimationInt", 0); // Idle
    //}
 
}
