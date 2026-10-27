using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public TreeManager tree;
    public GameManager gameManager;

    [Header("Position")]
    public float sideX = 2f;

    public Side currentSide = Side.Left;

    public bool isDead = false;

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ChopFrom(Side.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ChopFrom(Side.Right);
        }
    }

    void ChopFrom(Side side)
    {
        MoveTo(side);

        if (tree.BottomLog.branchSide == currentSide)
        {
            Die();
            return;
        }

        tree.ChopBottomLog();
        gameManager.AddScore(1);

        if (tree.BottomLog.branchSide == currentSide)
        {
            Die();
        }
    }

    void MoveTo(Side side)
    {
        currentSide = side;
        float x = (side == Side.Left) ? -sideX : sideX;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<SpriteRenderer>().color = Color.gray;
        gameManager.GameOver();
    }
}