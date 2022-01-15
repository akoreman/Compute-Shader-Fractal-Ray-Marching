using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RayMarcher : MonoBehaviour
{
    RenderTexture target;
    Camera camera;

    [SerializeField]
    ComputeShader rayMarcher;

    public bool CAST_SHADOWS = true;

    [Range(0.0f, 1.0f)]
    public float castShadowFactor;

    public bool LIT_SHADING = true;

    public bool AMBIENT_OCCLUSION = true;

    [Range(1f, 100.0f)]
    public float ambOccFudgeFactor;

    public bool GLOW = true;


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

        rayMarcher.SetFloat("castShadowFactor", castShadowFactor);
        rayMarcher.SetFloat("ambOccFudgeFactor", ambOccFudgeFactor);

        rayMarcher.SetTexture(0, "Source", source);
        rayMarcher.SetTexture(0, "Target", target);

        if (CAST_SHADOWS) { rayMarcher.EnableKeyword("CAST_SHADOWS"); } else { rayMarcher.DisableKeyword("CAST_SHADOWS"); }
        if (LIT_SHADING) { rayMarcher.EnableKeyword("LIT_SHADING"); } else { rayMarcher.DisableKeyword("LIT_SHADING"); }
        if (AMBIENT_OCCLUSION) { rayMarcher.EnableKeyword("AMBIENT_OCCLUSION"); } else { rayMarcher.DisableKeyword("AMBIENT_OCCLUSION"); }
        if (GLOW) { rayMarcher.EnableKeyword("GLOW"); } else { rayMarcher.DisableKeyword("GLOW"); }

        int threadGroupsX = (int) Mathf.Ceil(camera.pixelWidth / 8f);
        int threadGroupsY = (int) Mathf.Ceil(camera.pixelHeight / 8f);
 
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

        if (Input.GetKey("n"))
        {
            camera.transform.Rotate(new Vector3(0,0, rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKey("m"))
        {
            camera.transform.Rotate(new Vector3(0,0, -rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKeyDown("r"))
        {
            camera.transform.localPosition = new Vector3(0f,0f,0f);
            camera.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        //Vector3 currentAngles = camera.transform.localEulerAngles;
        Vector3 currentPosition = camera.transform.localPosition;

        //currentAngles.y += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        camera.transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime,0));
        //currentAngles.x -= Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        camera.transform.Rotate(new Vector3(-Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime, 0,0));

        //camera.transform.localEulerAngles = currentAngles;

        if (Input.GetKey("space"))
        {
            Vector3 Direction = camera.transform.forward;

            currentPosition += Direction * movementSpeed * Time.deltaTime;
        }

        camera.transform.localPosition = currentPosition;
    }

}


