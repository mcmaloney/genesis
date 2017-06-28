using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Genesis.Utils;
using Genesis.Core;


namespace Genesis.GeoPrimitives
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class GeoPolygon : MonoBehaviour
    {
        public Vector2d[] latLonVertices;
        public Vector2d[] xyVertices;
        public Vector2[] scaledVertices;
        public Material defaultMaterial;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Mesh mesh;
        private Mesh meshCollider;

        // Convert lat / lon coordinates to XY meters
        public void buildXYVertices()
        {
            xyVertices = new Vector2d[latLonVertices.Length - 1];
            for (int i = 0; i < latLonVertices.Length - 1; i++)
            {
                Vector2d xyVertex = Conversions.GeoToWorldPosition(latLonVertices[i].x, latLonVertices[i].y, new Vector2d(0, 0));
                xyVertices[i] = new Vector2d(xyVertex.x, xyVertex.y);
                Debug.Log("XY Vertex: (" + xyVertices[i].x + ", " + xyVertices[i].y + ")");
            }
        }

        // Scale the vertices to the world origin and the scaling factor
        public void buildScaledVertices(Vector2d referencePoint, float scaleFactor)
        {
            scaledVertices = new Vector2[xyVertices.Length];
            for (int i = 0; i < xyVertices.Length; i++)
            {
                Vector2 scaledVertex = new Vector2((float)(xyVertices[i].x - referencePoint.x) * scaleFactor, (float)(xyVertices[i].y - referencePoint.y) * scaleFactor);
                scaledVertices[i] = scaledVertex;
                Debug.Log("Scaled Vertex: (" + scaledVertices[i].x + ", " + scaledVertices[i].y + ")");
            }
        }

        // Build triangles, normals, mesh. Render the polygon with a mesh
        public void Draw(Vector2d[] vertices, GameObject world)
        {
            transform.parent = world.transform;
            World _world = world.GetComponent<World>();

            meshFilter = gameObject.GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>().sharedMesh;

            latLonVertices = vertices;
            buildXYVertices();
            buildScaledVertices(_world.RootTileOrigin, _world.WorldScaleFactor);

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
