using UnityEngine;

public class RulesContext : MonoBehaviour
{
    // Injecting the rules strategy
    internal RulesStrategy Rules
    {
        get => _rules;
        set => _rules = value;
    }
    private RulesStrategy _rules;
}