using UnityEngine;

public class Piece : MonoBehaviour
{
    public LayerMask PieceMask;
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 123, PieceMask))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    Debug.Log($"Clicked on {nameof(Piece)}");
                }
            }
        }
    }
}