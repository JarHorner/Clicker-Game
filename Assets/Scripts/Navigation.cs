using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Navigation : MonoBehaviour
{

    #region Variables
    public GameObject clickUpgradesSelected;
    public GameObject productionUpgradesSelected;
    public GameObject generatorUpgradesSelected;
    public TMP_Text clickUpgradesTitleText;
    public TMP_Text productionUpgradesTitleText;
    public TMP_Text generatorUpgradesTitleText;
    #endregion

    #region Unity Methods
    public void SwitchUpgrades(string location)
    {
        UpgradesManager.instance.upgradeHandlers[0].upgradesScroll.gameObject.SetActive(false);
        UpgradesManager.instance.upgradeHandlers[1].upgradesScroll.gameObject.SetActive(false);
        UpgradesManager.instance.upgradeHandlers[2].upgradesScroll.gameObject.SetActive(false);

        clickUpgradesSelected.SetActive(false);
        productionUpgradesSelected.SetActive(false);
        generatorUpgradesSelected.SetActive(false);
        
        clickUpgradesTitleText.color = Color.gray;
        productionUpgradesTitleText.color = Color.gray;
        generatorUpgradesTitleText.color = Color.gray;

        switch (location)
        {
            case "click":
                UpgradesManager.instance.upgradeHandlers[0].upgradesScroll.gameObject.SetActive(true);
                clickUpgradesSelected.SetActive(true);
                clickUpgradesTitleText.color = Color.white;
                break;
            case "production":
                UpgradesManager.instance.upgradeHandlers[1].upgradesScroll.gameObject.SetActive(true);
                productionUpgradesSelected.SetActive(true);
                productionUpgradesTitleText.color = Color.white;
                break;
            case "generator":
                UpgradesManager.instance.upgradeHandlers[2].upgradesScroll.gameObject.SetActive(true);
                generatorUpgradesSelected.SetActive(true);
                generatorUpgradesTitleText.color = Color.white;
                break;
        }
        
    }

    #endregion
}
