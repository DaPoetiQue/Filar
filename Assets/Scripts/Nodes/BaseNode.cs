using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Com.RedicalGames.Filar
{
	public class BaseNode : Node
	{
        #region Components

        #endregion

        #region Main

        // Use this for initialization
        protected override void Init()
		{
			base.Init();

		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}

		public virtual AppData.CallbackData<AppData.GraphNodeType> GetNodeType()
        {
			var callbackResults = new AppData.CallbackData<AppData.GraphNodeType>();

			callbackResults.result = "This Is A Base Node.";
			callbackResults.data = default;
			callbackResults.resultCode = AppData.Helpers.WarningCode;

			return callbackResults;
		}

        #endregion
    }
}