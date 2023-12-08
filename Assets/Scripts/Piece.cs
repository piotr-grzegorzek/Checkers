using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool IsKing;

    private bool _isDark;

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
            UpdateColor();
        }
    }

    private void UpdateColor()
    {
        if (_isDark && TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = Color.black;
        }
    }
}