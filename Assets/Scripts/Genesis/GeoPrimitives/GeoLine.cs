using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Genesis.Core;

namespace Genesis.GeoPrimitives
{
    public class GeoLine : MonoBehaviour
    {
        public Vector2d[] latLonNodes;
        public Vector2d[] xyNodes;
        public Vector2[] scaledNodes;
        public float lineRadius = 0.05f;
        public Material defaultMaterial;
        public Mesh cylinderMesh;

        // Convert lat / lon coordinates to XY meters
        public void buildXYNodes()
        {
            xyNodes = new Vector2d[latLonNodes.Length];
            for (int i = 0; i < latLonNodes.Length; i++)
            {
                Vector2d xyNode = Conversions.GeoToWorldPosition(latLonNodes[i].x, latLonNodes[i].y, new Vector2d(0, 0));
                xyNodes[i] = xyNode;
                Debug.Log("XY Node: (" + xyNodes[i].x + ", " + xyNodes[i].y + ")");
            }
        }

        // Scale the nodes to the world origin and the scaling factor
        public void buildScaledNodes(Vector2d referencePoint, float scaleFactor)
        {
            scaledNodes = new Vector2[xyNodes.Length];
            for (int i = 0; i < xyNodes.Length; i++)
            {
                Vector2 scaledNode = new Vector2((float)(xyNodes[i].x - referencePoint.x) * scaleFactor, (float)(xyNodes[i].y - referencePoint.y) * scaleFactor);
                scaledNodes[i] = scaledNode;
                Debug.Log("Scaled Node: (" + scaledNodes[i].x + ", " + scaledNodes[i].y + ")");
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

        public void Draw(Vector2d[] nodes, GameObject world)
        {
            transform.parent = world.transform;
            World _world = world.GetComponent<World>();

            latLonNodes = nodes;
            buildXYNodes();
            buildScaledNodes(_world.RootTileOrigin, _world.WorldScaleFactor);

            for (int i = 0; i < scaledNodes.Length - 1; i++)
            {
                GameObject startNode = createNode(scaledNodes[i]);
                GameObject endNode = createNode(scaledNodes[i + 1]);
                createEdge(startNode, endNode);
            }
        }
    }
}

