using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void SingletonInit()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Managers")]
    [SerializeField] private UIManager healthUIManager;
    [SerializeField] private BackgroundManager backgroundManager;
    [SerializeField] private PlayerControl playerControl;

    [Header("Normal")]
    [SerializeField] private TMP_Text moneyLabel;
    [SerializeField] private int maxRespawnCount = 3;
    private int currentRespawnCount = 0;

    [Header("Health")]
    [SerializeField] private int playerHealthMax;
    public int playerHealth { get; private set; }

    [Header("Events")]
    public UnityEvent onGameStart;
    public UnityEvent onPlayerDead;
    public UnityEvent onPlayerGainMoney;

    public int currentMoney { get; private set; } = 0;

    private WaitForSeconds waitForSeconds;

    private void Initialize()
    {
        healthUIManager.HpUiDisplayCalculator(playerHealth, playerHealthMax);

        backgroundManager.Initialize();
        playerControl.Initialize();

        waitForSeconds = new WaitForSeconds(1);
    }

    private void Awake()
    {
        SingletonInit();
    }

    private void Start()
    {
        currentMoney = 0;
        SetCurrentHpFull();
        Initialize();
        GameStart();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        moneyLabel.text = currentMoney.ToString();

        onPlayerGainMoney?.Invoke();
    }

    public void SetCurrentHp(int hp)
    {
        playerHealth = hp;
        PlayerDeadCheck();
        healthUIManager.HpUiDisplayCalculator(playerHealth, playerHealthMax);
    }

    public void SetCurrentHpFull()
    {
        SetCurrentHp(playerHealthMax);
    }

    public void RemoveCurrentHp(int hp)
    {
        playerHealth -= hp;
        PlayerDeadCheck();
        healthUIManager.HpUiDisplayCalculator(playerHealth, playerHealthMax);
    }

    private void PlayerDeadCheck()
    {
        playerHealth = Mathf.Clamp(playerHealth, 0, playerHealthMax);
        if (playerHealth > 0) return;

        Deadge();
    }

    private void GameStart()
    {
        StartCoroutine(LossHpOverTime());
        onGameStart?.Invoke();
    }

    private void GameOver()
    {
        Debug.Log("Game over!");
        LeanTween.delayedCall(7.5f, () => healthUIManager.Ending(currentMoney));
    }

    private void Deadge()
    {
        healthUIManager.YouDiedAnimation();
        playerControl.PlayerDiedAnimation();
        backgroundManager.ChangeFixedSpeedRatio(0f);
        currentRespawnCount++;

        onPlayerDead?.Invoke();

        // Game over check
        if (currentRespawnCount >= maxRespawnCount) GameOver();
        else
        {
            LeanTween.delayedCall(7.5f, () =>
            {
                SetCurrentHpFull();
                Initialize();
                GameStart();
            });
        }
    }

    private IEnumerator LossHpOverTime()
    {
        yield return waitForSeconds;
        
        if (playerHealth <= 0)
        {
            yield break;
        }
        RemoveCurrentHp(1);
        StartCoroutine(LossHpOverTime());
    }
}
