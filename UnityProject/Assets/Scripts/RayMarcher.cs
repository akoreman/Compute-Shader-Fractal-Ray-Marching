using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RayMarcher : MonoBehaviour
{
    RenderTexture target;
    Camera camera;

    [SerializeField]
    ComputeShader rayMarcher;

    float movementSpeed;
    float rotationSpeed;

    void Awake()
    {
        movementSpeed = 0.1f;
        rotationSpeed = 60f;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        camera = Camera.current;

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

        rayMarcher.SetTexture(0, "Source", source);
        rayMarcher.SetTexture(0, "Target", target);

        rayMarcher.EnableKeyword("CAST_SHADOWS");

        int threadGroupsX = (int) Mathf.Ceil(camera.pixelWidth / 8.0f);
        int threadGroupsY = (int) Mathf.Ceil(camera.pixelHeight / 8.0f);

      
        rayMarcher.Dispatch(0, threadGroupsX, threadGroupsY, 1);
  
        
        Graphics.Blit(target, destination);
    }

    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            movementSpeed /= Mathf.Sqrt(10f);
            print(movementSpeed);
        }

        if (Input.GetKeyDown("c"))
        {
            movementSpeed *= Mathf.Sqrt(10f);
            print(movementSpeed);
        }

        if (Input.GetKeyDown("v"))
        {
            rotationSpeed -= 5f;
            rotationSpeed = Mathf.Max(rotationSpeed, 0f);
            print(rotationSpeed);
        }

        if (Input.GetKeyDown("b"))
        {
            rotationSpeed += 5f;
            print(rotationSpeed);
        }

        if (Input.GetKeyDown("r"))
        {
            camera.transform.localPosition = new Vector3(0f,0f,0f);
            camera.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        Vector3 currentAngles = camera.transform.localEulerAngles;
        Vector3 currentPosition = camera.transform.localPosition;

        currentAngles.y += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        currentAngles.x -= Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        camera.transform.localEulerAngles = currentAngles;

        if (Input.GetKey("space"))
        {
            Vector3 Direction = camera.transform.forward;

            currentPosition += Direction * movementSpeed * Time.deltaTime;
        }

        camera.transform.localPosition = currentPosition;
    }

}


