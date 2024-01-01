using UnityEngine;

public class SingleInputController : MonoBehaviour
{
    private static SingleInputController _instance;

    [SerializeField]
    LayerMask _pieceMask;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Mouse clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, _pieceMask))
            {
                // Piece clicked
                Piece piece = hit.collider.GetComponent<Piece>();
                Debug.Log($"Piece clicked: {piece.Type} {piece.PieceColor}");
            }
        }
    }
}
