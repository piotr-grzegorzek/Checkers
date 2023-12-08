using UnityEngine;

public class Tile : MonoBehaviour
{
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
    private bool _isDark;

    private Renderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
}