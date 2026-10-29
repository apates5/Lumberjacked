using UnityEngine;

public class Pinecone : MonoBehaviour
{
    [Header("Parts")]
    public Transform cone;

    [Header("Fall")]
    public float startY = 7f;
    public float hitY = -3.8f;
    public float fallTime = 1.2f;

    private Side side;
    private Player player;
    private float timer = 0f;

    public void Setup(Side targetSide, float sideX, Player targetPlayer)
    {
        side = targetSide;
        player = targetPlayer;

        float x = (side == Side.Left) ? -sideX : sideX;
        transform.position = new Vector3(x, 0f, 0f);

        cone.localPosition = new Vector3(0f, startY, 0f);
        SoundManager.Instance.Play(SoundManager.Instance.pineconeFall);
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / fallTime);

        float y = Mathf.Lerp(startY, hitY, t * t);
        cone.localPosition = new Vector3(0f, y, 0f);

        // It landed
        if (t >= 1f)
        {
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