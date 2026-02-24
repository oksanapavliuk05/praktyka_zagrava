using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;   
    public GameObject[] dots;
    public GameObject[,] allDots;
    private BackgroundTile[,] allTiles; // масив для тайлів розміром нашого поля
    void Start()
    {
        allDots = new GameObject[width, height]; 
        allTiles = new BackgroundTile[width, height]; 
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
                
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";
                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name =  "(" + i + "," + j + ")";
                allDots[i, j] = dot;
            }
        }
    }
}
