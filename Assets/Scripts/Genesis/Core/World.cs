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
        public Vector2d originReferencePoint = new Vector2d(0, 0);
        public Dictionary<Vector2, UnityTile> _tiles;
        public int grabMaskLayerNumber = 9;

        public TextAsset testDataSource; // Need to replace with data from API calls!

        private List<DataSource> dataSources;
        private FeatureRenderer _FeatureRenderer;
        private Vector2 rawTileSize;
        private Vector2 tileGridSize;
        private float lastScale;
        private Vector3 scaleIncrement = new Vector3(0.001f, 0.001f, 0.001f);

        public void Build(Vector2d coordinates, int zoom, Vector4 frame, string name)
        {
            BuildWorldRootObject(name, grabMaskLayerNumber); // Grab mask. Should make this more configurable
            BuildWorldPrimitives(OriginTileMeters(OriginDegreesToWorldMeters(coordinates, originReferencePoint), zoom), frame, zoom);
            SetWorldScale(ReferenceTileRect.Size.x);
            BuildTiles(OriginTileMeters(OriginDegreesToWorldMeters(coordinates, originReferencePoint), zoom), coordinates, frame, zoom);
            AddPhysics();
            BuildCollider();
        }

        private void BuildWorldRootObject(string name, int grabMaskLayerNumber)
        {
            gameObject.name = name;
            gameObject.layer = grabMaskLayerNumber;
        }

        private Vector2d OriginDegreesToWorldMeters(Vector2d originCoordinatesDegrees, Vector2d referencePoint)
        {
            return Conversions.GeoToWorldPosition(originCoordinatesDegrees.y, originCoordinatesDegrees.x, referencePoint);
        }

        private Vector2 OriginTileMeters(Vector2d originTileMeters, int zoom)
        {
            return Conversions.MetersToTile(originTileMeters, zoom);
        }

        private void BuildWorldPrimitives(Vector2 tileMeters, Vector4 range, int zoom)
        {
            MapVisualization.Initialize(MapboxAccess.Instance);
            _tiles = new Dictionary<Vector2, UnityTile>();
            ReferenceTileRect = BuildReferenceTile(tileMeters, zoom);
            RootTileOrigin = BuildRootTile((int)tileMeters.x, (int)tileMeters.y, zoom);
            Range = range;
            Zoom = zoom;
        }

        private void SetWorldScale(double refTileSize)
        {
            WorldScaleFactor = (float)(TileSize / refTileSize);
            transform.localScale = Vector3.one * WorldScaleFactor;
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

        private void BuildTiles(Vector2 originTileMeters, Vector2d originCoordinatesDegrees, Vector4 range, int zoom)
        {
            for (int i = (int)(originTileMeters.x - range.x); i <= (originTileMeters.x + range.z); i++)
            {
                for (int j = (int)(originTileMeters.y - range.y); j <= (originTileMeters.y + range.w); j++)
                {
                    var tile = BuildTile(originCoordinatesDegrees, i, j, zoom);
                    _tiles.Add(new Vector2(i, j), tile);
                    MapVisualization.ShowTile(tile);
                }
            }
        }

        private UnityTile BuildTile(Vector2d coordinates, int xCoord, int yCoord, int zoom)
        {
            var tile = new GameObject("Tile - " + xCoord + " | " + yCoord).AddComponent<UnityTile>();
            tile.Zoom = zoom;
            tile.RelativeScale = Conversions.GetTileScaleInMeters(0, zoom) / Conversions.GetTileScaleInMeters((float)coordinates.y, zoom);
            tile.TileCoordinate = new Vector2(xCoord, yCoord);
            tile.Rect = Conversions.TileBounds(tile.TileCoordinate, zoom);
            rawTileSize = new Vector2((float)tile.Rect.Size.x, (float)tile.Rect.Size.y);
            tile.transform.position = new Vector3((float)(tile.Rect.Center.x - ReferenceTileRect.Center.x), 0, (float)(tile.Rect.Center.y - ReferenceTileRect.Center.y));
            tile.transform.SetParent(transform, false);
            return tile;
        }

        private void AddPhysics()
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
        }

        private void BuildCollider()
        {
            tileGridSize = new Vector2((Range.x * 2) + 1, (Range.z * 2) + 1);
            float xSize = tileGridSize.x * rawTileSize.x;
            float colliderHeight = 443f; // should be tallest building height
            float zSize = tileGridSize.y * rawTileSize.y;
            BoxCollider bc = gameObject.AddComponent<BoxCollider>();
            bc.center = new Vector3(0f, colliderHeight / 2, 0f);
            bc.size = new Vector3(xSize, colliderHeight, zSize);
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
    }
}

