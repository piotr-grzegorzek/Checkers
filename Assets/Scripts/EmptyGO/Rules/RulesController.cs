using UnityEngine;

public class RulesController : MonoBehaviour
{
    internal Rules Rules;
    internal static RulesController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Rules = new Brazilian();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
