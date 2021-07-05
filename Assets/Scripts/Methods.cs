using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Methods : MonoBehaviour
{

    #region Variables
    #endregion

    #region Unity Methods

    //uses Enumerable to create objects of a certain type, them converts them into a list
    public static List<T> CreateList<T>(int capacity)
    {
        return Enumerable.Repeat(default(T), capacity).ToList();
    }

    //*ensures the addition of new upgrades properly (Needed when saving data)*
    //checks to see if the number of upgrades we want is greater than the actual size of the list of what is loaded in.
    public static void UpgradeCheck<T>(List<T> list, int length) where T : new()
    {
        try
        {
            if (list.Count == 0) list = CreateList<T>(length);
            while(list.Count < length) list.Add(new T());
        }
        catch
        {
            list = CreateList<T>(length);
        }
    }

    #endregion
}
