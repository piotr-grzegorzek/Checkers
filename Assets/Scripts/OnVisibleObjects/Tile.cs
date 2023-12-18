using UnityEngine;

public class Tile : MonoBehaviour
{
    internal GameColor TileColor
    {
        get => _tileColor;
        set
        {
            _tileColor = value;
            RulesStrategy rules = SingleRulesContext.Instance.Rules;
            _renderer.material.color = value == GameColor.Light ? Color.white : rules.PlayableTileColor;
        }
    }
    private GameColor _tileColor;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
}