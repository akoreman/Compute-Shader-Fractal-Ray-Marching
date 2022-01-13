using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RayMarcher : MonoBehaviour
{
    RenderTexture target;
    Camera camera;

    [SerializeField]
    ComputeShader rayMarcher;


    
    void Init()
    {
        camera = Camera.current;
    }
    

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        Init();

        if (target == null)
        {
            target = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

            target.enableRandomWrite = true;
            target.Create();
        }

        rayMarcher.SetMatrix("_CameraToWorldProj", camera.cameraToWorldMatrix);
        rayMarcher.SetMatrix("_CameraInverseProj", camera.projectionMatrix.inverse);
        rayMarcher.SetVector("_CameraPosition", camera.transform.position);
        rayMarcher.SetVector("_CameraRotation", camera.transform.eulerAngles);

        //target = source;

        rayMarcher.SetTexture(0, "Source", source);
        rayMarcher.SetTexture(0, "Target", target);

        int threadGroupsX = (int) Mathf.Ceil(camera.pixelWidth / 8.0f);
        int threadGroupsY = (int) Mathf.Ceil(camera.pixelHeight / 8.0f);

       

        rayMarcher.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        
        
        Graphics.Blit(target, destination);
    }

    void Update()
    {
        Vector3 currentAngles = camera.transform.eulerAngles;
        Vector3 currentPosition = camera.transform.localPosition;

        currentAngles.y += Input.GetAxis("Horizontal") * 20.0f * Time.deltaTime;
        currentAngles.x -= Input.GetAxis("Vertical") * 20.0f * Time.deltaTime;

        camera.transform.eulerAngles = currentAngles;

        if (Input.GetKey("space"))
        {
            Vector3 Direction = camera.transform.forward;

            currentPosition += Direction * 0.1f * Time.deltaTime;
        }

        camera.transform.localPosition = currentPosition;
    }


}


