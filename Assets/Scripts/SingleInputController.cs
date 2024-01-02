using UnityEngine;

public class SingleInputController : MonoBehaviour
{
    private static SingleInputController _instance;

    [SerializeField]
    LayerMask _pieceMask;
    [SerializeField]
    LayerMask _tileMask;

    private Piece _selectedPiece;

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
            TrySelectingPiece();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Mouse released
            TryMovingPiece();
        }
    }

    private void TrySelectingPiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 25f, _pieceMask))
        {
            _selectedPiece = hit.collider.GetComponent<Piece>();
        }
    }

    private void TryMovingPiece()
    {
        if (_selectedPiece != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 25f, _tileMask))
            {
                _selectedPiece.MoveTo(hit.collider.GetComponent<Tile>());
            }
        }
    }
}