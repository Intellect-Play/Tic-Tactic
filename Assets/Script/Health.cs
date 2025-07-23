using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public static Health Instance;
    public float HealthPlayer;
    public float HealthEnemy;
    public float MaxHealthPlayer;
    public float MaxHealthEnemy;

    public PieceType pieceType;
    //[SerializeField] TextMeshProUGUI PlayerHealthText;
    //[SerializeField] TextMeshProUGUI EnemyHealthText;

    //[SerializeField] Slider PlayerHealthBar;
    //[SerializeField] Slider EnemyHealthBar;
    public bool isLive = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        isLive = true;
   
    }

    private void Start()
    {
        GameActions.Instance.OnStartGame += StartHealth;
    }
    private void OnDisable()
    {
        GameActions.Instance.OnStartGame -= StartHealth;
    }
    public void StartHealth()
    {
        InitHealth(GameManager.Instance.currenGameUnChangedData.PlayerHP, GameManager.Instance.currenGameUnChangedData.Enemies[0].EnemyHP);

    }
    public void InitHealth(int healthPlayer,int healthEnemy)
    {
        HealthPlayer = healthPlayer;
        MaxHealthPlayer = healthPlayer;
        InitEnemy(healthEnemy);
        //PlayerHealthBar.value = 1f;
        //EnemyHealthBar.value = 1f;
        //PlayerHealthText.text = healthPlayer.ToString();
        //EnemyHealthText.text = healthEnemy.ToString();
    }
    public void InitEnemy(int healthEnemy)
    {
        HealthEnemy = healthEnemy;
        MaxHealthEnemy = healthEnemy;

    }
    public void TextEnemy(float health)
    {
        //EnemyHealthBar.value = (float)health / MaxHealthEnemy;
        //EnemyHealthText.text = health.ToString();
    }
    public void TextPlayer(float health)
    {
        //PlayerHealthBar.value = (float)health / MaxHealthPlayer;
        //PlayerHealthText.text = health.ToString();
    }
    public void Damage(int health,PieceType pieceType)
    {
        StartCoroutine(DamageDelay(health, pieceType));
    }
    IEnumerator DamageDelay(int health, PieceType pieceType)
    {
        yield return new WaitForSeconds(1);
        if (pieceType == PieceType.Player)
        {
            HealthEnemy -= health;

            PlayerController.Instance.Attack();
            AIController.Instance.Damage((float)HealthEnemy / MaxHealthEnemy);

            TextEnemy(HealthEnemy);
        }
        else
        {
            HealthPlayer -= health;

            AIController.Instance.Attack();
            PlayerController.Instance.Damage((float)HealthPlayer / MaxHealthPlayer);
        }
        ChechkDiedCase();
    }
    public void Heal(int health, PieceType pieceType)
    {
        if (pieceType == PieceType.Player)
        {
            //AIController.Instance.Damage((float)health / MaxHealthEnemy);

            HealthPlayer += health;
            TextPlayer(HealthPlayer);
        }
        else
        {

            HealthEnemy += health;

            TextEnemy(HealthEnemy);
        }
    }

    public void ChechkDiedCase()
    {
        if (!isLive) return;
         // Prevent further checks until the next game starts
        if (HealthEnemy <= 0)
        {
            AIController.Instance.DiedAI();


        }
        if (HealthPlayer <= 0) {
            isLive = false;
            GameManager.Instance.DiedCase(PieceType.Player);


        }
    }
}
