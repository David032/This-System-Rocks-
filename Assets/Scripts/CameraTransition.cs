using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    [SerializeField] 
    private Camera mainCamera;
    [SerializeField] 
    private Camera backupCamera = new Camera();
    [SerializeField] 
    private List<Camera> cameras = new List<Camera>();
    [SerializeField] 
    
    private Camera activeCamera;
    private bool swappingCameras;
    private Camera selectedCamera;
    
    [SerializeField] 
    private int cameraIndex = 0;
    
    private float timescale = 0f;
    private GameObject earth;
    private Vector3 stopPoint;
    private float multiplier = 0.2f;

    FMOD.Studio.EventInstance whoosh;

    void Start()
    {
        whoosh = FMODUnity.RuntimeManager.CreateInstance("event:/atmos/whoosh");

        earth = GameObject.FindWithTag("Earth");
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(false);
            cameras[i].GetComponent<AudioListener>().enabled = false;
            
            if (!cameras[i].CompareTag("MainCamera")) continue;
            
            cameras[i].GetComponent<AudioListener>().enabled = true;
            cameraIndex = i;
        }
        
        // Initialise cameras and set index to the starting Camera.
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
            cameras[cameraIndex].gameObject.SetActive(false);

            cameraIndex = index;
            UpdateCameras();
            whoosh.start();
        }
        if (Input.GetKeyDown("e"))
        {
            var index = cameraIndex + 1;
            if (index >= cameras.Count)
                index = 0;
            cameras[cameraIndex].gameObject.SetActive(false);
            cameraIndex = index;
            UpdateCameras();
            whoosh.start();
        }
    }

    private void LateUpdate()
    {
        if (backupCamera.transform.position != selectedCamera.transform.position)
        {
            timescale += Time.smoothDeltaTime * multiplier;

            var moveCam = backupCamera.transform;
            var dest = selectedCamera.transform;
            
            //Just move to the location, if already moving use the stopPoint for lerping
            moveCam.position = Vector3.Lerp(moveCam.position, dest.position, timescale);
            moveCam.rotation = Quaternion.Lerp(moveCam.rotation, dest.rotation, timescale);
        }
        if(Vector3.Distance(backupCamera.transform.position, selectedCamera.transform.position) <= 5f)
        {
            ApplyNewCameras();
        }
    }


    private void UpdateCameras()
    {
        //Initialise backupCamera on startup or if it is lost.
        if (backupCamera == null)
            ResetBackupCamera();
        
        selectedCamera = cameras[cameraIndex];
        if (activeCamera != selectedCamera)
        {
            CameraLerp();
        }
    }

    private void ResetBackupCamera()
    {
        var go = Instantiate(new GameObject(), transform.position, transform.rotation);
        go.AddComponent<Camera>();
        backupCamera = go.GetComponent<Camera>();
        go.name = "Backup Camera";
        
        backupCamera.farClipPlane = 2000;
        backupCamera.enabled = false;
        var transform1 = backupCamera.transform;
        var transform2 = activeCamera.transform;
        transform1.position = transform2.position;
        transform1.rotation = transform2.rotation;
    }

    private void CameraLerp()
    {
        //Enable the backup camera, disable the active camera.
        timescale = 0;
        swappingCameras = true;
        backupCamera.enabled = true;
        activeCamera = backupCamera;
    }

    private void ApplyNewCameras()
    {
        //Enable the active camera, disable the backup camera.
        backupCamera.enabled = false;
        cameras[cameraIndex].gameObject.SetActive(true);
        activeCamera = selectedCamera;
        activeCamera.enabled = true;
        stopPoint = Vector3.zero;
        timescale = 0;
        swappingCameras = false;
    }

}
