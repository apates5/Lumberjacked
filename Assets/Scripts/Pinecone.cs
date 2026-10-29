using UnityEngine;

// falling pinecone, shadow stays on the ground to show where it lands
public class Pinecone : MonoBehaviour
{
    [Header("Parts")]
    public Transform cone;

    [Header("Fall")]
    public float startY = 7f;       // above the screen
    public float hitY = -3.8f;      // about head height
    public float fallTime = 1.2f;

    private Side side;
    private Player player;
    private float timer = 0f;

    // called by the spawner right after it makes the pinecone
    public void Setup(Side targetSide, float sideX, Player targetPlayer)
    {
        side = targetSide;
        player = targetPlayer;

        // line up with the left or right side of the tree
        float x = (side == Side.Left) ? -sideX : sideX;
        transform.position = new Vector3(x, 0f, 0f);

        cone.localPosition = new Vector3(0f, startY, 0f);
        SoundManager.Instance.Play(SoundManager.Instance.pineconeFall);
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 0 = just dropped, 1 = landed
        float t = Mathf.Clamp01(timer / fallTime);

        // t * t so it speeds up like gravity
        float y = Mathf.Lerp(startY, hitY, t * t);
        cone.localPosition = new Vector3(0f, y, 0f);

        // landed
        if (t >= 1f)
        {
            // player still on this side = dead
            if (player.currentSide == side)
            {
                player.Die();
            }
            else
            {
                SoundManager.Instance.Play(SoundManager.Instance.pineconeLand);
            }

            Destroy(gameObject);
        }
    }
}
