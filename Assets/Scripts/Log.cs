using UnityEngine;

public enum Side { None, Left, Right }

public enum LogKind { Normal, Knotted, Golden }

public class Log : MonoBehaviour
{
    [Header("Branch Objects")]
    public GameObject branchLeft;
    public GameObject branchRight;

    [Header("Trunk")]
    public SpriteRenderer trunkRenderer;

    [Header("Colors")]
    public Color normalColor  = new Color32(0x8B, 0x5A, 0x2B, 0xFF);
    public Color knottedColor = new Color32(0x4A, 0x30, 0x20, 0xFF);
    public Color crackedColor = new Color32(0xB0, 0x80, 0x50, 0xFF);
    public Color goldenColor  = new Color32(0xFF, 0xD7, 0x00, 0xFF);

    public Side branchSide = Side.None;

    public LogKind kind = LogKind.Normal;

    private int hitsLeft = 1;

    public void SetBranch(Side side)
    {
        branchSide = side;
        branchLeft.SetActive(side == Side.Left);
        branchRight.SetActive(side == Side.Right);
    }

    public void SetKind(LogKind newKind)
    {
        kind = newKind;

        if (kind == LogKind.Knotted)
        {
            hitsLeft = 2;
            trunkRenderer.color = knottedColor;
        }
        else if (kind == LogKind.Golden)
        {
            hitsLeft = 1;
            trunkRenderer.color = goldenColor;
        }
        else
        {
            hitsLeft = 1;
            trunkRenderer.color = normalColor;
        }
    }

    public bool TakeHit()
    {
        hitsLeft--;

        if (hitsLeft > 0)
        {
            trunkRenderer.color = crackedColor;
            return false;
        }

        return true;
    }
}