using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public bool createGrid;
    public float gridWidth;
    public float gridHeight;
    public int rowCount;
    public int columnCount;
    public bool square;

    [Space]
    public List<Node> nodes;
    public int activeNodes;

    [Space]
    [SerializeField] GameObject NodePrefab;
    [SerializeField] GameObject LinePrefab;
    public float lineWidth;
    //ReadLevel

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        if (createGrid) {
            createGrid = false;
            CreateGrid();
        }
    }

    public void CreateGrid() {

        foreach (Node obj in nodes) {
            Destroy(obj.gameObject);
        }
        foreach (GameObject projObj in GameManager.Instance.projectiles.projectiles) {
            projObj.SetActive(false);
            //Destroy(projObj);
        }

        nodes.Clear();
        for (int i = 0; i < (int)TeamTypes.Count; i++) {
            GameManager.Instance.teamCounts[(TeamTypes)i] = 0;
        }
        GameManager.Instance.gameSpeedSlider.enabled = true;

        if (square)
        {
            columnCount = rowCount;
            gridHeight = gridWidth;
        }

        activeNodes = rowCount * columnCount;
        float cellWidth = gridWidth / columnCount;
        float cellHeight = square ? cellWidth : gridHeight / rowCount;

        List<Vector2> arrayIndices = new List<Vector2>(rowCount*columnCount);

        float y = gridHeight / 2.0f;
        for (int i = 0; i < rowCount; i++) {
            float x = -gridWidth / 2.0f;
            for (int j = 0; j < columnCount; j++) {
                arrayIndices.Add(new Vector2(j, i));
                GameObject node = Instantiate(NodePrefab, transform);
                node.transform.localPosition = new Vector2(x + (cellWidth/2), y - (cellHeight/2));
                Node nodeComp = node.GetComponent<Node>();
                nodeComp.Team = (TeamTypes) Random.Range((int)0,(int)TeamTypes.Count);
                nodeComp.gridPos = new Vector2(j, i);
                nodes.Add(nodeComp);
                x += cellWidth;
            }
            y -= cellHeight;
        }
        
        int gridNodesToConnect = Random.Range(1, (rowCount * columnCount) + 1);
        List<Vector2> parentNodes = new List<Vector2>(gridNodesToConnect);
        for (int i = 0; i < gridNodesToConnect; i++) {
            int randomNode = Random.Range(0, arrayIndices.Count);
            parentNodes.Add(arrayIndices[randomNode]);
            arrayIndices.RemoveAt(randomNode);
        }
        
        for (int i = 0; i < parentNodes.Count; i++) {
            int directionCount = Random.Range(1, 5);
            for (int j = 0; j < directionCount; j++)
            {
                Vector3 vec = Vector3.zero;
                vec.Set(Random.Range((int)-1, (int)2), Random.Range((int)-1, (int)2), 0);
                if (vec.magnitude == 0)
                {
                    vec = Vector3.right;
                }
                if (parentNodes[i].x + vec.x > columnCount - 1 || parentNodes[i].x + vec.x < 0) {
                    vec.Set(-vec.x, vec.y, 0);
                }
                if (parentNodes[i].y + vec.y > rowCount - 1 || parentNodes[i].y + vec.y < 0) {
                    vec.Set(vec.x, -vec.y, 0);
                }
                Node node1 = nodes[(int)(parentNodes[i].x + (parentNodes[i].y * columnCount))];
                Node node2 = nodes[(int)((parentNodes[i].x + vec.x) + ((parentNodes[i].y + vec.y) * columnCount))];
                if (!node1.neighborNodes.Contains(node2))
                {
                    node1.neighborNodes.Add(node2);
                    node2.neighborNodes.Add(node1);

                    Vector3[] positions = { node1.transform.position, node2.transform.position };
                    float zOffset = .1f;
                    positions[0].Set(positions[0].x, positions[0].y, zOffset);
                    positions[1].Set(positions[1].x, positions[1].y, zOffset);
                    GameObject lineObj = Instantiate(LinePrefab, node1.transform);
                    LineRenderer line = lineObj.GetComponent<LineRenderer>();
                    Vector4 lineColor = new Vector4(1, 1, 1, 1.0f);
                    line.SetColors(lineColor, lineColor);
                    line.SetWidth(lineWidth, lineWidth);
                    line.SetPositions(positions);
                }
            }
        }

        for (int i = 0; i < nodes.Count; i++) {
            Node node = nodes[i];
            if (node.neighborNodes.Count == 0)
            {
                activeNodes--;
                nodes[i].gameObject.SetActive(false);
            }
            else {
                GameManager.Instance.teamCounts[node.Team]++;
            }
        }
        
    }
}
