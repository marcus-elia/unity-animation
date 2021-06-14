using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGlobe : MonoBehaviour
{
    public GameObject WaterCubePrefab;
    public GameObject LandCubePrefab;

    private List<GameObject> cubes;

    public int numLayersPerHalf = 5;
    public float radius = 10;
    public float divisionFactor = 1.5f; // determines how many cubes per layer


    // Start is called before the first frame update
    void Start()
    {
        cubes = new List<GameObject>();

        float theta; // around the y-axis
        float phi;   // measured up from the xz-plane
        float c;     // circumference
        float n;     // number of cubes in the current layer
        float curR;  // the radius of the current layer
        float dTheta;// how much to increase theta
        float x, y, z;
        float dPhi = Mathf.PI / (2 * numLayersPerHalf);

        // Create the equator first
        theta = -Mathf.PI;
        phi = 0f;
        y = 0f;
        c = 2 * Mathf.PI * radius;
        n = Mathf.RoundToInt(c / divisionFactor);
        dTheta = 2 * Mathf.PI / n;
        for (int i = 0; i < n; i++)
        {
            x = radius * Mathf.Cos(theta);
            z = radius * Mathf.Sin(theta);
            GameObject cube;
            if (IsLand(theta, phi))
            {
                cube = Instantiate(LandCubePrefab);
            }
            else
            {
                cube = Instantiate(WaterCubePrefab);
            }
            cube.transform.SetParent(transform);
            cube.transform.localPosition = new Vector3(x, y, z);
            cubes.Add(cube);

            theta += dTheta;
        }

        // Now do the other layers
        for (int j = 1; j <= numLayersPerHalf; j++)
        {
            phi += dPhi;
            curR = radius * Mathf.Cos(phi);
            c = 2 * Mathf.PI * curR;
            if (c > 0)
            {
                n = Mathf.RoundToInt(c / divisionFactor);
            }
            else
            {
                n = 1;
            }
            dTheta = 2 * Mathf.PI / n;

            // Do the layer above the equator
            y = radius * Mathf.Sin(phi);
            theta = -Mathf.PI;
            for (int i = 0; i < n; i++)
            {
                x = curR * Mathf.Cos(theta);
                z = curR * Mathf.Sin(theta);
                GameObject cube;
                if (IsLand(theta, phi))
                {
                    cube = Instantiate(LandCubePrefab);
                }
                else
                {
                    cube = Instantiate(WaterCubePrefab);
                }
                cube.transform.SetParent(transform);
                cube.transform.localPosition = new Vector3(x, y, z);
                cubes.Add(cube);

                theta += dTheta;
            }

            // Do the layer below the equator
            y = radius * Mathf.Sin(-phi);
            theta = -Mathf.PI;
            for (int i = 0; i < n; i++)
            {
                x = curR * Mathf.Cos(theta);
                z = curR * Mathf.Sin(theta);
                GameObject cube;
                if (IsLand(theta, -phi))
                {
                    cube = Instantiate(LandCubePrefab);
                }
                else
                {
                    cube = Instantiate(WaterCubePrefab);
                }
                cube.transform.SetParent(transform);
                cube.transform.localPosition = new Vector3(x, y, z);
                cubes.Add(cube);

                theta += dTheta;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Return true if the given lon/lat represents land on each
    private bool IsLand(float lon, float lat)
    {
        return IsNorthAmerica(lon, lat) || IsSouthAmerica(lon, lat) || IsEurope(lon, lat) || IsAsia(lon, lat) ||
            IsAfrica(lon, lat) || IsAustralia(lon, lat) || IsAntarctica(lon, lat);
    }

    private bool IsNorthAmerica(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((-125 < lon && lon < -70) && (35 <= lat && lat < 40)) ||                                // Central US
            ((-125 < lon && lon < -70) && (40 <= lat && lat < 45 && 0.833*lon + 104.167 < lat)) ||      // Northern US
            ((-125 < lon && lon < -78) && (30 < lat && lat < 35 && -lon - 90.0 < lat && 1.25*lon + 132.5 < lat)) ||  // Southern US
            ((-130 < lon && lon < -56) && (45 <= lat && lat < 49 && -1.67*lon - 163.33 < lat && 0.2*lon + 59 < lat)) || // Southern Canada
            ((-81 < lon && lon < -52) && (48 < lat && lat < -0.565*lon + 16.21)) ||                    // Northern Quebec and Labrador
            ((-140 < lon && lon <= -81) && (49 <= lat && lat <= 60 && -0.8*lon - 52 < lat && lat < -0.444*lon + 16))  ||  // Central Canada
            ((-52 < lon && lon < -28) && (60 < lat && lat < 80 && 0.4375*lon + 80.125 < lat))  ||     // Greenland
            ((-140 < lon && lon < -89) && (60 < lat && lat < 70 && 0.83*lon + 139.167 < lat)) ||      // Northern Canada
            ((-160 < lon && lon <= -140) && (60 < lat && lat < 70)) ||                                // Alaska
            ((-118 < lon && lon < -100) && (20 < lat && -0.769*lon - 60.769 < lat && lat < 30)) ||    // Northern Mexico
            ((-105 <= lon && lon < -90) && (-0.4*lon - 22.0 < lat && lat <= 20)) ||                    // Southern Mexico
            ((-84 < lon && lon < -81) && (25 < lat && lat < 31))   ||                                 // Florida
            ((-88 < lon && lon < -66) && (65 < lat && lat < 75 && - 0.333*lon + 41.67 < lat && lat < -0.7*lon + 19.0)) || // Baffin Island
            ((-90 < lon && lon < -78) && (-0.583*lon - 38.5 < lat && lat < -0.917*lon - 62.5));       // Central America
    }

    private bool IsSouthAmerica(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((-81 < lon && lon < -35) && (-7 < lat && lat < 11 && lat < 2*lon + 160 && lat < -0.457*lon - 21.0)) ||  // North
            ((-81 < lon && lon < -35) && (-18 <= lat && lat < -5 && -1.3 * lon - 110.3 < lat && 1.857*lon + 60.0 < lat))  ||       // Central
            ((-72 < lon && lon <= -43) && (-24 < lat && lat < -18)) ||     // South Central
            ((-73 < lon && lon < -50) && (-55 < lat && lat <= -22 && 1.55 * lon + 55.5 < lat));    // Argentina
    }

    private bool IsEurope(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((-10 < lon && lon < 0) && (36 < lat && lat < 43)) ||     // Spain
            ((-10 < lon && lon < 0) && (50 < lat && lat < 59 && lat < 1.4 * lon + 65 && lat < -1.2 * lon + 52)) ||         // UK
            ((-4 < lon && lon < 15) && (42 < lat && lat < 54 && lat < 0.556*lon + 50.2 && 0.4375 * lon + 41.125 < lat)) || // Western Europe
            ((5 < lon && lon < 21) && (55 < lat && lat < 70 && lat < 0.571*lon + 59.143 && -lon + 67 < lat && 2*lon + 26 < lat)) ||  // Sweden/Norway
            ((-25 < lon && lon < -19) && (65 < lat && lat < 69)) ||      // Iceland
            ((15 < lon && lon < 30) && (37 < lat && lat < 54 && -1.6*lon + 69 < lat && 0.833*lon + 18.67 < lat && 1.5*lon < lat)) ||  // Eastern Europe
            ((21 <= lon && lon < 30) && (54 <= lat && lat < 70)) ||     // Finland and Baltic states
            ((30 <= lon && lon < 48) && (46 <= lat && lat < 68)) || // Russia, Ukraine, Belarus
            ((33 < lon && lon < 47) && (41 <= lat && lat < 46 && - 0.5*lon + 62 < lat)) ||  // Between Capsian and Black Seas
            ((28 < lon && lon < 48) && (36 < lat && lat < 41));          // Turkey and Armenia
    }
    private bool IsAsia(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((48 <= lon && lon < 105) && (46 < lat && lat < 76 && lat < 0.1667 * lon + 58.5)) ||  // Russia and North Kazakhstan
            ((105 <= lon && lon < 135) && (46 < lat && lat < 76 && lat < -0.133 * lon + 90)) ||    // Russia, North Mongolia, Manchuria
            ((135 <= lon && lon <= 180) && (55 < lat && lat < 71 && 0.222 * lon + 25 < lat && lat < -0.1 * lon + 86.0)) || // Far east Russia
            ((85 < lon && lon < 120) && (23 < lat && lat <= 46 && 1.625 * lon - 159 < lat)) ||    // China and Mongolia
            ((120 < lon && lon < 130) && (35 < lat && lat <= 46 && -lon + 161 < lat)) ||        // Manchuria and Korea
            ((127 < lon && lon < 140) && (31 < lat && lat < 43 && lat < 1.125 * lon - 113.25 && 0.4285 * lon - 25 < lat)) ||    // Japan
            ((103 < lon && lon < 109) && (9 < lat && lat <= 23 && lat < -2.25 * lon + 259.25 && 25.5 * lon - 2668.5 < lat)) || // Vietnam
            ((92 < lon && lon <= 103) && (10 < lat && lat <= 23 && -1.0833 * lon + 122.67 < lat)) ||    // Thailand and Myanmar
            ((110 < lon && lon < 118) && (-3 < lat && lat < 7 && lat < 0.5 * lon - 52)) ||    // Indonesia 1
            ((96 < lon && lon < 107) && (-5 < lat && lat < 6 && lat < -0.75 * lon + 78.5 && -0.818 * lon + 81.73 < lat)) || // Indonesia 2
            ((120 < lon && lon < 124) && (8 < lat && lat < 14)) ||           // Philippines
            ((68 < lon && lon < 86) && (22 < lat && lat < 30)) ||            // India
            ((78 < lon && lon < 87) && (1.556 * lon - 113 < lat && lat <= 22)) ||     // Southeast India
            ((70 < lon && lon <= 78) && (-1.75 * lon + 144.5f < lat && lat <= 22)) ||  // Southwest Indiea
            ((54 < lon && lon < 85) && (25 < lat && lat < 46)) ||               // Central Asia
            ((45 < lon && lon < 60) && (25 < lat && lat <= 36 && -0.625 * lon + 60.625 < lat)) ||    // Iran
            ((35 < lon && lon < 45) && (28 < lat && lat <= 36)) ||    // Middle east
            ((35 < lon && lon < 50) && (13 < lat && lat <= 28 && -2.875 * lon + 140.625 < lat && 0.125 * lon + 8.625 < lat && lat < -1.33 * lon + 90.67)) || // Saudi Arabia
            ((50 <= lon && lon < 59) && (15 < lat && lat < 24 && 0.6667 * lon + -18.33 < lat)); // Saudi Arabia east tip
     }

    private bool IsAfrica(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((-18 < lon && lon < 9) && (5 < lat && lat < 36 && -0.8 * lon + -2.4 < lat && lat < 1.04 * lon + 40.78)) || // West Africa
            ((9 <= lon && lon < 20) && (5 < lat && lat < -0.4 * lon + 38.0)) ||              // Central North
            ((20 <= lon && lon < 45) && (5 < lat && lat < -1.428 * lon + 75.28 && lat < 32)) ||   // Egypt, Sudan, eastern Libya
            ((45 <= lon && lon < 50) && (1 < lat && lat < 12 && lat < 0.2 * lon + 2 && 2 * lon - 88 < lat)) || // Somalia
            ((9 <= lon && lon <= 45) && (-10 < lat && lat <= 5 && -5 * lon + 50 < lat && 2.4 * lon - 106 < lat)) || // Middle Strip
            ((11 < lon && lon < 41) && (-15 < lat && lat <= -10)) ||                          // Another strip
            ((11 < lon && lon < 40) && (-34 < lat && lat <= -15 && -2.125 * lon + 6.375 < lat && 1.363 * lon - 70.91 < lat)) ||  // The south
            ((43 < lon && lon < 52) && (-25 < lat && lat < -13 && lat < 0.428 * lon - 34.857 && 2.75 * lon - 154.25 < lat));  // Madagascar
    }

    private bool IsAustralia(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((111 < lon && lon < 130) && (0.23529 * lon - 62.26 < lat && lat < 0.615 * lon - 92.77)) ||  // Western Australia
            ((130 <= lon && lon < 138) && (-0.6667 * lon + 58.33 < lat && lat < -13)) ||  // Central Australia
            ((138 <= lon && lon < 151) && (-35 < lat && lat < -20)) ||  // Eastern Australia
            ((140 < lon && lon < 147) && (-20 < lat && lat < -1.6 * lon + 217.8)) ||      // Northeast triangle
            ((138 < lon && lon < 150) && (-38 < lat && lat < -35 && -lon + 103.0 < lat)) ||  // Bottom east
            ((135 < lon && lon < 141) && (-0.8 * lon + 104.8 < lat && lat < 0)) ||      // Papua New Guinea west
            ((141 <= lon && lon < 150) && (-9 < lat && lat < -lon + 140)) ||               // Papua New Guinea east
            ((167 < lon && lon < 178) && (-46 < lat && lat < -37 && lat < 1.6*lon - 310.6 && 0.8 * lon + -181.2 < lat));  // New Zealand
    }

    private bool IsAntarctica(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return lat < -70;
    }
}
