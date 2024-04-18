using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
    public class LoadingSequenceNode : BaseNode
    {
		#region Components

		[SerializeField]
		private List<AppData.LoadingSequenceState> sequences = new List<AppData.LoadingSequenceState>();

		[Input]
		public int input;

		[Output]
		public int successCode;

		[Output]
		public int errorCode;

		#endregion

		#region Main

		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}

		public AppData.CallbackDataList<AppData.LoadingSequenceState> GetSequences()
        {
			var callbackResults = new AppData.CallbackDataList<AppData.LoadingSequenceState>(AppData.Helpers.GetAppEnumValuesValid(sequences, "Sequences", "Get Sequences Failed - There Are No Sequences Assigned - Invalid Operation."));

			if(callbackResults.Success())
            {
				callbackResults.result = $"Get Sequences Success - {sequences.Count} Sequences Have Been Found.";
				callbackResults.data = sequences;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Loading Sequence Node.";
			callbackResults.data = AppData.GraphNodeType.LoadingSequenceNode;
			callbackResults.resultCode = AppData.Helpers.SuccessCode;

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
