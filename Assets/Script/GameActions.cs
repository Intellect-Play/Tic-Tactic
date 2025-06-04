using System;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    public static GameActions Instance { get; private set; }

    public event Action OnStartGame;
    public event Action OnEndTurn;
    public event Action OnAttack;
    public event Action OnWinGame;
    public event Action OnLoseGame;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Opsional olaraq səhnələrarası saxlayır
    }

    // Bu metodlar vasitəsilə çağırmaq daha doğrudur:
    public void InvokeStartGame() => OnStartGame?.Invoke();
    public void InvokeEndTurn() => OnEndTurn?.Invoke();
    public void InvokeAttack() => OnAttack?.Invoke();
    public void InvokeWinGame() => OnWinGame?.Invoke();
    public void InvokeLoseGame() => OnLoseGame?.Invoke();
}
