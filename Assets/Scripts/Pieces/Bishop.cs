using UnityEngine;
using UnityEngine.UI;

public class Bishop : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Base setup
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Bishop stuff
         mMovement = mColor == Color.white ? new Vector3Int(0, 0, 1) : new Vector3Int(0, -1, -1);
        mMovement = new Vector3Int(0, 1, 1);
        Sprite[] sprites = Resources.LoadAll<Sprite>("W");

        foreach (Sprite s in sprites)
        {
            if (s.name == "White_Bishop")
            {
                GetComponent<Image>().sprite = s;
                break;
            }
        }
    }
}
