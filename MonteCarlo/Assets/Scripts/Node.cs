using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public char[] state;
    public List<Node> children = new List<Node>();
    public Node parent = null;
    public int parentAction = -1;
    public float reward = 0;
    public int timesvisited = 0;


    public Node(char[] state)
    {
        this.state = state;
    }
}
