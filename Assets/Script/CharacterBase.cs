using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
    public GameObject MainCharacter;
    [SerializeField] Animator characterAnimator;
    AnimatorOverrideController overrideController;

    [SerializeField] Image characterImage;
    [SerializeField] Slider healthBar;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetupSpecial(CharacterData _character)
    {
        characterImage.sprite = _character.CharacterSprite;

        overrideController = new AnimatorOverrideController(characterAnimator.runtimeAnimatorController);
        characterAnimator.runtimeAnimatorController = overrideController;

        overrideController["_Idle"] = _character.AnimationIdle;
        overrideController["_Attack"] = _character.AnimationAttack;
        overrideController["_Hit"] = _character.AnimationHit;
        characterAnimator.Rebind();

        AnimationStart(AnimationsEnum.Idle);

    }

    public void GetPosition(RectTransform _rectTransform,float scale)
    {
        if (MainCharacter != null)
        {
            transform.SetParent(_rectTransform, false);

            rectTransform.anchoredPosition = Vector2.zero;
            Debug.Log(scale);
            rectTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
    public void SetHealthBar(float health)
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    public void AnimationStart(AnimationsEnum animation)
    {
        switch (animation)
        {
            case AnimationsEnum.Idle:
                characterAnimator.SetInteger("AnimationInt",0);
                break;
            case AnimationsEnum.Attack:
                characterAnimator.SetInteger("AnimationInt", 1);
                break;
            case AnimationsEnum.Hit:
                characterAnimator.SetInteger("AnimationInt", -1);
                break;
        }
    }
}
public enum AnimationsEnum
{
    Idle,
    Attack,
    Hit
}