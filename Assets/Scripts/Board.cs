using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    private GameObject[,] allTiles;
    //private BackgroundTile[,] allTiles; // масив для тайлів розміром нашого поля
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allTiles = new GameObject[width, height];  // масив для тайлів розміром нашого поля
        //allTiles = new BackgroundTile[width, height];  // масив для тайлів розміром нашого поля
        SetUp();
    }
    private void SetUp()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j =0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject; 
                //Instantiate(tilePrefab, tempPosition, Quaternion.identity); 
                allTiles[i, j] = backgroundTile;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";
            }
        }
    }
}
