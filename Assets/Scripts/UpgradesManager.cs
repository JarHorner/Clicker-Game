using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;

public class UpgradesManager : MonoBehaviour
{

    #region Variables
    public static UpgradesManager instance;
    public Upgrades clickUpgrade;
    public string clickUpgradeName;
    public BigDouble clickUpgradeBaseCost;
    public BigDouble clickUpgradeBaseMult;
    #endregion

    #region Unity Methods
    void Awake()
    {
        instance = this;
    }
    public void StartUpgradeManager()
    {
        clickUpgradeName = "Gald Per Click";
        clickUpgradeBaseCost = 10;
        clickUpgradeBaseMult = 1.5;
        UpdateClickUpgradeUI();
    }

    public void UpdateClickUpgradeUI()
    {
        var data = Controller.instance.data;
        clickUpgrade.levelText.text = data.clickUpgradeLevel.ToString();
        clickUpgrade.CostText.text = "Cost: " + Cost().ToString("F0") + " Gald";
        clickUpgrade.NameText.text = "+1 " + clickUpgradeName;
    }

    public BigDouble Cost()
    {
        return clickUpgradeBaseCost * BigDouble.Pow(clickUpgradeBaseMult, Controller.instance.data.clickUpgradeLevel);
    }

    public void BuyUpgrade()
    {
        var data = Controller.instance.data;
        if (data.gald >= Cost())
        {
            data.gald -= Cost();
            data.clickUpgradeLevel += 1;
            UpdateClickUpgradeUI();
        }
    }

    #endregion
}
