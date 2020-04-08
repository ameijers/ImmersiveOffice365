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
    private PersonEntityCollection persons = new PersonEntityCollection();
    private PersonRelationCollection relations = new PersonRelationCollection();

    private Dictionary<string, Node> nodes = new Dictionary<string, Node>();

    private bool error = false;

    private void Initialize()
    {
        graph = new Graph();
        fdg = new ForceDirected3D(graph, Stiffness, Repulsion, Damping);
        fdg.Threadshold = Threadshold;
        graphRenderer = new GraphRenderer(this, fdg);
    }

    private void InitializeConnection()
    {
        Office365DataHub.AuthenticationHelper.Instance.UseBetaAPI = false;
        Office365DataHub.AuthenticationHelper.Instance.UserLogon = true;
        Office365DataHub.AuthenticationHelper.Instance.RedirectUri = "https://www.appzinside.com";
        Office365DataHub.AuthenticationHelper.Instance.Authentication = new Office365DataHub.Authentication
        {
            ClientId = "8924f069-832c-40c0-bcb2-95f9ab328bd7",
            Scopes = new string[] { "User.Read", "User.Read.All", "People.Read", "People.Read.All" },
        };   
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

    private void CreateSampleData()
    {
        //List<Node> nodes = new List<Node>();

        //for (int i = 0; i < 50; i++)
        //{
        //    GameObject go = CreateNode();
        //    NodeInformation nodeInfo = go.GetComponent<NodeInformation>();
        //    nodes.Add(nodeInfo.node);
        //}

        //System.Random rand = new System.Random((int)Time.time);

        //for (int j = 0; j < 50; j++)
        //{
        //    int sourceIndex = rand.Next(50);
        //    int targetIndex = rand.Next(50);

        //    if (targetIndex != sourceIndex)
        //    {
        //        CreateEdge(nodes[sourceIndex], nodes[targetIndex]);
        //    }
        //}

        PersonEntity p1 = new PersonEntity { Id = "1", FullName = "Alexander" };
        PersonEntity p2 = new PersonEntity { Id = "2", FullName = "Colin" };
        PersonEntity p3 = new PersonEntity { Id = "3", FullName = "Owen" };
        PersonEntity p4 = new PersonEntity { Id = "4", FullName = "Tessa" };
        PersonEntity p5 = new PersonEntity { Id = "5", FullName = "Terry" };
        PersonEntity p6 = new PersonEntity { Id = "6", FullName = "Micheal" };
        PersonEntity p7 = new PersonEntity { Id = "7", FullName = "Jordy" };

        model.Queue.AddToQueue("", p1);
        model.Queue.AddToQueue(p1.Id, p2);
        model.Queue.AddToQueue(p1.Id, p3);
        model.Queue.AddToQueue(p1.Id, p4);
        model.Queue.AddToQueue(p3.Id, p4);
        model.Queue.AddToQueue(p2.Id, p3);
        model.Queue.AddToQueue(p1.Id, p5);
        model.Queue.AddToQueue(p5.Id, p6);
        model.Queue.AddToQueue(p6.Id, p7);
    }

    void Start()
    {
        Initialize();
        InitializeConnection();

#if UNITY_EDITOR
        CreateSampleData();
#endif

        Office365DataHub.Services.PeopleService.Instance.GetCurrentUser(OnGetPersonCompleted);
    }

    void OnGetPersonCompleted(PersonRequest request)
    {
        error = true;

        if (request.expection.Error != Office365DataHub.ServiceError.NoError)
        {
            DebugInformation.Instance.Log(request.expection.Exception.Message);
            DebugInformation.Instance.Log(request.expection.Exception.StackTrace);
            DebugInformation.Instance.Log(request.expection.Exception.InnerException.Message);
        }

        model.Queue.AddToQueue("", request.person);

        RelatedPeopleRequest relrequest = new RelatedPeopleRequest
        {
            person = request.person
        };

        Office365DataHub.Services.PeopleService.Instance.GetRelatedPeople(relrequest, OnGetRelatedPersonCompleted, OnGetRelatedPeopleCompleted);
    }
    void OnGetRelatedPersonCompleted(RelatedPeopleRequest request)
    {
        model.Queue.AddToQueue(request.person.Id, request.relatedPerson);
    }

    void OnGetRelatedPeopleCompleted(RelatedPeopleRequest request)
    {

    }


    void Update()
    {
        graphRenderer.Draw(NodeUpdate);

        HandleQueue(model.Queue);
    }

    public void HandleQueue(DataQueue queue)
    {
        string rootId;
        BaseEntity refering;
        GameObject go;

        if (queue.GetFromQueue(out rootId, out refering))
        {
            Node rootNode = nodes.ContainsKey(rootId) ? nodes[rootId] : null;
            Node referingNode = refering != null ? nodes.ContainsKey(refering.Id) ? nodes[refering.Id] : null : null;

            if (!string.IsNullOrEmpty(rootId) && rootNode == null)
            {
                go = CreateNode(rootId);
                NodeInformation nodeInfo = go.GetComponent<NodeInformation>();
                nodes[nodeInfo.node.ID] = nodeInfo.node;
                rootNode = nodeInfo.node;
            }

            if (refering != null && referingNode == null)
            {
                go = CreateNode(refering.Id);
                NodeInformation nodeInfo = go.GetComponent<NodeInformation>();
                nodes[nodeInfo.node.ID] = nodeInfo.node;
                referingNode = nodeInfo.node;
            }

            if (rootNode != null && referingNode != null)
            {
                go = CreateEdge(rootNode, referingNode);
                EdgeInformation edgeInfo = go.GetComponent<EdgeInformation>();
            }
        }
    }
}
