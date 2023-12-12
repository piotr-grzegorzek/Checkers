using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceType Type;
    public GameColor PieceColor
    {
        get => _pieceColor;
        set
        {
            _pieceColor = value;
            Rules rules = RulesController.Instance.Get();
            _renderer.material.color = value == GameColor.Light ? Color.white : rules.DarkPieceColor;
        }
    }
    private GameColor _pieceColor;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
}