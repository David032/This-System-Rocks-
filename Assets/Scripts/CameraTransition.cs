using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera backupCamera = new Camera();
    [SerializeField] private List<Camera> cameras = new List<Camera>();
    [SerializeField] private Camera activeCamera;
    private Camera selectedCamera;
    private int cameraIndex;

    void Start()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].enabled = false;
            
            if (cameras[i] == mainCamera) 
                cameraIndex = i;
        }
        
        selectedCamera = cameras[cameraIndex];
        activeCamera = selectedCamera;
        activeCamera.enabled = true;
        UpdateCameras();
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            var index = cameraIndex - 1;
            if (index < 0)
                index = cameras.Count - 1;
            cameraIndex = index;
            UpdateCameras();
        }
    }

    private void UpdateCameras()
    {
        if (backupCamera == null)
            ResetBackupCamera();
        
        selectedCamera = cameras[cameraIndex];
        if (activeCamera != selectedCamera && !backupCamera.enabled)
            CameraLerp();
    }

    private void ResetBackupCamera()
    {
        var go = Instantiate(new GameObject(), transform.position, transform.rotation);
        go.AddComponent<Camera>();
        backupCamera = go.GetComponent<Camera>();
        
        backupCamera.enabled = false;
        var transform1 = backupCamera.transform;
        var transform2 = activeCamera.transform;
        transform1.position = transform2.position;
        transform1.rotation = transform2.rotation;
    }

    private void CameraLerp()
    {
        backupCamera.enabled = true;
        activeCamera.enabled = false;
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        float timescale = 0;
        var dest = selectedCamera.transform;
        var moveCam = backupCamera.transform;
        while (moveCam.position != dest.position)
        {
            timescale += Time.deltaTime;
            moveCam.position = Vector3.Lerp(activeCamera.transform.position, dest.position,timescale);
            moveCam.rotation = Quaternion.Lerp(activeCamera.transform.rotation, dest.rotation, timescale);
            yield return null;
        }
        ApplyNewCameras();
    }

    void ApplyNewCameras()
    {
        backupCamera.enabled = false;
        activeCamera = selectedCamera;
        activeCamera.enabled = true;
    }
    
}
