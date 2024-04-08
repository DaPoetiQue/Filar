using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class WaitForSecondsNode : BaseNode
	{
		#region Components

		[SerializeField]
		private float seconds = 0.0f;

		[Input]
		public int input;

		[Output]
		public int output;

		#endregion

		#region Main

		public AppData.CallbackData<float> GetWaitTime()
		{
			var callbackResults = new AppData.CallbackData<float>();

			if (seconds > 0.0f)
			{
				callbackResults.result = $"Get Wait Success - Wait Time Is Set To : {seconds}.";
				callbackResults.data = seconds;
				callbackResults.resultCode = AppData.Helpers.SuccessCode;
			}
			else
            {
				callbackResults.result = $"Get Wait Failed - Wait Time Is Set To Default : {seconds} - Invalid Operation.";
				callbackResults.data = default;
				callbackResults.resultCode = AppData.Helpers.WarningCode;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Wait For Seconds Event Node.";
			callbackResults.data = AppData.GraphNodeType.WaitForSecondsNode;
			callbackResults.resultCode = AppData.Helpers.SuccessCode;

			return callbackResults;
		}

		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}

		#endregion
	}
}