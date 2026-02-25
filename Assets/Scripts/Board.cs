using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private GameObject tilePrefab;   
    [SerializeField]
    private GameObject[] dots;
    private GameObject[,] allDots;
    private BackgroundTile[,] allTiles; // масив для тайлів розміром нашого поля

    public int Width { 
        get {return width;} 
    } 
    public int Height { 
        get {return height;} 
    } 
    public GameObject[,] GetDots() {
        return allDots; 
    } 
    public void SetDots(int column, int row, GameObject newDot) {
        allDots[column, row] = newDot;
    }

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
    public void MovePieces(Dot dot, Vector2 startTouchPosition, Vector2 endTouchPosition)
    {        
        float swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
        int column = dot.Column;
        int row = dot.Row;
        Dot otherDot = null;
        float rightAngle = StaticSwipe.rightAngle;
        float leftAngle = StaticSwipe.leftAngle;
        if(swipeAngle > -rightAngle && swipeAngle <= rightAngle && column < width-1)
        {
            //Right Swipe
            otherDot = allDots[column + 1, row].GetComponent<Dot>();
            
            // int tempColumn = otherDot.Column;
            otherDot.SetColumn(otherDot.Column - 1);
            dot.SetColumn(dot.Column + 1);
            
        } else if(swipeAngle > rightAngle && swipeAngle <= leftAngle && row < height-1)
        {
            //UP Swipe
            otherDot = allDots[column, row + 1].GetComponent<Dot>();
            // int tempRow = otherDot.Row;
            otherDot.SetRow(otherDot.Row - 1);
            dot.SetRow(dot.Row + 1);
        } else if((swipeAngle > leftAngle || swipeAngle <= -leftAngle) && column > 0)
        {
            //Left Swipe
            otherDot = allDots[column - 1, row].GetComponent<Dot>();
            
            // int tempColumn = otherDot.Column;
            otherDot.SetColumn(otherDot.Column + 1);
            dot.SetColumn(dot.Column - 1);
        } else if((swipeAngle < -rightAngle || swipeAngle > leftAngle) && row > 0)
        {
            //DOWN Swipe
            otherDot = allDots[column, row - 1].GetComponent<Dot>();
            // int tempRow = otherDot.Row;
            otherDot.SetRow(otherDot.Row + 1);
            dot.SetRow(dot.Row - 1);
        }
        if(otherDot != null)
        {
            allDots[dot.Column, dot.Row] = dot.gameObject;
            allDots[otherDot.Column, otherDot.Row] = otherDot.gameObject;
        }
    }
}
