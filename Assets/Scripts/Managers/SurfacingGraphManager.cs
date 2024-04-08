using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class SurfacingGraphManager : AppData.SingletonBaseComponent<SurfacingGraphManager>
    {
        #region Components

        [Space(5)]
        [SerializeField]
        private List<SurfacingNodeGraph> graphs = new List<SurfacingNodeGraph>();

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
                    ConfigureGraphs(graphsConfiguredCallbackResults =>
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
                    var entryGraphs = GetGraphs().GetData().FindAll(graph => graph.GetCurrentNode().Success()).Where(graph => graph.GetEntryEventType().GetData() == entry && graph.CheckPrerequisiteGraphs().Success() && graph.Completed().UnSuccessful()).ToList();

                    callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(entryGraphs, "Entry Graphs", $"On Graph Entry Failed - There Are No Entry Graphs Found - Invalid Operation."));

                    if(callbackResults.Success())
                    {
                        for (int i = 0; i < entryGraphs.Count; i++)
                        {
                            LogInfo($"Logging_Cats:// Triggered Node {entryGraphs[i].name} With Entry Event : {entry}", this);
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

        private void ConfigureGraphs(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetGraphs());

            if(callbackResults.Success())
            {
                for (int i = 0; i < GetGraphs().GetData().Count; i++)
                {
                    if (GetGraphs().GetData()[i].nodes.Count > 0)
                    {
                        foreach (BaseNode node in GetGraphs().GetData()[i].nodes)
                        {
                            callbackResults.SetResult(node.GetNodeType());

                            if(callbackResults.Success())
                            {
                                if (node.GetNodeType().GetData() == AppData.GraphNodeType.EntryNode)
                                {
                                    callbackResults.SetResult(GetGraphs().GetData()[i].SetCurrentNode(node));

                                    if (callbackResults.Success())
                                        break;
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                }
                                else
                                {
                                    callbackResults.result = $"Not An Entry Node - {node.GetNodeType().GetResult}";
                                    callbackResults.resultCode = AppData.Helpers.WarningCode;
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                    }
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

        #endregion
    }
}
