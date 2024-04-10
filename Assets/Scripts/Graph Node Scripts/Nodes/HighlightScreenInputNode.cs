using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class HighlightScreenInputNode : BaseNode
	{
		#region Components

		[SerializeField]
		private InputActionHandler referencedInput = null;

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

		public override AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
		{
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Highlight Screen Input Node.";
			callbackResults.data = AppData.GraphNodeType.HighlightScreenInputNode;
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