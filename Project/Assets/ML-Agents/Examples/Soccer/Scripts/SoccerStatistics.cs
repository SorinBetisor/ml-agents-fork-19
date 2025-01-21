using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEditor;

public class SoccerStatistics : MonoBehaviour
{
    static public List<SoccerEnvController> soccerEnvironments = new List<SoccerEnvController>();
    static private float[] criticalValueMapper = new float[] {
         1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
        11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
        21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 40, 60, 120
    };
    static public int requiredLevelOfConfidence = 5;
    static private float[][] criticalValueTable = new float[][] {
// alpha = 0.40, 0.30,  0.20,  0.15,  0.10,  0.05, 0.025
        new float[] {0.325f, 0.727f, 1.376f, 1.963f, 3.078f, 6.314f, 12.706f}, // 1
        new float[] {0.289f, 0.617f, 1.061f, 1.386f, 1.886f, 2.920f, 4.303f}, // 2
        new float[] {0.277f, 0.584f, 0.978f, 1.250f, 1.638f, 2.353f, 3.182f}, // 3
        new float[] {0.271f, 0.569f, 0.941f, 1.190f, 1.533f, 2.132f, 2.776f}, // 4
        new float[] {0.267f, 0.559f, 0.920f, 1.156f, 1.476f, 2.015f, 2.571f}, // 5
        new float[] {0.265f, 0.553f, 0.906f, 1.134f, 1.440f, 1.943f, 2.447f}, // 6
        new float[] {0.263f, 0.549f, 0.896f, 1.119f, 1.415f, 1.895f, 2.365f}, // 7
        new float[] {0.262f, 0.546f, 0.889f, 1.108f, 1.397f, 1.860f, 2.306f}, // 8
        new float[] {0.261f, 0.543f, 0.883f, 1.100f, 1.383f, 1.833f, 2.262f}, // 9
        new float[] {0.260f, 0.542f, 0.879f, 1.093f, 1.372f, 1.812f, 2.228f}, // 10
        new float[] {0.260f, 0.540f, 0.876f, 1.088f, 1.363f, 1.796f, 2.201f}, // 11
        new float[] {0.259f, 0.539f, 0.873f, 1.083f, 1.356f, 1.782f, 2.179f}, // 12
        new float[] {0.259f, 0.538f, 0.870f, 1.079f, 1.350f, 1.771f, 2.160f}, // 13
        new float[] {0.258f, 0.537f, 0.868f, 1.076f, 1.345f, 1.761f, 2.145f}, // 14
        new float[] {0.258f, 0.536f, 0.866f, 1.074f, 1.341f, 1.753f, 2.131f}, // 15
        new float[] {0.258f, 0.535f, 0.865f, 1.071f, 1.337f, 1.746f, 2.120f}, // 16
        new float[] {0.257f, 0.534f, 0.863f, 1.069f, 1.333f, 1.740f, 2.110f}, // 17
        new float[] {0.257f, 0.534f, 0.862f, 1.067f, 1.330f, 1.734f, 2.101f}, // 18
        new float[] {0.257f, 0.533f, 0.861f, 1.066f, 1.328f, 1.729f, 2.093f}, // 19
        new float[] {0.257f, 0.533f, 0.860f, 1.064f, 1.325f, 1.725f, 2.086f}, // 20
        new float[] {0.257f, 0.532f, 0.859f, 1.063f, 1.323f, 1.721f, 2.080f}, // 21
        new float[] {0.256f, 0.532f, 0.858f, 1.061f, 1.321f, 1.717f, 2.074f}, // 22
        new float[] {0.256f, 0.532f, 0.858f, 1.060f, 1.319f, 1.714f, 2.069f}, // 23
        new float[] {0.256f, 0.531f, 0.857f, 1.059f, 1.318f, 1.711f, 2.064f}, // 24
        new float[] {0.256f, 0.531f, 0.856f, 1.058f, 1.316f, 1.708f, 2.060f}, // 25
        new float[] {0.256f, 0.531f, 0.856f, 1.058f, 1.315f, 1.706f, 2.056f}, // 26
        new float[] {0.256f, 0.531f, 0.855f, 1.057f, 1.314f, 1.703f, 2.052f}, // 27
        new float[] {0.256f, 0.530f, 0.855f, 1.056f, 1.313f, 1.701f, 2.048f}, // 28
        new float[] {0.256f, 0.530f, 0.854f, 1.055f, 1.311f, 1.699f, 2.045f}, // 29
        new float[] {0.256f, 0.530f, 0.854f, 1.055f, 1.310f, 1.697f, 2.042f}, // 30
        new float[] {0.255f, 0.529f, 0.851f, 1.050f, 1.303f, 1.684f, 2.021f}, // 40
        new float[] {0.254f, 0.527f, 0.848f, 1.045f, 1.296f, 1.671f, 2.000f}, // 60
        new float[] {0.254f, 0.526f, 0.845f, 1.041f, 1.289f, 1.658f, 1.980f}, // 120
        new float[] {0.253f, 0.524f, 0.842f, 1.036f, 1.282f, 1.645f, 1.960f}, // ∞
    };

    public int targetFrames = 10000;
    private int frames;
    private float timer;

    public void Start()
    {
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.GetComponent<SoccerEnvController>() != null)
            {
                soccerEnvironments.Add(root.GetComponent<SoccerEnvController>());
            }
        }
    }

    [MenuItem("GameObject/InitializeEnvironments")]
    static void Initialize()
    {
        int x = 0;
        int z = -1;
        int scale = 40;
        int j = 1;
        int i = 0;

        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (root.GetComponent<SoccerEnvController>() != null)
            {
                if(i == 5)
                {
                    z++;
                    j = 0;
                    x++;
                }
                else if(i==0){
                    j=1;
                    z++;
                    x--;
                }
                if(j == 0)
                {
                    x--;
                    i--;
                }else if (j == 1){
                    x++;
                    i++;
                }

                root.transform.position = new Vector3(x * scale, 0, z * scale);
            }
        }
    }

    public void Update()
    {
        frames++;
        timer += Time.deltaTime;

        // Show progress
        if (frames % (targetFrames / 100) == 0)
        {
            Debug.Log(((double) frames / (double) targetFrames) * 100d + "% - " + (long) timer + " seconds in");
        }

        if (frames % targetFrames == 0)
        {
            LogStatistics();
        }
    }

    public void OnApplicationQuit()
    {
        LogStatistics();
    }

    public void LogStatistics()
    {
        double[] samplePoints_1 = new double[soccerEnvironments.Count];
        double[] samplePoints_2 = new double[soccerEnvironments.Count];

        for (int i = 0; i < samplePoints_1.Length; i++)
        {
            double x_1 = soccerEnvironments[i].goalsBlue;
            double x_2 = soccerEnvironments[i].goalsPurple;
            samplePoints_1[i] = x_1;
            samplePoints_2[i] = x_2;
        }

        LogStatisticsTwoSamplePopulationMeanTest(samplePoints_1, samplePoints_2);
        LogFPS();
    }

    public void LogFPS() {
        float averageFPS = frames / timer;
        Debug.Log("Average FPS during testing: " + averageFPS + " FPS measured over " + (long) timer + "s");
    }

    static void LogStatisticsTwoSamplePopulationMeanTest(double[] samplePoints_1, double[] samplePoints_2)
    {
        int n_1 = samplePoints_1.Length;
        int n_2 = samplePoints_2.Length;
        double xHat_1 = 0; // Sample mean
        double xHat_2 = 0; // Sample mean
        double s2_1 = 0; // Sample deviation
        double s2_2 = 0; // Sample deviation

        // Compute sample mean
        for (int i = 0; i < samplePoints_1.Length; i++)
        {
            double x_1 = samplePoints_1[i];
            double x_2 = samplePoints_2[i];
            xHat_1 += x_1 / n_1;
            xHat_2 += x_2 / n_2;
        }

        // Compute sample variance
        for (int i = 0; i < samplePoints_1.Length; i++)
        {
            double x_1 = samplePoints_1[i];
            double x_2 = samplePoints_2[i];
            s2_1 += Math.Pow(x_1 - xHat_1, 2) / (n_1 - 1);
            s2_2 += Math.Pow(x_2 - xHat_2, 2) / (n_2 - 1);
        }

        Debug.Log("Sample mean Blue: " + xHat_1 + ", Sample Mean Purple: " + xHat_2 + ", Sample Deviation Blue: " + Math.Sqrt(s2_1) + ", Sample Deviation Purple: " + Math.Sqrt(s2_2));
        Debug.Log("verdict: " + (!PerformTwoSamplePopulationMeanTest(n_1, n_2, xHat_1, xHat_2, s2_1, s2_2)? " Blue and purple teams perform equally well." : (xHat_1 < xHat_2? " Team Purple is better." : " Team Blue is better.")));
    }

    static void LogLatexStatistics(int n_1, int n_2, double xHat_1, double xHat_2, double s2_1, double s2_2, double d_0, double s_p, double t, double v, double t_critical, bool conclusion)
    {
        Debug.Log(
            "1. $H_0: \\mu1 - \\mu2 = 0$.\\\\" + "\n" + 
            "2. $H_1: \\mu1 - \\mu2 \\neq 0$.\\\\" + "\n" + 
            "3. $\\alpha = 0.05$.\\\\" + "\n" + 
            "4. Critical region: $t > " + t_critical.ToString("F3") + "$, where $t = \\frac{(\\bar{x}_1 - \\bar{x}_2) - d_0}{S_p \\sqrt{\\frac{1}{n_1} + \\frac{1}{n_2}}}$ with $v = " + v + "$ degrees of freedom.\\\\" + "\n" + 
            "5. Computations:\\\\" + "\n" + 
            "" + "\n" + 
            "\\begin{align}" + "\n" + 
            "\\bar{x}_1 = " + xHat_1.ToString("F3") + ",\\ s_1 = " + Math.Sqrt(s2_1).ToString("F3") + ",\\ n_1 = " + n_1 + ",\\\\" + "\n" + 
            "\\bar{x}_2 = " + xHat_2.ToString("F3") + ",\\ s_2 = " + Math.Sqrt(s2_2).ToString("F3") + ",\\ n_2 = " + n_2 + "." + "\n" + 
            "\\end{align}" + "\n" + 
            "" + "\n" + 
            "Hence" + "\n" + 
            "" + "\n" + 
            "$$S_p = \\sqrt{\\frac{(" + n_1 + " - 1) (" + Math.Sqrt(s2_1).ToString("F3") + ")^2 + (" + n_2 + " - 1) (" + Math.Sqrt(s2_2).ToString("F3") + ")^2}{" + n_1 + " + " + n_2 + " - 2}}=" + s_p.ToString("F3") + "$$" + "\n" + 
            "$$t = \\frac{(" + xHat_1.ToString("F3") + " - " + xHat_2.ToString("F3") + ") - " + d_0.ToString("F3") + "}{" + s_p.ToString("F3") + "\\cdot \\sqrt{\\frac{1}{" + n_1 + "} + \\frac{1}{" + n_2 + "}}}=" + t.ToString("F3") + "$$\\\\" + "\n" + 
            "\\\\" + "\n" + 
            (!conclusion? "6. Decision: Do not reject $H_0$. We are unable to conclude that there is a significant difference between the two teams."
                       : "6. Decision: Reject $H_0$. We are able to conclude that there is a significant difference between the two teams. We can observe that, on average, " + ((xHat_1 < xHat_2? "team purple performs better." : "team blue performs better.")))
        );
    }

    static bool PerformTwoSamplePopulationMeanTest(int n_1, int n_2, double xHat_1, double xHat_2, double s2_1, double s2_2)
    {
        double d_0 = 0; // \mu_0 - \mu_1 = 0
        double v = (n_1 + n_2) - 2;

        double s_p = Math.Sqrt(((n_1 - 1) * s2_1 + (n_2 - 1) * s2_2) / v);
        double t = ((xHat_1 - xHat_2) - d_0) / (s_p * Math.Sqrt((1d / n_1) + (1d / n_2)));
        
        double t_critical = criticalValueTable[criticalValueTable.Length - 1][requiredLevelOfConfidence];

        for (int i = 0; i < criticalValueMapper.Length; i++)
        {
            if (v <= criticalValueMapper[i])
            {
                t_critical = criticalValueTable[i][requiredLevelOfConfidence];
                break;
            }
        }
        
        bool conclusion = Math.Abs(t) > t_critical;

        LogLatexStatistics(n_1, n_2, xHat_1, xHat_2, s2_1, s2_2, d_0, s_p, t, v, t_critical, conclusion);
        Debug.Log("t: " + t + ", Using critical value " + t_critical + " associated to degreesOfFreedom " + v);
        return conclusion;
    }
}
