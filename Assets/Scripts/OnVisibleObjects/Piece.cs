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

    internal void MoveTo(Vector3 position)
    {
        transform.position = position;
        CheckAndPromoteToKing(position);
    }

    private void CheckAndPromoteToKing(Vector3 position)
    {
        RulesStrategy rules = SingleRulesContext.Instance.Rules;
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