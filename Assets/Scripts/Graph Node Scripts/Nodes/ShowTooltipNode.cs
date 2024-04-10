using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class ShowTooltipNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.TooltipTemplateType tooltipTemplate = AppData.TooltipTemplateType.None;

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

		public AppData.CallbackData<AppData.TooltipTemplateType> GetTooltipTemplateType()
		{
			var callbackResults = new AppData.CallbackData<AppData.TooltipTemplateType>(AppData.Helpers.GetAppEnumValueValid(tooltipTemplate, "Tooltip Template Type", $"Get Tooltip Template Type Failed - Tooltip Template Type Is Set To Default : {tooltipTemplate} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Tooltip Templatet Type Success - Tooltip Template Type Is Set To : {tooltipTemplate}.";
				callbackResults.data = tooltipTemplate;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Show Tooltip Node.";
			callbackResults.data = AppData.GraphNodeType.ShowTooltipNode;
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