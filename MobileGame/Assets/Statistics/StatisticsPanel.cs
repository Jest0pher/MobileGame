using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatisticsPanel : MonoBehaviour
{
    public RectTransform statAreaRect;
    public RectTransform sliderRect;

    public PieGraph pieGraph;
    public Slider timeSlider;

    [Space]
    public bool createStat;

    [Space]
    [Header("Portrait Settings")]

    [Range(0, 1)]
    public float pieGraphSizePercentage = .5f;
    [Range(0,1)]
    public float sliderHeightPercentage = .1f;
    // Start is called before the first frame update
    void Start()
    {

        CreateStat();
    }

    public void CreateStat() {
        if (GameManager.Instance.portrait)
        {
            //PieGraph
            //Scale Pie graph
            float desiredWidth = statAreaRect.sizeDelta.x * pieGraphSizePercentage;
            float scalar = desiredWidth / pieGraph.pieRect.sizeDelta.x;
            pieGraph.transform.localScale *= scalar;
            pieGraph.pieRect.sizeDelta *= scalar;

            Vector3 translate = new Vector3(statAreaRect.position.x - pieGraph.transform.position.x, (statAreaRect.position.y + (-statAreaRect.sizeDelta.y / 2.0f)) - (pieGraph.transform.position.y + (-pieGraph.pieRect.sizeDelta.y / 2.0f)), 0);
            pieGraph.transform.position += translate;
            pieGraph.pieRect.position += translate;
            //DrawBox(WorldRectFromRectTransform(pieGraph.pieRect));
        }
        else
        {

        }




        //Slider
        //Scale slider to width of stat area
        if (statAreaRect.sizeDelta.y > pieGraph.pieRect.sizeDelta.y + sliderRect.sizeDelta.y)
        {
            sliderRect.sizeDelta = new Vector2(statAreaRect.sizeDelta.x, sliderRect.sizeDelta.y);
        }
        else
        {
            float height = (statAreaRect.sizeDelta.y / 2.0f + statAreaRect.position.y) - (pieGraph.pieRect.sizeDelta.y / 2.0f + pieGraph.pieRect.position.y);
            sliderRect.sizeDelta = new Vector2(statAreaRect.sizeDelta.x, height);
            timeSlider.handleRect.sizeDelta = new Vector2(height, timeSlider.handleRect.sizeDelta.y);
        }
        sliderRect.position += new Vector3(statAreaRect.position.x - sliderRect.position.x, (pieGraph.pieRect.position.y + (pieGraph.pieRect.sizeDelta.y / 2.0f)) - (sliderRect.position.y + (-sliderRect.sizeDelta.y / 2.0f)), 0);
        //DrawBox(WorldRectFromRectTransform(sliderRect));
    }

    // Update is called once per frame
    void Update()
    {
        if (createStat) {
            createStat = false;
            CreateStat();
        }
    }

    void ScaleRect(float desiredWidth, ref Rect rect, Transform obj) {
        float scalar = desiredWidth / rect.width;
        obj.localScale *= scalar;
        Vector2 holdCenter = rect.center;
        rect.size *= scalar;
        rect.center = holdCenter;
    }

    public static Rect WorldRectFromRectTransform(RectTransform rectTransform) {
        RectTransform[] rectTransforms = { rectTransform };
        return WorldRectFromRectTransform(rectTransforms);
    }
    public static Rect WorldRectFromRectTransform(RectTransform[] rectTransforms) {
        float minX;
        float minY;
        float maxX;
        float maxY;
        Vector3[] tempCorners = new Vector3[4];
        rectTransforms[0].GetWorldCorners(tempCorners);
        minX = tempCorners[0].x;
        maxX = tempCorners[0].x;
        minY = tempCorners[0].y;
        maxY = tempCorners[0].y;

        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i].GetWorldCorners(tempCorners);
            foreach (Vector3 vec in tempCorners)
            {
                if (vec.x < minX)
                {
                    minX = vec.x;
                }
                else if (vec.x > maxX)
                {
                    maxX = vec.x;
                }

                if (vec.y < minY)
                {
                    minY = vec.y;
                }
                else if (vec.y > maxY)
                {
                    maxY = vec.y;
                }
            }
        }

        float width = maxX - minX;
        float height = maxY - minY;

        return new Rect(minX, maxY, width, -height);
    }
    public static void DrawBox(Rect rect, float duration = 100) {
        Debug.DrawLine(rect.position,  rect.position + Vector2.right * rect.size, Color.red, duration);
        Debug.DrawLine(rect.position,  rect.position + Vector2.up *    rect.size, Color.green, duration);
        Debug.DrawLine(rect.position + rect.size, rect.position + Vector2.right * rect.size, Color.blue, duration);
        Debug.DrawLine(rect.position + rect.size, rect.position + Vector2.up *    rect.size, Color.yellow, duration);
    }
}
