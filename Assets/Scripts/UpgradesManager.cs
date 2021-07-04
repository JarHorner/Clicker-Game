using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{

    #region Variables
    public static UpgradesManager instance;
    public List<Upgrades> clickUpgrades;
    public Upgrades clickUpgradePrefab;
    public ScrollRect clickUpgradesScroll;
    public Transform clickUpgradesPanel;
    public string[] clickUpgradeNames;
    public BigDouble[] clickUpgradeBaseCost;
    public BigDouble[] clickUpgradeBaseMult;
    public BigDouble[] clickUpgradeBasePower;
    #endregion

    #region Unity Methods
    //creates singleton
    void Awake()
    {
        instance = this;
    }
    //called in the Start() method in Controller. sets up all the array information on upgrades and populates the
    //upgrades list with that info.
    public void StartUpgradeManager()
    {
        //set length in upgrade check and add info to arrays! (change # of upgrades)
        Methods.UpgradeCheck(ref Controller.instance.data.clickUpgradeLevel, 4);

        clickUpgradeNames = new []{"Click Power +1", "Click Power +5", "Click Power +10", "Click Power + 25"};
        clickUpgradeBaseCost = new BigDouble[]{10, 50, 100, 1000};
        clickUpgradeBaseMult = new BigDouble[]{1.25, 1.35, 1.55, 2};
        clickUpgradeBasePower = new BigDouble[]{1, 5, 10, 25};

        for (int i=0; i < Controller.instance.data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradesPanel);
            upgrade.UpgradeID = i;
            clickUpgrades.Add(upgrade);
        }
        //prevents growth from the middle in a ScrollRect
        clickUpgradesScroll.normalizedPosition = new Vector2(0,0);
        
        UpdateClickUpgradeUI();
    }

    //parameter is set to -1, if -1 updates every upgrade, if specified, updates upgrade with that ID
    //uses the UpdateUI helper method.
    public void UpdateClickUpgradeUI(int upgradeID = -1)
    {
        if (upgradeID == -1)
            for (int i = 0; i < clickUpgrades.Count; i++) UpdateUI(i);
        else UpdateUI(upgradeID);
    }

    //updates each aspect of an upgrade ui text according to information in arrays 
    void UpdateUI(int ID)
    {
        clickUpgrades[ID].levelText.text = Controller.instance.data.clickUpgradeLevel[ID].ToString();
        clickUpgrades[ID].CostText.text = $"Cost: {ClickUpgradeCost(ID).ToString("F2")} Gald";
        clickUpgrades[ID].NameText.text = clickUpgradeNames[ID];
    }

    //Calculates the cost of an upgrade based on the id of the upgrade
    public BigDouble ClickUpgradeCost(int upgradeID)
    {
        return clickUpgradeBaseCost[upgradeID] * BigDouble.Pow(clickUpgradeBaseMult[upgradeID], Controller.instance.data.clickUpgradeLevel[upgradeID]);
    }

    //Checks to see if buying the upgrade is possible, if so, subtracts money increases level and updates the Upgrade UI
    public void BuyUpgrade(int upgradeID)
    {
        var data = Controller.instance.data;
        if (data.gald >= ClickUpgradeCost(upgradeID))
        {
            data.gald -= ClickUpgradeCost(upgradeID);
            data.clickUpgradeLevel[upgradeID] += 1;
        }

        UpdateClickUpgradeUI(upgradeID);
    }

    #endregion
}
