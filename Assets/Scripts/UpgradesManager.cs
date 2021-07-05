using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{

    #region Variables
    public static UpgradesManager instance;
    //Click upgrade state
    public List<Upgrades> clickUpgrades;
    public Upgrades clickUpgradePrefab;
    public ScrollRect clickUpgradesScroll;
    public Transform clickUpgradesPanel;
    public string[] clickUpgradeNames;
    public BigDouble[] clickUpgradeBaseCost;
    public BigDouble[] clickUpgradeBaseMult;
    public BigDouble[] clickUpgradeBasePower;
    public BigDouble[] clickUpgradesUnlock;
    
    //Production upgrade state
    public List<Upgrades> productionUpgrades;
    public Upgrades productionUpgradePrefab;
    public ScrollRect productionUpgradesScroll;
    public Transform productionUpgradesPanel;
    public string[] productionUpgradeNames;
    public BigDouble[] productionUpgradeBaseCost;
    public BigDouble[] productionUpgradeBaseMult;
    public BigDouble[] productionUpgradeBasePower;
    public BigDouble[] productionUpgradesUnlock;
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

        //creates click upgrades
        clickUpgradeNames = new []{"Click Power +1", "Click Power +5", "Click Power +10", "Click Power + 25"};
        clickUpgradeBaseCost = new BigDouble[]{10, 50, 100, 500};
        clickUpgradeBaseMult = new BigDouble[]{1.25, 1.35, 1.55, 2};
        clickUpgradeBasePower = new BigDouble[]{1, 5, 10, 25};
        clickUpgradesUnlock = new BigDouble[]{0, 40, 90, 400};

        for (int i=0; i < data.clickUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(clickUpgradePrefab, clickUpgradesPanel);
            upgrade.UpgradeID = i;
            upgrade.gameObject.SetActive(false);
            clickUpgrades.Add(upgrade);
        }

        //creates production upgrades
        productionUpgradeNames = new []{"Beggar +1 gald/s", "Thief +5 gald/s", "Merchant +10 gald/s", "Adventurer +25 gald/s"};
        productionUpgradeBaseCost = new BigDouble[]{100, 500, 1000, 5000};
        productionUpgradeBaseMult = new BigDouble[]{1.5, 1.75, 2, 2.75};
        productionUpgradeBasePower = new BigDouble[]{1, 5, 10, 25};
        productionUpgradesUnlock = new BigDouble[]{0, 400, 900, 4000};

        for (int i=0; i < data.productionUpgradeLevel.Count; i++)
        {
            Upgrades upgrade = Instantiate(productionUpgradePrefab, productionUpgradesPanel);
            upgrade.UpgradeID = i;
            upgrade.gameObject.SetActive(false);
            productionUpgrades.Add(upgrade);
        }
        //prevents growth from the middle in a ScrollRect
        clickUpgradesScroll.normalizedPosition = new Vector2(0,0);
        productionUpgradesScroll.normalizedPosition = new Vector2(0,0);

        UpdateUpgradeUI("click");
        UpdateUpgradeUI("production");
    }

    void Update()
    {
        //check AvailableUpgrades IEnumerator for more information
        StartCoroutine("AvailableUpgrades");
    }

    //parameter is set to -1, depending on type of upgrade and ID given, it will update all of one type of upgrade or specific
    //upgrade of specific type. uses the UpdateUI helper method within.
    public void UpdateUpgradeUI(string type, int upgradeID = -1)
    {
        var data = Controller.instance.data;
        switch(type)
        {
            case "click":
                UpdateUI(clickUpgrades, clickUpgradeNames, data.clickUpgradeLevel, upgradeID);
                break;
            case "production":
                UpdateUI(productionUpgrades, productionUpgradeNames, data.productionUpgradeLevel, upgradeID);
                break;
        }

        //if given id is not specified, goes through each upgrade and updates it. if given id, chooses speficically and updates it.
        void UpdateUI(List<Upgrades> upgrades, string[] upgradeNames, List<int> upgradeLevels, int ID)
        {
            if (ID == -1)
            {
                for (int i = 0; i < upgrades.Count; i++)
                {
                    upgrades[i].levelText.text = upgradeLevels[i].ToString();
                    upgrades[i].CostText.text = $"Cost: {UpgradeCost(type, i).ToString("F2")}";
                    upgrades[i].NameText.text = upgradeNames[i];
                }
            }
            else
            {
                upgrades[ID].levelText.text = upgradeLevels[ID].ToString();
                upgrades[ID].CostText.text = $"Cost: {UpgradeCost(type, ID).ToString("F2")}";
                upgrades[ID].NameText.text = upgradeNames[ID];
            }
        }
    }

    //Calculates the cost of an upgrade based on the id of the upgrade based on type of upgrade
    public BigDouble UpgradeCost(string type, int upgradeID)
    {
        var data = Controller.instance.data;
        switch (type)
        {
            case "click":
                return clickUpgradeBaseCost[upgradeID] * BigDouble.Pow(clickUpgradeBaseMult[upgradeID], (BigDouble)data.clickUpgradeLevel[upgradeID]);
            case "production":
                return productionUpgradeBaseCost[upgradeID] * BigDouble.Pow(productionUpgradeBaseMult[upgradeID], (BigDouble)data.productionUpgradeLevel[upgradeID]);
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
                Buy(data.clickUpgradeLevel);
                break;
            case "production":
                Buy(data.productionUpgradeLevel);
                break;
        }

        //Checks to see if buying the upgrade is possible, if so, subtracts money increases level and updates the Upgrade UI
        void Buy(List<int> upgradeLevels)
        {
            if (data.gald >= UpgradeCost(type, upgradeID))
            {
                data.gald -= UpgradeCost(type, upgradeID);
                upgradeLevels[upgradeID] += 1;
            }

            UpdateUpgradeUI(type, upgradeID);
        }
    }

    //Checks for available upgrades based on the values of each upgrades unlock array every half a second.
    IEnumerator AvailableUpgrades()
    {
        for (int i = 0; i < clickUpgrades.Count; i++)
        {
            if (!clickUpgrades[i].gameObject.activeSelf)
                clickUpgrades[i].gameObject.SetActive(Controller.instance.data.gald >= clickUpgradesUnlock[i]); 
        }

        for (int i = 0; i < productionUpgrades.Count; i++)
        {
            if (!productionUpgrades[i].gameObject.activeSelf)
                productionUpgrades[i].gameObject.SetActive(Controller.instance.data.gald >= productionUpgradesUnlock[i]); 
        }
        yield return new WaitForSeconds(.5f);
    }

    #endregion
}
