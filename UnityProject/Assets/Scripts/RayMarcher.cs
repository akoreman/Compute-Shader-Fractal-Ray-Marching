using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalByTransforms : MonoBehaviour
{
    Transform[] TransformQueue;
    int counter;

    FractalByTransforms(int nTransforms)
    {
        this.TransformQueue = new Transform[nTransforms];
        this.counter = 0;
    }

    void AddFold()
    {
        //TransformQueue[counter] = null;
    }
}

public struct Transform
{
    string transformString;
}

//[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class RayMarcher : MonoBehaviour
{
    RenderTexture target;
    //RenderTexture target = new RenderTexture(0, 0, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
    //[SerializeField]
    Camera camera;

    [SerializeField]
    ComputeShader rayMarcher;

    void Awake()
    {
        camera = Camera.current;
    }

    
    void Init()
    {
        camera = Camera.current;
    }
    

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //RenderTexture target = new RenderTexture(10, 10, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

        Init();
        //InitRenderTexture();

        if (target == null)
        {
            target = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            //target = new RenderTexture(500, 500, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

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

        if (Input.GetKeyDown("space"))
        {
            currentPosition.x += 1f;
            currentPosition.y += 1f;
            currentPosition.z += 1f;
        }

        camera.transform.localPosition = currentPosition;
    }

    struct Shapes
    {
        public Vector3 Position;
        public Vector3 Colour;
        public int shapeType;
    }
    /*
    void InitRenderTexture()
    {
        if (target == null || target.width != camera.pixelWidth || target.height != camera.pixelHeight)
        {
            if (target != null)
            {
                target.Release();
            }
            target = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            //target = new RenderTexture(500, 500, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

            target.enableRandomWrite = true;
            target.Create();
        }
    }
    */
}


