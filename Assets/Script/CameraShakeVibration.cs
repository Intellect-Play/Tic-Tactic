using System.Collections;
using System.Collections.Generic;
using CartoonFX;
using UnityEngine;

public class CameraShakeVibration : MonoBehaviour
{
    public static CameraShakeVibration Instance;
    private Vector3 originalPos;
    public ShakeEffect shakeEffect; // Reference to the ShakeEffect component
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShakeButton() {
        VibrateCustom(50); // Vibrate for 100 milliseconds
    }
    public void Vibration() {
        VibrateCustom(300 ); // Vibrate for 100 milliseconds

    }
    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public void Shake(float duration = 0.3f, float magnitude = 0.4f)
    {
        shakeEffect.Shake();
        //VibrateCustom(100); // Vibrate for 100 milliseconds

        //StopAllCoroutines();
        //shakeEffect.Shake();
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void VibrateCustom(long milliseconds = 100)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    {
        var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");

        if (vibrator != null)
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                int sdkInt = version.GetStatic<int>("SDK_INT");

                if (sdkInt >= 26)
                {
                    var vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                    var effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, 50);
                    vibrator.Call("vibrate", effect);
                }
                else
                {
                    vibrator.Call("vibrate", milliseconds);
                }
            }
        }
    }
#endif
    }
}


