using UnityEngine;

public enum Side { None, Left, Right }

public class Log : MonoBehaviour
{
    [Header("Branch Objects")]
    public GameObject branchLeft;
    public GameObject branchRight;

    public Side branchSide = Side.None;
    public void SetBranch(Side side)
    {
        branchSide = side;
        branchLeft.SetActive(side == Side.Left);
        branchRight.SetActive(side == Side.Right);
    }
}