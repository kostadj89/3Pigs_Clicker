using System;
using System.Collections;
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
    public double moneyPerEnergy = 1;

    // text for the score
    public Text ScoreTxt;
    // text for the strength of click displayed on main genrate button
    public Text ClickStrengthTxt;
    // text for the energy per second
    public Text EnergyPerSecTxt;

    public Text LevelTxt;

    public float strengthMult = 1.08f;
    public float energyMult = 2f;

    #endregion public

    #region private

    private double score;

    //relating to strength of click upgrade
    private double clickStrength = 1;
    //private double costOfStrengthUpg = baseStrengthUpgCost ;

    //relating to energy per second upgrade
    private double energyPerSec = 0;
    //private double costOfEPerSecUpg = baseEPerSecUpgCost;
    //upgrades
    private int upgradesStrengthOfClick = 0;
    private int upgradesEPerSec = 0;    
    private int upgradesMoreMoneyPerEnergy = 0;

    #endregion private

    // Start is called before the first frame update
    void Start()
    {
        /*if we're going to get a save system, then here we could calculate all thevalues based on the level of upgrades*/

        StartCoroutine(AddEnergyPerSecond());
    }

    // Update is called once per frame
    void Update()
    {
        //score += Mathf.Floor((float)(energyPerSec * Time.deltaTime));
        //ScoreTxt.text = score.ToString();
    }

    #region methods
    public void GenerateClick()
    {
        score += clickStrength * moneyPerEnergy;
        ScoreTxt.text = score.ToString();
    }

    //public void UpgradeClickStrength()
    //{
    //    if ( score >= costOfStrengthUpg)
    //    {
    //        //inc number of bought upgrades
    //        upgradesStrengthOfClick++;

    //        //deduct the cost from score
    //        score -= costOfStrengthUpg;
    //        ScoreTxt.text = score.ToString();  
            
    //        //setting new cost for the srength upgrade
    //        costOfStrengthUpg = Mathf.Floor((baseStrengthUpgCost+ upgradesStrengthOfClick) * Mathf.Pow(strengthMult, upgradesStrengthOfClick));
    //        ClickStrengthCostTxt.text = costOfStrengthUpg.ToString();

    //        //setting new strength
    //        clickStrength = baseClickStrength + upgradesStrengthOfClick;
            
    //        //temporary for debbugging
    //        ClickStrengthUpgTxt.text = upgradesStrengthOfClick.ToString();

    //        ClickStrengthTxt.text = clickStrength.ToString();
    //    }
        
    //}

    //public void UpgradeEnergyPerSec()
    //{
    //    if (score >= costOfEPerSecUpg)
    //    {
    //        //inc number of bought upgrades
    //        upgradesEPerSec++;

    //        //deduct the cost from score
    //        score -= costOfEPerSecUpg;
    //        ScoreTxt.text = score.ToString();
            
    //        energyPerSec = baseEPerSec + upgradesEPerSec;

    //        EnergyPerSecTxt.text = energyPerSec.ToString();

    //        //setting new cost for the srength upgrade
    //        costOfEPerSecUpg = Mathf.Floor((baseEPerSecUpgCost + upgradesEPerSec) * Mathf.Pow(energyMult, upgradesEPerSec));

    //        EnergyPerSecCostTxt.text = costOfEPerSecUpg.ToString();
    //    }         
    //}

    public double GetCurrentScore() { return score; }

    public void SetScore(double value) { score = value; ScoreTxt.text = score.ToString(); }

    public double GetClickStrength() { return clickStrength; }
    public double GetBaseClickStrength() { return baseClickStrength; }
    public void SetClickStrength(double value) { clickStrength = value; ClickStrengthTxt.text = clickStrength.ToString(); }

    public double GetEPerSec() { return energyPerSec; }
    public double GetBaseEPerSec() { return baseEPerSec; }

    public void SetEPerSec(double value) { energyPerSec = value; EnergyPerSecTxt.text = energyPerSec.ToString(); }

    public double GetMoneyPerE() { return moneyPerEnergy; }
    public void SetMoneyPerE(double value) { moneyPerEnergy = value;  }
    #endregion methods

    #region coroutine
    IEnumerator AddEnergyPerSecond()
    {
        while (true)
        {
            score += energyPerSec;
            ScoreTxt.text = score.ToString();

            yield return new WaitForSeconds(1);
        }
    }

    public double GetBaseLevel()
    {
        return baseLevel;
    }

    public void SetLevel(double targetValue)
    {
        moneyPerEnergy = targetValue;
        LevelTxt.text = moneyPerEnergy.ToString();
    }
    #endregion coroutine
}
