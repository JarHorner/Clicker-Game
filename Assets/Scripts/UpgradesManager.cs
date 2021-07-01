using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;

public class UpgradesManager : MonoBehaviour
{

    #region Variables
    public Controller controller;
    public Upgrades clickUpgrade;
    public string clickUpgradeName;
    public BigDouble clickUpgradeBaseCost;
    public BigDouble clickUpgradeBaseMult;
    #endregion

    #region Unity Methods
    public void StartUpgradeManager()
    {
        clickUpgradeName = "Gald Per Click";
        clickUpgradeBaseCost = 10;
        clickUpgradeBaseMult = 1.5;
        UpdateClickUpgradeUI();
    }

    public void UpdateClickUpgradeUI()
    {
        clickUpgrade.levelText.text = controller.data.clickUpgradeLevel.ToString();
        clickUpgrade.CostText.text = "Cost: " + Cost() + " Gald";
        clickUpgrade.NameText.text = "+1 " + clickUpgradeName;
    }

    public BigDouble Cost()
    {
        return clickUpgradeBaseCost * BigDouble.Pow(clickUpgradeBaseMult, controller.data.clickUpgradeLevel);
    }

    public void BuyUpgrade()
    {
        if (controller.data.gald >= Cost())
        {
            controller.data.gald -= Cost();
            controller.data.clickUpgradeLevel += 1;
            UpdateClickUpgradeUI();
        }
    }

    #endregion
}
