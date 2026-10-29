using UnityEngine;

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
    public float retryDelay = 0.3f;

    private float timer;

    void Start()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        if (!gameManager.IsPlaying) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        if (TryDrop())
            timer = Random.Range(minDelay, maxDelay);
        else
            timer = retryDelay;
    }

    bool TryDrop()
    {
        Side target = player.currentSide;
        Side other = (target == Side.Left) ? Side.Right : Side.Left;

        if (!tree.IsSideClear(other, 2))
            return false;

        GameObject obj = Instantiate(pineconePrefab);
        obj.GetComponent<Pinecone>().Setup(target, player.sideX, player);
        return true;
    }
}