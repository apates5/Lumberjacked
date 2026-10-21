using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject logPrefab;
    public int logCount = 10;
    public float logHeight = 1f;
    public float bottomY = -4f;
    public int safeLogs = 2;

    [Header("Branches")]
    [Range(0f, 1f)]
    public float branchChance = 0.6f;

    private List<Log> logs = new List<Log>();

    void Start()
    {
        BuildTree();
    }

    void BuildTree()
    {
        for (int i = 0; i < logCount; i++)
        {
            Log newLog = SpawnLog(i);

            if (i < safeLogs)
                newLog.SetBranch(Side.None);
            else
                newLog.SetBranch(PickBranchSide());

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
        if (logs.Count > 0 && logs[logs.Count - 1].branchSide != Side.None)
            return Side.None;

        if (Random.value > branchChance)
            return Side.None;

        return (Random.value < 0.5f) ? Side.Left : Side.Right;
    }
}