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
            _renderer.material.color = value == GameColor.Light ? Color.white : Color.black;
        }
    }
    private GameColor _pieceColor;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
}