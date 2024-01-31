using TMPro;
using UnityEngine;

public class EndMenuText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Get game result from player prefs
        string result = PlayerPrefs.GetString("result");
        gameObject.GetComponent<TextMeshProUGUI>().text = result;
    }
}
