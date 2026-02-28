using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour
{   
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
    public bool IsMatched{
        get{return isMatched;}
        set{isMatched = value;}
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
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    // Update is called once per frame
    void Update()
    {   
        //Finding matches
        board.FindMatches(this);
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, .2f);
        }
        //update coordinates of point to new position
        targetX = column;
        targetY = row;
        if(targetX != transform.position.x)
        {
            //Move Towards the target 
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if(board.GetDot(column, row)!= this.gameObject)
            {
                board.SetDots(column, row, this.gameObject);
            }
        }
        if(targetY != transform.position.y)
        {
            //Move Towards the target 
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
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
