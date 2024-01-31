using UnityEngine;

public class SingleRulesContext : MonoBehaviour
{
    private static SingleRulesContext _instance;

    // Injecting the rules strategy
    internal RulesStrategy Rules
    {
        get => _rules;
        set => _rules = value;
    }
    private RulesStrategy _rules;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}