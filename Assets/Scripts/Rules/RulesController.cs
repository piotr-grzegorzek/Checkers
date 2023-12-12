using UnityEngine;

public class RulesController : MonoBehaviour
{
    // Singleton
    public static RulesController Instance;

    private Rules _rules;

    public Rules Get()
    {
        return _rules;
    }
    public void Set(Rules rules)
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
