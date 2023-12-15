using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    LayerMask _pieceMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 123, _pieceMask))
            {
                if (hit.collider.TryGetComponent<Piece>(out var piece))
                {
                    Debug.Log($"Clicked on {piece.Type} at {piece.transform.position}");
                }
            }
        }
    }
}
