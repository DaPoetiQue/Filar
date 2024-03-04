using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class NotificationSystemManager : AppData.SingletonBaseComponent<NotificationSystemManager>
    {
        #region Components

        [SerializeField]
        List<AppData.NotificationWidget> notificationWidgetsList = new List<AppData.NotificationWidget>();

        [Space(5)]
        [SerializeField]
        List<AppData.UIScreenWidgetContainer> notificationWidgetContainersList = new List<AppData.UIScreenWidgetContainer>();

        [Space(5)]
        [SerializeField]
        AppData.ScreenBlurObject screenBlur = new AppData.ScreenBlurObject();

        AppData.Notification notification;
        AppData.NotificationWidget notificationWidget = new AppData.NotificationWidget();

        bool inProgress = false;
        bool onShow = false;
        float mountDistance = 0.01f;

        Coroutine notificationRoutine;

        #endregion

        #region Unity Callbacks

        void Update() => OnShowWidgetTransition();

        #endregion

        #region Main

        protected override void Init()
        {
            //screenBlur.Init(this, initializationCallback =>
            //{
            //    if (!AppData.Helpers.IsSuccessCode(initializationCallback.resultCode))
            //        LogError(initializationCallback.result, this, () => Init());
            //});
        }

        public void ShowNotification(AppData.Notification notification)
        {
            AppData.NotificationWidget widget = notificationWidgetsList.Find((x) =>x.screenType == notification.screenType && x.notificationType == notification.notificationType);

            if (widget != null)
            {
                AppData.UIScreenWidgetContainer container = notificationWidgetContainersList.Find((container) => container.screenType == notification.screenType && container.notificationType == notification.notificationType);      

                if (container != null)
                {
                    if (container.value != null)
                    {
                        LogInfo($"Found Container : {container.name} - Value Name : {container.value.name} - Type : {container.notificationType} - Screen : {container.screenType}");

                        if (container.screenType == notification.screenType && container.screenPosition == notification.screenPosition)
                        {
                            this.notification = notification;
                            widget.SetNotificationScreenData(notification, container);
                            notificationWidget = widget;

                            if (notificationRoutine != null)
                            {
                                StopCoroutine(notificationRoutine);
                                notificationRoutine = null;
                            }

                            AppData.SceneConfigDataPacket dataPackets = new AppData.SceneConfigDataPacket();

                            dataPackets.SetScreenBlurState(notification.blurScreen);
                            dataPackets.SetReferencedUIScreenPlacementType(notification.blurLayer);

                            notificationRoutine = StartCoroutine(OnExecuteNotificationState(true, dataPackets));
                        }
                    }
                    else
                        LogWarning($"Show Notification Failed - No Value Assigned On Container : {container.name} For Notification Type : {notification.notificationType}", this);
                }
                else
                    LogWarning($"Container Of Type : {notification.notificationType} Not Found For Screen : {notification.screenType}", this);
            }
            else
                LogWarning($"Widget : {notification.notificationType} Not Found.", this, () => ShowNotification(notification));
        }

        public void ScheduleNotification(AppData.Notification notification)
        {
            AppData.ICommand notificatinCommand = new AppData.NotificationCommand(notification);
            notificatinCommand.Execute();
        }

        void OnShowWidgetTransition()
        {
            if (inProgress)
            {
                notificationWidget.GetTransform().anchoredPosition = (onShow) ? Vector2.Lerp(notificationWidget.GetTransform().anchoredPosition,
                    notificationWidget.GetVisibleScreenMountPoint().anchoredPosition,
                    AppDatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.NotificationTransitionalSpeed).value * Time.smoothDeltaTime) :
                    Vector2.Lerp(notificationWidget.GetTransform().anchoredPosition,
                    notificationWidget.GetHiddenScreenMountPoint().anchoredPosition,
                    AppDatabaseManager.Instance.GetDefaultExecutionValue(AppData.RuntimeExecution.NotificationTransitionalSpeed).value * Time.smoothDeltaTime);

                float mountDistance = (onShow) ? (notificationWidget.GetTransform().anchoredPosition - notificationWidget.GetVisibleScreenMountPoint().anchoredPosition).sqrMagnitude :
                    (notificationWidget.GetTransform().anchoredPosition - notificationWidget.GetHiddenScreenMountPoint().anchoredPosition).sqrMagnitude;

                if (mountDistance < this.mountDistance)
                {
                    if (onShow)
                    {
                        if (notificationRoutine != null)
                        {
                            StopCoroutine(notificationRoutine);
                            notificationRoutine = null;
                        }

                        notificationRoutine = StartCoroutine(OnExecuteNotificationState(false));
                    }
                    else
                    {
                        if (notification.blurScreen)
                        {
                            AppData.SceneConfigDataPacket dataPackets = new AppData.SceneConfigDataPacket();

                            dataPackets.SetScreenBlurState(false);
                            dataPackets.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.Default);

                            if (ScreenUIManager.Instance != null)
                                ScreenUIManager.Instance.GetCurrentScreen().GetData().Blur(dataPackets.GetScreenBlurConfig().GetData());
                            else
                                LogError("Screen UI Manager Instance Is Not Yet Initialized.", this);
                        }

                        inProgress = false;
                    }
                }
            }
            else
                return;
        }

        IEnumerator OnExecuteNotificationState(bool onShow, AppData.SceneConfigDataPacket dataPackets = null)
        {
            if (onShow)
            {
                if (dataPackets != null)
                {
                    yield return new WaitForSecondsRealtime(notification.delay);

                    if (dataPackets.blurScreen)
                    {
                        if (ScreenUIManager.Instance != null)
                            ScreenUIManager.Instance.GetCurrentScreen().GetData().Blur(dataPackets.GetScreenBlurConfig().GetData());
                        else
                            LogError("Screen UI Manager Instance Is Not Yet Initialized.", this);
                    }

                    this.onShow = true;
                    inProgress = true;
                }
            }
            else
            {
                inProgress = false;
                this.onShow = false;

                yield return new WaitForSecondsRealtime(notification.duration);

                inProgress = true;
                StopCoroutine(notificationRoutine);
            }

            StopCoroutine(notificationRoutine);
        }

        #endregion
    }
}
