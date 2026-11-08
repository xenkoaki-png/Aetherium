using UnityEngine;
using Aetherium.Core;

namespace Aetherium.Gameplay
{
    public class AetheriumGameplayBootstrap : MonoBehaviour
    {
        public const string AuthorFingerprint = "AETHERIUM_MECH_2026_CLEANROOM_SIG";

        [Header("Scene Setup")]
        public Vector3 FloorSize = new Vector3(32f, 1f, 32f);
        public Color FloorColor = new Color(0.12f, 0.16f, 0.20f, 1f);

        [Header("References")]
        public Transform PlayerRoot;
        public MainCameraRig CameraRig;

        private void Awake()
        {
            if (PlayerRoot != null && CameraRig != null)
            {
                return;
            }

            BootstrapScene();
        }

        [ContextMenu("Bootstrap Scene")]
        public void BootstrapScene()
        {
            if (PlayerRoot == null)
            {
                GameObject playerRoot = new GameObject("AetheriumPlayer");
                playerRoot.transform.position = new Vector3(0f, 0.5f, 0f);
                PlayerRoot = playerRoot.transform;

                var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                body.transform.SetParent(PlayerRoot, false);
                body.transform.localPosition = new Vector3(0f, 0.9f, 0f);
                body.transform.localScale = new Vector3(0.7f, 0.9f, 0.7f);
                body.name = "Body";

                var controller = PlayerRoot.gameObject.AddComponent<CharacterController>();
                controller.height = 1.8f;
                controller.radius = 0.35f;
                controller.center = new Vector3(0f, 0.9f, 0f);
                controller.enabled = true;

                var player = PlayerRoot.gameObject.AddComponent<AetheriumPlayerController>();
                var survival = PlayerRoot.gameObject.AddComponent<AetheriumSurvivalManager>();
                var inventory = PlayerRoot.gameObject.AddComponent<AetheriumInventorySystem>();
                var mission = PlayerRoot.gameObject.AddComponent<AetheriumMissionSystem>();

                GameObject runtimeObject = GameObject.Find("AetheriumWorldRuntime");
                if (runtimeObject == null)
                {
                    runtimeObject = new GameObject("AetheriumWorldRuntime");
                }

                var runtime = runtimeObject.GetComponent<AetheriumWorldRuntime>();
                if (runtime == null)
                {
                    runtime = runtimeObject.AddComponent<AetheriumWorldRuntime>();
                }

                runtime.RegisterPlayer(player);
                runtime.RegisterInventory(inventory);
                runtime.RegisterSurvival(survival);
                runtime.RegisterMission(mission);

                GameObject hudObject = GameObject.Find("AetheriumHUD");
                if (hudObject == null)
                {
                    hudObject = new GameObject("AetheriumHUD");
                }

                var hud = hudObject.GetComponent<AetheriumHUD>();
                if (hud == null)
                {
                    hud = hudObject.AddComponent<AetheriumHUD>();
                }

                hud.Survival = survival;
                hud.Inventory = inventory;
                hud.Runtime = runtime;
            }

            if (CameraRig == null)
            {
                GameObject cameraRoot = new GameObject("AetheriumCameraRig");
                cameraRoot.transform.position = new Vector3(0f, 10f, -10f);
                CameraRig = cameraRoot.AddComponent<MainCameraRig>();
            }

            CameraRig.Target = PlayerRoot;

            Camera sceneCamera = CameraRig.GetComponent<Camera>();
            if (sceneCamera == null)
            {
                sceneCamera = CameraRig.gameObject.AddComponent<Camera>();
            }

            sceneCamera.tag = "MainCamera";
            sceneCamera.enabled = true;

            if (GameObject.Find("AetheriumFloor") == null)
            {
                GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
                floor.name = "AetheriumFloor";
                floor.transform.localScale = FloorSize;
                floor.transform.position = Vector3.zero;
                var render = floor.GetComponent<Renderer>();
                if (render != null)
                {
                    render.sharedMaterial = new Material(Shader.Find("Standard"));
                    render.sharedMaterial.color = FloorColor;
                }
            }

            if (GameObject.Find("AetheriumEncounterSpawner") == null)
            {
                var spawner = new GameObject("AetheriumEncounterSpawner");
                spawner.AddComponent<AetheriumEncounterSpawner>();
            }

            if (GameObject.Find("AetheriumWaveManager") == null)
            {
                var waveManager = new GameObject("AetheriumWaveManager");
                var waveComponent = waveManager.AddComponent<AetheriumWaveManager>();
                if (AetheriumWorldRuntime.Instance != null)
                {
                    AetheriumWorldRuntime.Instance.RegisterWaveManager(waveComponent);
                }
            }

            if (GameObject.Find("AetheriumUpgradeStation") == null)
            {
                var station = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                station.name = "AetheriumUpgradeStation";
                station.transform.position = new Vector3(8f, 0.7f, 8f);
                station.transform.localScale = new Vector3(1.3f, 0.6f, 1.3f);

                var collider = station.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.isTrigger = true;
                }

                var stationComponent = station.AddComponent<AetheriumUpgradeStation>();
                stationComponent.ScrapCost = 12;
            }
        }

        [System.Serializable]
        public class MainCameraRig : MonoBehaviour
        {
            public Transform Target;
            public Vector3 Offset = new Vector3(0f, 10f, -10f);
            public float FollowSpeed = 8f;

            private void LateUpdate()
            {
                if (Target == null)
                {
                    return;
                }

                Vector3 desired = Target.position + Offset;
                transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * FollowSpeed);
                transform.LookAt(Target.position + Vector3.up * 1.25f);
            }
        }
    }
}
