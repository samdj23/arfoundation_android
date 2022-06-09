using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DepthViz : MonoBehaviour
{
    [SerializeField] private ARCameraManager _cameraManager;
    [SerializeField] private AROcclusionManager _occlusionManager;
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private Text _debug;

    public ARCameraManager CameraManager
    {
        get => _cameraManager;
        set => _cameraManager = value;
    }

    public AROcclusionManager occlusionManager
    {
        get => _occlusionManager;
        set => _occlusionManager = value;
    }


    public RawImage rawImage
    {
        get => _rawImage;
        set => _rawImage = value;
    }

    public Text debug
    {
        get => _debug;
        set => _debug = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        _debug.text = "true";
    }

    // Update is called once per frame
    void Update()
    {
        

        //_occlusion a la place de occlusion
        if (_occlusionManager.TryAcquireEnvironmentDepthCpuImage(out XRCpuImage image))
        {
            using(image)
            {
                UpdateRawImage(_rawImage, image);
            }
        }
    }

    
    unsafe void UpdateRawImage(RawImage rawImage, XRCpuImage cpuImage)
    {
        _debug.text = (cpuImage.FormatSupported(TextureFormat.RFloat)).ToString();

        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, cpuImage.width, cpuImage.height),

            outputDimensions = new Vector2Int(cpuImage.width , cpuImage.height),

            outputFormat = TextureFormat.RFloat,

            transformation = XRCpuImage.Transformation.MirrorY
        };

        int size = cpuImage.GetConvertedDataSize(conversionParams);

        //var rawTextureData = m_texture.GetRawTextureData<byte>();
        // allocation d'un buffer
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        
        //cpuImage.Convert(conversionParams, rawTextureData);
        cpuImage.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);

        

 
        var m_Texture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);
        rawImage.texture = m_Texture;


        m_Texture.LoadRawTextureData(buffer);
        m_Texture.Apply();

        cpuImage.Dispose();
        buffer.Dispose();
    }



//rawImage.enabled = true;
        //_debug.text = (cpuImage.FormatSupported(TextureFormat.RFloat)).ToString();
    //}
    
    /*
    private static void UpdateRawImage(RawImage rawImage, XRCpuImage cpuImage)
    {

        Texture2D texture = (Texture2D)rawImage.texture;


        if (texture == null || texture.width != cpuImage.width || texture.height != cpuImage.height)
        {
            texture = new Texture2D(cpuImage.width, cpuImage.height, cpuImage.format.AsTextureFormat(), false);
            rawImage.texture = texture;
        }

        var conversionParams = new XRCpuImage.ConversionParams(cpuImage, cpuImage.format.AsTextureFormat(), XRCpuImage.Transformation.MirrorY);


        var rawTextureData = texture.GetRawTextureData<byte>();

        cpuImage.Convert(conversionParams, rawTextureData);

        texture.LoadRawTextureData(rawTextureData);

        texture.Apply();
    }
    */
}
    


