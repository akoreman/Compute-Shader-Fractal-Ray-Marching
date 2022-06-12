using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ATTACH THIS SCRIPT TO THE MAIN CAMERA.

public class RayMarcher : MonoBehaviour
{
    [SerializeField]
    ComputeShader rayMarcher;

    // Compute shader compiler flags. //////
    [SerializeField]
    bool CAST_SHADOWS = true;

    [SerializeField]
    bool LIT_SHADING = true;

    [SerializeField]
    bool AMBIENT_OCCLUSION = true;

    [SerializeField]
    bool GLOW = true;
    /////////////////////////////////

    // Shadow contorl parameters.
    [SerializeField, Range(0.0f, 1.0f)]
    float castShadowFactor = 0.25f;

    [SerializeField, Range(1f, 100.0f)]
    float ambientOcclusionFactor = 80f;

    float movementSpeed;
    float rotationSpeed;

    RenderTexture target;
    Camera camera;

    void Awake()
    {
        // Default movement speeds.
        movementSpeed = 0.1f;
        rotationSpeed = 60f;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        camera = Camera.current;

        // If no render target defined, set one up.
        if (target == null)
        {
            target = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

            target.enableRandomWrite = true;
            target.Create();
        }

        // Send stuff to the computer shader.
        rayMarcher.SetMatrix("_cameraToWorldProj", camera.cameraToWorldMatrix);
        rayMarcher.SetMatrix("_cameraInverseProj", camera.projectionMatrix.inverse);
        rayMarcher.SetVector("_cameraPosition", camera.transform.position);
        rayMarcher.SetVector("_cameraRotation", camera.transform.eulerAngles);

        rayMarcher.SetFloat("_castShadowFactor", castShadowFactor);
        rayMarcher.SetFloat("_ambientOcclusionFactor", ambientOcclusionFactor);

        rayMarcher.SetTexture(0, "_source", source);
        rayMarcher.SetTexture(0, "_target", target);

        // Set keywords for the compute shader.
        if (CAST_SHADOWS) { rayMarcher.EnableKeyword("CAST_SHADOWS"); } else { rayMarcher.DisableKeyword("CAST_SHADOWS"); }
        if (LIT_SHADING) { rayMarcher.EnableKeyword("LIT_SHADING"); } else { rayMarcher.DisableKeyword("LIT_SHADING"); }
        if (AMBIENT_OCCLUSION) { rayMarcher.EnableKeyword("AMBIENT_OCCLUSION"); } else { rayMarcher.DisableKeyword("AMBIENT_OCCLUSION"); }
        if (GLOW) { rayMarcher.EnableKeyword("GLOW"); } else { rayMarcher.DisableKeyword("GLOW"); }

        int threadGroupsX = (int) Mathf.Ceil(camera.pixelWidth / 8f);
        int threadGroupsY = (int) Mathf.Ceil(camera.pixelHeight / 8f);
 
        // Run the compute shdader and render the final texture to screen.
        rayMarcher.Dispatch(0, threadGroupsX, threadGroupsY, 1);
        Graphics.Blit(target, destination);
    }

    // Handle the camera movement.
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

        Vector3 currentPosition = camera.transform.localPosition;
        Vector3 currentRotation = GetComponent<Camera>().transform.localEulerAngles;

        currentRotation = currentRotation + new Vector3(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0) * rotationSpeed * Time.deltaTime;
        camera.transform.localEulerAngles = currentRotation;

        if (Input.GetKey("space"))
        {
            Vector3 direction = camera.transform.forward;
            currentPosition += direction * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 direction = camera.transform.forward;
            currentPosition -= direction * movementSpeed * Time.deltaTime;
        }

        camera.transform.localPosition = currentPosition;

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }


       
    }

}


