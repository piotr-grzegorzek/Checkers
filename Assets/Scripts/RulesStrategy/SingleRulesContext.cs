using UnityEngine;

public class SingleRulesContext : MonoBehaviour
{
    internal static SingleRulesContext Instance { get; private set; }

    // Injecting the rules strategy
    internal RulesStrategy Rules
    {
        get => _rules;
        set => _rules = value;
    }
    private RulesStrategy _rules;

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