using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class ExitNode : BaseNode
	{

		#region Components

		[Input]
		public int input;

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

			callbackResults.result = "This Is An Exit Node.";
			callbackResults.data = AppData.GraphNodeType.ExitNode;
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