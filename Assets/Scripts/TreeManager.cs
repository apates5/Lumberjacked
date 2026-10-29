using System.Collections.Generic;
using UnityEngine;

// the tree is just a list of logs, index 0 is the bottom one
public class TreeManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject logPrefab;
    public int logCount = 10;
    public float logHeight = 1f;
    public float bottomY = -4f;
    public int safeLogs = 2;   // no branches at the start so you don't die right away

    [Header("Branches")]
    [Range(0f, 1f)]
    public float branchChance = 0.6f;

    [Header("Log Types")]
    [Range(0f, 1f)]
    public float knottedChance = 0.15f;
    [Range(0f, 1f)]
    public float goldenChance = 0.05f;

    private List<Log> logs = new List<Log>();

    // log next to the player
    public Log BottomLog
    {
        get { return logs[0]; }
    }

    void Start()
    {
        BuildTree();
    }

    // stack up the starting logs
    void BuildTree()
    {
        for (int i = 0; i < logCount; i++)
        {
            Log newLog = SpawnLog(i);

            if (i < safeLogs)
            {
                newLog.SetBranch(Side.None);
                newLog.SetKind(LogKind.Normal);
            }
            else
            {
                newLog.SetBranch(PickBranchSide());
                newLog.SetKind(PickLogType());
            }

            logs.Add(newLog);
        }
    }

    Log SpawnLog(int slot)
    {
        Vector3 pos = new Vector3(0f, bottomY + slot * logHeight, 0f);
        GameObject obj = Instantiate(logPrefab, pos, Quaternion.identity, transform);
        return obj.GetComponent<Log>();
    }

    Side PickBranchSide()
    {
        // log after a branch is always empty, so there's always a safe move
        if (logs.Count > 0 && logs[logs.Count - 1].branchSide != Side.None)
            return Side.None;

        if (Random.value > branchChance)
            return Side.None;

        return (Random.value < 0.5f) ? Side.Left : Side.Right;
    }

    // one roll decides golden, knotted or normal
    LogKind PickLogType()
    {
        float roll = Random.value;

        if (roll < goldenChance)
            return LogKind.Golden;

        if (roll < goldenChance + knottedChance)
            return LogKind.Knotted;

        return LogKind.Normal;
    }

    // true if none of the bottom 'count' logs have a branch on that side
    public bool IsSideClear(Side side, int count)
    {
        for (int i = 0; i < count && i < logs.Count; i++)
        {
            if (logs[i].branchSide == side)
                return false;
        }
        return true;
    }

    // take off the bottom log, shift everything down, reuse it on top
    public void ChopBottomLog()
    {
        Log chopped = logs[0];
        logs.RemoveAt(0);

        for (int i = 0; i < logs.Count; i++)
        {
            logs[i].transform.position = new Vector3(0f, bottomY + i * logHeight, 0f);
        }

        // reused instead of destroying and making a new one
        chopped.SetBranch(PickBranchSide());
        chopped.SetKind(PickLogType());
        chopped.transform.position = new Vector3(0f, bottomY + logs.Count * logHeight, 0f);
        logs.Add(chopped);
    }
}
