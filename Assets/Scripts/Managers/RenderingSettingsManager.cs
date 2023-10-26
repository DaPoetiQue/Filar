using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Com.RedicalGames.Filar
{
    public class RenderingSettingsManager : AppData.SingletonBaseComponent<RenderingSettingsManager>
    {
        #region Components

        [SerializeField]
        Camera sceneRenderCamera;

        [Space(5)]
        [SerializeField]
        AppData.RenderingSettingsData renderingSettings = new AppData.RenderingSettingsData();

        [Space(5)]
        [SerializeField]
        List<AppData.RendererMaterial> rendererMaterialList = new List<AppData.RendererMaterial>();

        [Space(5)]
        [SerializeField]
        List<DynamicWidgetsContainer> dynamicWidgetsContainersList = new List<DynamicWidgetsContainer>();

        [Space(5)]
        [SerializeField]
        string skyboxWidgetPrefabDirectory = "UI Prefabs/Skybox";

        [Space(5)]
        [SerializeField]
        List<AppData.MaterialShader> materialShadersList = new List<AppData.MaterialShader>();

        Material tempMaterial;

        const string mainTextureID = "_MainTex", normalMapTextureID = "_BumpMap", aoMapTextureID = "_OcclusionMap";

        List<MeshRenderer> currentSceneAssetMeshRendererList = new List<MeshRenderer>();
        Dictionary<MeshRenderer, Material> currentSceneAssetMaterialList = new Dictionary<MeshRenderer, Material>();
        List<Material> tempSceneAssetMaterialList = new List<Material>();

        AppData.SceneAssetRenderMode currentRenderMode;

        AppDatabaseManager assetsManager;
        SkyboxUIHandler skyboxUIHandlerPrefab = null;

        List<SkyboxUIHandler> skyboxUIHandlerComponentsList = new List<SkyboxUIHandler>();

        bool assetAssigned;

        public bool UpdateScreenWidgetInfo { get; set; }

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        //void Update() => RotateSkybox();

        #endregion

        #region Main

        void Init()
        {
            assetsManager = AppDatabaseManager.Instance;

            if (rendererMaterialList.Count > 0)
            {
                foreach (var rendererMaterial in rendererMaterialList)
                {
                    if (rendererMaterial.value != null)
                    {
                        rendererMaterial.Init();
                    }
                    else
                        Debug.Log($"--> Renderer Material : {rendererMaterial.name} Value Missing / Null.");
                }
            }

            // Load Skybox Handler From Resources Folder
            if (skyboxUIHandlerPrefab == null)
                skyboxUIHandlerPrefab = Resources.Load<SkyboxUIHandler>(skyboxWidgetPrefabDirectory);
            // CreateUIWidgets();
        }

        public void CreateUIWidgets()
        {
            if (skyboxUIHandlerPrefab)
            {
                if (renderingSettings.skyboxDataList.Count > 0)
                {
                    foreach (var skyboxData in renderingSettings.skyboxDataList)
                    {
                        CreateSkyboxWidget(skyboxData, AppData.ContentContainerType.SkyboxContent, AppData.OrientationType.Horizontal, (created) =>
                        {
                            if (!AppData.Helpers.IsSuccessCode(created.resultCode))
                                LogWarning(created.result, this, () => CreateUIWidgets());
                        });
                    }
                }
            }
        }


        public void SetCurrentRenderedSceneAsset(GameObject renderedSceneAsset, bool hasMTLFile, AppData.MaterialProperties materialProperties = null)
        {
            MeshRenderer meshRenderer = renderedSceneAsset.GetComponent<MeshRenderer>();

            if (meshRenderer)
                currentSceneAssetMeshRendererList.Add(meshRenderer);
            else
            {
                currentSceneAssetMeshRendererList = renderedSceneAsset.GetComponentsInChildren<MeshRenderer>().ToList();
            }

            if (currentSceneAssetMeshRendererList.Count > 0)
            {
                if (hasMTLFile)
                {
                    foreach (var renderer in currentSceneAssetMeshRendererList)
                    {
                        if (renderer.sharedMaterials.Length > 0)
                        {
                            foreach (var material in renderer.sharedMaterials)
                            {
                                if (material != null)
                                    currentSceneAssetMaterialList.Add(renderer, material);
                                else
                                    LogWarning("Material Is Null.", this, () => SetCurrentRenderedSceneAsset(renderedSceneAsset, hasMTLFile, materialProperties = null));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var renderer in currentSceneAssetMeshRendererList)
                    {
                        if (renderer.sharedMaterials.Length > 0)
                        {
                            foreach (var material in renderer.sharedMaterials)
                            {
                                if (material != null)
                                    currentSceneAssetMaterialList.Add(renderer, material);
                                else
                                    LogWarning("Material Is Null.", this, () => SetCurrentRenderedSceneAsset(renderedSceneAsset, hasMTLFile, materialProperties = null));
                            }
                        }
                    }
                }

                if (currentSceneAssetMaterialList.Count > 0)
                {
                    if (materialProperties != null)
                    {
                        if (!string.IsNullOrEmpty(materialProperties.mainTexturePath))
                            SetTexture(GetMaterialTextureID(AppData.MaterialTextureType.MainTexture), materialProperties.mainTexturePath);

                        if (!string.IsNullOrEmpty(materialProperties.normalMapTexturePath))
                            SetTexture(GetMaterialTextureID(AppData.MaterialTextureType.NormalMapTexture), materialProperties.normalMapTexturePath);

                        if (!string.IsNullOrEmpty(materialProperties.aoMapTexturePath))
                            SetTexture(GetMaterialTextureID(AppData.MaterialTextureType.AOMapTexture), materialProperties.aoMapTexturePath);
                    }

                    assetAssigned = true;
                }
            }
            else
                LogWarning($"There Are No Renderer Components Found In Game Object : {renderedSceneAsset.name}", this, () => SetCurrentRenderedSceneAsset(renderedSceneAsset, hasMTLFile, materialProperties = null));
        }

        public void SetTexture(string materialPropertyName, string path, bool resetColor = true)
        {

            if (!string.IsNullOrEmpty(path))
            {
                Texture2D texture = AppData.Helpers.LoadTextureFile(path);

                if (texture != null)
                {
                    if (SelectableManager.Instance.GetSelectionList().Count > 0)
                    {

                        if (currentSceneAssetMaterialList.Count > 0)
                            foreach (var material in currentSceneAssetMaterialList)
                            {
                                foreach (var item in SelectableManager.Instance.GetSelectionList())
                                {
                                    if (item.GetSelectableAssetRenderer() == material.Key)
                                    {
                                        if (resetColor)
                                            material.Value.SetColor("_Color", Color.white);

                                        material.Value.SetTexture(materialPropertyName, texture);

                                        item.GetSelectableAssetRenderer().sharedMaterial = material.Value;
                                    }
                                }
                            }
                        else
                            Debug.LogWarning("--> No Materials Found To Assign Texture.");
                    }
                    else
                        Debug.Log("Selected Material Null.");
                }
                else
                    Debug.LogWarning("--> Texture Is Null / Invalid.");
            }
            else
                Debug.LogWarning($"-->path Texture Path : {path} Null / Invalid.");
        }

        public void SetTexture(string materialPropertyName, Texture2D texture, int selectedMeshIndex)
        {
            if (currentSceneAssetMaterialList.Count > 0)
                foreach (var material in currentSceneAssetMaterialList)
                    material.Value.SetTexture(materialPropertyName, texture);
            else
                Debug.LogWarning("--> No Materials Found To Assign Main Texture.");
        }

        public Material GetSelectedMeshMaterial(int meshIndex)
        {
            if (currentSceneAssetMaterialList.Count > 0 && meshIndex <= currentSceneAssetMaterialList.Count - 1)
                return null;  //currentSceneAssetMaterialList[meshIndex];
            else
                Debug.LogWarning($"--> No Material Found For Mesh At Index : {meshIndex}");

            return null;
        }


        public void SetMaterialValue(AppData.AssetFieldSettingsType assetFieldType, float value)
        {
            Debug.Log($"--> Setting Material Value : {value} For : {assetFieldType.ToString()}");

            if (SelectableManager.Instance)
            {
                if (SelectableManager.Instance.GetSelectionList().Count > 0)
                {
                    foreach (var item in SelectableManager.Instance.GetSelectionList())
                    {
                        switch (assetFieldType)
                        {
                            case AppData.AssetFieldSettingsType.MainTextureSettings:

                                item.GetSelectableAssetRenderer().sharedMaterial.SetFloat("_Glossiness", value);

                                if (assetsManager)
                                {
                                    AppData.MaterialProperties materialProperties = assetsManager.GetCurrentSceneAsset().GetMaterialProperties();

                                    materialProperties.glossiness = value;

                                    assetsManager.GetCurrentSceneAsset().SetMaterialProperties(materialProperties);
                                }
                                else
                                    Debug.LogWarning("--> Scene Asset Manager Not Yet Initialized.");

                                break;


                            case AppData.AssetFieldSettingsType.NormalMapSettings:

                                item.GetSelectableAssetRenderer().sharedMaterial.SetFloat("_BumpScale", value);

                                if (assetsManager)
                                {
                                    AppData.MaterialProperties materialProperties = assetsManager.GetCurrentSceneAsset().GetMaterialProperties();

                                    materialProperties.bumpScale = value;

                                    assetsManager.GetCurrentSceneAsset().SetMaterialProperties(materialProperties);
                                }
                                else
                                    Debug.LogWarning("--> Scene Asset Manager Not Yet Initialized.");

                                break;

                            case AppData.AssetFieldSettingsType.AOMapSettings:

                                item.GetSelectableAssetRenderer().sharedMaterial.SetFloat("_OcclusionStrength", value);

                                if (assetsManager)
                                {
                                    AppData.MaterialProperties materialProperties = assetsManager.GetCurrentSceneAsset().GetMaterialProperties();

                                    materialProperties.aoStrength = value;

                                    assetsManager.GetCurrentSceneAsset().SetMaterialProperties(materialProperties);
                                }
                                else
                                    Debug.LogWarning("--> Scene Asset Manager Not Yet Initialized.");

                                break;
                        }
                    }
                }
                else
                    Debug.LogWarning("--> No Selected Scene Assets Found.");
            }
            else
                Debug.LogWarning("--> Selectable Manager Not Yet Initialized.");

            //if (currentSceneAssetMaterialList.Count > 0)
            //{
            //    foreach (var material in currentSceneAssetMaterialList)
            //    {
            //        if (material.Value != null)
            //        {

            //            foreach (var item in SelectableManager.Instance.GetSelectionList())
            //            {
            //                if (item.GetSelectableAssetRenderer() == material.Key)
            //                {
            //                    switch (assetFieldType)
            //                    {
            //                        case AppData.AssetFieldSettingsType.MainTextureSettings:

            //                            material.Value.SetFloat("_Glossiness", value);

            //                            if (assetsManager)
            //                            {
            //                                AppData.MaterialProperties materialProperties = assetsManager.GetCurrentSceneAsset().GetMaterialProperties();

            //                                materialProperties.glossiness = value;

            //                                assetsManager.GetCurrentSceneAsset().SetMaterialProperties(materialProperties);
            //                            }
            //                            else
            //                                Debug.LogWarning("--> Scene Asset Manager Not Yet Initialized.");

            //                            break;


            //                        case AppData.AssetFieldSettingsType.NormalMapSettings:

            //                            material.Value.SetFloat("_BumpScale", value);

            //                            if (assetsManager)
            //                            {
            //                                AppData.MaterialProperties materialProperties = assetsManager.GetCurrentSceneAsset().GetMaterialProperties();

            //                                materialProperties.bumpScale = value;

            //                                assetsManager.GetCurrentSceneAsset().SetMaterialProperties(materialProperties);
            //                            }
            //                            else
            //                                Debug.LogWarning("--> Scene Asset Manager Not Yet Initialized.");

            //                            break;

            //                        case AppData.AssetFieldSettingsType.AOMapSettings:

            //                            material.Value.SetFloat("_OcclusionStrength", value);

            //                            if (assetsManager)
            //                            {
            //                                AppData.MaterialProperties materialProperties = assetsManager.GetCurrentSceneAsset().GetMaterialProperties();

            //                                materialProperties.aoStrength = value;

            //                                assetsManager.GetCurrentSceneAsset().SetMaterialProperties(materialProperties);
            //                            }
            //                            else
            //                                Debug.LogWarning("--> Scene Asset Manager Not Yet Initialized.");

            //                            break;
            //                    }

            //                    item.GetSelectableAssetRenderer().sharedMaterial = material.Value;
            //                }
            //            }
            //        }
            //        else
            //            Debug.LogWarning("--> Material Invalid / Null.");
            //        //material.Value.SetFloat("_Glossiness", value);
            //    }
            //}
        }

        public List<MeshRenderer> GetCurrentRenderedSceneAsset()
        {
            return currentSceneAssetMeshRendererList;
        }

        public float GetGlobalMaterialIntensity(AppData.SceneConfigDataPacket dataPackets)
        {
            var renderers = currentSceneAssetMeshRendererList;

            float value = 0.0f;

            switch (dataPackets.assetFieldConfiguration)
            {
                case AppData.AssetFieldSettingsType.MainTextureSettings:

                    if (renderers.Count > 0)
                    {
                        foreach (var renderer in renderers)
                        {
                            value = renderer.sharedMaterial.GetFloat("_Glossiness");
                        }

                        Debug.Log($"--> Glossiness : {value}");
                    }

                    break;

                case AppData.AssetFieldSettingsType.NormalMapSettings:

                    if (renderers.Count > 0)
                    {
                        foreach (var renderer in renderers)
                        {
                            value = renderer.sharedMaterial.GetFloat("_BumpScale");
                        }
                    }

                    Debug.Log($"--> Normals : {value}");

                    break;

                case AppData.AssetFieldSettingsType.AOMapSettings:

                    if (renderers.Count > 0)
                    {
                        foreach (var renderer in renderers)
                        {
                            value = renderer.sharedMaterial.GetFloat("_OcclusionStrength");
                        }

                        Debug.Log($"--> AO : {value}");
                    }

                    break;
            }

            Debug.Log($"--> Getting Global Material Intensity For : {dataPackets.assetFieldConfiguration.ToString()} - With Value : {value}");

            return value;
        }

        public void SetRenderCamera(Camera renderCamera)
        {
            sceneRenderCamera = renderCamera;
        }

        public Camera GetRenderCamera()
        {
            return sceneRenderCamera;
        }

        public bool IsAssetRendererReady()
        {
            return assetAssigned;
        }

        public string GetMaterialTextureID(AppData.MaterialTextureType materialTextureType)
        {
            string textureID = string.Empty;

            switch (materialTextureType)
            {
                case AppData.MaterialTextureType.MainTexture:

                    textureID = mainTextureID;

                    break;

                case AppData.MaterialTextureType.NormalMapTexture:

                    textureID = normalMapTextureID;

                    break;


                case AppData.MaterialTextureType.AOMapTexture:

                    textureID = aoMapTextureID;

                    break;
            }

            return textureID;
        }

        public void OnRenderMode(AppData.SceneAssetRenderMode renderMode)
        {
            if (IsAssetRendererReady())
            {

                currentRenderMode = renderMode;

                switch (renderMode)
                {
                    case AppData.SceneAssetRenderMode.Shaded:

                        AppData.ActionEvents.OnActionCheckboxStateEvent(false, false);

                        SetShadedMaterial();

                        break;

                    case AppData.SceneAssetRenderMode.Wireframe:

                        //tempSceneAssetMaterialList = currentSceneAssetMaterialList;

                        AppData.ActionEvents.OnActionCheckboxStateEvent(true, true);

                        SetWireframeMaterial(GetMaterialFromList(AppData.RendererMaterialType.WireframeMaterial));

                        break;
                }
            }
            else
                Debug.LogWarning("--> Asset Renderer Not Ready.");
        }

        void SetShadedMaterial()
        {
            if (tempSceneAssetMaterialList.Count > 0)
            {

                for (int i = 0; i < tempSceneAssetMaterialList.Count; i++)
                {
                    currentSceneAssetMeshRendererList[i].sharedMaterial = tempSceneAssetMaterialList[0];
                }
            }
            else
                Debug.LogWarning("--> Temp Scene Asset Material List Null.");
        }

        void SetWireframeMaterial(Material material)
        {
            if (currentSceneAssetMeshRendererList.Count > 0)
            {
                foreach (var render in currentSceneAssetMeshRendererList)
                {
                    render.sharedMaterial = material;
                }
            }
            else
                Debug.LogWarning("--> Current Scene Asset Renderer List Null.");

        }

        public void TriangulateWireframe(bool triangulate)
        {

            if (currentRenderMode != AppData.SceneAssetRenderMode.Wireframe)
                return;

            if (triangulate)
                SetWireframeMaterial(GetMaterialFromList(AppData.RendererMaterialType.TriangulatedWireframeMaterial));
            else
                SetWireframeMaterial(GetMaterialFromList(AppData.RendererMaterialType.WireframeMaterial));
        }


        public void SetSelectedAssetMaterial(SelectableSceneAssetHandler selectableAsset)
        {
            if (GetMaterialFromList(AppData.RendererMaterialType.SelectionMaterial))
            {
                if (selectableAsset != null)
                {
                    MeshRenderer meshRenderer = selectableAsset.GetSelectableAssetRenderer();

                    if (meshRenderer != null)
                    {
                        if (tempMaterial == null)
                            tempMaterial = meshRenderer.sharedMaterial;

                        if (GetMaterialFromList(AppData.RendererMaterialType.SelectionMaterial))
                            UpdateToMatchMaterial(tempMaterial, AppData.RendererMaterialType.SelectionMaterial);
                        else
                            Debug.LogWarning("--> Selection Material Missing / Not Assigned.");

                        if (assetsManager != null)
                        {
                            if (assetsManager.GetSceneAssetRenderMode() == AppData.SceneAssetRenderMode.Shaded)
                            {
                                if (GetMaterialFromList(AppData.RendererMaterialType.SelectionMaterial) != null)
                                    meshRenderer.sharedMaterial = GetMaterialFromList(AppData.RendererMaterialType.SelectionMaterial);
                                else
                                    Debug.LogWarning("--> Material For Selection Missing.");
                            }

                            if (assetsManager.GetSceneAssetRenderMode() == AppData.SceneAssetRenderMode.Wireframe)
                            {
                                if (GetMaterialFromList(AppData.RendererMaterialType.SelectionWireframeMaterial) != null)
                                    meshRenderer.sharedMaterial = GetMaterialFromList(AppData.RendererMaterialType.SelectionWireframeMaterial);
                                else
                                    Debug.LogWarning("--> Material For Selection Wireframe Missing.");
                            }
                        }
                        else
                            Debug.LogWarning("--> Scene Asset Manager Not Yet Initialized.");

                        selectableAsset.SetIsSelectedState(true);
                    }
                    else
                        Debug.LogWarning("--> Selectable Asset Doesn't Have A Mesh Renderer Assigned.");

                }
                else
                    Debug.LogWarning("--> Selectable Asset Is Null.");

            }
            else
                Debug.Log("--> Selected Asset Shader Material Is Missing / Null.");
        }

        public void RevertSelectedAssetMaterial(SelectableSceneAssetHandler selectableAsset)
        {
            if (tempMaterial)
            {
                if (selectableAsset != null)
                {
                    MeshRenderer meshRenderer = selectableAsset.GetSelectableAssetRenderer();

                    if (meshRenderer != null)
                    {

                        meshRenderer.sharedMaterial = tempMaterial;

                        if (tempMaterial != null)
                            tempMaterial = null;


                        selectableAsset.SetIsSelectedState(false);
                    }
                    else
                        Debug.LogWarning("--> Selectable Asset Doesn't Have A Mesh Renderer Assigned.");

                }
                else
                    Debug.LogWarning("--> Selectable Asset Is Null.");

            }
            else
                Debug.Log("--> Temp Asset Shader Material Is Missing / Null.");
        }


        public Material GetMaterialFromList(AppData.RendererMaterialType materialType)
        {
            Material material = null;

            if (rendererMaterialList.Count > 0)
            {
                foreach (var rendererMaterial in rendererMaterialList)
                {
                    if (rendererMaterial.materialType == materialType)
                    {
                        if (rendererMaterial.value != null)
                        {
                            material = rendererMaterial.value;

                            break;
                        }
                        else
                            Debug.LogWarning($"--> Material Value For : {rendererMaterial.name} Missing / Null.");
                    }
                    else
                        continue;
                }
            }

            return material;
        }

        public void UpdateToMatchMaterial(Material materialToMatch, AppData.RendererMaterialType materialType)
        {
            if (rendererMaterialList.Count > 0)
            {
                foreach (var rendererMaterial in rendererMaterialList)
                {
                    if (rendererMaterial.materialType == materialType)
                    {
                        if (rendererMaterial.value != null)
                        {
                            rendererMaterial.UpdateToMatchMaterial(materialToMatch);

                            break;
                        }
                        else
                            Debug.LogWarning($"--> Material Value For : {rendererMaterial.name} Missing / Null.");
                    }
                    else
                        continue;
                }
            }
        }

        public void GetMaterialShader(AppData.ShaderType shaderType, Action<AppData.CallbackData<AppData.MaterialShader>> callback)
        {
            AppData.CallbackData<AppData.MaterialShader> callbackResults = new AppData.CallbackData<AppData.MaterialShader>();

            if (materialShadersList.Count > 0)
            {
                AppData.MaterialShader shader = materialShadersList.Find((shader) => shader.shaderType == shaderType);

                if (shader.value != null)
                {
                    callbackResults.result = "Success : Material Shader Loaded.";
                    callbackResults.data = shader;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = "Failed : Material Shader Value Is Missing  / Null.";
                    callbackResults.data = default;
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.result = "Failed : Material Shader List Is Empty / Null.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.SceneAssetRenderMode GetCurrentSceneAssetRenderMode()
        {
            return currentRenderMode;
        }

        public AppData.RenderingSettingsData GetRenderingSettingsData()
        {
            return renderingSettings;
        }

        public void CreateSkyboxSettings(AppData.SkyboxSettings settings, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            CreateSkyboxWidget(settings, AppData.ContentContainerType.SkyboxContent, AppData.OrientationType.Horizontal, (created) =>
            {
                callbackResults = created;
            });

            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            callback?.Invoke(callbackResults);
        }

        public void ApplySkyboxSettings(AppData.LightingSettingsData settings, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (settings.hdrTexture != null)
                renderingSettings.CurrentSkyboxSettings.SetSkyBoxTexture(settings.hdrTexture);
            else
                Debug.LogWarning("--> ApplySkyboxSettings Failed : GetSkyboxHDRTexture Returns Null.");

            renderingSettings.CurrentSkyboxSettings.SetLightIntensity(settings.lightingData.lightIntensity);
            renderingSettings.CurrentSkyboxSettings.SetLightColor(settings.lightingData.lightColor.color);
            renderingSettings.CurrentSkyboxSettings.SetSkyBoxExposure(settings.lightingData.skyboxExposure);

            //renderingSettings.CurrentSkyboxSettings = settings;

            callbackResults.resultCode = AppData.Helpers.SuccessCode;

            callback?.Invoke(callbackResults);
        }

        void CreateSkyboxWidget(AppData.SkyboxSettings settings, AppData.ContentContainerType containerType, AppData.OrientationType orientationType, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            DynamicWidgetsContainer container = dynamicWidgetsContainersList.Find((x) => x.GetContentContainerType() == containerType);

            if (container != null && container.GetActive().Success())
            {
                if (skyboxUIHandlerPrefab != null)
                {
                    GameObject skyboxAsset = Instantiate(skyboxUIHandlerPrefab.gameObject);

                    if (skyboxAsset != null)
                    {
                        SkyboxUIHandler skyboxAssetHandler = skyboxAsset.GetComponent<SkyboxUIHandler>();

                        if (skyboxAssetHandler != null)
                        {
                            skyboxAssetHandler.SetDataOnInitialization(settings);
                            skyboxUIHandlerComponentsList.Add(skyboxAssetHandler);
                        }
                        else
                            skyboxUIHandlerComponentsList.Add(skyboxAsset.AddComponent<SkyboxUIHandler>());

                        AddContentToDynamicWidgetContainer(skyboxAsset.GetComponent<AppData.UIScreenWidget>(), container, orientationType);

                        callbackResults.result = "Skybox Handler Created Successfully.";
                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.result = "Create Skybox Handler Failed : skyboxAsset Was Instantiated.";
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.result = "Create Skybox Handler Failed : skyboxUIHandlerPrefab Is Null.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.result = "AddContentToDynamicWidgetContainer Failed : DynamicWidgetsContainer Is Null.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void AddContentToDynamicWidgetContainer(AppData.UIScreenWidget contentWidget, DynamicWidgetsContainer container, AppData.OrientationType orientation)
        {
            if (contentWidget != null)
            {
                container.AddContent(content: contentWidget, keepWorldPosition: false, overrideActiveState: false, updateContainer: true);
                container.UpdateContentOnRefresh();
            }
            else
                Debug.LogWarning("--> AddContentToDynamicWidgetContainer Failed : Content Widget Is Null.");
        }

        #endregion
    }
}
