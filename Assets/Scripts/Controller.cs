using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BreakInfinity;
using System.Collections.Generic;
using System.Linq;

public class Controller : MonoBehaviour
{

    #region Variables
    public static Controller instance;
    public GameData data;
    [SerializeField] private TMP_Text galdText;
    [SerializeField] private TMP_Text galdClickPowerText;
    #endregion

    #region Unity Methods

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        data = new GameData();
        UpgradesManager.instance.StartUpgradeManager();
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

    public List<T> CreateList<T>(int capacity)
    {
        return Enumerable.Repeat(default(T), capacity).ToList();
    } 

    #endregion
}
