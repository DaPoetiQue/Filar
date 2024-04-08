using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class WaitForButtonEventNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.InputActionButtonType eventType = AppData.InputActionButtonType.None;

		[Input]
		public int input;

		[Output]
		public int output;

		#endregion

		#region Main

		public AppData.CallbackData<AppData.InputActionButtonType> GetButtonEventType()
		{
			var callbackResults = new AppData.CallbackData<AppData.InputActionButtonType>(AppData.Helpers.GetAppEnumValueValid(eventType, "Button Event Type", $"Get Button Event Type Failed - Button Event Type Is Set To Default : {eventType} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Button Event Type Success - Button Event Type Is Set To : {eventType}.";
				callbackResults.data = eventType;
			}

			return callbackResults;
		}

		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}


		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Wait For Button Event Node.";
			callbackResults.data = AppData.GraphNodeType.WaitForButtonEventNode;
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