using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerController : MonoBehaviour
{
    private CanvasScaler canvasScaler;
    
    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        SetupCanvasScaler();
    }

    private void SetupCanvasScaler()
    {
        // Set to scale with screen size
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        canvasScaler.referenceResolution = new Vector2(960, 540);
        
        // 0.5 means it will blend between height and width scaling
        // 0 = width based scaling
        // 1 = height based scaling
        canvasScaler.matchWidthOrHeight = 0.5f;
        
        // Set reference pixels per unit
        canvasScaler.referencePixelsPerUnit = 100f;
        
        // Set screen match mode
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    }
}