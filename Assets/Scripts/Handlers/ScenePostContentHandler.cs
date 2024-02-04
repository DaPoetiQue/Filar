using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.RedicalGames.Filar
{
    public class ScenePostContentHandler : AppData.SelectableWidgetComponent
    {
        #region Components

        public GameObject model;

        public AppData.Post post;

        [SerializeField]
        private List<SelectableSceneAssetHandler> selectableAssets = new List<SelectableSceneAssetHandler>();

        #endregion

        #region Main

        public void Initialize(AppData.SceneEventCamera eventCamera, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetSelectableAssets());

            if(callbackResults.Success())
            {
                callbackResults.SetResult(eventCamera.GetEventCamera());

                if(callbackResults.Success())
                {
                    foreach (var selectable in GetSelectableAssets().GetData())
                    {
                        selectable.SetEventCamera(eventCamera.GetEventCamera().GetData(), eventCameraSetCallbackResults => 
                        {
                            callbackResults.SetResult(eventCameraSetCallbackResults);
                        });

                        if(callbackResults.UnSuccessful())
                        {
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            break;
                        }
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }


        #region Data Setters

        public void SetContent(GameObject model) => this.model = model;
        public void SetPost(AppData.Post post) => this.post = post;

        public void ShowContent()
        {
            if(model.transform.childCount > 0)
                for (int i = 0; i < model.transform.childCount; i++)
                    model.transform.GetChild(i).gameObject.Show();

            model.Show();
        }


        public void HideContent()
        {
            if (model.transform.childCount > 0)
                for (int i = 0; i < model.transform.childCount; i++)
                    model.transform.GetChild(i).gameObject.Hide();

            model.Hide();
        }

        public void SetContentPose((Vector3 position, Vector3 scale, Quaternion rotation) pose)
        {
            model.transform.position = pose.position;
            model.transform.rotation = pose.rotation;
            model.transform.localScale = pose.scale;
        }

        public void SetContenLocaltPose((Vector3 position, Vector3 scale, Quaternion rotation) pose)
        {
            model.transform.localPosition = pose.position;
            model.transform.localRotation = pose.rotation;
            model.transform.localScale = pose.scale;
        }

        #endregion

        #region Data Getters

        public AppData.CallbackData<GameObject> GetModel()
        {
            var callbackResults = new AppData.CallbackData<GameObject>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(model, "Model", $"Get Model Failed - Model Value For : {GetName()} Is Null - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Model Success - Model Value For : {GetName()} Has Been Successfully Initialized.";
                callbackResults.data = model;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.Post> GetPost()
        {
            var callbackResults = new AppData.CallbackData<AppData.Post>(AppData.Helpers.GetAppComponentValid(post, "Post", $"Get Post Failed - There Is No Post Assigned For Scene Post Content : {GetName()}"));

            if (callbackResults.Success())
                callbackResults.data = post;
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackDataList<SelectableSceneAssetHandler> GetSelectableAssets()
        {
            var callbackResults = new AppData.CallbackDataList<SelectableSceneAssetHandler>(GetModel());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(selectableAssets, "Selectable Assets", $"Get Selectable Assets Failed - There Are No Selectable Assets Found For : {GetName()} - Invalid Operation."));

                if (callbackResults.Success())
                {
                    callbackResults.result = $"Get Selectable Assets Success - {selectableAssets.Count} : Selectable Asset(s) Value For : {GetName()} Has Been Successfully Initialized.";
                    callbackResults.data = selectableAssets;
                }
                else
                {
                    if(GetModel().GetData().transform.childCount > 0)
                    {
                        for (int i = 0; i < GetModel().GetData().transform.childCount; i++)
                        {
                            var selectable = GetModel().GetData().transform.GetChild(i).GetComponent<SelectableSceneAssetHandler>();

                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(selectable, "Selectable", $"Get Selectable Assets Failed - Couldn't Find Selectable Component In Children For : {GetName()} At index : {i} - Invalid Operation."));

                            if(callbackResults.Success())
                            {
                                if (!selectableAssets.Contains(selectable))
                                {
                                    selectableAssets.Add(selectable);

                                    if (selectableAssets.Contains(selectable))
                                        callbackResults.result = $"Add Selectable Asset Success : {selectable.GetName()} have Been Added To Selectable Assets For : {GetName()}";
                                    else
                                    {
                                        callbackResults.result = $"Add Selectable Asset Failed - Selectable Asset : {selectable.GetName()} Couldn't Be Been Added To Selectable Assets For : {GetName()} - Invalid Operation.";
                                        callbackResults.resultCode = AppData.Helpers.ErrorCode;

                                        break;
                                    }
                                }
                                else
                                {
                                    callbackResults.result = $"Add Selectable Asset Failed - Selectable Asset : {selectable.GetName()} Already Exists In Selectable Assets For : {GetName()} - Invalid Operation.";
                                    callbackResults.resultCode = AppData.Helpers.WarningCode;

                                    break;
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }

                        if(callbackResults.Success())
                        {
                            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(selectableAssets, "Selectable Assets", $"Get Selectable Assets Failed - There Are No Selectable Assets Found For : {GetName()} - Invalid Operation."));

                            if (callbackResults.Success())
                            {
                                callbackResults.result = $"Get Selectable Assets Success - {selectableAssets.Count} : Selectable Asset(s) Value For : {GetName()} Has Been Successfully Initialized.";
                                callbackResults.data = selectableAssets;
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                    {
                        callbackResults.result = $"Get Selectable Assets Failed - There Are No Selectable Assets Found For : {GetName()} - {GetName()}'s Child Count IS : {GetModel().GetData().transform.childCount} - Invalid Operation.";
                        callbackResults.data = default;
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        protected override void OnBeginDragExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDragExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnEndDragExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPointerDownExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPointerUpExecuted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>> OnGetState()
        {
            return null;
        }

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>>> callback)
        {
         
        }

        protected override void OnScreenWidgetShownEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
            throw new NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.SelectableWidgetType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
    }
}