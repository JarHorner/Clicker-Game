using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BreakInfinity;

public class Controller : MonoBehaviour
{

    #region Variables
    public UpgradesManager upgradesManager;
    public GameData data;
    [SerializeField] private TMP_Text galdText;
    [SerializeField] private TMP_Text galdClickPowerText;
    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        data = new GameData();
        upgradesManager.StartUpgradeManager();
    }

    public void Update()
    {
        galdText.text = data.gald + " Gald";
        galdClickPowerText.text = "+" + ClickPower() + " Gald";
    }

    public void GenerateGald()
    {
        data.gald += ClickPower();
    }
    public BigDouble ClickPower()
    {
        return 1 + data.clickUpgradeLevel;
    }

    #endregion
}
