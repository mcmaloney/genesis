using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Genesis.Utils;


namespace Genesis.GeoPrimitives
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class GeoPolygon : MonoBehaviour
    {
        public TextAsset sampleJson; // Placeholder asset now until we get API calls working...
        public JSONObject polygonData;
        public Vector2d[] latLonVertices;
        public Vector2d[] xyVertices;
        public Vector2[] scaledVertices;
        public Material defaultMaterial;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh mesh;
        private Mesh meshCollider;


        // Parse JSON and build an array of the "coordinates" item
        public void buildLatLonVerticesFromJSON()
        {
            // The parsing of the input JSON should be broken into a new function
            polygonData = new JSONObject(sampleJson.text);
            latLonVertices = new Vector2d[polygonData["geometry"][1].Count];

            for (int i = 0; i < polygonData["geometry"][1].Count; i++)
            {
                Vector2d vertex = new Vector2d(polygonData["geometry"][1][i][0].n, polygonData["geometry"][1][i][1].n);
                latLonVertices[i] = vertex;
                Debug.Log("Lat/Lon vertex: " + latLonVertices[i]); 
            }
        }

        // Convert lat / lon coordinates to XY meters
        public void buildXYVertices()
        {
            xyVertices = new Vector2d[latLonVertices.Length - 1];
            for (int i = 0; i < latLonVertices.Length - 1; i++)
            {
                Vector2d xyVertex = Conversions.GeoToWorldPosition(latLonVertices[i].y, latLonVertices[i].x, new Vector2d(0, 0));
                xyVertices[i] = new Vector2d(xyVertex.y, xyVertex.x);
                Debug.Log("XY Vertex: " + xyVertices[i]);
            }
        }

        // Scale the vertices to the world origin and the scaling factor
        public void buildScaledVertices(Vector2d referencePoint, float scaleFactor)
        {
            scaledVertices = new Vector2[xyVertices.Length];
            for (int i = 0; i < xyVertices.Length; i++)
            {
                Vector2 scaledVertex = new Vector2((float)(xyVertices[i].y - referencePoint.x) * scaleFactor, (float)(xyVertices[i].x - referencePoint.y) * scaleFactor);
                scaledVertices[i] = scaledVertex;
                Debug.Log("Scaled Vertex: " + scaledVertices[i]);
            }
        }

        // Build triangles, normals, mesh. Render the polygon with a mesh
        public void Draw(Vector2d origin, float scale)
        {
            meshFilter = gameObject.GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>().sharedMesh;

            buildLatLonVerticesFromJSON();
            buildXYVertices();
            buildScaledVertices(origin, scale);

            Triangulator tr = new Triangulator(scaledVertices);
            int[] indices = tr.Triangulate();

            Vector3[] vertices3D = new Vector3[scaledVertices.Length];
            for (int i = 0; i < vertices3D.Length; i++)
            {
                vertices3D[i] = new Vector3(scaledVertices[i].x, 0f, scaledVertices[i].y);
                Debug.Log(vertices3D[i]);
            }

            mesh = new Mesh();
            mesh.vertices = vertices3D;
            mesh.triangles = indices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshFilter.mesh = mesh;
            meshRenderer.material = defaultMaterial;
            meshCollider = null;
            meshCollider = mesh;
        }
    }
}
