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
            ((-90 < lon && lon < -78) && (-0.583*lon - 38.5 < lat && lat < -0.917*lon - 62.5));       // Central America
    }

    private bool IsSouthAmerica(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((-80 < lon && lon < -65) && (-20 < lat && lat < 10)) ||  // Northwest
            ((-65 < lon && lon < -48) && (0 <= lat && lat < 7))  ||       // Northeast
            ((-70 < lon && lon < -40) && (-30 < lat && lat < 0)) ||     // Brazil
            ((-70 < lon && lon < -50) && (-50 < lat && lat <= -20));    // Argentina
    }

    private bool IsEurope(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((-10 < lon && lon < 0) && (35 < lat && lat < 43)) ||     // Spain
            ((-10 < lon && lon < 0) && (50 < lat && lat < 58)) ||        // UK
            ((0 < lon && lon < 15) && (43 < lat && lat < 50)) ||        // Western Europe
            ((5 < lon && lon < 25) && (58 < lat && lat < 70)) ||        // Scandanavia
            ((15 < lon && lon < 35) && (40 < lat && lat < 53)) ||        // Eastern Europe
            ((25 < lon && lon < 45) && (45 < lat && lat < 70)) ||        // Russia
            ((27 < lon && lon < 43) && (36 < lat && lat < 41));          // Turkey
    }
    private bool IsAsia(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((45 < lon && lon < 135) && (45 < lat && lat < 70)) ||  // Russia
            ((135 < lon && lon < 180) && (60 < lat && lat < 70)) ||    // East Russia
            ((85 < lon && lon < 120) && (33 < lat && lat < 46)) ||    // China and Mongolia
            ((120 < lon && lon < 137) && (38 < lat && lat < 46)) ||    // Manchuria and Korea
            ((130 < lon && lon < 136) && (31 < lat && lat < 35)) ||    // Japan
            ((92 < lon && lon < 106) && (10 < lat && lat < 33)) ||    // Southeast Asia
            ((110 < lon && lon < 118) && (-5 < lat && lat < 6)) ||    // Indonesia
            ((120 < lon && lon < 124) && (7 < lat && lat < 14)) ||    // Philippines
            ((68 < lon && lon < 86) && (22 < lat && lat < 30)) ||    // India
            ((78 < lon && lon < 87) && (1.556*lon - 113 < lat && lat <= 22)) ||     // Southeast India
            ((70 < lon && lon <= 78) && (-1.75*lon + 144.5f < lat && lat <= 22)) ||  // Southwest Indiea
            ((54 < lon && lon < 85) && (25 < lat && lat < 46)) ||    // Central Asia
            ((45 < lon && lon < 60) && (27 < lat && lat < 36)) ||    // Iran
            ((35 < lon && lon < 45) && (28 < lat && lat < 36)) ||    // Middle east
            ((40 < lon && lon < 54) && (17 < lat && lat < 28));      // Saudi Arabia
    }

    private bool IsAfrica(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return ((-16 < lon && lon < 10) && (5 < lat && lat < 29)) ||   // West Africa
            ((-8 < lon && lon < 31) && (28 < lat && lat < 32)) ||   // North Africa
            ((9 < lon && lon < 33) && (-10 < lat && lat < 28)) ||   // Central Africa
            ((12 < lon && lon < 32) && (-34 < lat && lat < -9)) ||   // Southern Africa
            ((32 < lon && lon < 45) && (0 < lat && lat < 10)) ||    // Eastern Africa
            ((45 < lon && lon < 51) && (-25 < lat && lat < -13));   // Madagascar
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
            ((141 <= lon && lon < 150) && (-9 < lat && lat < -lon + 140));               // Papua New Guinea east
    }

    private bool IsAntarctica(float lon, float lat)
    {
        lon *= Mathf.Rad2Deg;
        lat *= Mathf.Rad2Deg;

        return lat < -70;
    }
}
