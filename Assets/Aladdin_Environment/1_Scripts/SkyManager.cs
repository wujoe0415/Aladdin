using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public Trigger ice2desert;
    public Trigger desert2forest;

    public Material IceSkybox;
    public Material DesertSkybox;
    public Material ForestSkybox;
    void Start()
    {
        RenderSettings.skybox = IceSkybox;
    }

    // Update is called once per frame
    void Update()
    {
        if (ice2desert.GetTrigger())
        {
            RenderSettings.skybox = DesertSkybox;
        }
        else if (desert2forest.GetTrigger())
        {
            RenderSettings.skybox = ForestSkybox;
        }
    }
}
