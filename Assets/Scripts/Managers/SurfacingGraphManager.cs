using System;
using System.Collections;
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

        [Space(5)]
        [SerializeField]
        private List<SurfacingNodeGraph> graphs = new List<SurfacingNodeGraph>();

        private List<SurfacingNodeGraph> selectedGraphs = new List<SurfacingNodeGraph>();
        private Coroutine _process;

        private Action waitForEventAction, 
                       waitForButtonEventAction;

        #endregion

        #region Main

        protected override void Init()
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
                    OnConfig(graphsConfiguredCallbackResults =>
                    {
                        callbackResults.SetResult(graphsConfiguredCallbackResults);

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

                    if(callbackResults.Success())
                    {
                        callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur());

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen().GetData().GetScreenBlur().GetData().IsScreenBlured());

                            if(callbackResults.Success())
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

        private void OnGraphEntry(AppData.GraphEntryEventType entry, Action<AppData.Callback> callback = null)
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
                        SetSelectedGraphs(entryGraphs, entryGraphsAssignedCallbackResults =>
                        {
                            callbackResults.SetResult(entryGraphsAssignedCallbackResults);

                            if (callbackResults.Success())
                            {
                                ExecuteGraph(onProcessGraphsCallbackResults => { callbackResults.SetResult(onProcessGraphsCallbackResults); });
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
            var callbackResults = new AppData.CallbackDataList<SurfacingNodeGraph>(AppData.Helpers.GetAppComponentsValid(graphs, "Graphs", "Get Graphs Failed - There Are No Graphs Found - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Graphs Success - There Are : {graphs.Count} Graphs Found.";
                callbackResults.data = graphs;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private void SetSelectedGraphs(List<SurfacingNodeGraph> selectedGraphs, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(selectedGraphs, "Selected Graphs", "Set Selected Graphs Failed - Selected Graphs Parameter Value Is Null - Invalid Operation."));

            if(callbackResults.Success())
            {
                this.selectedGraphs = selectedGraphs;
                callbackResults.result = $"Set Selected Graphs Success - There Are : {selectedGraphs.Count} - Selected Graphs Assigned.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private AppData.CallbackDataList<SurfacingNodeGraph> GetSelectedGraphs()
        {
            var callbackResults = new AppData.CallbackDataList<SurfacingNodeGraph>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(selectedGraphs, "Selected Graphs", "Get Selected Graphs Failed - There Are No Selected Graphs Found - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Selected Graphs Success - There Are : {selectedGraphs.Count} Selected Graphs Found.";
                callbackResults.data = selectedGraphs;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        private async void ExecuteGraph(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetSelectedGraphs());

            if(callbackResults.Success())
            {
                for (int i = 0; i < GetSelectedGraphs().GetData().Count; i++)
                {
                    callbackResults.SetResult(GetSelectedGraphs().GetData()[i].Completed());

                    if (callbackResults.UnSuccessful())
                    {
                        switch (GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().GetNodeType().GetData())
                        {
                            case AppData.GraphNodeType.EntryNode:

                                ProccessNextNode(GetSelectedGraphs().GetData()[i], proccessNextNodeCallbackResults =>
                                {
                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        LogInfo($"Logging_Cats:// Finished Executing Node : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().name} - " +
                                                $"Of Type : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().GetNodeType().GetData()} - " +
                                                $"In Graph {GetSelectedGraphs().GetData()[i].name} With Entry Event : {GetSelectedGraphs().GetData()[i].GetEntryEventType().GetData()}", this);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });

                                break;

                            case AppData.GraphNodeType.CurrentScreenNode:

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Execute Graph Failed - Screen UI Manager Instance Is Not Initialized Yet - Invalid Operation."));

                                if(callbackResults.Success())
                                {
                                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreenType());

                                    if(callbackResults.Success())
                                    {
                                        var currentScreenNode = GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData() as CurrentScreenNode;

                                        callbackResults.SetResult(currentScreenNode.GetCurrentScreenType());

                                        if(callbackResults.Success())
                                        {
                                            callbackResults.SetResult(AppData.Helpers.GetAppEnumValuesEqual(currentScreenNode.GetCurrentScreenType().GetData(), screenUIManagerInstance.GetCurrentScreenType().GetData()));

                                            if (callbackResults.Success())
                                            {
                                                ProccessNextNode(GetSelectedGraphs().GetData()[i], proccessNextNodeCallbackResults =>
                                                {
                                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        LogInfo($"Logging_Cats:// Finished Executing Node : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().name} - " +
                                                                $"Of Type : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().GetNodeType().GetData()} - " +
                                                                $"In Graph {GetSelectedGraphs().GetData()[i].name} With Entry Event : {GetSelectedGraphs().GetData()[i].GetEntryEventType().GetData()}", this);
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                            {
                                                GetSelectedGraphs().GetData()[i].Reset(callback: graphResetedCallbackResults =>
                                                {
                                                    callbackResults.SetResult(graphResetedCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        StopProcessing(proccessingStoppedCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(proccessingStoppedCallbackResults);

                                                            if (callbackResults.UnSuccessful())
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                        });
                                                    }
                                                    else
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

                            case AppData.GraphNodeType.ShowPopupNode:

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager Instance", "Execute Graph Failed -Surfacing Manager Instance Is Not Initialized Yet - Invalid Operation."));

                                if(callbackResults.Success())
                                {
                                    var surfacingManagerInstance = AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager Instance").GetData();
                                    var showPopupNode = GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData() as ShowPopupNode;

                                    callbackResults.SetResult(showPopupNode.GetSurfacingTemplateType());

                                    if(callbackResults.Success())
                                    {
                                        surfacingManagerInstance.ShowPopUp(showPopupNode.GetSurfacingTemplateType().GetData(), popUpSurfacedCallbackResults => 
                                        {
                                            callbackResults.SetResult(popUpSurfacedCallbackResults);

                                            if(callbackResults.Success())
                                            {
                                                ProccessNextNode(GetSelectedGraphs().GetData()[i], proccessNextNodeCallbackResults =>
                                                {
                                                    callbackResults.SetResult(proccessNextNodeCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        LogInfo($"Logging_Cats:// Finished Executing Node : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().name} - " +
                                                                $"Of Type : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().GetNodeType().GetData()} - " +
                                                                $"In Graph {GetSelectedGraphs().GetData()[i].name} With Entry Event : {GetSelectedGraphs().GetData()[i].GetEntryEventType().GetData()}", this);
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                            {
                                                StopProcessing(proccessingStoppedCallbackResults =>
                                                {
                                                    callbackResults.SetResult(proccessingStoppedCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        GetSelectedGraphs().GetData()[i].Reset(callback: graphResetedCallbackResults =>
                                                        {
                                                            callbackResults.SetResult(graphResetedCallbackResults);

                                                            if (callbackResults.UnSuccessful())
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                        });
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                        });
                                    }
                                    else
                                    {
                                        StopProcessing(proccessingStoppedCallbackResults =>
                                        {
                                            callbackResults.SetResult(proccessingStoppedCallbackResults);

                                            if (callbackResults.Success())
                                            {
                                                GetSelectedGraphs().GetData()[i].Reset(callback: graphResetedCallbackResults =>
                                                {
                                                    callbackResults.SetResult(graphResetedCallbackResults);

                                                    if (callbackResults.UnSuccessful())
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        });
                                    }
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                break;

                            case AppData.GraphNodeType.ShowTooltipNode:

                                var showTooltipNode = GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData() as ShowTooltipNode;

                                break;

                            case AppData.GraphNodeType.WaitForEventNode:

                                var waitForEventNode = GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData() as WaitForEventNode;

                                

                                break;

                            case AppData.GraphNodeType.WaitForButtonEventNode:

                                var waitForButtonEventNode = GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData() as WaitForButtonEventNode;



                                break;

                            case AppData.GraphNodeType.WaitForSecondsNode:

                                var waitForSecondsNode = GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData() as WaitForSecondsNode;

                                callbackResults.SetResult(waitForSecondsNode.GetWaitTime());

                                if (callbackResults.Success())
                                {
                                    await Task.Delay(AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(waitForSecondsNode.GetWaitTime().GetData()));

                                    ProccessNextNode(GetSelectedGraphs().GetData()[i], proccessNextNodeCallbackResults =>
                                    {
                                        callbackResults.SetResult(proccessNextNodeCallbackResults);

                                        if (callbackResults.Success())
                                        {
                                            LogInfo($"Logging_Cats:// Finished Executing Node : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().name} - " +
                                                    $"Of Type : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().GetNodeType().GetData()} - " +
                                                    $"In Graph {GetSelectedGraphs().GetData()[i].name} With Entry Event : {GetSelectedGraphs().GetData()[i].GetEntryEventType().GetData()}", this);
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    });

                                    LogInfo($"Logging_Cats:// Code : {callbackResults.GetResultCode} - Results : {callbackResults.GetResult}", this);
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                break;

                            case AppData.GraphNodeType.ExitNode:

                                callbackResults.SetResult(GetSelectedGraphs().GetData()[i].CompleteGraph());

                                if (callbackResults.Success())
                                {
                                    callbackResults.SetResult(StopProcessing());

                                    if (callbackResults.Success())
                                    {
                                        LogInfo($"Logging_Cats:// Finished Executing Node : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().name} - " +
                                                $"Of Type : {GetSelectedGraphs().GetData()[i].GetCurrentNode().GetData().GetNodeType().GetData()} - " +
                                                $"In Graph {GetSelectedGraphs().GetData()[i].name} With Entry Event : {GetSelectedGraphs().GetData()[i].GetEntryEventType().GetData()}", this);
                                    }
                                    else
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
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private void ProccessNextNode(SurfacingNodeGraph graph, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(StopProcessing());

            if (callbackResults.Success())
            {
                foreach (NodePort port in graph.GetCurrentNode().GetData().Ports)
                {
                    if (port.fieldName == "output")
                    {
                        callbackResults.SetResult(graph.SetCurrentNode(port.Connection.node as BaseNode));

                        if (callbackResults.Success())
                        {
                            ExecuteGraph(onProcessGraphsCallbackResults => { callbackResults.SetResult(onProcessGraphsCallbackResults); });

                            break;
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        continue;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);


            callback?.Invoke(callbackResults);
        }

        private void StopProcessing(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            StopCoroutine(_process);
            _process = null;

            if (_process == null)
            {
                callbackResults.result = "Stop Processing Success -  All Running Proccesses Have Been Successfully Stopped.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Stop Processing Failed - There Is No Running Proccess To Stop - Invalid Operation.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            callback?.Invoke(callbackResults);
        }

        private AppData.Callback StopProcessing()
        {
            var callbackResults = new AppData.Callback();

            if (_process != null)
            {
                StopCoroutine(_process);
                _process = null;
            }

            if(_process == null)
            {
                callbackResults.result = "Stop Processing Success -  All Running Proccesses Have Been Successfully Stopped.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Stop Processing Failed - Process Couldn't Be Stopped - Please Check Here - Invalid Operation.";
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }


        #endregion
    }
}
