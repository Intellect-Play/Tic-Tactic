using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterBase : MonoBehaviour
{
    public GameObject MainCharacter;
    [SerializeField] Animator characterAnimator;
    AnimatorOverrideController overrideController;

    [SerializeField] Image characterImage;
    [SerializeField] Slider healthBar;

    RectTransform rectTransform;
    Vector3 originalAnchoredPos;
    private void Awake()
    {
        characterAnimator = MainCharacter.GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        healthBar.value = 1f; // Initialize health bar to full
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
        originalAnchoredPos = rectTransform.anchoredPosition;
    }

    public void GetPosition(RectTransform _rectTransform,float scale)
    {
        if (MainCharacter != null)
        {
            transform.SetParent(_rectTransform, false);

            rectTransform.anchoredPosition = Vector2.zero;
            if (scale < 1) scale = .5f;
            else scale = 1;
            
            rectTransform.localScale = new Vector3(scale, scale, scale);
        }
        originalAnchoredPos = rectTransform.anchoredPosition;
    }
    public void Damage(float health)
    {
        AnimationStart(AnimationsEnum.Hit);

        ChangeHealth(health);
    }
    public void ChangeHealth(float health)
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }
    public void DestroyPiece()
    {
        StartCoroutine(DiedE());
        AnimationStart(AnimationsEnum.Hit);
    }
    public void Attack(RectTransform _rectTransform) {

        AnimationStart(AnimationsEnum.Attack);
        MoveToAndBack(_rectTransform);
    }
    public void MovePosition(RectTransform _rectTransform)
    {
        transform.SetParent(_rectTransform, false);

        rectTransform.DOAnchorPos(new Vector2(0, 0), 2).SetEase(Ease.OutQuad);
        rectTransform.DOScale(new Vector3(1, 1,1), 1).SetEase(Ease.OutBack);
        originalAnchoredPos = rectTransform.anchoredPosition;
    }
    public void PlayDeathEffect(float duration = 1.2f)
    {
   

        // Əsl rəngi yadda saxla
        Color originalColor = characterImage.color;

        // Qızarma rəngi: tam qırmızı yox, daha canlı və şəffaflıqla
        Color redColor = new Color(1f, 0.2f, 0.2f, 1f);

        // DOTween sequence – paralel effektlər üçün
        Sequence deathEffect = DOTween.Sequence();

        // Qızarma və fade paralel getsin
        deathEffect.Join(characterImage.DOColor(redColor, duration).SetEase(Ease.InOutQuad));
        deathEffect.Join(characterImage.DOFade(0f, duration).SetEase(Ease.OutQuad));

        // Sonda tamamilə disable et və orijinal rəngi bərpa et (əgər təkrar istifadə olunacaqsa)
        deathEffect.OnComplete(() =>
        {
            characterImage.gameObject.SetActive(false); // Obyekti gizlə
            characterImage.color = originalColor; // Əgər yenidən göstəriləcəksə, rəngi düzəlt
        });
    }
    public void MoveToAndBack(RectTransform target)
    {
      

        // Gediləcək mövqe (hədəfin local space-də mövqeyi fərqli ola bilər, amma sadə halda belə götür)
        Vector3 targetAnchoredPos = rectTransform.InverseTransformPoint(target.position);

        // DOTween ilə ardıcıl animasiya
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPos(targetAnchoredPos, 1f).SetEase(Ease.OutQuad));
        //seq.AppendInterval(0.3f);
        seq.Append(rectTransform.DOAnchorPos(originalAnchoredPos, 0.5f).SetEase(Ease.OutQuad));
    }
    public void AnimationStart(AnimationsEnum animation, RectTransform rectTransform = null)
    {
        switch (animation)
        {
            case AnimationsEnum.Idle:
                characterAnimator.SetInteger("AnimationInt",0);
                break;
            case AnimationsEnum.Attack:
                characterAnimator.SetInteger("AnimationInt", 1);
                StartCoroutine(ResetToIdleAfterAnimation());
                break;
            case AnimationsEnum.Hit:
                characterAnimator.SetInteger("AnimationInt", -1);
                StartCoroutine(ResetToIdleAfterAnimation());
                break;
        }
        //characterAnimator.SetInteger("AnimationInt", 0);

    }
    private IEnumerator ResetToIdleAfterAnimation()
    {
        // Hal-hazırki state-i al
        AnimatorStateInfo info = characterAnimator.GetCurrentAnimatorStateInfo(0);

        // İndiki animasiya bitənə qədər gözlə
        yield return new WaitForSeconds(info.length);

        characterAnimator.SetInteger("AnimationInt", 0); // Idle
    }
    private IEnumerator DiedE()
    {
        yield return new WaitForSeconds(1f);

        PlayDeathEffect();
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}
public enum AnimationsEnum
{
    Idle,
    Attack,
    Hit
}