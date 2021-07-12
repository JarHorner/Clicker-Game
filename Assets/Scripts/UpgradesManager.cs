using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{

    #region Variables
    public UpgradeHandler[] upgradeHandlers;
    public static UpgradesManager instance;
    #endregion

    #region Unity Methods
    //creates singleton
    void Awake()
    {
        instance = this;
    }
    //called in the Start() method in Controller. sets up all the array information on upgrades and populates each
    //upgrade list with that info.
    public void StartUpgradeManager()
    {
        var data = Controller.instance.data;
        //set length in upgrade check and add info to arrays! (change # of upgrades)
        Methods.UpgradeCheck(data.clickUpgradeLevel, 4);
        Methods.UpgradeCheck(data.productionUpgradeLevel, 4);
        Methods.UpgradeCheck(data.productionUpgradeGenerated, 4);
        Methods.UpgradeCheck(data.generatorUpgradeLevel, 4);


        //creates click upgrades
        upgradeHandlers[0].upgradeNames = new []{"Click Power +1", "Click Power +5", "Click Power +10", "Click Power + 25"};
        upgradeHandlers[0].upgradeBaseCost = new BigDouble[]{10, 50, 100, 500};
        upgradeHandlers[0].upgradeBaseMult = new BigDouble[]{1.25, 1.35, 1.55, 2};
        upgradeHandlers[0].upgradeBasePower = new BigDouble[]{1, 5, 10, 25};
        upgradeHandlers[0].upgradesUnlock = new BigDouble[]{0, 25, 50, 250};

        //creates production upgrades
        upgradeHandlers[1].upgradeNames = new []{"Beggar +1 gald/s", "Thief +5 gald/s", "Merchant +10 gald/s", "Adventurer +25 gald/s"};
        upgradeHandlers[1].upgradeBaseCost = new BigDouble[]{100, 500, 1000, 5000};
        upgradeHandlers[1].upgradeBaseMult = new BigDouble[]{1.4, 1.65, 1.9, 2.65};
        upgradeHandlers[1].upgradeBasePower = new BigDouble[]{1, 5, 10, 25};
        upgradeHandlers[1].upgradesUnlock = new BigDouble[]{0, 250, 500, 2500};

        //creates generator upgrades
        upgradeHandlers[2].upgradeNames = new []
        {
            $"Produces +0.1 \"{upgradeHandlers[1].upgradeNames[0]}\" Upgrades/s",
            $"Produces +0.05 \"{upgradeHandlers[1].upgradeNames[1]}\" Upgrades/s",
            $"Produces +0.02 \"{upgradeHandlers[1].upgradeNames[2]}\" Upgrades/s",
            $"Produces +0.01 \"{upgradeHandlers[1].upgradeNames[3]}\" Upgrades/s"
        };
        upgradeHandlers[2].upgradeBaseCost = new BigDouble[]{5000, 1e4, 5e4, 1e5};
        upgradeHandlers[2].upgradeBaseMult = new BigDouble[]{1.25, 1.4, 1.9, 2.4};
        upgradeHandlers[2].upgradeBasePower = new BigDouble[]{0.2, 0.1, 0.05, 0.02};
        upgradeHandlers[2].upgradesUnlock = new BigDouble[]{2500, 5000, 25e3, 5e4};

        CreateUpgrades(data.clickUpgradeLevel, 0);
        CreateUpgrades(data.productionUpgradeLevel, 1);
        CreateUpgrades(data.generatorUpgradeLevel, 2);
        UpdateUpgradeUI("click");
        UpdateUpgradeUI("production");
        UpdateUpgradeUI("generator");

        void CreateUpgrades<T>(List<T> level, int index)
        {
            for (int i=0; i < level.Count; i++)
            {
                Upgrades upgrade = Instantiate(upgradeHandlers[index].upgradePrefab, upgradeHandlers[index].upgradesPanel);
                upgrade.UpgradeID = i;
                upgrade.gameObject.SetActive(false);
                upgradeHandlers[index].upgrades.Add(upgrade);
            }
            //prevents growth from the middle in a ScrollRect
            upgradeHandlers[index].upgradesScroll.normalizedPosition = new Vector2(0,0);
        }
    }

    void Update()
    {
        UpgradeUnlockSystem(Controller.instance.data.gald, upgradeHandlers[0].upgradesUnlock, 0);
        UpgradeUnlockSystem(Controller.instance.data.gald, upgradeHandlers[1].upgradesUnlock, 1);
        UpgradeUnlockSystem(Controller.instance.data.gald, upgradeHandlers[2].upgradesUnlock, 2);

        if(upgradeHandlers[1].upgradesScroll.gameObject.activeSelf) UpdateUpgradeUI("production");

        void UpgradeUnlockSystem(BigDouble currency, BigDouble[] unlock, int index)
        {
            for (int i = 0; i < upgradeHandlers[index].upgrades.Count; i++)
            {
                if (!upgradeHandlers[index].upgrades[i].gameObject.activeSelf)
                    upgradeHandlers[index].upgrades[i].gameObject.SetActive(currency >= unlock[i]); 
            }
        }
    }

    //parameter is set to -1, depending on type of upgrade and ID given, it will update all of one type of upgrade or specific
    //upgrade of specific type. uses the UpdateUI helper method within.
    public void UpdateUpgradeUI(string type, int upgradeID = -1)
    {
        var data = Controller.instance.data;
        switch(type)
        {
            case "click":
                UpdateAllUI(upgradeHandlers[0].upgrades, upgradeHandlers[0].upgradeNames, data.clickUpgradeLevel, 0, upgradeID, type);
                break;
            case "production":
                UpdateAllUI(upgradeHandlers[1].upgrades, upgradeHandlers[1].upgradeNames, data.productionUpgradeLevel, 1, upgradeID, type, data.productionUpgradeGenerated);
                break;
            case "generator":
                UpdateAllUI(upgradeHandlers[2].upgrades, upgradeHandlers[2].upgradeNames, data.generatorUpgradeLevel, 2, upgradeID, type);
                break;
        }
    }

    //Calculates the cost of an upgrade based on the id of the upgrade based on type of upgrade
    public BigDouble UpgradeCost(string type, int upgradeID)
    {
        var data = Controller.instance.data;
        switch (type)
        {
            case "click":
                return UpgradeCost_Int(0, data.clickUpgradeLevel, upgradeID);
            case "production":
                return UpgradeCost_BigDouble(1, data.productionUpgradeLevel, upgradeID);
            case "generator":
                return UpgradeCost_Int(2, data.generatorUpgradeLevel, upgradeID);
        }

        return 0;
    }

    //depending on the type of upgrade selected, the Buy() helper method within is given a different param.
    public void BuyUpgrade(string type, int upgradeID)
    {
        var data = Controller.instance.data;

        switch (type)
        {
            case "click":
                Buy(data.clickUpgradeLevel, type, upgradeID);
                break;
            case "production":
                Buy(data.productionUpgradeLevel, type, upgradeID);
                break;
            case "generator":
                Buy(data.generatorUpgradeLevel, type, upgradeID);
                break;
        }
    }

    //int version
    private void UpdateAllUI(List<Upgrades> upgrades, string[] upgradeNames, List<int> upgradeLevels, int index, int upgradeID, string type)
    {
        if (upgradeID == -1)
            for (int i = 0; i < upgradeHandlers[index].upgrades.Count; i++)
                UpdateUI(i);
        else
            UpdateUI(upgradeID);

        void UpdateUI(int ID)
        {
            upgrades[ID].levelText.text = upgradeLevels[ID].ToString("F0");
            upgrades[ID].CostText.text = $"Cost: {UpgradeCost(type, ID).ToString("F2")}";
            upgrades[ID].NameText.text = upgradeNames[ID];
        }
    }

    //BigDouble version
    private void UpdateAllUI(List<Upgrades> upgrades, string[] upgradeNames, List<BigDouble> upgradeLevels, int index, int upgradeID, string type, List<BigDouble> upgradesGenerated = null)
    {
        if (upgradeID == -1)
            for (int i = 0; i < upgradeHandlers[index].upgrades.Count; i++)
                UpdateUI(i);
        else
            UpdateUI(upgradeID);

        void UpdateUI(int ID)
        {
            BigDouble generated = upgradesGenerated == null ? 0 : upgradesGenerated[ID];
            upgrades[ID].levelText.text = (upgradeLevels[ID] + generated).ToString("F2");
            upgrades[ID].CostText.text = $"Cost: {UpgradeCost(type, ID).ToString("F2")}";
            upgrades[ID].NameText.text = upgradeNames[ID];
        }
    }

    //int version
    private void Buy(List<int> upgradeLevels, string type, int upgradeID)
    {
        var data = Controller.instance.data;

        if (data.gald >= UpgradeCost(type, upgradeID))
        {
           data.gald -= UpgradeCost(type, upgradeID);
            upgradeLevels[upgradeID] += 1;
        }

        UpdateUpgradeUI(type, upgradeID);
    }

    //BigDouble version
    private void Buy(List<BigDouble> upgradeLevels, string type, int upgradeID)
    {
        var data = Controller.instance.data;

        if (data.gald >= UpgradeCost(type, upgradeID))
        {
            data.gald -= UpgradeCost(type, upgradeID);
            upgradeLevels[upgradeID] += 1;
        }

        UpdateUpgradeUI(type, upgradeID);
    }

    //int version
    private BigDouble UpgradeCost_Int(int index, List<int> levels, int upgradeID) 
    {
        return upgradeHandlers[index].upgradeBaseCost[upgradeID] * BigDouble.Pow(upgradeHandlers[index].upgradeBaseMult[upgradeID], (BigDouble)levels[upgradeID]); 
    }

    //BigDouble version
    private BigDouble UpgradeCost_BigDouble(int index, List<BigDouble> levels, int upgradeID) 
    {
        return upgradeHandlers[index].upgradeBaseCost[upgradeID] * BigDouble.Pow(upgradeHandlers[index].upgradeBaseMult[upgradeID], (BigDouble)levels[upgradeID]); 
    }
    #endregion
}
