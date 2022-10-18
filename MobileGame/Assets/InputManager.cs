using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 touchPos;
    public Vector2 touchDelta;
    public Vector2 worldTouch;
    public bool canTouch = true;
    public Node selectedNode;
    
    [Space]
    [Header("Testing")]
    [SerializeField] private Vector2 newTouchDir;
    [SerializeField] private float theta;
    [SerializeField] private float newTheta;
    [SerializeField] private List<Vector2> availableDirections;

    private void Start()
    {
        Input.multiTouchEnabled = false;
        newTouchDir = new Vector2();
        availableDirections = new List<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (canTouch)
            {
                canTouch = false;
                touchPos = Input.GetTouch(0).position;
                touchDelta = Vector2.zero;
                worldTouch = Camera.main.ScreenToWorldPoint(touchPos);
                foreach (Node node in GameManager.Instance.grid.nodes) {
                    if (node.isActiveAndEnabled)
                    {
                        if (node.touchCollider.OverlapPoint(worldTouch))
                        {
                            selectedNode = node;
                            for (int i = 0; i < selectedNode.neighborNodes.Count; i++) {
                                availableDirections.Add((selectedNode.neighborNodes[i].transform.position - selectedNode.transform.position).normalized);
                            }
                            print(node.Team);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (selectedNode)
                {
                    touchDelta = (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - selectedNode.transform.position).normalized;
                    float angleStep = 45.0f;
                    float angleStepR = angleStep * Mathf.Deg2Rad;
                
                    theta = Mathf.Atan(touchDelta.y / touchDelta.x) + angleStepR/2;

                    if (touchDelta.x < 0)
                    {
                        theta += Mathf.PI;
                    }
                    else { 
                        if (touchDelta.y < 0)
                        {
                            theta += 2 * Mathf.PI;
                        }
                    }

                    int closestDirIndex = 0;
                    float shortestTheta = 2 * Mathf.PI;
                    for(int i = 0; i < availableDirections.Count; i++) {
                        float dirTheta = Mathf.Atan(availableDirections[i].y / availableDirections[i].x) + angleStepR / 2;
                        if (availableDirections[i].x < 0)
                        {
                            dirTheta += Mathf.PI;
                        }
                        else
                        {
                            if (availableDirections[i].y < 0)
                            {
                                dirTheta += 2 * Mathf.PI;
                            }
                        }

                        if (theta > Mathf.PI * 2) {
                            theta -= Mathf.PI * 2;
                        }
                        float thetaDiff = Mathf.Abs(dirTheta - theta);
                        if (thetaDiff < shortestTheta)
                        {
                            shortestTheta = thetaDiff;
                            closestDirIndex = i;
                        }
                    }
                    //int steps = (int)(theta / angleStep);
                    //newTheta = angleStepR * steps;
                    newTouchDir = availableDirections[closestDirIndex];
                    
                    /*
                    //Debug
                    Vector2 debugDir = new Vector2();
                    Debug.DrawLine(selectedNode.transform.position + (Vector3)touchDelta*5 + Vector3.left * 5, selectedNode.transform.position + (Vector3)touchDelta*5 + Vector3.right * 5, Color.green);
                    Debug.DrawLine(selectedNode.transform.position + (Vector3)touchDelta*5 + Vector3.down * 5, selectedNode.transform.position + (Vector3)touchDelta*5 + Vector3.up * 5, Color.green);
                    float r = 10;
                    for (int i = 0; i < 8; i++) {
                        debugDir.Set(Mathf.Cos(angleStepR * i), Mathf.Sin(angleStepR * i));
                        Debug.DrawRay(selectedNode.transform.position, debugDir * r, Color.blue);
                    }
                    Vector2 touchDeltaMod = newTouchDir * r;
                    Debug.DrawLine(new Vector3(selectedNode.transform.position.x, selectedNode.transform.position.y, -1), new Vector3(selectedNode.transform.position.x + touchDeltaMod.x, selectedNode.transform.position.y + touchDeltaMod.y, -1), Color.red);
                    */
                }
            }
        }
        else {
            if (selectedNode) {
                Node getNode = selectedNode.GetNeighborFromDirection(newTouchDir);
                selectedNode.SetToNode(getNode);
            }
                
            canTouch = true;
            selectedNode = null;
            availableDirections.Clear();
        }
    }
}
