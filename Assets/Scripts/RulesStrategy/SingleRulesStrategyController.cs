using UnityEngine;

public class SingleRulesStrategyController : MonoBehaviour
{
    internal static SingleRulesStrategyController Instance { get; private set; }

    internal RulesStrategy Rules { get; set; }

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