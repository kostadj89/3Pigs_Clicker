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

public enum EnergyUpgradeSubtypes
{
    None,
    SolarPanel,
    Tourbines,
    Robot
}
public class UpgradeButton : MonoBehaviour
{
    public Transform gameManagerTransform;
    public UpgradeType upgradeType;
    public EnergyUpgradeSubtypes energyUpgradeSubtypes = EnergyUpgradeSubtypes.None;
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

        //add yourself to the list of upgrade buttons
        gameManager.AddToUpgradeButtons(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Upgrade()
    {
        double currentScore = gameManager.GetCurrentCoins();
        if (currentScore >= cost)
        {
            //inc number of bought upgrades
            numberOfUpgrades++;

            //deduct the cost from coins
            //currentScore -= cost;
            gameManager.PayCost(cost);

            //setting new value based on baseValue and number of upgrades
            double targetValue = CalculateValue();       
            
            SendTargetValue(targetValue);
            SetUpgradeVisibility();

            //setting new cost
            cost = Mathf.Floor((float)((baseCost + numberOfUpgrades) * Mathf.Pow((float)costMultiplyer, numberOfUpgrades)));

            numberOfUpgradesTxt.text = numberOfUpgrades.ToString();
            costTxt.text = cost.ToString();

            PlaySound();
        }
    }

    private void SetUpgradeVisibility()
    {
        int upgradeIndex=-1;

        if ( numberOfUpgrades == 1 && upgradeType == UpgradeType.EnergyPerSecond)
        {
            switch (energyUpgradeSubtypes)
            {
                case EnergyUpgradeSubtypes.None:
                    return;
                case EnergyUpgradeSubtypes.SolarPanel:
                    upgradeIndex = 0;
                    break;
                case EnergyUpgradeSubtypes.Tourbines:
                    upgradeIndex = 1;
                    break;
                case EnergyUpgradeSubtypes.Robot:
                    upgradeIndex = 2;
                    break;               
                default:
                    return;
            }

            gameManager.TurnOnUpgradeVisibility(upgradeIndex);
        }
        else if(upgradeType == UpgradeType.Level)
        {
            for (int i = 0; i < 3; i++)
            {
                gameManager.TurnOffUpgradeVisibility(i);
            }
        }

    }

    private void PlaySound()
    {
        return;
    }

    //this method sends the values of various fields to the game manager, ie. strength of the click, energy per second, or increases level number
    private void SendTargetValue(double targetValue)
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
                //return gameManager.GetClickStrength() + numberOfUpgrades * valueMultiplyer ;
                return gameManager.baseClickStrength + numberOfUpgrades * valueMultiplyer;
            case UpgradeType.EnergyPerSecond:
                return gameManager.GetEPerSec()+valueMultiplyer;
            case UpgradeType.Level:
                return gameManager.GetCoinsPerE()+1;
            default:
                return 0;
        }


    }

    public void ResetUpgrades()
    {
        //we want to show on which level we're on
        if (upgradeType != UpgradeType.Level)
        {
            numberOfUpgrades = 0;
            numberOfUpgradesTxt.text = numberOfUpgrades.ToString();
        }
        
        //reset all costs
        cost = baseCost;
        costTxt.text = cost.ToString();
        
    }
}
