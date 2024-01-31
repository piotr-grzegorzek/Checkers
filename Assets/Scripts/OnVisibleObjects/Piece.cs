using UnityEngine;

public class Piece : MonoBehaviour
{
    private const float _kingScaleMultiplier = 4;

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
        float y = (Type == PieceType.King) ? _board.PieceUpOffset * _kingScaleMultiplier : _board.PieceUpOffset;
        transform.position = new Vector3(position.x, y, position.z);
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
        Vector3 scale = transform.localScale;
        scale.y *= _kingScaleMultiplier;
        transform.localScale = scale;
        MoveTo(transform.position);
    }
}