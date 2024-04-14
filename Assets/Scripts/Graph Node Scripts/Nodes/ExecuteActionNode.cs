using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
    public class ExecuteActionNode : BaseNode
    {
		#region Components

		[SerializeField]
		private AppData.ExecutiveActionType action = AppData.ExecutiveActionType.None;

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

		public AppData.CallbackData<AppData.ExecutiveActionType> GetAction()
		{
			var callbackResults = new AppData.CallbackData<AppData.ExecutiveActionType>(AppData.Helpers.GetAppEnumValueValid(action, "Action", $"Get Action Failed - Action Is Set To Default : {action} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Action Success - Action Is Set To : {action}.";
				callbackResults.data = action;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is An Execute Action Node.";
			callbackResults.data = AppData.GraphNodeType.ExecuteActionNode;
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
