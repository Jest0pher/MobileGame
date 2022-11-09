using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public RectTransform worldRect;

    public float gameSpeedMultiplier;

    public float GameSpeedMultiplier { get { return gameSpeedMultiplier; }
                                       set { gameSpeedMultiplier = value;
                                             gameSpeedSlider.value = value;
                                           }
                                     }
    public Dictionary<TeamTypes, int> teamCounts;
    public GridMap grid;
    public StatisticsPanel stat;
    public Slider gameSpeedSlider;

    [Space]
    public ProjectilePool projectiles;

    [Space]
    public bool portrait = true;
    public RectTransform gridRect;
    public RectTransform statRect;
    RectTransform sliderRect;

    [Range(0, .5f)]
    public float padding = .05f;
    
    [HideInInspector]
    public float adjustedTimeDelta;

    [Space]
    [Header("PlayerSettings")]
    public TeamTypes team;


    float prevAspect;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;


        teamCounts = new Dictionary<TeamTypes, int>();
        for (int i = 0; i < (int)TeamTypes.Count; i++) { 
            teamCounts.Add((TeamTypes)i, 0);
        }

        portrait = Camera.main.aspect < 1 ? true : false;
        prevAspect = Camera.main.aspect;

        worldRect.sizeDelta = new Vector2(Camera.main.orthographicSize * Camera.main.aspect * 2 * (1 - padding), Camera.main.orthographicSize * 2 * (1 - padding));
        StatisticsPanel.DrawBox(worldRect.rect, 0);

        gridRect.sizeDelta = new Vector2(worldRect.sizeDelta.x, (worldRect.sizeDelta.y / 2.0f - worldRect.position.y - Camera.main.orthographicSize * (padding / 2.0f)));
        gridRect.position = new Vector3(worldRect.position.x, (worldRect.sizeDelta.y / 2.0f - worldRect.position.y) / 2.0f + Camera.main.orthographicSize * (padding / 4.0f), 0);
        StatisticsPanel.DrawBox(StatisticsPanel.WorldRectFromRectTransform(gridRect), 0);

        statRect.sizeDelta = gridRect.sizeDelta;
        statRect.position = new Vector3(worldRect.position.x, (-worldRect.sizeDelta.y / 2.0f - worldRect.position.y) / 2.0f - Camera.main.orthographicSize * (padding / 4.0f), 0);
        StatisticsPanel.DrawBox(StatisticsPanel.WorldRectFromRectTransform(statRect), 0);
    }

    // Update is called once per frame
    void Update()
    {
        adjustedTimeDelta = Time.deltaTime * GameSpeedMultiplier;
        for (int i = 1; i < (int)TeamTypes.Count; i++) {
            if (teamCounts[(TeamTypes)i] == grid.activeNodes) {
                GameSpeedMultiplier = 0;
                gameSpeedSlider.enabled = false;
            }
        }

        worldRect.sizeDelta = new Vector2(Camera.main.orthographicSize * Camera.main.aspect * 2 * (1 - padding), Camera.main.orthographicSize * 2 * (1 - padding));
        StatisticsPanel.DrawBox(worldRect.rect,0);

        gridRect.sizeDelta = new Vector2(worldRect.sizeDelta.x, (worldRect.sizeDelta.y / 2.0f - worldRect.position.y - Camera.main.orthographicSize * (padding / 2.0f)));
        gridRect.position = new Vector3(worldRect.position.x, (worldRect.sizeDelta.y / 2.0f - worldRect.position.y) / 2.0f + Camera.main.orthographicSize * (padding / 4.0f), 0);
        StatisticsPanel.DrawBox(StatisticsPanel.WorldRectFromRectTransform(gridRect),0);

        statRect.sizeDelta = gridRect.sizeDelta;
        statRect.position = new Vector3(worldRect.position.x, (-worldRect.sizeDelta.y / 2.0f - worldRect.position.y) / 2.0f - Camera.main.orthographicSize * (padding / 4.0f), 0);
        StatisticsPanel.DrawBox(StatisticsPanel.WorldRectFromRectTransform(statRect),0);

        if (Camera.main.aspect != prevAspect) {
            prevAspect = Camera.main.aspect;
            ResetLevel();
        }
    }

    public void ResetLevel(bool newLevel = false) {
        gameSpeedSlider.enabled = true;
        gameSpeedSlider.value = 1;

        for (int i = 0; i < (int)TeamTypes.Count; i++)
        {
            teamCounts[(TeamTypes)i] = 0;
        }

        if (newLevel)
        {
            grid.CreateGrid();
        }
        else {
            grid.CreateGrid(grid.GetMapDesc());
        }
        stat.CreateStat();
    }

}
