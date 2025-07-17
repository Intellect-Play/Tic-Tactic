//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2025 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------


//--------------------------------------------------------------------------------------------------------------------------------

// Use the defines below to globally disable features:

//#define DISABLE_CAMERA_SHAKE
//#define DISABLE_LIGHTS
//#define DISABLE_LIGHTS_LINEAR_REMAPPING
//#define DISABLE_CLEAR_BEHAVIOR

//--------------------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Rendering;
using static CartoonFX.CFXR_Effect;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CartoonFX
{
    [DisallowMultipleComponent]
    public partial class ShakeEffect : MonoBehaviour
    {
        // Change this value to easily tune the camera shake strength for all effects
        const float GLOBAL_CAMERA_SHAKE_MULTIPLIER = 1.0f;

      

        public enum ClearBehavior
        {
            None,
            Disable,
            Destroy
        }


        //public static bool GlobalDisableCameraShake;
        //public static bool GlobalDisableLights;

        // ================================================================================================================================

        //  [Tooltip("Defines an action to execute when the Particle System has completely finished playing and emitting particles.")]
        //public ClearBehavior clearBehavior = ClearBehavior.Destroy;
        [Space]
        public CameraShake cameraShake;
        [Space]
        [Tooltip("Defines which Particle System to track to trigger light fading out.\nLeave empty if not using fading out.")]


        float time;

        private void OnEnable()
        {
            time = 3;
        }
        // ================================================================================================================================

        public void ResetState()
        {
            time = 0f;
            fadingOutStartTime = 0f;
            isFadingOut = false;

#if !DISABLE_LIGHTS

#endif

#if !DISABLE_CAMERA_SHAKE
            if (cameraShake != null && cameraShake.enabled)
            {
                cameraShake.StopShake();
            }
#endif
        }


        public void Shake()
        {
            time = 0;

        }

        void OnDisable()
        {
            ResetState();
        }

#if !DISABLE_LIGHTS || !DISABLE_CAMERA_SHAKE || !DISABLE_CLEAR_BEHAVIOR
        const int CHECK_EVERY_N_FRAME = 20;
        static int GlobalStartFrameOffset = 0;
        int startFrameOffset;
        void Update()
        {
#if !DISABLE_LIGHTS || !DISABLE_CAMERA_SHAKE
            time += Time.deltaTime;

            Animate(time);


#endif
#if !DISABLE_CLEAR_BEHAVIOR

#endif

        }
#endif

#if !DISABLE_LIGHTS || !DISABLE_CAMERA_SHAKE
        public void Animate(float time)
        {

#if !DISABLE_CAMERA_SHAKE
            if (cameraShake != null && cameraShake.enabled && !GlobalDisableCameraShake)
            {
                if (!cameraShake.isShaking)
                {
                    //Debug.Log(time + " - " + this.name + " - CameraShake is not shaking, fetching cameras again.");
                    cameraShake.fetchCameras();
                }
#endif
                cameraShake.animate(time);
            }
#endif
        }

#if !DISABLE_LIGHTS
        bool isFadingOut;
        float fadingOutStartTime;
        //public void FadeOut(float time)
        //{


        //    if (!isFadingOut)
        //    {
        //        isFadingOut = true;
        //        fadingOutStartTime = time;
        //    }


        //}
#endif

    }
};