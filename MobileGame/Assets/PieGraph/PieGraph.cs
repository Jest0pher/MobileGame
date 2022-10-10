using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieGraph : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Material pieMat;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        pieMat = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        pieMat.SetFloat("_NeutralCount", GameManager.Instance.teamCounts[TeamTypes.Neutral]);
        pieMat.SetFloat("_RedCount", GameManager.Instance.teamCounts[TeamTypes.Red]);
        pieMat.SetFloat("_GreenCount", GameManager.Instance.teamCounts[TeamTypes.Green]);
        pieMat.SetFloat("_BlueCount", GameManager.Instance.teamCounts[TeamTypes.Blue]);
        pieMat.SetFloat("_PurpleCount", GameManager.Instance.teamCounts[TeamTypes.Purple]);
        pieMat.SetFloat("_OrangeCount", GameManager.Instance.teamCounts[TeamTypes.Orange]);
        pieMat.SetFloat("_YellowCount", GameManager.Instance.teamCounts[TeamTypes.Yellow]);
        pieMat.SetFloat("_TotalCount", GameManager.Instance.grid.activeNodes);
    }
}
