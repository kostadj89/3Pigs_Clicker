using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGM : MonoBehaviour
{
    #region const
    //maybe expose these as public vars to tune from editor
    #endregion const

    #region public

    //base values    
    public int baseClickStrength = 1;
    public int baseStrengthUpgCost = 10;

    public int baseEPerSec = 0;
    public int baseEPerSecUpgCost = 100;
    public int baseLevel = 1;
    //values
    //this will depend on the level
    public double levelCoinsPerEnergy = 1;

    // text for the coins
    public Text CoinsScoreTxt;
    // text for the strength of click displayed on main genrate button
    public Text ClickStrengthTxt;
    // text for the energy per second
    public Text EnergyPerSecTxt;   

    public float strengthMult = 1.08f;
    public float energyMult = 2f;

    public Text CoinsPerEnergy;

    public GameObject Upgrade1;
    public GameObject Upgrade2;
    public GameObject Upgrade3;
    #endregion public

    #region private

    //list of upgrade bttns
    private List<UpgradeButton> upgradeButtons;

    private double coins;

    //relating to strength of click upgrade
    private double clickStrength = 1;
    //private double costOfStrengthUpg = baseStrengthUpgCost ;

    //relating to energy per second upgrade
    private double energyPerSec = 0;
    //private double costOfEPerSecUpg = baseEPerSecUpgCost;
    //upgrades
    private int upgradesStrengthOfClick = 0;
    private int upgradesEPerSec = 0;

    //sound
    AudioSource audioSource;
    AudioClip audioClip;

    #endregion private

    private void Awake()
    {
        upgradeButtons = new List<UpgradeButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //turn off upgrade visibility
        for (int i = 0; i < 3; i++)
        {
            TurnOffUpgradeVisibility(i);
        }

        /*if we're going to get a save system, then here we could calculate all thevalues based on the level of upgrades*/
        StartCoroutine(AddEnergyPerSecond());

        audioSource = gameObject.GetComponent<AudioSource>();
        audioClip = audioSource.clip;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region methods
    public void GenerateClick()
    {
        ConvertAndAddToCoins(clickStrength);//coins += clickStrength * levelCoinsPerEnergy;        

        //play sound
        audioSource.PlayOneShot(audioClip);
    }

    internal void ConvertAndAddToCoins(double value)
    {
        coins+= value * levelCoinsPerEnergy;
        CoinsScoreTxt.text = coins.ToString();
    }

    internal void PayCost(double value)
    {
        coins -= value;
        CoinsScoreTxt.text = coins.ToString();
    }

    public double GetCurrentCoins() { return coins; }

    public double GetClickStrength() { return clickStrength; }
    
    public void SetClickStrength(double value) { clickStrength = value; ClickStrengthTxt.text = clickStrength.ToString(); }

    public double GetEPerSec() { return energyPerSec; }
    public double GetBaseEPerSec() { return baseEPerSec; }

    public void SetEPerSec(double value) { energyPerSec = value; EnergyPerSecTxt.text = energyPerSec.ToString(); }

    public double GetCoinsPerE() { return levelCoinsPerEnergy; }
    public void SetCoinsPerE(double value) { levelCoinsPerEnergy = value;  }
    #endregion methods

    #region coroutine
    IEnumerator AddEnergyPerSecond()
    {
        while (true)
        {           
            ConvertAndAddToCoins(energyPerSec);

            yield return new WaitForSeconds(1);
        }
    }

    public int GetBaseLevel()
    {
        return baseLevel;
    }

    public void SetLevel(double targetValue)
    {
        levelCoinsPerEnergy = targetValue;        
        
        CoinsPerEnergy.text = "1 energy = " + levelCoinsPerEnergy + " coins";

        SetEPerSec(0);
        SetClickStrength(1);

        //when we get to the higher level energy is worth more money(coins ie on level 2, 1 energy will be 2 coins(coins points) ), but we reset all of the upgrades
        foreach (UpgradeButton upgradeButton in upgradeButtons)
        {
            upgradeButton.ResetUpgrades();
        }
    }

    internal void AddToUpgradeButtons(UpgradeButton upgradeButton)
    {
        upgradeButtons.Add(upgradeButton);
    }

    internal void TurnOnUpgradeVisibility(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 0:
                Upgrade1.SetActive(true); break;
            case 1:
                Upgrade2.SetActive(true); break;
            case 2:
                Upgrade3.SetActive(true); break;
            default:
                break;
        }
    }

    internal void TurnOffUpgradeVisibility(int upgradeIndex)
    {
        switch (upgradeIndex)
        {
            case 0:
                Upgrade1.SetActive(false); break;
            case 1:
                Upgrade2.SetActive(false); break;
            case 2:
                Upgrade3.SetActive(false); break;
            default:
                break;
        }
    }
    #endregion coroutine
}
