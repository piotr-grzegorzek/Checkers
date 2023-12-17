using UnityEngine;

public class RulesController : MonoBehaviour
{
    internal static RulesController Instance { get; private set; }
    internal Rules Rules { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}