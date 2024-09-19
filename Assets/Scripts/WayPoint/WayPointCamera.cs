using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WayPointCamera : MonoBehaviour
{
    public Dictionary<GameObject,WayPoint> wayPointMarkers = new Dictionary<GameObject, WayPoint>(); //onScreenMarker,waypoint
    GameObject canvas;
    [SerializeField] KeyCode keybind;

    [SerializeField] List<string> wayPointTags;

    [SerializeField] Camera FPP;
    [SerializeField] Camera TPP;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        Debug.Assert(canvas != null, "Canvas not Found");
        Debug.Assert(FPP != null, "FPPcam not Found");
        Debug.Assert(TPP != null, "TPPcam not Found");
    }

    void Update()
    {
        if (Input.GetKey(keybind))
        {
            FindWayPoints();
            foreach (GameObject onScreenMarker in wayPointMarkers.Keys)
            {
                onScreenMarker.SetActive(true);
                GameObject wayPointObj = wayPointMarkers.GetValueOrDefault(onScreenMarker).gameObject;
                if (wayPointObj == null || !wayPointObj.activeSelf || wayPointObj.GetComponent<WayPoint>() == null)
                {
                    Destroy(onScreenMarker);
                    wayPointMarkers.Remove(onScreenMarker);
                    break;
                }
                WayPoint wayPoint = wayPointObj.GetComponent<WayPoint>();
                Camera activeCam = null;
                if (FPP.gameObject.activeSelf) activeCam = FPP;
                else if (TPP.gameObject.activeSelf) activeCam = TPP;
                if (activeCam != null) onScreenMarker.transform.position = activeCam.WorldToScreenPoint(wayPointObj.transform.position);
            }
        }
        else
        {
            foreach (GameObject onScreenMarker in wayPointMarkers.Keys)
            {
                onScreenMarker.SetActive(false);
            }
        }
    }

    private void FindWayPoints()
    {
        // searches for all gameObjects in each tag mentioned in "wayPointTags"
        foreach (string tag in wayPointTags)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                if (obj.GetComponent<WayPoint>() != null) AddWayPoint(obj);
            }
        }
    }

    private void AddWayPoint(GameObject wayPointObj)
    {
        if (!wayPointObj.activeSelf) return;
        WayPoint wayPoint = wayPointObj.GetComponent<WayPoint>();
        if (wayPointMarkers.Values.Contains(wayPoint)) return;
        if (wayPoint == null) return;   
        GameObject prefab = wayPoint.getPrefab();
        GameObject onScreenMarker = Instantiate(prefab);
        onScreenMarker.transform.SetParent(canvas.transform);
        wayPointMarkers.Add(onScreenMarker, wayPoint);
    }
}