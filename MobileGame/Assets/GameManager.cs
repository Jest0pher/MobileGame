using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameSpeedMultiplier;
    public Dictionary<TeamTypes, int> teamCounts;
    public GridMap grid;

    [HideInInspector]
    public float adjustedTimeDelta;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        teamCounts = new Dictionary<TeamTypes, int>();
        for (int i = 0; i < (int)TeamTypes.Count; i++) { 
            teamCounts.Add((TeamTypes)i, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        adjustedTimeDelta = Time.deltaTime * gameSpeedMultiplier;
    }
}
