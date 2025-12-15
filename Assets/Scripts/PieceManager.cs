using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;

    public GameObject mPiecePrefab;

    private List<BasePiece> mWhitePieces = null;
    private List<BasePiece> mBlackPieces = null;
    private List<BasePiece> mPromotedPieces = new List<BasePiece>();

    private string[] wPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "K", "Q", "B", "KN", "R"
    };

    private string[] bPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "Q", "K", "B", "KN", "R"
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"P",  typeof(Pawn)},
        {"R",  typeof(Rook)},
        {"KN", typeof(Knight)},
        {"B",  typeof(Bishop)},
        {"K",  typeof(King)},
        {"Q",  typeof(Queen)}
    };

    // 🔹 ADDED: scale per piece type
    private Dictionary<Type, Vector3> mPieceScales = new Dictionary<Type, Vector3>()
    {
        { typeof(Pawn),   new Vector3(0.9f, 0.9f, 1f) },
        { typeof(Rook),   new Vector3(0.88f, 0.88f, 1f) },
        { typeof(Knight), new Vector3(0.76f, 1.17f, 1f) },
        { typeof(Bishop), new Vector3(0.70f, 1.05f, 1f) },
        { typeof(Queen),  new Vector3(0.53f, 1.11f, 1f) },
        { typeof(King),   new Vector3(0.82f,1.29f,1f) },
    };

    public void Setup(Board board)
    {
        mWhitePieces = CreatePieces(Color.white, new Color32(255, 255, 255, 255), wPieceOrder);

        mBlackPieces = CreatePieces(
            Color.black,
            new Color32(193, 154, 107, 255),
            bPieceOrder
        );

        PlacePieces(2, 0, mWhitePieces, board);
        PlacePieces(5, 7, mBlackPieces, board);

        SwitchSides(Color.black);
    }

    // 🔹 MODIFIED: added pieceOrder parameter
    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, string[] pieceOrder)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < pieceOrder.Length; i++)
        {
            string key = pieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            BasePiece newPiece = CreatePiece(pieceType);
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, spriteColor, this);
        }

        return newPieces;
    }

    // 🔹 MODIFIED (scale applied, no other logic touched)
    private BasePiece CreatePiece(Type pieceType)
    {
        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(transform);
        newPieceObject.transform.localRotation = Quaternion.identity;

        BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);

        // Apply scale based on piece type
        if (mPieceScales.TryGetValue(pieceType, out Vector3 scale))
            newPieceObject.transform.localScale = scale;
        else
            newPieceObject.transform.localScale = Vector3.one;

        return newPiece;
    }

    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            pieces[i].Place(board.mAllCells[i, pawnRow]);
            pieces[i + 8].Place(board.mAllCells[i, royaltyRow]);
        }
    }

    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (BasePiece piece in allPieces)
            piece.enabled = value;
    }

    public void SwitchSides(Color color)
    {
        if (!mIsKingAlive)
        {
            ResetPieces();
            mIsKingAlive = true;
            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;

        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);

        foreach (BasePiece piece in mPromotedPieces)
        {
            bool isBlackPiece = piece.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackPiece == true ? isBlackTurn : !isBlackTurn;
            piece.enabled = isPartOfTeam;
        }
    }

    public void ResetPieces()
    {
        foreach (BasePiece piece in mPromotedPieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }

        mPromotedPieces.Clear();

        foreach (BasePiece piece in mWhitePieces)
            piece.Reset();

        foreach (BasePiece piece in mBlackPieces)
            piece.Reset();
    }

    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        pawn.Kill();

        BasePiece promotedPiece = CreatePiece(typeof(Queen));
        promotedPiece.Setup(teamColor, spriteColor, this);
        promotedPiece.Place(cell);

        mPromotedPieces.Add(promotedPiece);
    }
}
