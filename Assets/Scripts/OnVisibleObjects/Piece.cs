using UnityEngine;

public class Piece : MonoBehaviour
{
    internal PieceType Type;
    internal GameColor PieceColor
    {
        get => _pieceColor;
        set
        {
            _pieceColor = value;
            RulesStrategy rules = SingleRulesContext.Instance.Rules;
            _renderer.material.color = value == GameColor.Light ? Color.white : rules.DarkPieceColor;
        }
    }
    private GameColor _pieceColor;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    internal void MoveTo(Tile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
    }
}