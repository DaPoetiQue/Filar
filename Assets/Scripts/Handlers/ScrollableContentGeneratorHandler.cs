using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScrollableContentGeneratorHandler : AppMonoBaseClass
    {
        #region Components


        #endregion

        #region Main

        public async void Init(AppData.Widget parentWidget, DynamicUITextContentConfigDataPacket textContentConfigDataPacket, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(parentWidget.Initialized());

            if(callbackResults.Success())
            {
                callbackResults.SetResult(textContentConfigDataPacket?.Initialized());

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, " App Database Manager Instance", "Init Failed - App Database Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                        callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                        if (callbackResults.Success())
                        {
                            var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                            var awaitLoadedScreensTaskCallbackResults = await assetBundlesLibrary.OnAwaitAssetsInitialization(AppData.AssetBundleResourceLocatorType.Widget);

                            callbackResults.SetResult(awaitLoadedScreensTaskCallbackResults);

                            if (callbackResults.Success())
                            {
                                for (int i = 0; i < textContentConfigDataPacket.GetDynamicUITextComponents().GetData().Count; i++)
                                {
                                    var configData = textContentConfigDataPacket.GetDynamicUITextComponents().GetData()[i];
                                    var referencedWidgetDependencyAsset = configData.GetReferencedWidgetDependencyAsset().GetData();
                                    
                                    callbackResults.SetResult(assetBundlesLibrary.GetLoadedWidgets(AppData.ScreenType.Any, referencedWidgetDependencyAsset));

                                    if (callbackResults.Success())
                                    {
                                        var loadedWidgets = assetBundlesLibrary.GetLoadedWidgets(AppData.ScreenType.Any, referencedWidgetDependencyAsset).GetData();

                                        for (int j = 0; j < loadedWidgets.Count; j++)
                                        {
                                            var widgetComponent = Instantiate(loadedWidgets[j].gameObject).GetComponent<AppData.Widget>();
                                            widgetComponent.gameObject.SetName(loadedWidgets[j].GetName());

                                            widgetComponent.SetScreenType(parentWidget.GetScreenType().GetData());
                                            widgetComponent.SetContentContainerType(referencedWidgetDependencyAsset.GetContentContainerType().GetData());
                                            widgetComponent.SetScreenUIPlacementType(referencedWidgetDependencyAsset.GetScreenUIPlacementType().GetData());
                                            widgetComponent.SetUIScreenWidgetVisibilityState(referencedWidgetDependencyAsset.GetInitialVisibilityState().GetData());

                                            callbackResults.SetResult(referencedWidgetDependencyAsset.GetWidgetConstraints());

                                            if (callbackResults.Success())
                                            {
                                                widgetComponent.ApplyConstraints(constraintsAppliedCallbackResults =>
                                                {
                                                    callbackResults.SetResult(constraintsAppliedCallbackResults);

                                                }, AppData.Helpers.GetArray(referencedWidgetDependencyAsset.GetWidgetConstraints().GetData()));
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(widgetComponent, "Widget Component", $"Initialize Widgets Failed - Widget Component Not Found From Instantiated Object For Widget : {loadedWidgets[j].GetName()} - Of Type : {loadedWidgets[j].GetType().GetData()} - Invalid Operation, Please Check Here."));

                                            if (callbackResults.Success())
                                            {
                                                widgetComponent.Initilize(initializationCallbackResults =>
                                                {
                                                    callbackResults.SetResult(initializationCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        callbackResults.SetResult(parentWidget.GetDynamicContainer(referencedWidgetDependencyAsset.GetContentContainerType().GetData(), referencedWidgetDependencyAsset.GetScreenUIPlacementType().GetData()));

                                                        if(callbackResults.Success())
                                                        {
                                                            var container = parentWidget.GetDynamicContainer(referencedWidgetDependencyAsset.GetContentContainerType().GetData(), referencedWidgetDependencyAsset.GetScreenUIPlacementType().GetData()).GetData();

                                                            container.AddContent<AppData.Widget, AppData.WidgetType, AppData.TabViewType, AppData.Widget>(uiScreenWidgetComponent: widgetComponent, keepWorldPosition: false, isActive: widgetComponent.GetInitialVisibility().GetData(), overrideContainerActiveState: true, updateContainer: true, widgetnAddedCallbackResults =>
                                                            {
                                                                callbackResults.SetResult(widgetnAddedCallbackResults);

                                                                if (callbackResults.Success())
                                                                {
                                                                    #region Set Content

                                                                    widgetComponent.SetUITextDisplayerValue(configData.GetScreenTextDisplayerType().GetData(), configData.GetContentLocalizationKey().GetData(), contentSetCallbackResults => 
                                                                    {
                                                                        callbackResults.SetResult(contentSetCallbackResults);

                                                                        if(callbackResults.Success())
                                                                        {
                                                                            widgetComponent.SetUITextComponent(configData.GetScreenTextDisplayerType().GetData(), configData.GetTextComponent().GetData(), textComponentSetCallbackResults => 
                                                                            {
                                                                                callbackResults.SetResult(textComponentSetCallbackResults);

                                                                                // - Set Config Data
                                                                                LogInfo($"Logged_Cat//: Set Content Info Here Dawg...");

                                                                                if (callbackResults.UnSuccessful())
                                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                            });
                                                                        }
                                                                        else
                                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                    });

                                                                    #endregion
                                                                }
                                                                else
                                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                            });
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

       

        #endregion
    }
}