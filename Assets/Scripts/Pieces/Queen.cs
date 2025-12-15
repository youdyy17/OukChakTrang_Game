using UnityEngine;
using UnityEngine.UI;

public class Queen : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Queen movement direction aligns with team color
        mMovement = mColor == Color.white ? new Vector3Int(0, 0, 1) : new Vector3Int(0, -1, -1);
        Sprite[] sprites = Resources.LoadAll<Sprite>("W");

        foreach (Sprite s in sprites)
        {
            if (s.name == "White_Queen")
            {
                GetComponent<Image>().sprite = s;
                break;
            }
        }
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        CellState cellState = mCurrentCell.mBoard.ValidateCell(targetX, targetY, this);

        if (cellState == targetState)
        {
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            return true;
        }

        return false;
    }

    protected override void CheckPathing()
    {
        // Keep default queen pathing (diagonals, ranks, files, etc.)
        base.CheckPathing();

        // Add custom rule: on the first move, queen may move forward two squares like a pawn.
        // Determine current position
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // In this project, forward direction uses mMovement.z for +/- along Y.
        // Only allow forward to the second square on the very first move,
        // regardless of whether the first square is blocked.
        if (mIsFirstMove)
        {
            MatchesState(currentX, currentY + (mMovement.z * 2), CellState.Free);
        }
    }
}
