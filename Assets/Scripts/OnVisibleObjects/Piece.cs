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
            RulesStrategy rules = _rulesContext.Rules;
            _renderer.material.color = value == GameColor.Light ? Color.white : rules.DarkPieceColor;
        }
    }
    private GameColor _pieceColor;

    private Board _board;
    private SingleRulesContext _rulesContext;
    private Renderer _renderer;

    void Awake()
    {
        // Couldnt serialize due to type mismatch (prefab and gameobject)
        _board = FindObjectOfType<Board>();
        _rulesContext = FindObjectOfType<SingleRulesContext>();
        _renderer = GetComponent<Renderer>();
    }

    internal void MoveTo(Vector3 position)
    {
        transform.position = position;
        transform.position = new Vector3(position.x, _board.PieceUpOffset, position.z);
        if (Type == PieceType.Pawn)
        {
            CheckAndPromoteToKing(position);
        }
    }

    private void CheckAndPromoteToKing(Vector3 position)
    {
        RulesStrategy rules = _rulesContext.Rules;
        switch (PieceColor)
        {
            case GameColor.Light when position.z >= rules.BoardSize - 1:
            case GameColor.Dark when position.z <= 0:
                PromoteToKing();
                break;
        }
    }
    private void PromoteToKing()
    {
        Type = PieceType.King;
        _renderer.material.color = Color.red;
    }
}