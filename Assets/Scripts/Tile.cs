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
                GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }

    private bool _isDark;
}