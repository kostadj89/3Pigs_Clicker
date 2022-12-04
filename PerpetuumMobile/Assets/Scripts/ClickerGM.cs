using UnityEngine;
using UnityEngine.UI;

public class ClickerGM : MonoBehaviour
{
    #region public

    public Text ScoreText;

    #endregion public

    #region private
    private double score;

    //upgrades
    private int numberOfUpgradesStrengthOfClick = 0;
    private int numberOfUpgradesEnergyAuto = 0;
    private int numberOfUpgradesMoreMoneyPerEnergy = 0;

    //values
    private double moneyPerEnergy = 1;
    private double strengthOfClick = 1;

    #endregion private

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        score += strengthOfClick * moneyPerEnergy;
        ScoreText.text = score.ToString();
    }
}
