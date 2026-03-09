using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{   
    //Bombs Sprites
    public Sprite bombSprite;

    [Header("Board Variables")]
    private int column;
    private int row;
    private int targetX;
    private int targetY;
    private int previousColumn;
    private int previousRow;  

    private Board board;
    private MatchFinder matchFinder;
    private GameObject otherDot;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition;


    public bool  isMatched = false;
    public bool isBomb = false;
    private bool isHorizontalBomb = false;
    public bool isColorBomb = false;
    public bool isSwipe = false;

    public int Column{
        get{ return column;}
    }
    public void SetColumn(int col)
    {
        column = col;
    }
    public int Row{
        get{return row;}
    }
    public void SetRow(int r)
    {
        row = r;
    }
    public void SetTargetX(int x)
    {
        targetX = x;
    }
    public void SetTargetY(int y)
    {
        targetY = y;
    }
    public int TargetX{
        get{return targetX;}
    }
    public int TargetY{
        get{return targetY;}
    }
    public bool IsMatched{
        get{return isMatched;}
        set{isMatched = value;}
    }
    public bool IsBomb{
        get{return isBomb;}
        set{isBomb = value;}
    }
    public bool IsHorizontalBomb{
        get{return isHorizontalBomb;}
        set{isHorizontalBomb = value;}
    }
    public bool IsSwipe{
        get{return isSwipe;}
        set{isSwipe = value;}
    }

    public bool IsColorBomb
    {
        get{return isColorBomb;}
        set{isColorBomb = value;}
    }
    public int PreviousColumn{
        get{return previousColumn;}
        set{previousColumn = value;}
    }
    public int PreviousRow{
        get{return previousRow;}
        set{previousRow = value;}
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board  = Object.FindFirstObjectByType<Board>();;
        matchFinder = Object.FindFirstObjectByType<MatchFinder>();;
        //початкові координати
        // targetX = (int)transform.position.x;
        // targetY = (int)transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {   
        //Finding matches
        matchFinder.FindMatches(this);
        //Finding bombs
        matchFinder.FindBombVertical(this);
        matchFinder.FindBombHorizontal(this);
        matchFinder.FindColorBomb(this);
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, .2f);
        }
        //If the dot is a bomb, make it visible
        if (isBomb)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, 1f);
        }
        //update coordinates of point to new position
        targetX = column;
        targetY = row;
        float speed = 5f;
        if(targetX != transform.position.x)
        {
            //Move Towards the target 
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, speed*Time.deltaTime);
            if(board.GetDot(column, row)!= this.gameObject)
            {
                board.SetDots(column, row, this.gameObject);
            }
        }
        if(targetY != transform.position.y)
        {
            //Move Towards the target 
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, speed*Time.deltaTime);
        }
    }
    private void OnMouseDown()
    {
        if(board.currentState == GameState.move)
        {
            startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if(board.currentState == GameState.move)
        {
            endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            board.calculateAngle(this, startTouchPosition, endTouchPosition);  
        }
    }
    
}
