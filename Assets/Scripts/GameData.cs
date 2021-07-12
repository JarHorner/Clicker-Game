using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;

public class GameData
{

    #region Variables
    public BigDouble gald;
    public List<int> clickUpgradeLevel;
    public List<BigDouble> productionUpgradeLevel;
    public List<BigDouble> productionUpgradeGenerated;
    public List<int> generatorUpgradeLevel;
    #endregion

    #region Unity Methods
    //contructor, defaults gald to zero and creates list of upgrades
    public GameData()
    {
        gald = 0;
        //if you want to change # of upgrade, increase capacity and check UpgradesManager, StartUpgradeManager method
        clickUpgradeLevel = Methods.CreateList<int>(4);
        productionUpgradeLevel = Methods.CreateList<BigDouble>(4);
        productionUpgradeGenerated = Methods.CreateList<BigDouble>(4);
        generatorUpgradeLevel = Methods.CreateList<int>(4);
    }

    #endregion
}
