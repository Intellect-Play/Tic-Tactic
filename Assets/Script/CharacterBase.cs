using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CartoonFX;
using TMPro;

public class CharacterBase : MonoBehaviour
{
    public GameObject MainCharacter;
    [SerializeField] public Animator characterAnimator;
    AnimatorOverrideController overrideController;

    [SerializeField] Image characterImage;
    [SerializeField] Slider healthBar;
    [SerializeField] ParticleSystem HitEnemy;
    [SerializeField] ParticleSystem DamageParticle;
    [SerializeField] List<Image> DeathImages;

    [SerializeField] ShakeEffect HitShakeEnemy;
    [SerializeField] TextMeshProUGUI HealthCharacter;

    Button attackButton;
    RectTransform rectTransform;
    Vector3 originalAnchoredPos;
    int healthCharacter;

    private Color flashColor = Color.red;
    private int flashCount = 2;
    private float flashDuration = .3f;
    Sequence flashSequence;
    private Color originalColor;
    private void Awake()
    {
        characterAnimator = MainCharacter.GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        healthBar.value = 1f; // Initialize health bar to full
        attackButton = MainCharacter.GetComponent<Button>();
        if(attackButton!=null)
        attackButton.onClick.AddListener(AttackButton);
        originalColor = characterImage.color;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void SetupSpecial(CharacterData _character, float health)
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
        healthCharacter = (int)health;
        HealthCharacter.text = health.ToString()+"/"+health.ToString();
    }
    public void AttackButton()
    {
        if (MainCharacter != null&&attackButton!=null)
        {
            AnimationStart(AnimationsEnum.Hit);
        }
    }
    public void GetPosition(RectTransform _rectTransform,float scale)
    {
        if (MainCharacter != null)
        {
            transform.SetParent(_rectTransform, false);

            rectTransform.anchoredPosition = Vector2.zero;
            if (scale < 1) scale = .5f;
            else scale = 1;
            
            //rectTransform.localScale = new Vector3(scale, scale, scale);
        }
        originalAnchoredPos = rectTransform.anchoredPosition;
    }
    public void Damage(float health)
    {
        StartCoroutine(DamageE(health));

      
    }
    public void ChangeHealth(float health)
    {
        if (healthBar != null)
        {
            if(health <= 0)
            {
                health = 0;
            }
            HealthCharacter.text = health.ToString() + "/" + healthCharacter.ToString();

            healthBar.value = (float)health/healthCharacter;
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
        SoundManager.Instance.PlaySound(SoundType.Swoop);

    }
    public void MovePosition(RectTransform _rectTransform)
    {
        transform.SetParent(_rectTransform);

        rectTransform.DOAnchorPos(new Vector2(0, 0), 3.5f).SetEase(Ease.Linear);
       // rectTransform.DOScale(new Vector3(1, 1,1), 1).SetEase(Ease.OutBack);
        //originalAnchoredPos = rectTransform.anchoredPosition;
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
        foreach (var image in DeathImages)
        {
            // Hər bir image üçün fade out
            deathEffect.Join(image.DOFade(0f, duration).SetEase(Ease.OutQuad));
        }
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
            case AnimationsEnum.Run:
                characterAnimator.SetBool("Run",true);
                //StartCoroutine(ResetToIdleAfterAnimation());
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
    private IEnumerator DamageE(float health)
    {
        AnimationStart(AnimationsEnum.Hit);
        
        ChangeHealth(health);
        yield return new WaitForSeconds(.6f);
        Flash();
        if (DamageParticle != null)
            DamageParticle.Play();
    }
    public void Flash()
    {
        if (flashSequence!=null&&flashSequence.active) return;
        characterImage.DOKill();
        flashSequence = DOTween.Sequence();

        for (int i = 0; i < flashCount; i++)
        {
            flashSequence.Append(characterImage.DOColor(flashColor, flashDuration));
            flashSequence.Append(characterImage.DOColor(originalColor, flashDuration));
        }

        flashSequence.Play();
    }
    private IEnumerator DiedE()
    {
        yield return new WaitForSeconds(.8f);
        if (HitEnemy != null)
        {
            HitEnemy.gameObject.SetActive(true);
            HitShakeEnemy.Shake();

        }
        PlayDeathEffect(); 
        yield return new WaitForSeconds(.7f); 
        if (HitEnemy != null)
        {

            HitShakeEnemy.Shake();
        }
        yield return new WaitForSeconds(.3f);

        Destroy(gameObject);
    }
}
public enum AnimationsEnum
{
    Idle,
    Attack,
    Hit,
    Run
}