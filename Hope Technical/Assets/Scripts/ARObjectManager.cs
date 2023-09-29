using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class ARObjectManager : MonoBehaviour
{
    public ARRaycastManager arRaycast;
    public GameObject numberObject;
    public TextMeshProUGUI text;

    public void SetNumber(int number)
    {
        text.text = number.ToString();
        SpawnInWorld();
    }

    void SpawnInWorld()
    {
        Vector2 spawnVector = new Vector2();
        spawnVector.x = Random.Range(0, Screen.width);
        spawnVector.y = Random.Range(0, Screen.height);

        List<ARRaycastHit> hit = new List<ARRaycastHit>();

        if (arRaycast.Raycast(spawnVector, hit, TrackableType.PlaneWithinPolygon))
        {
            numberObject.transform.position = hit[0].pose.position;
        }
    }
}
