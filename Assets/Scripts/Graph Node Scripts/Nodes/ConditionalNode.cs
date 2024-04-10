using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class ConditionalNode : BaseNode
	{
		#region Components

		[SerializeField]
		private AppData.AppExecutionalConditionType condition = AppData.AppExecutionalConditionType.None;

		[Input]
		public int input;

		[Output]
		public int isTrue;

		[Output]
		public int isFalse;

		#endregion

		#region Main

		// Use this for initialization
		protected override void Init()
		{
			base.Init();

		}

		public AppData.CallbackData<AppData.AppExecutionalConditionType> GetCondition()
		{
			var callbackResults = new AppData.CallbackData<AppData.AppExecutionalConditionType>(AppData.Helpers.GetAppEnumValueValid(condition, "Condition", $"Get Condition Failed - Condition Is Set To Default : {condition} - Invalid Operation."));

			if (callbackResults.Success())
			{
				callbackResults.result = $"Get Condition Success - Condition Is Set To : {condition}.";
				callbackResults.data = condition;
			}

			return callbackResults;
		}

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Conditional Node.";
			callbackResults.data = AppData.GraphNodeType.ConditionalNode;
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