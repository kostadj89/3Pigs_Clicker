using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGM : MonoBehaviour
{
    #region const
    //maybe expose these as public vars to tune from editor
    const int baseClickStrength = 1;
    const int baseStrengthUpgCost = 10;
    const int baseEPerSec = 0;
    const int baseEPerSecUpgCost = 1000;
    #endregion const
    #region public

    // text for the score
    public Text ScoreTxt;
    // text for the energy per second
    public Text EnergyPerSecTxt;
    // text for the strength of click displayed on main genrate button
    public Text ClickStrengthTxt;    
    // text for the number of upgrades for click strength
    public Text ClickStrengthUpgTxt;
    //strength cost
    public Text ClickStrengthCostTxt;

    #endregion public

    #region private

    private double score;

    //relating to strength of click upgrade
    private double strengthOfClick = baseClickStrength;
    private double costOfStrengthUpg = baseStrengthUpgCost ;

    //relating to energy per second upgrade
    private double energyPerSec = 0;
    private double costOfEPerSecUpg = baseEPerSecUpgCost;
    //upgrades
    private int upgradesStrengthOfClick = 0;
    private int upgradesEnergyAuto = 0;    

    //values
    //this will depend on the level
    private double moneyPerEnergy = 1;
    private int upgradesMoreMoneyPerEnergy = 0;

    #endregion private

    // Start is called before the first frame update
    void Start()
    {
        /*if we're going to get a save system, then here we could calculate all thevalues based on the level of upgrades*/

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        score += strengthOfClick * moneyPerEnergy;
        ScoreTxt.text = score.ToString();
    }

    public void UpgradeClickStrength()
    {
        if ( score >= costOfStrengthUpg)
        {
            //inc number of bought upgrades
            upgradesStrengthOfClick++;

            //deduct the cost from score
            score -= costOfStrengthUpg;
            ScoreTxt.text = score.ToString();  
            
            //setting new cost for the srength upgrade
            costOfStrengthUpg = (baseStrengthUpgCost+ upgradesStrengthOfClick) * (0.5 * Mathf.Pow(2, upgradesStrengthOfClick));
            ClickStrengthCostTxt.text = costOfStrengthUpg.ToString();

            //setting new strength
            strengthOfClick = baseClickStrength + upgradesStrengthOfClick;
            
            //temporary for debbugging
            ClickStrengthUpgTxt.text = upgradesStrengthOfClick.ToString();

            ClickStrengthTxt.text = strengthOfClick.ToString();
        }
        
    }

    public void UpgradeEnergyPerSec()
    {
        upgradesEnergyAuto++;
        energyPerSec = baseEPerSec + upgradesEnergyAuto;
        //temporary for debbugging
        //ClickStrengthUpgTxt.text = upgradesStrengthOfClick.ToString();

        EnergyPerSecTxt.text = energyPerSec.ToString();
    }
}
