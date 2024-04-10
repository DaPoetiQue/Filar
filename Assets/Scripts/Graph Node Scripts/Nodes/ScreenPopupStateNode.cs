using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class ScreenPopupStateNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.PopupTemplateType popupTemplate = AppData.PopupTemplateType.None;

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

		public AppData.CallbackData<AppData.PopupTemplateType> GetPopupTemplateType()
		{
			var callbackResults = new AppData.CallbackData<AppData.PopupTemplateType>(AppData.Helpers.GetAppEnumValueValid(popupTemplate, "Surfacing Template Type", $"Get Surfacing Template Type Failed - Surfacing Template Type Is Set To Default : {popupTemplate} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Surfacing Templatet Type Success - Surfacing Template Type Is Set To : {popupTemplate}.";
				callbackResults.data = popupTemplate;
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

			callbackResults.result = "This Is A Screen Popup State Node.";
			callbackResults.data = AppData.GraphNodeType.ScreenPopupStateNode;
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