using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class WidgetNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.WidgetType widget = AppData.WidgetType.None;

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

		public AppData.CallbackData<AppData.WidgetType> GetWidgetType()
		{
			var callbackResults = new AppData.CallbackData<AppData.WidgetType>(AppData.Helpers.GetAppEnumValueValid(widget, "Widget Type", $"Get Widget Type Failed - Widget Type Is Set To Default : {widget} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Widget Type Success - Widget Type Is Set To : {widget}.";
				callbackResults.data = widget;
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

			callbackResults.result = "This Is A Widget Node.";
			callbackResults.data = AppData.GraphNodeType.WidgetNode;
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