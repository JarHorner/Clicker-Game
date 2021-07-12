using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;

public class UpgradeHandler : MonoBehaviour
{
    public List<Upgrades> upgrades;
    public Upgrades upgradePrefab;
    public ScrollRect upgradesScroll;
    public Transform upgradesPanel;
    public string[] upgradeNames;
    public BigDouble[] upgradeBaseCost;
    public BigDouble[] upgradeBaseMult;
    public BigDouble[] upgradeBasePower;
    public BigDouble[] upgradesUnlock;
}
