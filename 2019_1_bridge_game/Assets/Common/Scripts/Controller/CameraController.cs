using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviourSingleton<CameraController>
{
    #region variables
    public enum IncreaseType { NORMAL, INCREASE, DECREASE }

    private Transform cameraTransform;
    private Vector2 velocity = Vector2.zero;

    [SerializeField]
    private float cameraDepth = -1;
    private Vector3 zeroPos;
    private bool isShaking;
    private new Camera camera;
    #endregion

    #region get / set
    public Vector3 GetWorldToScreen(Vector3 worldPos)
    {
        return camera.WorldToScreenPoint(worldPos);
    }
    #endregion

    #region unityFunc
    private void Awake()
    {
        cameraTransform = this.transform;
        zeroPos = new Vector3(0, 0, cameraDepth);
        camera = GetComponent<Camera>();
    }
    #endregion

    #region func
    private void SlideOther(Vector2 _targetPos)
    {
        Vector2 temp = Vector2.SmoothDamp(cameraTransform.position, _targetPos, ref velocity, 5, 0.5f, .45f);
        cameraTransform.position = new Vector3(temp.x, temp.y, cameraDepth);
    }

    public void ComeBackPosition()
    {
        cameraTransform.localPosition = zeroPos;
    }
    public void AttachObject(Transform targetTransform)
    {
        cameraTransform.parent = targetTransform;
        cameraTransform.localPosition = zeroPos;
    }
    public void AttachObject(Transform targetTransform, Vector2 localPosition)
    {
        cameraTransform.parent = targetTransform;
        cameraTransform.localPosition = new Vector3(localPosition.x, localPosition.y, cameraDepth);
    }
    public void FindOther(Vector2 dest)
    {
        cameraTransform.position = new Vector3(dest.x, dest.y, cameraDepth);
    }
    public void Shake(float amount, float time, IncreaseType increaseType = IncreaseType.NORMAL)
    {
        if (isShaking)
            return;
        isShaking = true;
        StartCoroutine(CoroutineShaking(time, amount, increaseType));
    }
    #endregion

    #region coroutine
    IEnumerator CoroutineShaking(float duration, float magnitude, IncreaseType increaseType)
    {
        float elapsed = 0.0f;
        float value = 1;

        while (elapsed < duration)
        {
            if(increaseType == IncreaseType.INCREASE)
            {
                value = elapsed / duration;
            }
            else if(increaseType == IncreaseType.DECREASE)
            {
                value = (duration - elapsed) / duration;
            }
            float x = Random.Range(-1.0f, 1.0f) * magnitude * value;
            float y = Random.Range(-1.0f, 1.0f) * magnitude * value;

            cameraTransform.localPosition = new Vector3(x, y, cameraDepth);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = zeroPos;
        isShaking = false;
    }
    #endregion

    
}
