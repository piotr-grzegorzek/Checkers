using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameColor TileColor
    {
        get => _tileColor;
        set
        {
            _tileColor = value;
            Rules rules = RulesController.Instance.Get();
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