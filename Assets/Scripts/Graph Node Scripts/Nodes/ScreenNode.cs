using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class ScreenNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.ScreenType screen = AppData.ScreenType.None;

		[Space(5)]
		[SerializeField]
		private AppData.UIVisibilityStateEvent state = AppData.UIVisibilityStateEvent.None;

		[Input]
		public int input;

		[Output]
		public int output;

		#endregion

		#region Main

		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}

		public AppData.CallbackData<AppData.ScreenType> GetScreenType()
		{
			var callbackResults = new AppData.CallbackData<AppData.ScreenType>(AppData.Helpers.GetAppEnumValueValid(screen, "Screen Type", $"Get Screen Type Failed - Screen Type Is Set To Default : {screen} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Screen Type Success - Screen Type Is Set To : {screen}.";
				callbackResults.data = screen;
			}

			return callbackResults;
		}

		public AppData.CallbackData<AppData.UIVisibilityStateEvent> GetState()
		{
			var callbackResults = new AppData.CallbackData<AppData.UIVisibilityStateEvent>(AppData.Helpers.GetAppEnumValueValid(state, "State", $"Get State Failed - State Is Set To Default : {state} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get State Success - State Is Set To : {state}.";
				callbackResults.data = state;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Screen Node.";
			callbackResults.data = AppData.GraphNodeType.ScreenNode;
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