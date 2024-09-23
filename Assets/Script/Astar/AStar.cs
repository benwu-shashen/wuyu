using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    private GridNodes gridNodes;
    private Node starNode;
    private Node targetNode;
    private int gridWidth;
    private int gridHeight;
    private int originX;
    private int originY;

    private List<Node> openNodeList;
    private HashSet<Node> closedNodeList;

    private bool pathFound;

    private bool GenerateGridNodes()
    {
        return false;
    }
}
