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
    public TMP_Text clickupgradesTitleText;
    public TMP_Text productionupgradesTitleText;
    #endregion

    #region Unity Methods
    public void SwitchUpgrades(string location)
    {
        UpgradesManager.instance.clickUpgradesScroll.gameObject.SetActive(false);
        UpgradesManager.instance.productionUpgradesScroll.gameObject.SetActive(false);

        clickUpgradesSelected.SetActive(false);
        productionUpgradesSelected.SetActive(false);
        clickupgradesTitleText.color = Color.gray;
        productionupgradesTitleText.color = Color.gray;

        switch (location)
        {
            case "click":
                UpgradesManager.instance.clickUpgradesScroll.gameObject.SetActive(true);
                clickUpgradesSelected.SetActive(true);
                clickupgradesTitleText.color = Color.white;
                break;
            case "production":
                UpgradesManager.instance.productionUpgradesScroll.gameObject.SetActive(true);
                productionUpgradesSelected.SetActive(true);
                productionupgradesTitleText.color = Color.white;
                break;
        }
        
    }

    #endregion
}
