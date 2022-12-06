using System.Collections;
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
    const int baseEPerSecUpgCost = 100;
    #endregion const
    #region public

    // text for the score
    public Text ScoreTxt;
    // text for the strength of click displayed on main genrate button
    public Text ClickStrengthTxt;

    // text for the energy per second
    public Text EnergyPerSecTxt;
    // text for the energy per second upgrade cost
    public Text EnergyPerSecCostTxt;


    // text for the number of upgrades for click strength
    public Text ClickStrengthUpgTxt;
    //strength cost
    public Text ClickStrengthCostTxt;

    public float strengthMult = 1.08f;
    public float energyMult = 2f;

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
    private int upgradesEPerSec = 0;    

    //values
    //this will depend on the level
    private double moneyPerEnergy = 1;
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
            costOfStrengthUpg = Mathf.Floor((baseStrengthUpgCost+ upgradesStrengthOfClick) * Mathf.Pow(strengthMult, upgradesStrengthOfClick));
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
        if (score >= costOfEPerSecUpg)
        {
            //inc number of bought upgrades
            upgradesEPerSec++;

            //deduct the cost from score
            score -= costOfEPerSecUpg;
            ScoreTxt.text = score.ToString();
            
            energyPerSec = baseEPerSec + upgradesEPerSec;

            EnergyPerSecTxt.text = energyPerSec.ToString();

            //setting new cost for the srength upgrade
            costOfEPerSecUpg = Mathf.Floor((baseEPerSecUpgCost + upgradesEPerSec) * Mathf.Pow(strengthMult, upgradesStrengthOfClick));

            EnergyPerSecCostTxt.text = costOfEPerSecUpg.ToString();
        }         
    }
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
    #endregion coroutine
}
