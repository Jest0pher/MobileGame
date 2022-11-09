using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public bool createGrid;
    public float gridAreaWidth;
    public float gridAreaHeight;
    public int rowCount;
    public int columnCount;
    

    [Space]
    public List<Node> nodes;
    public int activeNodes;
    public Rect gridRect;

    [Space]
    public RectTransform gridAreaRect;

    [Space]
    [SerializeField] GameObject NodePrefab;
    [SerializeField] GameObject LinePrefab;

    public float lineWidth;
    [Range(1,8)]
    public int maxConnections;

    //ReadLevel

    // Start is called before the first frame update
    void Start()
    {
        gridAreaRect = GetComponent<RectTransform>();
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

    public GridSave GetMapDesc() {
        return new GridSave(rowCount, columnCount, nodes);
    }

    public void CreateGrid(GridSave map = null) {

        foreach (Node obj in nodes)
        {
            Destroy(obj.gameObject);
        }
        foreach (GameObject projObj in GameManager.Instance.projectiles.projectiles)
        {
            projObj.SetActive(false);
        }

        nodes.Clear();

        if (map != null) {
            rowCount = map.rowCount;
            columnCount = map.columnCount;
        }

        gridAreaWidth = gridAreaRect.rect.size.x;
        gridAreaHeight = gridAreaRect.rect.size.y;

        activeNodes = rowCount * columnCount;
        float cellWidth = gridAreaWidth / columnCount;
        float cellHeight = gridAreaHeight / rowCount;
        gridRect = new Rect(0, 0, columnCount, rowCount);
        float xScalar = (5.0f / columnCount);
        float yScalar = (5.0f / rowCount);
        float scalarMag = (xScalar < yScalar ? xScalar : yScalar) * (GameManager.Instance.portrait ? 1.0f : Camera.main.aspect);
        lineWidth *= scalarMag ;

        float y = gridAreaHeight / 2.0f;
        for (int i = 0; i < rowCount; i++) {
            float x = -gridAreaWidth / 2.0f;
            for (int j = 0; j < columnCount; j++) {
                GameObject node = Instantiate(NodePrefab, transform);
                node.gameObject.name = "Node (" + j.ToString() + "," + i.ToString() + ")";
                node.transform.localPosition = new Vector2(x + (cellWidth/2), y - (cellHeight/2));
                node.transform.localScale = new Vector3(node.transform.localScale.x * scalarMag, node.transform.localScale.y * scalarMag);
                Node nodeComp;
                nodeComp = node.GetComponent<Node>();
                //nodeComp.Team = (TeamTypes) Random.Range((int)0,(int)TeamTypes.Count);
                nodeComp.gridPos = new Vector2(j, i);
                nodeComp.projectileScale = scalarMag;//new Vector3(xScalar, yScalar,1);
                nodes.Add(nodeComp);
                x += cellWidth;
            }
            y -= cellHeight;
        }

        if (map == null)
        {
            List<Node> currentNodesToConnect;
            currentNodesToConnect = NodeConnections(nodes[Random.Range(0, nodes.Count)]);
            while (currentNodesToConnect.Count > 0)
            {
                List<Node> newNodes = new List<Node>();
                for (int i = 0; i < currentNodesToConnect.Count; i++)
                {
                    if (currentNodesToConnect[i].neighborNodes.Count < maxConnections)
                        newNodes.AddRange(NodeConnections(currentNodesToConnect[i]));
                }
                currentNodesToConnect.Clear();
                currentNodesToConnect = newNodes;
            }
            #region Old
            /*
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
            */
            #endregion
            for (int i = 0; i < nodes.Count; i++)
            {
                Node node = nodes[i];
                if (node.neighborNodes.Count == 0)
                {
                    activeNodes--;
                    nodes[i].gameObject.SetActive(false);
                }
                else
                {
                    if (i == 0)
                    {
                        node.Team = GameManager.Instance.team;
                    }
                    else if (i == nodes.Count - 1)
                    {
                        node.Team = (TeamTypes)Random.Range(1, (int)TeamTypes.Count);
                    }
                    GameManager.Instance.teamCounts[node.Team]++;
                }
            }
        }
        else {
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].Team = map.nodeMap[i].team;
                nodes[i].maxAmmo = map.nodeMap[i].maxAmmo;
                nodes[i].fireRate = map.nodeMap[i].fireRate;
                nodes[i].rechargeRate = map.nodeMap[i].rechargeRate;
                nodes[i].maxHP = map.nodeMap[i].maxHP;
                foreach (int neighbor in map.nodeMap[i].neighborIndices) {
                    if (!nodes[i].neighborNodes.Contains(nodes[neighbor]))
                    {
                        nodes[i].neighborNodes.Add(nodes[neighbor]);
                        nodes[neighbor].neighborNodes.Add(nodes[i]);

                        Vector3[] positions = { nodes[i].transform.position, nodes[neighbor].transform.position };
                        float zOffset = .1f;
                        positions[0].Set(positions[0].x, positions[0].y, zOffset);
                        positions[1].Set(positions[1].x, positions[1].y, zOffset);
                        GameObject lineObj = Instantiate(LinePrefab, nodes[i].transform);
                        LineRenderer line = lineObj.GetComponent<LineRenderer>();
                        Vector4 lineColor = new Vector4(1, 1, 1, 1.0f);
                        line.SetColors(lineColor, lineColor);
                        line.SetWidth(lineWidth, lineWidth);
                        line.SetPositions(positions);
                    }
                }
            }
        }
    }

    List<Node> NodeConnections(Node node) {
        List<Node> newConnections = new List<Node>();
        List<Node> possibleConnections = new List<Node>();
        //Find available nodes
        for (int i = -1; i < 2; i++) {
            int newY = (int)node.gridPos.y + i;
            for (int j = -1; j < 2; j++) {
                int newX = (int)node.gridPos.x + j;
                if (!(i == 0 && j == 0)) {
                    Vector2 newPos = new Vector2(newX, newY);
                    if (gridRect.Contains(newPos)){
                        Node nodeToAdd = nodes[newX + columnCount * newY];
                        if (!nodeToAdd.neighborNodes.Contains(node) && nodeToAdd.neighborNodes.Count < maxConnections)
                        {
                            possibleConnections.Add(nodeToAdd);
                        }
                    }
                }
            }
        }

        //Connect
        if (possibleConnections.Count > 0)
        {
            int maxCount = possibleConnections.Count < (maxConnections - node.neighborNodes.Count) ? possibleConnections.Count : maxConnections - node.neighborNodes.Count;
            int randomConnectionCount = maxCount;//Random.Range(1, maxCount);
            for (int i = 0; i < randomConnectionCount; i++)
            {
                int index = Random.Range(0, possibleConnections.Count);
                Node neighbor = possibleConnections[index];
                possibleConnections.RemoveAt(index);
                newConnections.Add(neighbor);

                node.neighborNodes.Add(neighbor);
                neighbor.neighborNodes.Add(node);

                Vector3[] positions = { node.transform.position, neighbor.transform.position };
                float zOffset = .1f;
                positions[0].Set(positions[0].x, positions[0].y, zOffset);
                positions[1].Set(positions[1].x, positions[1].y, zOffset);
                GameObject lineObj = Instantiate(LinePrefab, node.transform);
                LineRenderer line = lineObj.GetComponent<LineRenderer>();
                Vector4 lineColor = new Vector4(1, 1, 1, 1.0f);
                line.SetColors(lineColor, lineColor);
                line.SetWidth(lineWidth, lineWidth);
                line.SetPositions(positions);

            }
        }
        return newConnections;
    }
}
