using UnityEngine;
using System.Collections;


public class LocationConverter
{
    private float k0 = 0.9996f; // Scale on central meridian
    private float a = 6378137.0f; // Equatorial radius for WGS84
    private float datumFlat = 0.003352811f; // Polar flattening for WGS84

    public Vector3 latLonToUTM(Vector2 latLon)
    {
        float lat = latLon.x;
        float lon = latLon.y;
        float f = 1f / datumFlat;
        float b = 6356752.314f;
        float e = Mathf.Sqrt(1f - (b / a) * (b / a));
        float degreesToRadians = Mathf.PI / 180f;
        float phi = lat * degreesToRadians;
        float lng = lon * degreesToRadians;

        // UTM zone (z)
        float utmz = 1f + Mathf.Floor((lon + 180f) / 6f);

        float zcm = 3f + 6f * (utmz - 1f) - 180f;
        float e0 = e / Mathf.Sqrt(1f - e * e);
        float esq = (1f - (b / a) * (b / a));
        float e0sq = e * e / (1f - e * e);
        float N = a / Mathf.Sqrt(1f - Mathf.Pow(e * Mathf.Sin(phi), 2f));
        float T = Mathf.Pow(Mathf.Tan(phi), 2f);
        float C = e0sq * Mathf.Pow(Mathf.Cos(phi), 2f);
        float A = (lon - zcm) * degreesToRadians * Mathf.Cos(phi);
        float M = phi * (1f - esq * (1f / 4f + esq * (3f / 64f + 5f * esq / 256f)));
        M = M - Mathf.Sin(2f * phi) * (esq * (3f / 8f + esq * (3f / 32f + 45f * esq / 1024f)));
        M = M + Mathf.Sin(4 * phi) * (esq * esq * (15f / 256f + esq * 45f / 1024f));
        M = M - Mathf.Sin(6 * phi) * (esq * esq * esq * (35f / 3072f));
        M = M * a;
        float M0 = 0f;

        // Easting relative to Central Meridian (x)
        float x = k0 * N * A * (1f + A * A * ((1f - T + C) / 6 + A * A * (5f - 18f * T + T * T + 72f * C - 58f * e0sq) / 120f));
        x = x + 500000f;

        // Northing (y)
        float y = k0 * (M - M0 + N * Mathf.Tan(phi) * (A * A * (1f / 2f + A * A * ((5f - T + 9f * C + 4f * C * C) / 24f + A * A * (61f - 58f * T + T * T + 600f * C - 330f * e0sq) / 720f))));
        float yg = y + 10000000f; // Northing global: from South Pole
        if (y < 0)
        {
            y = 10000000f + y;
        }

        // Latitude zone: A-B S of -80, C-W -80 to +72, X 72-84, Y,Z N of 84
        float latz = 0f;

        if (lat > -80 && lat < 72)
        {
            latz = Mathf.Floor((lat + 80f) / 8f) + 2;
        }

        if (lat > 72 && lat < 84)
        {
            latz = 21;
        }

        if (lat > 84)
        {
            latz = 23;
        }

        Vector3 UTMCoords = new Vector3(x, y, utmz);
        return UTMCoords;
    }
}
