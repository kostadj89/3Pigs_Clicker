using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum UpgradeType
{
    ClickStrength,
    EnergyPerSecond,
    Level
}
public class UpgradeButton : MonoBehaviour
{
    public Transform gameManagerTransform;
    public UpgradeType upgradeType;
    public double baseCost=10;
    public double costMultiplyer=1;
    public int valueMultiplyer = 1;
    //value and cost labels
    public Text numberOfUpgradesTxt;
    public Text costTxt;

    private int numberOfUpgrades = 0;
    private ClickerGM gameManager;
    private double cost;
    


    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerTransform.GetComponent<ClickerGM>();
        cost = Mathf.Floor((float)((baseCost + numberOfUpgrades) * Mathf.Pow((float)costMultiplyer, numberOfUpgrades)));
        costTxt.text = cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Upgrade()
    {
        double currentScore = gameManager.GetCurrentScore();
        if (currentScore >= cost)
        {
            //inc number of bought upgrades
            numberOfUpgrades++;

            //deduct the cost from score
            currentScore -= cost;
            gameManager.SetScore(currentScore);

            //setting new value based on baseValue and number of upgrades
            double targetValue = CalculateValue();       
            
            SetTargetValue(targetValue);

            //setting new cost
            cost = Mathf.Floor((float)((baseCost + numberOfUpgrades) * Mathf.Pow((float)costMultiplyer, numberOfUpgrades)));

            numberOfUpgradesTxt.text = numberOfUpgrades.ToString();
            costTxt.text = cost.ToString();
        }
    }

    private void SetTargetValue(double targetValue)
    {
        switch (upgradeType)
        {
            case UpgradeType.ClickStrength:
                 gameManager.SetClickStrength(targetValue);
                break;
            case UpgradeType.EnergyPerSecond:
                gameManager.SetEPerSec(targetValue);
                break;
            case UpgradeType.Level:
                gameManager.SetLevel(targetValue);
                break;                
            default:
                return;
        }
    }

    private double CalculateValue()
    {
        switch (upgradeType)
        {
            case UpgradeType.ClickStrength:
                return gameManager.GetBaseClickStrength() + numberOfUpgrades * valueMultiplyer; ;
            case UpgradeType.EnergyPerSecond:
                return gameManager.GetEPerSec()+valueMultiplyer;
            case UpgradeType.Level:
                return gameManager.GetBaseLevel();
            default:
                return 0;
        }


    }
}
