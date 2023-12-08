using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameColor TileColor
    {
        get => _tileColor;
        set
        {
            _tileColor = value;
            _renderer.material.color = value == GameColor.Light ? Color.white : Color.black;
        }
    }
    private GameColor _tileColor;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
}