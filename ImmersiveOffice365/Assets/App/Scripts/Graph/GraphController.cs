using EpForceDirectedGraph.cs;
using Office365DataHub.Data;
using Office365DataHub.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GraphController : MonoBehaviour
{
    public float Stiffness = 81.76f;
    public float Repulsion = 40000.0f;
    public float Damping = 0.5f;
    public float Threadshold = 0.1f;

    public Dictionary<string, GameObject> gameNodes = new Dictionary<string, GameObject>();
    public float NodeUpdate = 0.05f; // in seconds
    public float ScaleFactor = 0.003f;

    private ForceDirected3D fdg = null;
    private Graph graph = null;
    private GraphRenderer graphRenderer = null;

    public GameObject NodePrefab = null;
    public GameObject EdgePrefab = null;

    private DataModel model = new DataModel();

    private Dictionary<string, Node> nodes = new Dictionary<string, Node>();

    private void Initialize()
    {
        graph = new Graph();
        fdg = new ForceDirected3D(graph, Stiffness, Repulsion, Damping);
        fdg.Threadshold = Threadshold;
        graphRenderer = new GraphRenderer(this, fdg);
    }

    public void Reload()
    {
        gameObject.SetActive(true);

        nodes.Clear();

        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Initialize();

        model.LoadData();
    }

    public GameObject CreateNode(string id = "")
    {
        // Create node in graph
        NodeData data = new NodeData();
        data.mass = 10.0f;
        data.label = "node";
        string newId = id != "" ? id : Guid.NewGuid().ToString();
        Node newNode = new Node(newId, data);
        Node createdNode = graph.AddNode(newNode);

        // create unity node object
        GameObject go = GameObject.Instantiate(NodePrefab);
        go.transform.parent = gameObject.transform;
        gameNodes[createdNode.ID] = go;

        NodeInformation nodeInfo = go.GetComponent<NodeInformation>();
        nodeInfo.node = createdNode;

        return go;
    }

    public GameObject CreateEdge(Node source, Node target)
    {
        // create edge in graph
        EdgeData data = new EdgeData();
        data.label = "edge";
        data.length = 60.0f;
        Edge newEdge = new Edge(Guid.NewGuid().ToString(), source, target, data);
        Edge createdEdge = graph.AddEdge(newEdge);

        // create unity edge object
        GameObject go = GameObject.Instantiate(EdgePrefab);
        go.transform.parent = gameObject.transform;
        gameNodes[createdEdge.ID] = go;

        EdgeInformation edgeInfo = go.GetComponent<EdgeInformation>();
        edgeInfo.edge = createdEdge;

        return go;
    }

    void Start()
    {
        Initialize();

        model.LoadData();
    }

    void Update()
    {
        graphRenderer.Draw(NodeUpdate);

        HandleQueue(model.Queue);
    }

    public void HandleQueue(DataQueue queue)
    {
        BaseEntity root;
        BaseEntity refering;
        GameObject go;

        if (queue.GetFromQueue(out root, out refering))
        {
            Node rootNode = root != null ? nodes.ContainsKey(root.Id) ? nodes[root.Id] : null : null;
            Node referingNode = refering != null ? nodes.ContainsKey(refering.Id) ? nodes[refering.Id] : null : null;

            if (root != null && rootNode == null)
            {
                go = CreateNode(root.Id);
                NodeInformation nodeInfo = go.GetComponent<NodeInformation>();
                nodes[nodeInfo.node.ID] = nodeInfo.node;
                rootNode = nodeInfo.node;
                nodeInfo.entity = root;
            }

            if (refering != null && referingNode == null)
            {
                go = CreateNode(refering.Id);
                NodeInformation nodeInfo = go.GetComponent<NodeInformation>();
                nodes[nodeInfo.node.ID] = nodeInfo.node;
                referingNode = nodeInfo.node;
                nodeInfo.entity = refering;
            }

            if (rootNode != null && referingNode != null)
            {
                go = CreateEdge(rootNode, referingNode);
                EdgeInformation edgeInfo = go.GetComponent<EdgeInformation>();
            }
        }
    }
}
