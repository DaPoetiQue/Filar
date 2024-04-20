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

        protected override void Init()
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance", "Surfacing Data Init Failed - App Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if(callbackResults.Success())
            {
                var appManagerInstance = AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance").GetData();

                callbackResults.SetResult(appManagerInstance.BootSequenceEnabled());

                if(callbackResults.Success())
                {
                    BuildGraphs(buildGraphsCallbackResults => 
                    {
                        callbackResults.SetResult(buildGraphsCallbackResults);

                        if(callbackResults.UnSuccessful())
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

        public async void BuildGraphs(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenEnterEvent, AppData.EventType.OnScreenShown, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });
                appEventsManagerInstance.OnEventSubscription<Screen>(OnScreenExitEvent, AppData.EventType.OnScreenHidden, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });

                appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnScreenBluredEvent, AppData.EventType.OnWidgetShown, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });
                appEventsManagerInstance.OnEventSubscription<AppData.Widget>(OnScreenFocusedEvent, AppData.EventType.OnWidgetHidden, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });

                appEventsManagerInstance.OnEventSubscription<AppData.TabView<AppData.WidgetType>>(OnScreenBluredEvent, AppData.EventType.OnTabViewShown, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });
                appEventsManagerInstance.OnEventSubscription<AppData.TabView<AppData.WidgetType>>(OnScreenFocusedEvent, AppData.EventType.OnTabViewHidden, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });

                appEventsManagerInstance.OnEventSubscription(OnAppEvents, true, subscribedToEventCallbackResults => { callbackResults.SetResult(subscribedToEventCallbackResults); });

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(GetScriptExecutionMode());

                    if (callbackResults.Success())
                    {
                        if (GetScriptExecutionMode().GetData() == AppData.BuildType.Debug)
                        {
                            #region Debug Execution

                            callbackResults.SetResult(GetGraphs());

                            if (callbackResults.Success())
                            {
                                OnConfig(graphsConfiguredCallbackResults =>
                                {
                                    callbackResults.SetResult(graphsConfiguredCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        appEventsManagerInstance.InvokeEvent(AppData.EventType.OnStart, onStartEventTriggeredCallbackResults =>
                                        {
                                            callbackResults.SetResult(onStartEventTriggeredCallbackResults);

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

                            #endregion
                        }
                        else
                        {
                            #region Build Execution

                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "Surfacing Manager Init Failed - App Database Manager Instance Is Not Yet Initialized - Invalid Operation."));

                            if (callbackResults.Success())
                            {
                                var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                                callbackResults.SetResult(appDatabaseManagerInstance.GetAssetBundlesLibrary());

                                if (callbackResults.Success())
                                {
                                    var assetBundlesLibrary = appDatabaseManagerInstance.GetAssetBundlesLibrary().GetData();

                                    var awaitLoadedGraphsTaskCallbackResults = await assetBundlesLibrary.OnAwaitAssetsInitialization(AppData.AssetBundleResourceLocatorType.Graph);

                                    callbackResults.SetResult(awaitLoadedGraphsTaskCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(GetActiveGraphKeys());

                                        if (callbackResults.Success())
                                        {
                                            for (int i = 0; i < GetActiveGraphKeys().GetData().Count; i++)
                                            {
                                                callbackResults.SetResult(assetBundlesLibrary.GetLoadedGraphs(GetActiveGraphKeys().GetData()[i]));

                                                if (callbackResults.Success())
                                                {
                                                    var graphs = assetBundlesLibrary.GetLoadedGraphs(GetActiveGraphKeys().GetData()[i]).GetData();

                                                    AddGraphs(graphs, graphsAddedCallbackResults =>
                                                    {
                                                        callbackResults.SetResult(graphsAddedCallbackResults);

                                                        if (callbackResults.UnSuccessful())
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

                                                        if (callbackResults.Success())
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

                            #endregion
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

            callback?.Invoke(callbackResults);
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

        private void OnAppEvents(AppData.EventType eventType)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(eventType, "Event Type", $"On App Events Failed - Event Type Parameter Value Is Set To Default : {eventType} - Invalid Operation."));

            if(callbackResults.Success())
            {
                if(eventType == AppData.EventType.OnStart)
                {
                    OnGraphEntry(AppData.GraphEntryEventType.OnStart, screenEnterEventCallbackResults =>
                    {
                        callbackResults.SetResult(screenEnterEventCallbackResults);

                        if (callbackResults.UnSuccessful())
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        }

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
            //var callbackResults = new AppData.Callback(widget.GetType());

            //if (callbackResults.Success())
            //{
            //    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "On Screen Blured Event Failed - Screen UI Manager Instance Is Not Yet Initialized."));

            //    if (callbackResults.Success())
            //    {
            //        var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

            //        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

            //        if (callbackResults.Success())
            //        {
            //            callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur());

            //            if (callbackResults.Success())
            //            {
            //                callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur().GetData().IsScreenBlured());

            //                if (callbackResults.UnSuccessful())
            //                {
            //                    OnGraphEntry(AppData.GraphEntryEventType.OnScreenFocused, screenFocusedEventCallbackResults =>
            //                    {
            //                        callbackResults.SetResult(screenFocusedEventCallbackResults);

            //                        if (callbackResults.UnSuccessful())
            //                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //                    });
            //                }
            //                else
            //                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //            }
            //            else
            //                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //        }
            //        else
            //            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //    }
            //    else
            //        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            //}
            //else
            //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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
                        Where(graph => graph.GetEntryEventType().GetData() == entry && 
                        graph.Completed().UnSuccessful() && graph.InProgress().UnSuccessful()).ToList();

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

                        var checkPrerequisiteGraphCallbackResults = await graph.CheckPrerequisiteGraphs();

                        callbackResults.SetResult(checkPrerequisiteGraphCallbackResults);

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

                        break;

                    case AppData.GraphNodeType.ConditionalNode:

                        var conditionalNode = graph.GetCurrentNode().GetData() as ConditionalNode;

                        callbackResults.SetResult(conditionalNode.GetCondition());

                        if(callbackResults.Success())
                        {
                            var executeConditionalStatementCallbackResultsTask = await OnExecutionalCondition(conditionalNode.GetCondition().GetData());

                            callbackResults.SetResult(executeConditionalStatementCallbackResultsTask);

                            if(callbackResults.Success())
                            {
                                ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, "isTrue");
                            }
                            else
                            {
                                ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, "isFalse");
                            }
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.ExecuteActionNode:

                        var executeActionNode = graph.GetCurrentNode().GetData() as ExecuteActionNode;

                        callbackResults.SetResult(executeActionNode.GetAction());

                        if (callbackResults.Success())
                        {
                            var executeConditionalActionStatementCallbackResultsTask = await OnExecutionalAction(executeActionNode.GetAction().GetData());

                            callbackResults.SetResult(executeConditionalActionStatementCallbackResultsTask);

                            if (callbackResults.Success())
                            {
                                ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, "successCode");
                            }
                            else
                            {
                                ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, "errorCode");
                            }
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        break;

                    case AppData.GraphNodeType.LoadingSequenceNode:

                        var loadingSequenceNode = graph.GetCurrentNode().GetData() as LoadingSequenceNode;

                        callbackResults.SetResult(loadingSequenceNode.GetSequences());

                        if(callbackResults.Success())
                        {
                            var processLoadingSequenceCallbackResultsTask = await ProcessLoadingSequence(loadingSequenceNode.GetSequences().GetData());

                            callbackResults.SetResult(processLoadingSequenceCallbackResultsTask);

                            if (callbackResults.Success())
                            {
                                ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, "successCode");
                            }
                            else
                            {
                                ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.UnSuccessful())
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                }, "errorCode");
                            }
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

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
                                    callbackResults.SetResult(screenNode.GetState());

                                    if (callbackResults.Success())
                                    {
                                        switch(screenNode.GetState().GetData())
                                        {
                                            case AppData.UIVisibilityStateEvent.Show:

                                                screenUIManagerInstance.ShowScreenNode(screenNode.GetScreenType().GetData(), showScreenCallbackResults => 
                                                {
                                                    callbackResults.SetResult(showScreenCallbackResults);

                                                    if(callbackResults.Success())
                                                    {
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
                                                });

                                                break;

                                            case AppData.UIVisibilityStateEvent.Hide:

                                                screenUIManagerInstance.HideScreenNode(screenNode.GetScreenType().GetData(), hideScreenCallbackResults => 
                                                {
                                                    callbackResults.SetResult(hideScreenCallbackResults);

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
                            var widgetNode = graph.GetCurrentNode().GetData() as WidgetNode;

                            callbackResults.SetResult(widgetNode.GetWidgetType());

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResult(widgetNode.GetState());

                                if (callbackResults.Success())
                                {
                                    switch (widgetNode.GetState().GetData())
                                    {
                                        case AppData.UIVisibilityStateEvent.Show:

                                            surfacingManagerInstance.SurfaceWidget(widgetNode.GetWidgetType().GetData(), popUpSurfacedCallbackResults =>
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

                                            }, widgetNode.GetScreenBlurConfig().GetData());

                                            break;

                                        case AppData.UIVisibilityStateEvent.Hide:

                                            surfacingManagerInstance.HidePopUp(widgetNode.GetWidgetType().GetData(), popUpSurfacedCallbackResults =>
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

                                            }, widgetNode.GetScreenBlurConfig()?.GetData());

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
                                callbackResults.SetResult(waitForEventNode.GetTimeOut());

                                var timedOut = false;

                                if(callbackResults.Success())
                                {
                                    var timeout = waitForEventNode.GetTimeOut().GetData();

                                    while (appEventsManagerInstance.GetCurrentEvent().GetData() != waitForEventNode.GetEventType().GetData() && timeout > 0.0f)
                                    {
                                        timeout -= 1.0f * Time.deltaTime;
                                        await Task.Yield();
                                    }

                                    if(appEventsManagerInstance.GetCurrentEvent().GetData() == waitForEventNode.GetEventType().GetData())
                                    {
                                        callbackResults.result = "Execution Completed Successfully.";
                                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                    }
                                    else
                                    {
                                        timedOut = true;
                                        callbackResults.result = "Execution Failed With Timeout.";
                                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                                    }
                                }
                                else
                                {
                                    while (appEventsManagerInstance.GetCurrentEvent().GetData() != waitForEventNode.GetEventType().GetData())
                                        await Task.Yield();

                                    if (appEventsManagerInstance.GetCurrentEvent().GetData() == waitForEventNode.GetEventType().GetData())
                                    {
                                        callbackResults.result = "Execution Completed Successfully.";
                                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                                    }
                                    else
                                    {
                                        callbackResults.result = "Execution Failed With With Unknown Error - Please Check Here.";
                                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                                    }
                                }

                                if (callbackResults.Success())
                                {
                                    ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                    {
                                        callbackResults.SetResult(proccessNextNodeCallbackResults);

                                        if (callbackResults.UnSuccessful())
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                    }, "successCode");
                                }
                                else
                                {
                                    if(timedOut)
                                    {
                                        ProccessNextNode(graph, proccessNextNodeCallbackResults =>
                                        {
                                            callbackResults.SetResult(proccessNextNodeCallbackResults);

                                            if (callbackResults.UnSuccessful())
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                        }, "timedOut");
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

        private async Task<AppData.Callback> OnExecutionalCondition(AppData.AppExecutionalConditionType conditionType)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(conditionType, "Condition Type", $"On Executional Condition Failed - Condition Type Parameter Value Is Set To Default : {conditionType} - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance", "On Executional Condition Failed - App Manager Instance Is Not Initialized Yet - Invalid Operation."));

                if (callbackResults.Success())
                {
                    var appManagerInstance = AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance").GetData();

                    switch (conditionType)
                    {
                        case AppData.AppExecutionalConditionType.AppLanguageSelected:

                            callbackResults.SetResult(appManagerInstance.GetAppSettingsDataFile());

                            if (callbackResults.Success())
                            {

                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                            break;

                        case AppData.AppExecutionalConditionType.PermissionsGranted:

                            var permissionsGrantedCallbackResultsTask = await appManagerInstance.PermissionsGranted();

                            callbackResults.SetResult(permissionsGrantedCallbackResultsTask);

                            if(callbackResults.UnSuccessful())
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                            break;
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private async Task<AppData.Callback> OnExecutionalAction(AppData.ExecutiveActionType actionType)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValueValid(actionType, "Action Type", $"On Executional Action Failed - Action Type Parameter Value Is Set To Default : {actionType} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance", "On Executional Condition Failed - App Manager Instance Is Not Initialized Yet - Invalid Operation."));

                if (callbackResults.Success())
                {
                    var appManagerInstance = AppData.Helpers.GetAppComponentValid(AppManager.Instance, "App Manager Instance").GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "On Executional Condition Failed - App Database Manager Instance Is Not Initialized Yet - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                        callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, "Network Manager Instance", " Is Not Yet Initialized - Invalid Operation."));

                        if (callbackResults.Success())
                        {
                            var networkManager = AppData.Helpers.GetAppComponentValid(NetworkManager.Instance, "Network Manager Instance").GetData();

                            callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized - Invalid Operation."));

                            if (callbackResults.Success())
                            {
                                var profileManager = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                                callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "App Events Manager Instance Is Not Yet Initialized - Invalid Operation."));

                                if (callbackResults.Success())
                                {
                                    var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                                    callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance", "Loading Manager Instance Is Not Yet Initialized - Invalid Operation."));

                                    if (callbackResults.Success())
                                    {
                                        var loadingManagerInstance = AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance").GetData();

                                        callbackResults.SetResults(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized - Invalid Operation."));

                                        if (callbackResults.Success())
                                        {
                                            var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                                            switch (actionType)
                                            {
                                                case AppData.ExecutiveActionType.RequestAppUserPermissions:

                                                    var checkUserAppUserPermissionsStatus = await appManagerInstance.PermissionsGranted();

                                                    callbackResults.SetResult(checkUserAppUserPermissionsStatus);

                                                    if (callbackResults.UnSuccessful())
                                                    {

                                                    }

                                                    break;

                                                case AppData.ExecutiveActionType.BootLoadSequence:

                                                    //var progressReport = new Progress<int>(appEventsManagerInstance.InvokeEvent);

                                                    //var processBootSequenceCallbackResultsTask = await loadingManagerInstance.ProcessBootSequence(progressReport);

                                                    //callbackResults.SetResult(processBootSequenceCallbackResultsTask);

                                                    //if (callbackResults.UnSuccessful())
                                                    //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;

                                                case AppData.ExecutiveActionType.CheckNetworkConnection:

                                                    //var networkConnectionCallbackResults = await networkManager.OnCheckNetworkConnectionStatus();

                                                    //callbackResults.SetResult(networkConnectionCallbackResults);

                                                    //if (callbackResults.UnSuccessful())
                                                    //    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;

                                                case AppData.ExecutiveActionType.CheckCompitability:

                                                    var getCompatibilityStatusAsyncCallbackResultsTask = await appManagerInstance.GetCompatibilityStatusAsync();

                                                    callbackResults.SetResult(getCompatibilityStatusAsyncCallbackResultsTask);

                                                    if (callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;

                                                case AppData.ExecutiveActionType.ConnectClientToServer:

                                                    var serverConnectedCallbackResults = await networkManager.ServerConnected();

                                                    callbackResults.SetResult(serverConnectedCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        var synchronizingAppInfoCallbackResults = await appManagerInstance.SynchronizingAppInfo();

                                                        callbackResults.SetResult(synchronizingAppInfoCallbackResults);

                                                        if (callbackResults.Success())
                                                        {
                                                            var checkEntryPointAsyncCallbackResults = await appManagerInstance.CheckEntryPointAsync();

                                                            callbackResults.SetResult(checkEntryPointAsyncCallbackResults);

                                                            if (callbackResults.UnSuccessful())
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                        }
                                                        else
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;

                                                case AppData.ExecutiveActionType.DownloadContent:

                                                    var downloadPostEntryDataAsyncCallbackResults = await appManagerInstance.DownloadPostEntryDataAsync();

                                                    callbackResults.SetResult(downloadPostEntryDataAsyncCallbackResults);

                                                    if (callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;

                                                case AppData.ExecutiveActionType.SyncUserProfile:

                                                    var synchronizingProfileCallbackResults = await profileManager.SynchronizingProfile();

                                                    callbackResults.SetResult(synchronizingProfileCallbackResults);

                                                    if (callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;

                                                case AppData.ExecutiveActionType.SignInApp:

                                                    var appSignInAsyncCallbackResults = await profileManager.AppSignInAsync();

                                                    callbackResults.SetResult(appSignInAsyncCallbackResults);

                                                    if (callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;

                                                case AppData.ExecutiveActionType.RefreshScreenData:

                                                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                                                    if (callbackResults.Success())
                                                    {
                                                        var screenRefreshCallbackResultsTask = await appDatabaseManagerInstance.RefreshedAsync(screenUIManagerInstance.GetCurrentScreen().GetData());

                                                        callbackResults.SetResult(screenRefreshCallbackResultsTask);

                                                        if (callbackResults.UnSuccessful())
                                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                    break;
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
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private async Task<AppData.Callback> ProcessLoadingSequence(List<AppData.ProgressReportInfoState> sequences)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppEnumValuesValid(sequences, "Sequences", "Process Loading Sequence Failed - There Are No Sequences Assigned - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance", "Process Loading Sequence Failed - Loading Manager Instance Is Not Initialized Yet - Invalid Operation."));

                if(callbackResults.Success())
                {
                    var loadingManagerInstance = AppData.Helpers.GetAppComponentValid(LoadingManager.Instance, "Loading Manager Instance").GetData();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance", "Process Loading Sequence Failed - App Events Manager Instance Is Not Initialized Yet - Invalid Operation."));

                    if (callbackResults.Success())
                    {
                        var appEventsManagerInstance = AppData.Helpers.GetAppComponentValid(AppEventsManager.Instance, "App Events Manager Instance").GetData();

                        var progressReport = new Progress<int>(appEventsManagerInstance.InvokeEvent);

                        var processSequenceCallbackResultsTask = await loadingManagerInstance.ProcessLoadingSequence(sequences, progressReport);

                        if(callbackResults.UnSuccessful())
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

            return callbackResults;
        }

        private async void ProccessNextNode(SurfacingNodeGraph graph, Action<AppData.Callback> callback = null, string portName = "output")
        {
            var callbackResults = new AppData.Callback(graph.GetCurrentNode());

            if (callbackResults.Success())
            {
                foreach (NodePort port in graph.GetCurrentNode().GetData().Ports)
                {
                    if (port.fieldName == portName)
                    {
                        if (port.Connection != null)
                        {
                            callbackResults.SetResult(graph.SetCurrentNode(port.Connection.node as BaseNode));

                            if (callbackResults.Success())
                                break;
                        }
                        else
                        {
                            callbackResults.result = "There Is No Connection To This Node Output - Exiting Graph";
                            callbackResults.resultCode = AppData.Helpers.WarningCode;

                            break;
                        }
                    }
                    else
                        continue;
                }

                if(callbackResults.Success())
                    await ExecuteGraph(graph);
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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
