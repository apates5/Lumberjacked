using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public TreeManager tree;
    public GameManager gameManager;

    [Header("Position")]
    public float sideX = 2f;

    [Header("Sprites")]
    public SpriteRenderer bodyRenderer;
    public SpriteRenderer axeRenderer;
    public Sprite deadBody;
    public Sprite axeRest;
    public Sprite axeSwing;
    public Sprite deadAxe;
    public float swingTime = 0.1f;

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

        if (swingTimer > 0f)
        {
            swingTimer -= Time.deltaTime;
            if (swingTimer <= 0f)
            {
                axeRenderer.sprite = axeRest;
            }
        }

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

        if (tree.BottomLog.branchSide == currentSide)
        {
            Die();
            return;
        }

        Log bottom = tree.BottomLog;

        if (!bottom.TakeHit())
        {
            SoundManager.Instance.Play(SoundManager.Instance.knottedHit);
            return;
        }

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

        Vector3 scale = transform.localScale;
        scale.x = (side == Side.Left) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void SwingAxe()
    {
        axeRenderer.sprite = axeSwing;
        swingTimer = swingTime;
    }

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