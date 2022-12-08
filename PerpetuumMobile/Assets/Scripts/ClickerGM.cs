using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

enum LevelTypes
{
    Grassland,
    Desert,
    SnowWaste,
    Volcanic
}

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

    //level look
    public GameObject GroundPlane;
    //this will be a fixed list of 4 materials, it could be changed in the future
    public List<Material> groundMaterials;

    //will get PS_Ground, and we'll match material with the ground material
    public GameObject groundLandscape;

    //desert particle systems
    public List<GameObject> PSObjects;

    //debug menu
    public Canvas MenuCanvas;

    public bool SoundOn = true;
    public Text SoundText;

    public GameObject BGMusicPlayer;

    public InputField coinsCheatInput;

    //cameras    
    public Camera Camera1;
    public Camera Camera2;

    //help menu
    public Canvas HelpCanvas;

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

    //level look
    LevelTypes currentLevelType = LevelTypes.Grassland;

    //cameras
    private bool camera1Active;

    #endregion private

    void Awake()
    {
        upgradeButtons = new List<UpgradeButton>();
        CloseDebugMenu();
        CloseHelpMenu();
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

        //initialize audio
        audioSource = gameObject.GetComponent<AudioSource>();
        audioClip = audioSource.clip;

        //setup particle systems
        ChangeParticleSystems(0);

        camera1Active = false;
        ToggleCamera();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region methods

    #region coins

    //click event of the main generate button, user generates energy which is then converted to coins
    public void GenerateClick()
    {
        ConvertAndAddToCoins(clickStrength);//coins += clickStrength * levelCoinsPerEnergy;        

        if (SoundOn)
        {
            //play sound
            audioSource.PlayOneShot(audioClip);
        }        
    }

    internal void ConvertAndAddToCoins(double value)
    {
        coins+= value * levelCoinsPerEnergy;
        CoinsScoreTxt.text = coins.ToString();
    }

    //called when purchasing an upgrade
    internal void PayCost(double value)
    {
        coins -= value;
        CoinsScoreTxt.text = coins.ToString();
    }

    public double GetCurrentCoins() { return coins; }

    #endregion coins

    #region upgrades
    
    public double GetClickStrength() { return clickStrength; }
    
    public void SetClickStrength(double value) { clickStrength = value; ClickStrengthTxt.text = clickStrength.ToString(); }

    public double GetEPerSec() { return energyPerSec; }
    public double GetBaseEPerSec() { return baseEPerSec; }

    public void SetEPerSec(double value) { energyPerSec = value; EnergyPerSecTxt.text = energyPerSec.ToString(); }

    public double GetCoinsPerE() { return levelCoinsPerEnergy; }
    public void SetCoinsPerE(double value) { levelCoinsPerEnergy = value;  }

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

    #endregion upgrades

    #region level management

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

        PrepareLevelVisuals();
    }

    private void PrepareLevelVisuals()
    {
        /*
        levelIndex determins the type of ground
        0-Grass
        1-Sand
        2-Snow
        3-Volcanic
        */
        int levelIndex = (int)levelCoinsPerEnergy % 4;

        //change ground material
        ChangeGroundMaterial(levelIndex);

        //change main camera background color
        ChangeCameraColor();

        //activate apropriate particle systems
        ChangeParticleSystems(levelIndex);
    }
    private void ChangeParticleSystems(int levelIndex)
    {
        //really bad i know, desert 
        if (levelIndex == 1)
        {
            foreach (GameObject g in PSObjects)
            {
                ParticleSystem ps = g.GetComponent<ParticleSystem>();

                if (g.tag != "Desert") { ps.Stop(); }
                else { ps.Play(); }
            }
        }
        // lava
        else if (levelIndex == 3)
        {
            foreach (GameObject g in PSObjects)
            {
                ParticleSystem ps = g.GetComponent<ParticleSystem>();

                if (g.tag != "Lava") { ps.Stop(); }
                else { ps.Play(); }
            }
        }
        //others
        else
        {
            foreach (GameObject g in PSObjects)
            {
                ParticleSystem ps = g.GetComponent<ParticleSystem>();

                if (g.tag != "Desert" && g.tag != "Lava") ps.Play();
                else ps.Stop();
            }
        }

    }

    private void ChangeCameraColor()
    {
        Color color = Random.ColorHSV(0, 1, 0.75f, 1);
        Camera.main.backgroundColor = color;
    }

    private void ChangeGroundMaterial(int levelIndex)
    {
        MeshRenderer groundMeshRenderer = GroundPlane.GetComponent<MeshRenderer>();
        groundMeshRenderer.material = groundMaterials[levelIndex];

        ParticleSystemRenderer psr = groundLandscape.GetComponentInChildren<ParticleSystemRenderer>();
        psr.material = groundMeshRenderer.material;
    }

    #endregion level management

    #region debug menu
    
    public void OpenDebugMenu()
    {
        MenuCanvas.enabled = true;
    }
    public void CloseDebugMenu()
    {
        MenuCanvas.enabled = false;
    }

    public void ToogleSound()
    {
        AudioSource bgAudioSource = BGMusicPlayer.GetComponent<AudioSource>();

        if (SoundOn)
        {
            SoundOn = false;
            bgAudioSource.Pause();
            SoundText.text = "SOUND OFF";
        }
        else
        {
            SoundOn = true;
            bgAudioSource.Play();
            SoundText.text = "SOUND ON";
        }
    }

    public void CheatAddCoins()
    {
        double cheatCoins;
        bool isNotEmpty = Double.TryParse(coinsCheatInput.text,out cheatCoins);

        if (isNotEmpty && cheatCoins > 1000000)
            cheatCoins = 1000000;

        PayCost((-1) * cheatCoins);
    }

    public void ExitGame()
    {
        //works only for android
        Application.Quit();
    }

    public void ToggleCamera() 
    {
        if (camera1Active)
        {
            camera1Active = false;
            Camera1.enabled = false;
            Camera2.enabled = true;
        }
        else 
        {
            camera1Active = true;
            Camera1.enabled = true;
            Camera2.enabled = false;
        }
    }

    #endregion debug menu

    #region help menu
    public void OpenHelpMenu()
    {
        HelpCanvas.enabled = true;
    }
    public void CloseHelpMenu()
    {
        HelpCanvas.enabled = false;
    }

    #endregion help menu

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

    
    #endregion coroutine
}
