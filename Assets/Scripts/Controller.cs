using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    #region Variables
    public TMP_Text galdText;
    public double gald;
    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Update()
    {
        galdText.text = gald + " Gald";
    }

    public void GenerateGald()
    {
        gald++;
    }

    #endregion
}
