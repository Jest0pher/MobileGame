using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NodeDesc {
    public TeamTypes team;
    public int maxAmmo;
    public float fireRate;
    public float rechargeRate;
    public int maxHP;
    public List<int> neighborIndices;
}
public class GridSave
{
    public int rowCount;
    public int columnCount;
    public List<NodeDesc> nodeMap;

    public GridSave(int _row, int _column, List<Node> _map) {
        rowCount = _row;
        columnCount = _column;
        nodeMap = new List<NodeDesc>(_map.Capacity);
        foreach(Node node in _map) {
            List<int> indices = new List<int>(node.neighborNodes.Count);
            foreach (Node neighbor in node.neighborNodes) {
                indices.Add((int)(neighbor.gridPos.x + (columnCount * neighbor.gridPos.y)));
            }
            int toNode = (int)(node.gridPos.x + (columnCount * node.gridPos.y));
            NodeDesc desc;
            desc.team = node.Team;
            desc.maxAmmo = node.maxAmmo;
            desc.fireRate = node.fireRate;
            desc.rechargeRate = node.rechargeRate;
            desc.maxHP = node.maxHP;
            desc.neighborIndices = indices;

            nodeMap.Add(desc);
        }
    }
}
