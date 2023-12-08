using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    public int BoardSize = 8;
    public GameObject DarkTilePrefab;
    public GameObject LightTilePrefab;
    public GameObject DarkPiecePrefab;
    public GameObject LightPiecePrefab;

    void Start()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int z = 0; z < BoardSize; z++)
            {
                GameObject tilePrefab = (x + z) % 2 == 0 ? DarkTilePrefab : LightTilePrefab;
                Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }
    void Update()
    {

    }
}
