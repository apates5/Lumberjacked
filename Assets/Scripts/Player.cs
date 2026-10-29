using UnityEngine;

// lumberjack controls: switch sides, chop, die
public class Player : MonoBehaviour
{
    [Header("References")]
    public TreeManager tree;
    public GameManager gameManager;

    [Header("Position")]
    public float sideX = 2f;   // distance from the middle of the tree

    [Header("Sprites")]
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer axeRenderer;
    public Sprite deadBody;
    public Sprite axeRest;
    public Sprite axeSwing;
    public Sprite deadAxe;
    public float swingTime = 0.1f;   // how long the swing frame shows

    public Side currentSide = Side.Left;
    public bool isDead = false;

    private float swingTimer = 0f;

    void Start()
    {
        MoveTo(currentSide);
    }

    void Update()
    {
        if (isDead) return;

        // put the axe back after a swing
        if (swingTimer > 0f)
        {
            swingTimer -= Time.deltaTime;
            if (swingTimer <= 0f)
            {
                axeRenderer.sprite = axeRest;
            }
        }

        // arrows or A/D
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
        SwingAxe();

        // walked into a branch
        if (tree.BottomLog.branchSide == currentSide)
        {
            Die();
            return;
        }

        Log bottom = tree.BottomLog;

        // knotted log took its first hit but didn't break yet
        if (!bottom.TakeHit())
        {
            SoundManager.Instance.Play(SoundManager.Instance.knottedHit);
            return;
        }

        // grab the type now, the log gets reused at the top after the chop
        LogKind choppedKind = bottom.kind;


        if (choppedKind == LogKind.Golden)
        {
            SoundManager.Instance.Play(SoundManager.Instance.golden);
        }
        else
        {
            SoundManager.Instance.Play(SoundManager.Instance.chop);
        }

        tree.ChopBottomLog();
        gameManager.LogChopped(choppedKind);

        // branch dropped down onto the player
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

        // flip so he always faces the tree
        Vector3 scale = transform.localScale;
        scale.x = (side == Side.Left) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void SwingAxe()
    {
        axeRenderer.sprite = axeSwing;
        swingTimer = swingTime;
    }

    // branch, pinecone, or timer running out all end up here
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        SoundManager.Instance.Play(SoundManager.Instance.death);
        bodyRenderer.sprite = deadBody;
        axeRenderer.sprite = deadAxe;
        gameManager.GameOver();
    }
}
