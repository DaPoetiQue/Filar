using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class NotificationSystemManager : AppMonoBaseClass
    {
        #region Static Instance

        static NotificationSystemManager _instance;

        public static NotificationSystemManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<NotificationSystemManager>();

                return _instance;
            }
        }

        #endregion

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

        void Start() => Init();

        void Update() => OnShowWidgetTransition();

        #endregion

        #region Main

        void Init()
        {
            screenBlur.Init(this, initializationCallback => 
            {
                if (!AppData.Helpers.IsSuccessCode(initializationCallback.resultsCode))
                    LogError(initializationCallback.results, this, () => Init());
            });
        }

        public void ShowNotification(AppData.Notification notification)
        {
            AppData.NotificationWidget widget = notificationWidgetsList.Find((x) => x.notificationType == notification.notificationType);

            if (widget != null)
            {
                AppData.UIScreenWidgetContainer container = notificationWidgetContainersList.Find((container) => container.notificationType == notification.notificationType);

                if (container.value != null)
                {
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

                        notificationRoutine = StartCoroutine(OnExecuteNotificationState(true));
                    }
                    else
                        LogWarning($"Show Notification Failed - No Container Found For Notification Type : {notification.notificationType}", this, () => ShowNotification(notification));
                }
                else
                    LogWarning("Container Not Found", this, () => ShowNotification(notification));
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
                    SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.NotificationTransitionalSpeed).value * Time.smoothDeltaTime) :
                    Vector2.Lerp(notificationWidget.GetTransform().anchoredPosition,
                    notificationWidget.GetHiddenScreenMountPoint().anchoredPosition,
                    SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.NotificationTransitionalSpeed).value * Time.smoothDeltaTime);

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
                            screenBlur.Hide(true, true);

                        inProgress = false;
                    }
                }
            }
            else
                return;
        }

        IEnumerator OnExecuteNotificationState(bool onShow)
        {
            if (onShow)
            {
                yield return new WaitForSecondsRealtime(notification.delay);

                if (notification.blurScreen)
                    screenBlur.Show(AppData.ScreenBlurContainerLayerType.Default, true);

                this.onShow = true;
                inProgress = true;
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
