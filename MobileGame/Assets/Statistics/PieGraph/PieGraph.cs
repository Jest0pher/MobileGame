using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieGraph : MonoBehaviour
{
    Image image;
    Material pieMat;
    public Percentages percentages;
    public RectTransform pieRect;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        pieMat = image.material;
        pieRect.SetParent(transform.parent);

        float minX;
        float minY;
        float maxX;
        float maxY;
        Vector3[] tempCorners = new Vector3[4];
        percentages.textMeshes[0].GetComponent<RectTransform>().GetWorldCorners(tempCorners);
        minX = tempCorners[0].x;
        maxX = tempCorners[0].x;
        minY = tempCorners[0].y;
        maxY = tempCorners[0].y;

        for (int i = 0; i < percentages.textMeshes.Count; i++) {
            percentages.textMeshes[i].GetComponent<RectTransform>().GetWorldCorners(tempCorners);
            foreach (Vector3 vec in tempCorners) {
                if (vec.x < minX)
                {
                    minX = vec.x;
                }
                else if (vec.x > maxX){
                    maxX = vec.x;
                }

                if (vec.y < minY)
                {
                    minY = vec.y;
                }
                else if (vec.y > maxY) {
                    maxY = vec.y;
                }
            }
        }

        float width = maxX - minX;
        float height = maxY - minY;

        pieRect.sizeDelta = new Vector2(width, height);

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
