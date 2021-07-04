using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{

    #region Variables
    public int UpgradeID;
    public Image UpgradeButton;
    public TMP_Text levelText;
    public TMP_Text NameText;
    public TMP_Text CostText;
    #endregion

    #region Unity Methods
    //method used in Unity engine for button to generate gald
    public void BuyClickUpgrade()
    {
        UpgradesManager.instance.BuyUpgrade(UpgradeID);
    }

    #endregion
}
