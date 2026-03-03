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

    private Board board;
    private GameObject otherDot;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition;

    private bool  isMatched = false;
    public bool isBomb = false;
    public bool isSwipe = false;
    private int previousColumn;
    private int previousRow;  

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
        board  = FindObjectOfType<Board>();
        //початкові координати
        // targetX = (int)transform.position.x;
        // targetY = (int)transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {   
        //Finding matches
        board.FindMatches(this);
        //Finding bombs
        board.FindBombVertical(this);
        board.FindBombHorizontal(this);
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
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        board.calculateAngle(this, startTouchPosition, endTouchPosition);
    }
    
}
