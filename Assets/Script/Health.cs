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
    [SerializeField] TextMeshProUGUI PlayerHealthText;
    [SerializeField] TextMeshProUGUI EnemyHealthText;

    [SerializeField] Slider PlayerHealthBar;
    [SerializeField] Slider EnemyHealthBar;
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
        HealthEnemy = healthEnemy;
        MaxHealthPlayer = healthPlayer;
        MaxHealthEnemy = healthEnemy;

        PlayerHealthBar.value = 1f;
        EnemyHealthBar.value = 1f;
        PlayerHealthText.text = healthPlayer.ToString();
        EnemyHealthText.text = healthEnemy.ToString();
    }
    public void TextEnemy(float health)
    {
        EnemyHealthBar.value = (float)health / MaxHealthEnemy;
        EnemyHealthText.text = health.ToString();
    }
    public void TextPlayer(float health)
    {
        PlayerHealthBar.value = (float)health / MaxHealthPlayer;
        PlayerHealthText.text = health.ToString();
    }
    public void Damage(int health,PieceType pieceType)
    {
        if (pieceType == PieceType.Player)
        {
            HealthEnemy -= health;
            TextEnemy(HealthEnemy);
        }
        else { 
        
            HealthPlayer -= health;
            TextPlayer(HealthPlayer);
        }
        ChechkDiedCase();
    }
    public void Heal(int health, PieceType pieceType)
    {
        if (pieceType == PieceType.Player)
        {
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
            isLive = false;
            GameManager.Instance.DiedCase(PieceType.Enemy);

        } if (HealthPlayer <= 0) {
            isLive = false;
            GameManager.Instance.DiedCase(PieceType.Player);


        }
    }
}
