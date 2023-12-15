using UnityEngine;

public class RulesController : MonoBehaviour
{
    // Singleton
    internal static RulesController Instance;

    private Rules _rules;

    internal Rules Get()
    {
        return _rules;
    }
    internal void Set(Rules rules)
    {
        _rules = rules;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Set(new Brazilian());
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
