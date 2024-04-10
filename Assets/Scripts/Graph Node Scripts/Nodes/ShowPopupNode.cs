using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class ShowPopupNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.PopupTemplateType popupTemplate = AppData.PopupTemplateType.None;

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

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Show Popup Node.";
			callbackResults.data = AppData.GraphNodeType.ShowPopupNode;
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