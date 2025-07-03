using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health Instance;
    public int HealthPlayer;
    public int HealthEnemy;
    public int MaxHealth;
    public PieceType pieceType;
    [SerializeField] TextMeshProUGUI PlayerHealthText;
    [SerializeField] TextMeshProUGUI EnemyHealthText;


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
        InitHealth(GameManager.Instance.currenGameUnChangedData.PlayerHP, GameManager.Instance.currenGameUnChangedData.EnemyHP);

    }
    public void InitHealth(int healthPlayer,int healthEnemy)
    {
        HealthPlayer = healthPlayer;
        HealthEnemy = healthEnemy;

        PlayerHealthText.text = healthPlayer.ToString();
        EnemyHealthText.text = healthEnemy.ToString();
    }
    public void TextEnemy(int health)
    {
        EnemyHealthText.text = health.ToString();
    }
    public void TextPlayer(int health)
    {
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
        if (HealthEnemy <= 0)
        {
            GameManager.Instance.DiedCase(PieceType.Enemy);

        } if (HealthPlayer <= 0) {

            GameManager.Instance.DiedCase(PieceType.Player);


        }
    }
}
