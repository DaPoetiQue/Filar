using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class TriggerEventNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.EventType eventType = AppData.EventType.None;

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

		public AppData.CallbackData<AppData.EventType> GetEventType()
		{
			var callbackResults = new AppData.CallbackData<AppData.EventType>(AppData.Helpers.GetAppEnumValueValid(eventType, "Event Type", $"Get Event Type Failed - Event Type Is Set To Default : {eventType} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Event Type Success - Event Type Is Set To : {eventType}.";
				callbackResults.data = eventType;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Trigger Event Node.";
			callbackResults.data = AppData.GraphNodeType.TriggerEventNode;
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