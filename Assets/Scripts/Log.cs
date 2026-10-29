using UnityEngine;

public enum Side { None, Left, Right }

public enum LogKind { Normal, Knotted, Golden }

// one piece of the tree, handles its branch, type and hits
public class Log : MonoBehaviour
{
    [Header("Branch Objects")]
    public GameObject branchLeft;
    public GameObject branchRight;

    [Header("Trunk")]
    public SpriteRenderer trunkRenderer;

    [Header("Trunk Sprites")]
    public Sprite normalSprite;
    public Sprite knottedSprite;
    public Sprite crackedSprite;
    public Sprite goldenSprite;

    public Side branchSide = Side.None;
    public LogKind kind = LogKind.Normal;

    private int hitsLeft = 1;

    // only show the branch on the matching side
    public void SetBranch(Side side)
    {
        branchSide = side;
        branchLeft.SetActive(side == Side.Left);
        branchRight.SetActive(side == Side.Right);
    }

    // sets hits needed + sprite for the log type
    public void SetKind(LogKind newKind)
    {
        kind = newKind;

        if (kind == LogKind.Knotted)
        {
            hitsLeft = 2;
            trunkRenderer.sprite = knottedSprite;
        }
        else if (kind == LogKind.Golden)
        {
            hitsLeft = 1;
            trunkRenderer.sprite = goldenSprite;
        }
        else
        {
            hitsLeft = 1;
            trunkRenderer.sprite = normalSprite;
        }
    }

    // returns true if the log broke
    public bool TakeHit()
    {
        hitsLeft--;

        // knotted log survived, show the crack
        if (hitsLeft > 0)
        {
            trunkRenderer.sprite = crackedSprite;
            return false;
        }

        return true;
    }
}
