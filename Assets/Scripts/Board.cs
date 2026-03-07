using UnityEngine;
using System.Collections;
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
    private MatchFinder matchFinder;
    private ScoreManager scoreManager;
    private GoalManager goalManager;
    private float swipeResist = 1f;
    private int matchScore = 20;
    public int Width { 
        get {return width;} 
    } 
    public int Height { 
        get {return height;} 
    } 
    public GameObject[,] GetDots() {
        return allDots; 
    } 
    public GameObject GetDot(int column, int row) {
        return allDots[column, row]; 
    } 
    public void SetDots(int column, int row, GameObject newDot) {
        allDots[column, row] = newDot;
    }
    
    void Start()
    {
        allDots = new GameObject[width, height]; 
        allTiles = new BackgroundTile[width, height]; 
        matchFinder = GetComponent<MatchFinder>();
        if (matchFinder == null)
        {
            matchFinder = gameObject.AddComponent<MatchFinder>();
        }
        matchFinder.Start();
        scoreManager = Object.FindFirstObjectByType<ScoreManager>(); 
        goalManager = Object.FindFirstObjectByType<GoalManager>();
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
                int maxIter = 0;
                //Check if we have a match at the start
                while(matchFinder.MatchesAt(i, j, dots[dotToUse]) && maxIter < 100)
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIter++;  
                }
                maxIter = 0;
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                //початкові координати
                dot.GetComponent<Dot>().SetColumn(i);
                dot.GetComponent<Dot>().SetRow(j);
                dot.transform.parent = this.transform;
                dot.name =  "(" + i + "," + j + ")";
                allDots[i, j] = dot;
            }
        }
    }
    public void calculateAngle(Dot dot, Vector2 startTouchPosition, Vector2 endTouchPosition)
    {
        dot.IsSwipe = true;
        if(Mathf.Abs(endTouchPosition.y - startTouchPosition.y) > swipeResist || Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > swipeResist)
        {        
            float swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
            MovePieces(dot, swipeAngle);
        }
    }
    public void MovePieces(Dot dot, float swipeAngle)
    {        
        int column = dot.Column;
        int row = dot.Row;
        Dot otherDot = null;
        dot.PreviousColumn = column;
        dot.PreviousRow = row;
        float rightAngle = StaticSwipe.rightAngle;
        float leftAngle = StaticSwipe.leftAngle;
        if(swipeAngle > -rightAngle && swipeAngle <= rightAngle && column < width-1)
        {
            //Right Swipe
            if(allDots[column + 1, row] != null){
                otherDot = allDots[column + 1, row].GetComponent<Dot>();
                // int tempColumn = otherDot.Column;
                otherDot.SetColumn(otherDot.Column - 1);
                dot.SetColumn(dot.Column + 1);
            }
            
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
        //Explode bomb after swipe bomb
        if(dot.IsBomb)
        {
            StartCoroutine(BombDelayCo(dot));
        }
        StartCoroutine(CheckMoveCo(dot, otherDot));
    }
    private IEnumerator BombDelayCo(Dot dot)
    {
        yield return new WaitForSeconds(.3f);
        ExplodeBombCo(dot);
        dot.IsBomb = false;
    }
    //Check all gems in the board for exploding bomb    
    private void ExplodeBombCo(Dot dot)
    {
        //yield return new WaitForSeconds(.3f);
        if (dot.IsHorizontalBomb)
        {
            for(int i =0; i <width; i++)
            {
                if(allDots[i, dot.Row] != null)
                { 
                    allDots[i, dot.Row].GetComponent<Dot>().IsMatched = true;
                }
            }
        }  
        else
        {
            for(int j =0; j <height; j++)
            {
                if(allDots[dot.Column, j] != null)
                {
                    allDots[dot.Column, j].GetComponent<Dot>().IsMatched = true;
                }
            }
        } 
    }
    //Move back if there are no matche
    public IEnumerator CheckMoveCo(Dot dot, Dot otherDot)
    {
        yield return new WaitForSeconds(.5f);
        goalManager.IncreaseMove();
        if(otherDot != null)
        {
            if(!dot.IsMatched && !otherDot.GetComponent<Dot>().IsMatched)
            {
                otherDot.GetComponent<Dot>().SetRow(dot.Row);
                otherDot.GetComponent<Dot>().SetColumn(dot.Column);
                dot.GetComponent<Dot>().SetRow(dot.PreviousRow);
                dot.GetComponent<Dot>().SetColumn(dot.PreviousColumn);
                allDots[dot.Column, dot.Row] = dot.gameObject;
                allDots[otherDot.Column, otherDot.Row] = otherDot.gameObject;
            }
            else
            {
                DestroyMatches();
            }
            otherDot = null;
        }
        
    }
    public void CreateBomb(Dot dot, bool isHorizontal)
    {
        //Increase score for create bomb 
        scoreManager.IncreaseScore(40);
        dot.IsBomb = true;
        dot.IsMatched = false;
        dot.IsSwipe = false;
        SpriteRenderer bs = dot.gameObject.GetComponent<SpriteRenderer>();
        bs.sprite = dot.bombSprite;
        goalManager.IncreaseClaimed(dot);
        //Rotate bomb if it is horizontal
        if(isHorizontal)
        {
            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    
    //Destroy gems in chosen row and column
    private void DestroyMatchesAt(int column, int row)
    {
        if(allDots[column, row].GetComponent<Dot>().IsMatched && !allDots[column, row].GetComponent<Dot>().IsBomb)
        {
            Destroy(allDots[column, row]);
            scoreManager.IncreaseScore(matchScore);
            goalManager.IncreaseClaimed(allDots[column, row].GetComponent<Dot>());
            allDots[column, row] = null;
        }
    }

    //Destroy all matched gems
    public void DestroyMatches()
    {
        for(int i =0; i < width; i++)
        {
            for(int j = 0; j <height; j++)
            {
                if(allDots[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo());
    }

    //Coroutine for gems falling down after destroying
    private IEnumerator DecreaseRowCo()
    {   
        //how much gems is null in the colunm
        int nullCount = 0;
        for(int i = 0; i <width; i++)
        {
            for(int j = 0; j <height; j++)
            {
                if(allDots[i, j] == null)
                {
                    nullCount++;
                }else if(nullCount > 0)
                {
                    allDots[i,j].GetComponent<Dot>().SetRow(j - nullCount);
                    allDots[i, j- nullCount] = allDots[i, j];
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void FillBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                { 
                    //create a new dot to slide in from the top
                    Vector2 newPosition = new Vector2(i, j + height);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject dot = Instantiate(dots[dotToUse], newPosition, Quaternion.identity);
                    dot.transform.parent = this.transform;
                    dot.name =  "(" + i + "," + j + ")";
                    dot.GetComponent<Dot>().SetColumn(i);
                    dot.GetComponent<Dot>().SetRow(j);
                    allDots[i, j] = dot;
                }
            }
        }
    }
    private IEnumerator FillBoardCo(){
        FillBoard();
        yield return new WaitForSeconds(.2f);
        while (matchFinder.CheckMatches())
        {
            yield return new WaitForSeconds(.2f);
            DestroyMatches();
        }
    }
}
