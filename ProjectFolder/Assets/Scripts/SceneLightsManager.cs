using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightsManager : MonoBehaviour
{
    public Light DirectionalLight;



    private static float MIN_DEATH_INTENSITY = 0.1f; 

    private float default_intensity;
    private float delta_intensity = 0.0f;

    void Start()
    {
        default_intensity = DirectionalLight.intensity;
    }
    public void ResetSceneLight()
    {
        DirectionalLight.intensity = default_intensity;
    }

    public void StartDarkenScene(float timer)
    {
        Debug.Log("Function Called, Start Darkening Scene Now...");
       
        delta_intensity = (default_intensity - MIN_DEATH_INTENSITY) / timer;
        StartCoroutine(DarkenScene());
    }

    IEnumerator DarkenScene()
    {

        while (DirectionalLight.intensity > MIN_DEATH_INTENSITY)
        {
            DarkenSceneLight();
            yield return null;
        }
        
    }


    void DarkenSceneLight()
    {
        DirectionalLight.intensity -= delta_intensity * Time.deltaTime;
    }
}
