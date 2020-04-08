using EpForceDirectedGraph.cs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphRenderer : AbstractRenderer
{
    private GraphController controller = null;

    public GraphRenderer(GraphController controller, IForceDirected forceDirected) : base(forceDirected)
    {
        this.controller = controller;
    }

    public override void Clear()
    {
    }

    protected override void drawEdge(Edge iEdge, AbstractVector iPosition1, AbstractVector iPosition2)
    {
        if (controller.gameNodes.ContainsKey(iEdge.ID))
        {
            GameObject gameEdge = controller.gameNodes[iEdge.ID];

            GameObject source = controller.gameNodes[iEdge.Source.ID];
            GameObject target = controller.gameNodes[iEdge.Target.ID];

            LineRenderer line = gameEdge.GetComponent<LineRenderer>();

            if (source != null && target != null & line != null)
            {
                line.SetPositions(new Vector3[] { source.transform.position, target.transform.position });
            }
        }
    }

    protected override void drawNode(Node iNode, AbstractVector iPosition)
    {
        if (controller.gameNodes.ContainsKey(iNode.ID))
        {
            GameObject gameNode = controller.gameNodes[iNode.ID];

            // use localPosition, since you can determine the precise location by the transform of the gameobject which these
            // nodes are child of
            gameNode.transform.localPosition = new Vector3(iPosition.x * controller.ScaleFactor, iPosition.y * controller.ScaleFactor, iPosition.z * controller.ScaleFactor);
        }
    }
}
