using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void SingletonInit()
    {
        if (Instance == null)
        {
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

    [Header("Normal")]
    [SerializeField] private TMP_Text moneyLabel;

    [Header("Health")]
    [SerializeField] private int playerHealthMax;
    public int playerHealth { get; private set; }

    public int currentMoney { get; private set; } = 0;

    private void Initialize()
    {
        currentMoney = 0;
        healthUIManager.HpUiDisplayCalculator(playerHealth, playerHealthMax);
    }

    private void Awake()
    {
        SingletonInit();
    }

    private void Start()
    {
        playerHealth = playerHealthMax;
        Initialize();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        moneyLabel.text = currentMoney.ToString();
    }

    public void SetCurrentHp(int hp)
    {
        playerHealth = hp;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerHealthMax);
        if (playerHealth == 0) healthUIManager.YouDiedAnimation();
        healthUIManager.HpUiDisplayCalculator(playerHealth, playerHealthMax);
    }

    public void RemoveCurrentHp(int hp)
    {
        playerHealth -= hp;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerHealthMax);
        if (playerHealth == 0) healthUIManager.YouDiedAnimation();
        healthUIManager.HpUiDisplayCalculator(playerHealth, playerHealthMax);
    } 
}
