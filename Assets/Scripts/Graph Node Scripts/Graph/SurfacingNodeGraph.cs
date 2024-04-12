using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
    [CreateAssetMenu]
    public class SurfacingNodeGraph : NodeGraph
    {
        #region Components

        [SerializeField]
        private AppData.GraphType type = AppData.GraphType.None;

        [Space(5)]
        [SerializeField]
        private BaseNode currentNode = null;

        private BaseNode entryNode = null;
        private AppData.GraphEntryEventType entryEvent = AppData.GraphEntryEventType.None;
        private AppData.GraphMode graphMode = AppData.GraphMode.Once;
        private List<SurfacingNodeGraph> prerequisiteGraphs = new List<SurfacingNodeGraph>();

        [SerializeField]
        private bool completed;

        #endregion

        #region Main

        public void Config(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(nodes, "Node", $"Config Failed - There Are No Nodes Found For Graph : {name}"));

            if(callbackResults.Success())
            {
                foreach (BaseNode node in nodes)
                {
                    callbackResults.SetResult(node.GetNodeType());

                    if (callbackResults.Success())
                    {
                        if (node.GetNodeType().GetData() == AppData.GraphNodeType.EntryNode)
                        {
                            callbackResults.SetResult(SetCurrentNode(node));

                            if (callbackResults.Success())
                            {
                                completed = false;
                                break;
                            }    
                        }
                        else
                        {
                            callbackResults.result = $"Not An Entry Node - {node.GetNodeType().GetResult}";
                            callbackResults.resultCode = AppData.Helpers.WarningCode;
                        }
                    }
                }
            }

            callback?.Invoke(callbackResults);
        }

        public void SetCurrentNode(BaseNode currentNode, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(currentNode, "Current Node", "Set Current Node Failed - Current Node Parameter Value Is Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(currentNode.GetNodeType());

                if(callbackResults.Success())
                {
                    if(currentNode.GetNodeType().GetData() == AppData.GraphNodeType.EntryNode)
                    {
                        var node = currentNode as EntryNode;

                        callbackResults.SetResult(node.GetEntryEventType());

                        if(callbackResults.Success())
                        {
                            entryEvent = node.GetEntryEventType().GetData();
                            graphMode = node.GetGraphMode().GetData();

                            callbackResults.SetResult(node.GetPrerequisiteGraphs());

                            if(callbackResults.Success())
                                prerequisiteGraphs = node.GetPrerequisiteGraphs().GetData();
                            else
                            {
                                callbackResults.result = "There Are No Prerequisite Graphs Assigned - Successfully Continuing Execution";
                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                            }
                        }
                    }

                    this.currentNode = currentNode;
                    callbackResults.result = "Set Current Node Success - Current Node Has Benen Successfully Assigned.";
                }
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.Callback SetCurrentNode(BaseNode currentNode)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(currentNode, "Current Node", "Set Current Node Failed - Current Node Parameter Value Is Null - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(currentNode.GetNodeType());

                if (callbackResults.Success())
                {
                    if (currentNode.GetNodeType().GetData() == AppData.GraphNodeType.EntryNode)
                    {
                        var node = currentNode as EntryNode;

                        callbackResults.SetResult(node.GetEntryEventType());

                        if (callbackResults.Success())
                        {
                            entryEvent = node.GetEntryEventType().GetData();
                            graphMode = node.GetGraphMode().GetData();

                            callbackResults.SetResult(node.GetPrerequisiteGraphs());

                            if (callbackResults.Success())
                                prerequisiteGraphs = node.GetPrerequisiteGraphs().GetData();
                            else
                            {
                                callbackResults.result = "There Are No Prerequisite Graphs Assigned - Successfully Continuing Execution";
                                callbackResults.resultCode = AppData.Helpers.SuccessCode;
                            }
                        }
                    }

                    this.currentNode = currentNode;
                    callbackResults.result = "Set Current Node Success - Current Node Has Benen Successfully Assigned.";
                }
            }

            return callbackResults;
        }

        public AppData.CallbackData<BaseNode> GetCurrentNode()
        {
            var callbackResults = new AppData.CallbackData<BaseNode>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(currentNode, "Current Node", "Get Current Node Failed - Current Node Value Is Not Assigned - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = "Get Current Node Success - Current Node Value Is Assigned";
                callbackResults.data = currentNode;
            }

            return callbackResults;
        }

        public AppData.CallbackData<AppData.GraphEntryEventType> GetEntryEventType()
        {
            var callbackResults = new AppData.CallbackData<AppData.GraphEntryEventType>(AppData.Helpers.GetAppEnumValueValid(entryEvent, "Entry Event", $"Get Entry Event Failed - Entry Event Value Is Set To Default : {entryEvent} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Entry Event Success - Entry Event Value Is Set To : {entryEvent}";
                callbackResults.data = entryEvent;
            }

            return callbackResults;
        }

        public AppData.Callback Completed()
        {
            var callbackResults = new AppData.Callback();

            if(completed)
            {
                callbackResults.result = "This Graph Has Completed!";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "This Graph Has Not Completed!";
                callbackResults.resultCode = AppData.Helpers.WarningCode;
            }

            return callbackResults;
        }

        public AppData.Callback CheckPrerequisiteGraphs()
        {
            var callbackResults = new AppData.Callback(GetPrerequisiteGraphs());

            if(callbackResults.Success())
            {
                for (int i = 0; i < GetPrerequisiteGraphs().GetData().Count; i++)
                {
                    callbackResults.SetResult(GetPrerequisiteGraphs().GetData()[i].Completed());

                    if (callbackResults.UnSuccessful())
                        break;
                }
            }
            else
            {
                callbackResults.result = "There Are No Prerequisite Graphs - Continuing Execution.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }

            return callbackResults;
        }

        private AppData.CallbackDataList<SurfacingNodeGraph> GetPrerequisiteGraphs()
        {
            var callbackResults = new AppData.CallbackDataList<SurfacingNodeGraph>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(prerequisiteGraphs, "Prerequisite Graphs", "Get Prerequisite Graphs Failed - There Are No Prerequisite Graphs Assigned - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Prerequisite Graphs Success - There Are : {prerequisiteGraphs.Count} Prerequisite Graphs Found.";
                callbackResults.data = prerequisiteGraphs;
            }

            return callbackResults;
        }

        public void CompleteGraph(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPrerequisiteGraphs());

            if(callbackResults.Success())
            {
                completed = true;
                callbackResults.result = $"Graph : {name} Has Been Successfully Completed.";
            }

            callback?.Invoke(callbackResults);
        }

        public AppData.Callback CompleteGraph()
        {
            var callbackResults = new AppData.Callback(Completed());

            if (callbackResults.UnSuccessful())
            {
                completed = true;

                if (GetGraphMode().GetData() == AppData.GraphMode.Repeat)
                {
                    Reset(true, resetedCallbackResults => 
                    {
                        callbackResults.SetResult(resetedCallbackResults);

                        if(callbackResults.Success())
                            callbackResults.result = $"Graph : {name} Has Been Successfully Restarted.";
                    });
                }
                else
                    callbackResults.result = $"Graph : {name} Has Been Successfully Completed.";
            }
            else
                callbackResults.result = $"Graph : {name} Has Been Successfully Completed.";

            return callbackResults;
        }

        public AppData.Callback InProgress()
        {
            var callbackResults = new AppData.Callback(GetCurrentNode());

            if (callbackResults.Success())
            {
                if(GetCurrentNode().GetData().GetNodeType().GetData() != AppData.GraphNodeType.EntryNode)
                    callbackResults.result = $"Graph : {name} Is In Progress.";
                else
                {

                    callbackResults.result = $"Graph : {name} Is Not In Progress.";
                    callbackResults.resultCode = AppData.Helpers.WarningCode;
                }
            }

            return callbackResults;
        }

        public void Reset(bool resetComplition = false, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            Config(configCallbackResults => 
            {
                callbackResults.SetResult(configCallbackResults);

                if(callbackResults.Success())
                {
                    if(resetComplition)
                    {
                        callbackResults.SetResult(Completed());

                        if (callbackResults.Success())
                        {
                            completed = false;
                            callbackResults.result = $"Graph : {name} Has Been Successfully Reset.";
                        }
                    }
                }
            });

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.GraphMode> GetGraphMode()
        {
            var callbackResults = new AppData.CallbackData<AppData.GraphMode>(AppData.Helpers.GetAppEnumValueValid(graphMode, "Graph Mode", $"Get Graph Mode Failed - Graph Mode Is Set To Default : {graphMode} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Graph Mode Success - Get Graph Mode Is Set To : {graphMode}.";
                callbackResults.data = graphMode;
            }

            return callbackResults;
        }

        public new AppData.CallbackData<AppData.GraphType> GetType()
        {
            var callbackResults = new AppData.CallbackData<AppData.GraphType>(AppData.Helpers.GetAppEnumValueValid(type, "Type", $"Get Type Failed - Type Is Set To Default : {type} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Type Success - Type Is Set To : {type}.";
                callbackResults.data = type;
            }

            return callbackResults;
        }

        #endregion

    }
}