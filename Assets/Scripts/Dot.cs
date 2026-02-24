using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    private Board board;
    private GameObject otherDot;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0;
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
        //update coordinates of point to new position
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x)> .1)
        {
            //Move Towards the target 
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directrly set position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
        if(Mathf.Abs(targetY - transform.position.y)> .1)
        {
            //Move Towards the target 
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directrly set position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown()
    {
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(startTouchPosition);
    }

    private void OnMouseUp()
    {
        endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle(); 
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
        //Debug.Log(swipeAngle);
        MovePieces();
    }

    void MovePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            //Right Swipe
            otherDot = board.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -=   1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            //UP Swipe
            otherDot = board.allDots[column, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left Swipe
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        } else if((swipeAngle < -45 || swipeAngle > 135) && row > 0)
        {
            //DOWN Swipe
            otherDot = board.allDots[column, row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
    }
}
