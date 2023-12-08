using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool IsKing;

    private bool _isDark;
    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    void OnMouseDown()
    {
        // Called when the mouse is clicked over the collider
        Debug.Log($"Clicked on {nameof(Piece)}");
    }

    public bool IsDark
    {
        get => _isDark;
        set
        {
            _isDark = value;
            if (_isDark)
            {
                _renderer.material.color = Color.black;
            }
        }
    }
}