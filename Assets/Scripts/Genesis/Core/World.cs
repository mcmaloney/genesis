using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Genesis.Data;

namespace Genesis.Core
{
    public class World : MonoBehaviour
    {
        public MapVisualization MapVisualization;
        public GameObject FeatureRenderer;
        public RectD ReferenceTileRect { get; set; }
        public float WorldScaleFactor { get; set; }
        public Vector2d RootTileOrigin { get; set; }
        public int Zoom;
        public Vector4 Range;
        public float TileSize = 100;
        public Dictionary<Vector2, UnityTile> _tiles;
        public float userZoomInput;

        public TextAsset testDataSource; // Need to replace with data from API calls!

        private List<DataSource> dataSources;
        private FeatureRenderer _FeatureRenderer;
        private float lastScale;
        private Vector3 scaleIncrement = new Vector3(0.001f, 0.001f, 0.001f);

        public void Update()
        {
            Scale();
        }

        public void Build(Vector2d coordinates, int zoom, Vector4 frame, string name)
        {
            gameObject.name = name;

            MapVisualization.Initialize(MapboxAccess.Instance);
            _tiles = new Dictionary<Vector2, UnityTile>();

            var v2 = Conversions.GeoToWorldPosition(coordinates.y, coordinates.x, new Vector2d(0, 0));
            var tms = Conversions.MetersToTile(v2, zoom);

            ReferenceTileRect = BuildReferenceTile(tms, zoom);
            RootTileOrigin = BuildRootTile((int)tms.x, (int)tms.y, zoom);

            WorldScaleFactor = (float)(TileSize / ReferenceTileRect.Size.x);

            transform.localScale = Vector3.one * WorldScaleFactor;

            for (int i = (int)(tms.x - frame.x); i <= (tms.x + frame.z); i++)
            {
                for (int j = (int)(tms.y - frame.y); j <= (tms.y + frame.w); j++)
                {
                    var tile = BuildTile(coordinates, i, j, zoom);
                    _tiles.Add(new Vector2(i, j), tile);
                    MapVisualization.ShowTile(tile);
                }
            }
        }

        private RectD BuildReferenceTile(Vector2 rootTileMeters, int zoom)
        {
            RectD referenceTile = Conversions.TileBounds(rootTileMeters, zoom);
            return referenceTile;
        }

        private Vector2d BuildRootTile(int x, int y, int zoom)
        {
            Vector2d rootTileOriginCoordinates = Conversions.TileIdToCenterLatitudeLongitude(x, y, zoom);
            return Conversions.GeoToWorldPosition(rootTileOriginCoordinates, new Vector2d(0, 0));
        }

        private UnityTile BuildTile(Vector2d coordinates, int xCoord, int yCoord, int zoom)
        {
            var tile = new GameObject("Tile - " + xCoord + " | " + yCoord).AddComponent<UnityTile>();
            tile.Zoom = zoom;
            tile.RelativeScale = Conversions.GetTileScaleInMeters(0, zoom) / Conversions.GetTileScaleInMeters((float)coordinates.y, zoom);
            tile.TileCoordinate = new Vector2(xCoord, yCoord);
            tile.Rect = Conversions.TileBounds(tile.TileCoordinate, zoom);
            tile.transform.position = new Vector3((float)(tile.Rect.Center.x - ReferenceTileRect.Center.x), 0, (float)(tile.Rect.Center.y - ReferenceTileRect.Center.y));
            tile.transform.SetParent(transform, false);
            return tile;
        }

        public void AddDataSource(TextAsset source)
        {
            DataSource dataSource = new DataSource(source);
            dataSources.Add(dataSource);
        }

        public void RenderDataSource(DataSource source)
        {
            _FeatureRenderer = FeatureRenderer.GetComponent<FeatureRenderer>();
            _FeatureRenderer.RenderDataSource(source, gameObject);
        }

        [ContextMenu("Test AddDataSource")]
        public void TestAddDataSource()
        {
            dataSources = new List<DataSource>();
            AddDataSource(testDataSource);
            RenderDataSource(dataSources[0]);
        }

        public void Scale()
        {
            float currentScale = userZoomInput;
            if (currentScale > 0 && transform.localScale.x > 0)
            {
                if (currentScale > lastScale)
                {
                    transform.localScale += scaleIncrement;
                }
                else
                {
                    transform.localScale -= scaleIncrement;
                }
            }

            lastScale = currentScale;
        }
    }
}

