using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
    public class SurfacingGraphManager : AppData.SingletonBaseComponent<SurfacingGraphManager>
    {
        #region Components

        [Header("Active Graphs")]

        [Space(5)]
        [SerializeField]
        private List<AppData.GraphType> activeGraphKeys = new List<AppData.GraphType>();

        [Tooltip("Do Not Initialize - Graphs Are Loaded Dynamically : Warning - Data Overrides At Runtime.")]
        [Space(5)]
        [SerializeField]
        private List<SurfacingNodeGraph> loadedGraphs = new List<SurfacingNodeGraph>();

        #endregion

        #region Main

        protected override async void Init()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenEnterEvent, AppData.EventType.OnScreenShownEvent, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });
                appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenExitEvent, AppData.EventType.OnScreenHiddenEvent, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });

                appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnScreenBluredEvent, AppData.EventType.OnWidgetShownEvent, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });
                appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnScreenFocusedEvent, AppData.EventType.OnWidgetHiddenEvent, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });

                appEventsManagerInstance.OnEventSubscription<AppData.TabView<AppData.WidgetType>>(OnScreenBluredEvent, AppData.EventType.OnTabViewShownEvent, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });
                appEventsManagerInstance.OnEventSubscription<AppData.TabView<AppData.WidgetType>>(OnScreenFocusedEvent, AppData.EventType.OnTabViewHiddenEvent, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "Surfacing Manager Init Failed - App Database Manager Instance Is Not Yet Initialized - Invalid Operation."));

                    if(callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                        callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                        if(callbackResults.Success())
                        {
                            var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                            var awaitLoadedGraphsTaskCallbackResults = await assetBundlesLibrary.OnAwaitAssetsInitialization( AppData.AssetBundleResourceLocatorType.Graph);

                            callbackResults.SetResult(awaitLoadedGraphsTaskCallbackResults);

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResult(GetActiveGraphKeys());

                                if(callbackResults.Success())
                                {
                                    for (int i = 0; i < GetActiveGraphKeys().GetData().Count; i++)
                                    {
                                        callbackResults.SetResult(assetBundlesLibrary.GetLoadedGraphs(GetActiveGraphKeys().GetData()[i]));

                                        if(callbackResults.Success())
                                        {
                                            var graphs = assetBundlesLibrary.GetLoadedGraphs(GetActiveGraphKeys().GetData()[i]).GetData();

                                            AddGraphs(graphs, graphsAddedCallbackResults => 
                                            {
                                                callbackResults.SetResult(graphsAddedCallbackResults);

                                                if(callbackResults.UnSuccessful())
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            });
                                        }

                                        if (callbackResults.UnSuccessful())
                                        {
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            break;
                                        }
                                    }

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(GetGraphs());

                                        if (callbackResults.Success())
                                        {
                                            appEventsManagerInstance.InvokeEvent(AppData.EventType.OnStart, onStartEventTriggeredCallbackResults => 
                                            {
                                                callbackResults.SetResult(onStartEventTriggeredCallbackResults);

                                                if(callbackResults.Success())
                                                {
                                                    OnConfig(graphsConfiguredCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(graphsConfiguredCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            });
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

        private void AddGraphs(List<SurfacingNodeGraph> graphs, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(graphs, "Graphs", "Add graphs Failed - Graphs Parameter value Is Null - Invalid Operations."));

            if(callbackResults.Success())
                loadedGraphs.AddRange(graphs);
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #region Entry Event Callbacks

        private void OnScreenEnterEvent(Screen screen)
        {
            var callbackResults = new AppData.Callback(screen.GetType());

            if (callbackResults.Success())
            {
                OnGraphEntry(AppData.GraphEntryEventType.OnScreenEnter, screenEnterEventCallbackResults =>
                {
                    callbackResults.SetResult(screenEnterEventCallbackResults);

                    if (callbackResults.UnSuccessful())
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnScreenExitEvent(Screen screen)
        {
            var callbackResults = new AppData.Callback(screen.GetType());

            if (callbackResults.Success())
            {
                OnGraphEntry(AppData.GraphEntryEventType.OnScreenExit, screenExitEventCallbackResults =>
                {
                    callbackResults.SetResult(screenExitEventCallbackResults);

                    if (callbackResults.UnSuccessful())
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        private void OnScreenBluredEvent(AppData.Widget widget)
        {
            var callbackResults = new AppData.Callback(widget.GetType());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Screen Blured Event Failed - Screen UI Manager Instance Is Not Yet Initialized."));

                if(callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur());

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur().GetData().IsScreenBlured());

                            if (callbackResults.Success())
                            {
                                OnGraphEntry(AppData.GraphEntryEventType.OnScreenBlured, screenFocusedEventCallbackResults =>
                                {
                                    callbackResults.SetResult(screenFocusedEventCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
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
        }

        private void OnScreenFocusedEvent(AppData.Widget widget)
        {
            var callbackResults = new AppData.Callback(widget.GetType());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Screen Blured Event Failed - Screen UI Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur());

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur().GetData().IsScreenBlured());

                            if (callbackResults.UnSuccessful())
                            {
                                OnGraphEntry(AppData.GraphEntryEventType.OnScreenFocused, screenFocusedEventCallbackResults =>
                                {
                                    callbackResults.SetResult(screenFocusedEventCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
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
        }

        private void OnScreenBluredEvent(AppData.TabView<AppData.WidgetType> tabView)
        {
            var callbackResults = new AppData.Callback(tabView.GetType());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Screen Blured Event Failed - Screen UI Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur());

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur().GetData().IsScreenBlured());

                            if (callbackResults.Success())
                            {
                                OnGraphEntry(AppData.GraphEntryEventType.OnScreenBlured, screenFocusedEventCallbackResults =>
                                {
                                    callbackResults.SetResult(screenFocusedEventCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
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
        }

        private void OnScreenFocusedEvent(AppData.TabView<AppData.WidgetType> tabView)
        {
            var callbackResults = new AppData.Callback(tabView.GetType());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Screen Blured Event Failed - Screen UI Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur());

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur().GetData().IsScreenBlured());

                            if (callbackResults.UnSuccessful())
                            {
                                OnGraphEntry(AppData.GraphEntryEventType.OnScreenFocused, screenFocusedEventCallbackResults =>
                                {
                                    callbackResults.SetResult(screenFocusedEventCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
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
        }

        #endregion

        private async void OnGraphEntry(AppData.GraphEntryEventType entry, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(entry, "Entry", $"On Graph Entry Failed - Entry Parameter Value Is Set To Default : {entry} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetGraphs());

                if(callbackResults.Success())
                {
                    var entryGraphs = GetGraphs().GetData().FindAll(graph => graph.GetCurrentNode().Success()).
                        Where(graph => graph.GetEntryEventType().GetData() == entry && graph.CheckPrerequisiteGraphs().Success() 
                        && graph.Completed().UnSuccessful() && graph.InProgress().UnSuccessful()).ToList();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(entryGraphs, "Entry Graphs", $"On Graph Entry Failed - There Are No Entry Graphs Found - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var graphExecutionTasks = new List<Task>();

                        for (int i = 0; i < entryGraphs.Count; i++)
                            graphExecutionTasks.Add(ExecuteGraph(entryGraphs[i]));

                        await Task.WhenAll(graphExecutionTasks);
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

        private void OnConfig(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetGraphs());

            if(callbackResults.Success())
            {
                for (int i = 0; i < GetGraphs().GetData().Count; i++)
                {
                    GetGraphs().GetData()[i].Config(graphConfigedCallbackResults => 
                    {
                        callbackResults.SetResult(graphConfigedCallbackResults);

                        if(callbackResults.UnSuccessful())
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackDataList<SurfacingNodeGraph> GetGraphs()
        {
            var callbackResults = new AppData.CallbackDataList<SurfacingNodeGraph>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(loadedGraphs, "Graphs", "Get Graphs Failed - There Are No Graphs Found - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Graphs Success - There Are : {loadedGraphs.Count} Graphs Found.";
                callbackResults.data = loadedGraphs;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private async Task ExecuteGraph(SurfacingNodeGraph graph)
        {
            var callbackResults = new AppData.CallbackData<Task>(graph.Completed());

            if (callbackResults.UnSuccessful())
            {
                switch (graph.GetCurrentNode().GetData().GetNodeType().GetData())
                {
                    case AppData.GraphNodeType.EntryNode:

                        ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                        {
                            callbackResults.SetResult(proccessNextNodeCallbackResults);

                            if (callbackResults.UnSuccessful())
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });

                        break;

                    case AppData.GraphNodeType.CurrentScreenNode:

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Execute Graph Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                            callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreenType());

                            if (callbackResults.Success())
                            {
                                var currentScreenNode = graph.GetCurrentNode().GetData() as CurrentScreenNode;

                                callbackResults.SetResult(currentScreenNode.GetCurrentScreenType());

                                if (callbackResults.Success())
                                {
                                    callbackResults.SetResult(AppData.Helpers.GetAppEnumValuesEqual(currentScreenNode.GetCurrentScreenType().GetData(), screenUIManagerInstance.GetCurrentScreenType().GetData()));

                                    if (callbackResults.Success())
                                    {
                                        ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                        {
                                            callbackResults.SetResult(proccessNextNodeCallbackResults);

                                            if (callbackResults.UnSuccessful())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
                                    }
                                    else
                                    {
                                        graph.Reset(callback: graphResetedCallbackResults =>
                                        {
                                            callbackResults.SetResult(graphResetedCallbackResults);

                                            if (callbackResults.UnSuccessful())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
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

                        break;

                    case AppData.GraphNodeType.ScreenNode:

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Execute Graph Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                            if (callbackResults.Success())
                            {
                                var screenNode = graph.GetCurrentNode().GetData() as ScreenNode;

                                callbackResults.SetResult(screenNode.GetScreenType());

                                if (callbackResults.Success())
                                {
                                    var screenShowAsyncCallbackResultsTask = await screenUIManagerInstance.ShowScreenAsync(screenNode.GetScreenType().GetData());

                                    callbackResults.SetResult(screenShowAsyncCallbackResultsTask);

                                    if(callbackResults.Success())
                                    {
                                        ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                        {
                                            callbackResults.SetResult(proccessNextNodeCallbackResults);

                                            if (callbackResults.UnSuccessful())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
                                    }
                                    else
                                    {
                                        graph.Reset(callback: graphResetedCallbackResults =>
                                        {
                                            callbackResults.SetResult(graphResetedCallbackResults);

                                            if (callbackResults.UnSuccessful())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
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

                        break;

                    case AppData.GraphNodeType.WidgetNode:

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager Instance", "Execute Graph Failed -Surfacing Manager Instance Is Not Initialized Yet - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var surfacingManagerInstance = AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager Instance").GetData();
                            var screenWidgetStateNode = graph.GetCurrentNode().GetData() as WidgetNode;

                            callbackResults.SetResult(screenWidgetStateNode.GetWidgetType());

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResult(screenWidgetStateNode.GetState());

                                if (callbackResults.Success())
                                {
                                    switch (screenWidgetStateNode.GetState().GetData())
                                    {
                                        case AppData.UIVisibilityStateEvent.Show:

                                            surfacingManagerInstance.SurfaceWidget(screenWidgetStateNode.GetWidgetType().GetData(), popUpSurfacedCallbackResults =>
                                            {
                                                callbackResults.SetResult(popUpSurfacedCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(proccessNextNodeCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                {
                                                    graph.Reset(callback: graphResetedCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(graphResetedCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                            });

                                            break;

                                        case AppData.UIVisibilityStateEvent.Hide:

                                            surfacingManagerInstance.HidePopUp(screenWidgetStateNode.GetWidgetType().GetData(), popUpSurfacedCallbackResults =>
                                            {
                                                callbackResults.SetResult(popUpSurfacedCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(proccessNextNodeCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                {
                                                    graph.Reset(callback: graphResetedCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(graphResetedCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                            });

                                            break;
                                    }
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                            {
                                graph.Reset(callback: graphResetedCallbackResults =>
                                {
                                    callbackResults.SetResult(graphResetedCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.PopupNode:

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager Instance", "Execute Graph Failed -Surfacing Manager Instance Is Not Initialized Yet - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var surfacingManagerInstance = AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager Instance").GetData();
                            var screenPopUpStateNode = graph.GetCurrentNode().GetData() as PopupNode;

                            callbackResults.SetResult(screenPopUpStateNode.GetPopupTemplateType());

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResult(screenPopUpStateNode.GetState());

                                if (callbackResults.Success())
                                {
                                    switch(screenPopUpStateNode.GetState().GetData())
                                    {
                                        case AppData.UIVisibilityStateEvent.Show:

                                            surfacingManagerInstance.ShowPopUp(screenPopUpStateNode.GetPopupTemplateType().GetData(), popUpSurfacedCallbackResults =>
                                            {
                                                callbackResults.SetResult(popUpSurfacedCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(proccessNextNodeCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                {
                                                    graph.Reset(callback: graphResetedCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(graphResetedCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                            });

                                            break;

                                        case AppData.UIVisibilityStateEvent.Hide:

                                            surfacingManagerInstance.HidePopUp(screenPopUpStateNode.GetPopupTemplateType().GetData(), popUpSurfacedCallbackResults =>
                                            {
                                                callbackResults.SetResult(popUpSurfacedCallbackResults);

                                                if (callbackResults.Success())
                                                {
                                                    ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(proccessNextNodeCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                                else
                                                {
                                                    graph.Reset(callback: graphResetedCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(graphResetedCallbackResults);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    });
                                                }
                                            });

                                            break;
                                    }
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                            {
                                graph.Reset(callback: graphResetedCallbackResults =>
                                {
                                    callbackResults.SetResult(graphResetedCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.ShowTooltipNode:

                        var showTooltipNode = graph.GetCurrentNode().GetData() as ShowTooltipNode;

                        break;

                    case AppData.GraphNodeType.HideTooltipNode:

                        var hideTooltipNode = graph.GetCurrentNode().GetData() as HideTooltipNode;

                        break;

                    case AppData.GraphNodeType.TriggerEventNode:

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "Execute Graph Failed -App Events Manager Instance Is Not Initialized Yet - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();
                            var triggerEventNode = graph.GetCurrentNode().GetData() as TriggerEventNode;

                            callbackResults.SetResult(triggerEventNode.GetEventType());

                            if (callbackResults.Success())
                            {
                                appEventsManagerInstance.InvokeEvent(triggerEventNode.GetEventType().GetData(), eventInvokedCallbackResults =>
                                {
                                    callbackResults.SetResult(eventInvokedCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                        {
                                            callbackResults.SetResult(proccessNextNodeCallbackResults);

                                            if (callbackResults.UnSuccessful())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.WaitForEventNode:

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "Execute Graph Failed - App Events Manager Instance Is Not Yet Initialized - Invalid Operation."));

                        if(callbackResults.Success())
                        {
                            var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                            var waitForEventNode = graph.GetCurrentNode().GetData() as WaitForEventNode;

                            callbackResults.SetResult(waitForEventNode.GetEventType());

                            if (callbackResults.Success())
                            {
                                while (appEventsManagerInstance.GetCurrentEvent().GetData() != waitForEventNode.GetEventType().GetData())
                                    await Task.Yield();

                                ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.DisableScreenInputsNode:

                        var waitForButtonEventNode = graph.GetCurrentNode().GetData() as DisableScreenInputsNode;

                        if (callbackResults.Success())
                        {

                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.WaitForSecondsNode:

                        var waitForSecondsNode = graph.GetCurrentNode().GetData() as WaitForSecondsNode;

                        callbackResults.SetResult(waitForSecondsNode.GetWaitTime());

                        if (callbackResults.Success())
                        {
                            await Task.Delay(AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(waitForSecondsNode.GetWaitTime().GetData()));

                            ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                            {
                                callbackResults.SetResult(proccessNextNodeCallbackResults);

                                if (callbackResults.UnSuccessful())
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                            });
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.ExitNode:

                        callbackResults.SetResult(graph.CompleteGraph());

                        if (callbackResults.UnSuccessful())
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            await Task.Yield();
        }

        private async void ProccessNextNode(SurfacingNodeGraph graph, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(graph.GetCurrentNode());

            if (callbackResults.Success())
            {
                foreach (NodePort port in graph.GetCurrentNode().GetData().Ports)
                {
                    if (port.fieldName == "output")
                    {
                        callbackResults.SetResult(graph.SetCurrentNode(port.Connection.node as BaseNode));

                        if (callbackResults.Success())
                            break;
                    }
                    else
                        continue;
                }

                await ExecuteGraph(graph);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);


            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackDataList<AppData.GraphType> GetActiveGraphKeys()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.GraphType>();

            callbackResults.SetResult(AppData.Helpers.GetAppEnumValuesValid(activeGraphKeys, "Active Graph Keys", $"Get Active Graph Keys Failed - There Are No Active Graph Keys Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Active Graph Keys Success - There's : {activeGraphKeys.Count} Active Graph Key(s) Found.";
                callbackResults.data = activeGraphKeys;
            }

            return callbackResults;
        }

        #endregion
    }
}
