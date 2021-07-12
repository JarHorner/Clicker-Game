using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BreakInfinity;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{

    #region Variables
    public static Controller instance;
    public GameData data;
    [SerializeField] private TMP_Text galdText;
    [SerializeField] private TMP_Text galdPerSecondText;
    [SerializeField] private TMP_Text galdClickPowerText;

    #endregion

    #region Unity Methods
    //creates singleton
    void Awake()
    {
        instance = this;
    }

    //see StartUpgrademanager method in UpgradesManager for more info
    void Start()
    {
        data = new GameData();
        UpgradesManager.instance.StartUpgradeManager();
    }

    //updates the amount of glad and power of clicker text every frame
    public void Update()
    {
        galdText.text = data.gald.ToString("F2") + " Gald";
        galdPerSecondText.text = $"{GaldPerSecond():F2}/s";
        galdClickPowerText.text = "+" + ClickPower() + " Gald";

        data.gald += GaldPerSecond() * Time.deltaTime;

        for (var i = 0; i < data.productionUpgradeLevel.Count; i++)
            data.productionUpgradeGenerated[i] += UpgradesPerSecond(i) * Time.deltaTime;
    }

    //generates gald based on ClickPower
    public void GenerateGald()
    {
        data.gald += ClickPower();
    }

    //calculates the amount of gold generated per second
    public BigDouble GaldPerSecond()
    {
        BigDouble total = 0;
        for (int i = 0; i < data.productionUpgradeLevel.Count; i++)
        {
            total += UpgradesManager.instance.upgradeHandlers[1].upgradeBasePower[i] 
                        * (data.productionUpgradeLevel[i] + data.productionUpgradeGenerated[i]);
        }
        return total;
    }

    //grabs base power of generator method and multiplies it by its level
    public BigDouble UpgradesPerSecond(int index) 
    {
        return UpgradesManager.instance.upgradeHandlers[2].upgradeBasePower[index] * data.generatorUpgradeLevel[index];
    }

    //starts at 1, and increases depening on amount of upgrades and the power those upgrades give
    public BigDouble ClickPower()
    {
        BigDouble total = 1;
        for (int i = 0; i < data.clickUpgradeLevel.Count; i++)
        {
            total += UpgradesManager.instance.upgradeHandlers[0].upgradeBasePower[i] * data.clickUpgradeLevel[i];
        }
        return total;
    }

    #endregion
}
