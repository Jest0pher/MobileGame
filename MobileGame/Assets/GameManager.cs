using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameSpeedMultiplier;

    public float GameSpeedMultiplier { get { return gameSpeedMultiplier; }
                                       set { gameSpeedMultiplier = value;
                                             gameSpeedSlider.value = value;
                                           }
                                     }
    public Dictionary<TeamTypes, int> teamCounts;
    public GridMap grid;
    public Slider gameSpeedSlider;

    [Space]
    public ProjectilePool projectiles;

    [HideInInspector]
    public float adjustedTimeDelta;

    [Space]
    [Header("PlayerSettings")]
    public TeamTypes team;
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
        adjustedTimeDelta = Time.deltaTime * GameSpeedMultiplier;
        for (int i = 0; i < (int)TeamTypes.Count; i++) {
            if (teamCounts[(TeamTypes)i] == grid.activeNodes) {
                GameSpeedMultiplier = 0;
                gameSpeedSlider.enabled = false;
            }
        }
    }

}
