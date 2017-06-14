using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;

public class GeoLine : MonoBehaviour {
    public TextAsset sampleJson;
    public JSONObject lineData;
    public Vector2d[] latLonNodes;
    public Vector2d[] xyNodes;
    public Vector2[] scaledNodes;
    public float lineRadius = 0.05f;
    public Material defaultMaterial;
    public Mesh cylinderMesh;

    public void buildLatLonNodesFromJSON()
    {
        // The parsing of the input JSON should be broken into a new function
        lineData = new JSONObject(sampleJson.text);
        latLonNodes = new Vector2d[lineData["geometry"][1].Count];

        for (int i = 0; i < lineData["geometry"][1].Count; i++)
        {
            Vector2d node = new Vector2d(lineData["geometry"][1][i][0].n, lineData["geometry"][1][i][1].n);
            latLonNodes[i] = node;
            Debug.Log("Lat/Lon node: " + latLonNodes[i]);
        }
    }

    // Convert lat / lon coordinates to XY meters
    public void buildXYNodes()
    {
        xyNodes = new Vector2d[latLonNodes.Length];
        for (int i = 0; i < latLonNodes.Length; i++)
        {
            Vector2d xyNode = Conversions.GeoToWorldPosition(latLonNodes[i].y, latLonNodes[i].x, new Vector2d(0, 0));
            xyNodes[i] = new Vector2d(xyNode.y, xyNode.x);
            Debug.Log("XY Vertex: " + xyNodes[i]);
        }
    }

    // Scale the nodes to the world origin and the scaling factor
    public void buildScaledNodes(Vector2d referencePoint, float scaleFactor)
    {
        scaledNodes = new Vector2[xyNodes.Length];
        for (int i = 0; i < xyNodes.Length; i++)
        {
            Vector2 scaledNode = new Vector2((float)(xyNodes[i].y - referencePoint.x) * scaleFactor, (float)(xyNodes[i].x - referencePoint.y) * scaleFactor);
            scaledNodes[i] = scaledNode;
            Debug.Log("Scaled Vertex: " + scaledNodes[i]);
        }
    }

    // Node from a scaled node position
    public GameObject createNode(Vector2 position)
    {
        GameObject node = new GameObject();
        node.transform.parent = gameObject.transform;
        node.transform.position = new Vector3(position.x, node.transform.parent.transform.position.y, position.y);
        node.name = "Node: " + node.transform.position;
        return node;
    }

    // Edge between two nodes
    public void createEdge(GameObject startNode, GameObject endNode)
    {
        GameObject edge = new GameObject();
        edge.name = "Edge: " + startNode.name + ", " + endNode.name;
        edge.transform.parent = gameObject.transform;

        createEdgeMesh(edge);

        edge.transform.position = endNode.transform.position;
        float edgeLength = 0.5f * Vector3.Distance(endNode.transform.position, startNode.transform.position);
        edge.transform.localScale = new Vector3(edge.transform.localScale.x, edgeLength, edge.transform.localScale.z);
        edge.transform.LookAt(startNode.transform, Vector3.up);
        edge.transform.rotation *= Quaternion.Euler(90, 0, 0);
    }

    // Add a mesh filter and renderer to an edge
    public void createEdgeMesh(GameObject edge)
    {
        GameObject edgeMesh = new GameObject();
        edgeMesh.transform.parent = edge.transform;
        edgeMesh.transform.localPosition = new Vector3(0f, 1f, 0f);
        edgeMesh.transform.localScale = new Vector3(lineRadius, 1f, lineRadius);

        MeshFilter edgeMeshFiter = edgeMesh.AddComponent<MeshFilter>();
        edgeMeshFiter.mesh = cylinderMesh;
        MeshRenderer edgeRenderer = edgeMesh.AddComponent<MeshRenderer>();

        edgeRenderer.material = defaultMaterial;
    }

    public void Draw(Vector2d origin, float scale)
    {
        buildLatLonNodesFromJSON();
        buildXYNodes();
        buildScaledNodes(origin, scale);

        for (int i = 0; i < scaledNodes.Length - 1; i++)
        {
            GameObject startNode = createNode(scaledNodes[i]);
            GameObject endNode = createNode(scaledNodes[i + 1]);
            createEdge(startNode, endNode);
        }
    }
}
