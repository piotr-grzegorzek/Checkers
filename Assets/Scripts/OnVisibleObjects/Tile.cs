using UnityEngine;

public class Tile : MonoBehaviour
{
    internal GameColor TileColor
    {
        get => _tileColor;
        set
        {
            _tileColor = value;
            RulesStrategy rules = _rulesContext.Rules;
            _renderer.material.color = value == GameColor.Light ? Color.white : rules.PlayableTileColor;
        }
    }
    private GameColor _tileColor;

    private RulesContext _rulesContext;
    private Renderer _renderer;

    void Awake()
    {
        // Couldnt serialize due to type mismatch (prefab and gameobject)
        _rulesContext = FindObjectOfType<RulesContext>();
        _renderer = GetComponent<Renderer>();
    }
}