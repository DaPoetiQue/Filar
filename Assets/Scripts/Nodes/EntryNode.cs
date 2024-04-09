using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class EntryNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.GraphEntryEventType entryEvent = AppData.GraphEntryEventType.None;

		[Space(5)]
		[SerializeField]
		private List<SurfacingNodeGraph> prerequisiteGraphs = new List<SurfacingNodeGraph>();

		[Space(5)]
		[SerializeField]
		private AppData.GraphMode graphMode = AppData.GraphMode.Once;

		[Output]
		public int output;

		#endregion

		#region Main

		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}

        public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
        {
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is An Entry Node.";
			callbackResults.data = AppData.GraphNodeType.EntryNode;
			callbackResults.resultCode = AppData.Helpers.SuccessCode;

			return callbackResults;
		}

        public AppData.CallbackData<AppData.GraphEntryEventType> GetEntryEventType()
        {
			var callbackResults = new AppData.CallbackData<AppData.GraphEntryEventType>(AppData.Helpers.GetAppEnumValueValid(entryEvent, "Entry Event", $"Get Entry Event Type Failed - Entry Event Type Is Set To Default : {entryEvent} - Invalid Operation."));

			if(callbackResults.Success())
            {
				callbackResults.result = $"Get Entry Event Type Success - Entry Event Type Is Set To : {entryEvent}.";
				callbackResults.data = entryEvent;
			}

			return callbackResults;
		}

		public AppData.CallbackDataList<SurfacingNodeGraph> GetPrerequisiteGraphs()
		{
			var callbackResults = new AppData.CallbackDataList<SurfacingNodeGraph>();

			callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(prerequisiteGraphs, "Prerequisite Graphs", $"Get Prerequisite Graphs Failed - There Are No Prerequisite Graphs Assigned - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Prerequisite Graphs Success - There Are : {prerequisiteGraphs.Count} Prerequisite Graphs Assigned.";
				callbackResults.data = prerequisiteGraphs;
			}

			return callbackResults;
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

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}

		#endregion
	}
}