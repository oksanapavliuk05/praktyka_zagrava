using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        board = Object.FindFirstObjectByType<Board>();
    }

    public bool MatchesAt(int column, int row, GameObject obj)
    {
        if(column >1 && row > 1)
        {
            if(board.GetDot(column - 1, row).tag == obj.tag && board.GetDot(column - 2, row).tag == obj.tag)
            {
                return true;
            }
            if(board.GetDot(column, row-1).tag == obj.tag && board.GetDot(column, row-2).tag == obj.tag)
            {
                return true;
            }
        } else if(column <= 1 || row <=1){
            if(row > 1)
            {
                if(board.GetDot(column, row-1).tag == obj.tag && board.GetDot(column, row-2).tag == obj.tag)
                {
                    return true;
                }
            }else if (column > 1)
            {
                if(board.GetDot(column - 1, row).tag == obj.tag && board.GetDot(column - 2, row).tag == obj.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void FindMatches(Dot dot)
    {
        int column = dot.Column;
        int row = dot.Row;
        if(column<0 || column >= board.Width || row < 0 || row >= board.Height)
        {
            return;
        }
        if(column > 0 && column < board.Width - 1)
        {
            GameObject leftDot1 = board.GetDot(column - 1, row);
            GameObject rightDot1 = board.GetDot(column + 1, row);
            if(leftDot1 != null && rightDot1 != null && leftDot1.tag == dot.gameObject.tag && rightDot1.tag == dot.gameObject.tag)
            {
                leftDot1.GetComponent<Dot>().IsMatched = true;
                rightDot1.GetComponent<Dot>().IsMatched = true;
                dot.IsMatched = true;
            }
        }
        if(row > 0 && row < board.Height - 1)
        {
            GameObject upDot1 = board.GetDot(column, row + 1);
            GameObject downDot1 = board.GetDot(column, row - 1);
            if(upDot1 != null && downDot1 != null)
            {
                if(upDot1.tag == dot.gameObject.tag && downDot1.tag == dot.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().IsMatched = true;
                    downDot1.GetComponent<Dot>().IsMatched = true;
                    dot.IsMatched = true;
                }
            }
        }
    }
    public void FindBombVertical(Dot dot)
    {
        int column = dot.Column;
        int row = dot.Row;
        if(column > 0 && column < board.Width - 1)
        {
            GameObject leftDot1 = board.GetDot(column - 1, row);
            GameObject rightDot1 = board.GetDot(column + 1, row);
            if(leftDot1 != null && rightDot1 != null && leftDot1.tag == dot.gameObject.tag && rightDot1.tag == dot.gameObject.tag)
            {
                leftDot1.GetComponent<Dot>().IsMatched = true;
                rightDot1.GetComponent<Dot>().IsMatched = true;
                dot.IsMatched = true;
                    
                if((column > 1 && board.GetDot(column - 2, row) != null && board.GetDot(column - 2, row).tag == dot.gameObject.tag) || (column < board.Width - 2 && board.GetDot(column + 2, row) != null && board.GetDot(column + 2, row).tag == dot.gameObject.tag))
                {
                    dot.IsHorizontalBomb = true;
                    if(dot.IsSwipe)
                    {
                        board.CreateBomb(dot, dot.IsHorizontalBomb);
                    }
                }
                
            }
        }
    }
    public void FindBombHorizontal(Dot dot)
    {
        int column = dot.Column;
        int row = dot.Row;
        if(row > 0 && row < board.Height - 1)
        {
            GameObject leftDot1 = board.GetDot(column, row - 1);
            GameObject rightDot1 = board.GetDot(column, row + 1);
            if(leftDot1 != null && rightDot1 != null && leftDot1.tag == dot.gameObject.tag && rightDot1.tag == dot.gameObject.tag)
            {
                leftDot1.GetComponent<Dot>().IsMatched = true;
                rightDot1.GetComponent<Dot>().IsMatched = true;
                dot.IsMatched = true;
                    
                if((row > 1 && board.GetDot(column, row - 2) != null && board.GetDot(column, row - 2).tag == dot.gameObject.tag) || (row < board.Height - 2 && board.GetDot(column, row + 2) != null && board.GetDot(column, row + 2).tag == dot.gameObject.tag))
                {
                    dot.IsHorizontalBomb = false;
                    if(dot.IsSwipe)
                    {
                        board.CreateBomb(dot, dot.IsHorizontalBomb);
                    }
                }
              
            }
        }
    }

    public void FindColorBomb(Dot dot)
    {
        int column = dot.Column;
        int row = dot.Row;
        
        if(column >= 0 && row >=0 && row < board.Height - 1 && column <board.Width-1)
        {
            GameObject Dot2 = board.GetDot(column + 1, row);
            GameObject Dot3 = board.GetDot(column + 1, row+1);
            GameObject Dot4 = board.GetDot(column, row+1);
            if(dot != null && Dot2 != null && Dot3 != null && Dot4 != null && Dot3.tag == dot.gameObject.tag && Dot2.tag == dot.gameObject.tag && Dot4.tag == dot.gameObject.tag)
            {
                Dot2.GetComponent<Dot>().IsMatched = true;
                Dot3.GetComponent<Dot>().IsMatched = true;
                Dot4.GetComponent<Dot>().IsMatched = true;
                dot.IsMatched = false;
                board.CreateColorBomb(dot);
            }
        }
    }
    public bool CheckMatches()
    {
        for(int i = 0; i < board.Width; i++)
        {
            for(int j = 0; j < board.Height; j++)
            {
                if(board.GetDot(i, j) != null)
                {
                    Dot dot = board.GetDot(i, j).GetComponent<Dot>();
                    if(dot.IsMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}