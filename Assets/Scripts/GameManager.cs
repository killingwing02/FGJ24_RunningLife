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
        currentMoney = 0;
        healthUIManager.HpUiDisplayCalculator(playerHealth, playerHealthMax);
        waitForSeconds = new WaitForSeconds(1);
    }

    private void Awake()
    {
        SingletonInit();
    }

    private void Start()
    {
        playerHealth = playerHealthMax;
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

        healthUIManager.YouDiedAnimation();
        playerControl.PlayerDiedAnimation();
        backgroundManager.ChangeFixedSpeedRatio(0f);

        onPlayerDead?.Invoke();
    }

    private void GameStart()
    {
        StartCoroutine(LossHpOverTime());
        onGameStart?.Invoke();
    }

    private IEnumerator LossHpOverTime()
    {
        if (playerHealth <= 0)
        {
            yield break;
        }

        yield return waitForSeconds;
        RemoveCurrentHp(1);
        StartCoroutine(LossHpOverTime());
    }
}
