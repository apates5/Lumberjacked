using UnityEngine;

// drops pinecones on the player's side every few seconds
public class HazardSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject pineconePrefab;
    public Player player;
    public TreeManager tree;
    public GameManager gameManager;

    [Header("Timing")]
    public float minDelay = 3f;
    public float maxDelay = 6f;
    public float retryDelay = 0.3f;   // wait time if a drop wasn't safe

    private float timer;

    void Start()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        // only drop stuff while the game is running
        if (!gameManager.IsPlaying) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        // dropped one, wait a random time. otherwise try again soon
        if (TryDrop())
            timer = Random.Range(minDelay, maxDelay);
        else
            timer = retryDelay;
    }

    bool TryDrop()
    {
        // aim at the player, so they have to move
        Side target = player.currentSide;
        Side other = (target == Side.Left) ? Side.Right : Side.Left;

        // don't drop if the other side has a branch, no way to dodge
        if (!tree.IsSideClear(other, 2))
            return false;

        GameObject obj = Instantiate(pineconePrefab);
        obj.GetComponent<Pinecone>().Setup(target, player.sideX, player);
        return true;
    }
}
