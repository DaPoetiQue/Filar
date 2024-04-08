using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class CurrentScreenNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.ScreenType screen = AppData.ScreenType.None;


		[Output]
		public int output;

		#endregion

		#region Main


		public AppData.CallbackData<AppData.ScreenType> GetCurrentScreenType()
		{
			var callbackResults = new AppData.CallbackData<AppData.ScreenType>(AppData.Helpers.GetAppEnumValueValid(screen, "Screen Type", $"Get Current Screen Type Failed - Current Screen Type Is Set To Default : {screen} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Current Screen Type Success - Current Screen Type Is Set To : {screen}.";
				callbackResults.data = screen;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Current Screen Node.";
			callbackResults.data = AppData.GraphNodeType.CurrentScreenNode;
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