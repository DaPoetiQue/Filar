using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class DynamicWidgetsContainer : AppMonoBaseClass
    {

        #region Components

        [SerializeField]
        AppData.ContentContainerType containerType;

        [Space(5)]
        [SerializeField]
        AppData.UIScreenType screenType;

        [Space(5)]
        [SerializeField]
        AppData.OrientationType orientation;

        [Space(5)]
        [SerializeField]
        bool selectableContentContainer;

        [Space(5)]
        [SerializeField]
        List<AppData.UILayoutDimensions> widgetsUILayoutDimensionList = new List<AppData.UILayoutDimensions>();

        [Space(5)]
        [SerializeField]
        AppData.UIScroller<AppData.SceneDataPackets> scroller;

        [Space(5)]
        [SerializeField]
        FolderEmptyContentWidgetHandler emptyFolderWidget = null;

        [Space(5)]
        [SerializeField]
        RectTransform itemDragContainer = null;

        [Space(5)]
        [SerializeField]
        bool snapDraggedWidgetToHoveredFolder = false;

        [Space(5)]
        [SerializeField]
        AppData.WidgetPlaceHolder placeHolder;

        [Space(5)]
        [SerializeField]
        AppData.PaginationComponent paginationComponent = new AppData.PaginationComponent();


        [Space(5)]
        [SerializeField]
        bool assetsLoaded = false;

        AppData.PaginationViewType currentPaginationView;

        RectTransform container;

        GridLayoutGroup layoutComponent;

        [SerializeField]
        AppData.FolderLayoutView layout = new AppData.FolderLayoutView();

        AppData.ScreenBounds screenBounds = new AppData.ScreenBounds();

        bool transitionToFocusPosition = false;
        Vector2 currentFocusPosition = Vector2.zero;

        bool scrollToTop = false;
        bool scrollToBottom = false;

        bool edgeScrolling = false;
        AppData.DirectionType edgeScrollDirection;

        bool isFingerDown = false;

        [SerializeField]
        bool canFadeInScrollBar = false;

        [SerializeField]
        bool canFadeOutScrollBar = false;

        float currentScrollBarFadeDelayDuration = 0.0f;

        bool containerRefreshed = false;

        Coroutine containerUpdateRoutine;
        Coroutine focusedSelectionStateRoutine;

        //Queue<AppData.UIWidgetInfo> focusedWidget = new Queue<AppData.UIWidgetInfo>();
        //Queue<AppData.UIWidgetInfo> currentFocusedWidget = new Queue<AppData.UIWidgetInfo>();

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        void OnEnable() => ActionEventsSubscriptions(true);

        void OnDisable() => ActionEventsSubscriptions(false);

        private void Update() => UIBaseUpdate();

        #endregion

        #region Main

        void Init() => InitializeContainer();

        void ActionEventsSubscriptions(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnAssetDeleteRefresh += OnAssetDeleteRefreshEvent;
                AppData.ActionEvents._OnScrollAndFocusToSelectionEvent += ActionEvents__ScrollAndFocusToSelectionEvent;
                AppData.ActionEvents._OnNavigateAndFocusToSelectionEvent += ActionEvents__OnNavigateAndFocusToSelectionEvent;
            }
            else
            {
                AppData.ActionEvents._OnAssetDeleteRefresh -= OnAssetDeleteRefreshEvent;
                AppData.ActionEvents._OnScrollAndFocusToSelectionEvent -= ActionEvents__ScrollAndFocusToSelectionEvent;
                AppData.ActionEvents._OnNavigateAndFocusToSelectionEvent -= ActionEvents__OnNavigateAndFocusToSelectionEvent;
            }

            ClearWidgets();
        }

        private void ActionEvents__OnNavigateAndFocusToSelectionEvent(string widgetName) => OnFocusToSelection(widgetName);

        private void ActionEvents__ScrollAndFocusToSelectionEvent(Vector2 position, bool transition) => OnFocusToSelection(position, transition);

        public void InitializeContainer()
        {
            container = GetComponent<RectTransform>();
            layoutComponent = GetComponent<GridLayoutGroup>();

            Vector2 containerSize = container.sizeDelta;

            switch (orientation)
            {
                case AppData.OrientationType.Vertical:

                    containerSize.y = 0.0f;

                    break;

                case AppData.OrientationType.Horizontal:

                    containerSize.x = 0.0f;

                    break;
            }

            container.sizeDelta = containerSize;

            scroller.Initialized(scrollerInitializedCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(scrollerInitializedCallback.resultsCode))
                {
                    if (scroller.GetFadeUIScrollBar())
                        scroller.GetUIScrollBarComponent().SetVisibilityState(AppData.UIWidgetVisibilityState.Hidden);
                }
                else
                    LogWarning(scrollerInitializedCallback.results, this, () => InitializeContainer());
            });
        }

        public void ScrollToTop()
        {
            scrollToBottom = false;
            scrollToTop = true;
        }

        public void ScrollToBottom()
        {
            scrollToTop = false;
            scrollToBottom = true;
        }

        public void SetFingerDown(bool isFingerDown) => this.isFingerDown = isFingerDown;

        public bool SnapDraggedWidgetToHoveredFolder()
        {
            return snapDraggedWidgetToHoveredFolder;
        }

        public void UpdateWidgetsList(List<AppData.UIScreenWidget> widgets)
        {
            if (widgets != null)
            {
                for (int i = 0; i < widgets.Count; i++)
                {

                }
            }
            else
                Debug.LogWarning("--> UpdateWidgetsList Failed : No Widgets Found.");
        }

        public AppData.UIScreenType GetUIScreenType()
        {
            return screenType;
        }

        public RectTransform GetItemDragContainer()
        {
            return itemDragContainer;
        }

        public RectTransform GetScrollerDragViewPort()
        {
            return scroller.GetDragViewPort();
        }

        public AppData.UIScroller<AppData.SceneDataPackets> GetUIScroller()
        {
            return scroller;
        }

        void OnAssetDeleteRefreshEvent()
        {
            ClearWidgets(false, onScreenWigetsCleared =>
            {
                if (AppData.Helpers.IsSuccessCode(onScreenWigetsCleared.resultsCode))
                    InitializeContainer();
                else
                    Debug.LogWarning($"---> Clear Widgets Results : {onScreenWigetsCleared.results}.");
            });
        }

        #region On Update Functions

        void UIBaseUpdate()
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                return;

            OnContainerUpdate();
            OnEdgeScrollingUpdate();

            if (canFadeInScrollBar)
                OnScrollbarFadeUpdate();

            if (!isFingerDown)
            {
                OnScrollToTopUpdate();
                OnScrollToBottomUpdate();
            }
            else
            {
                scrollToTop = false;
                scrollToBottom = false;
            }
        }

        public void InterruptAutoScrollerProccess()
        {
            scrollToTop = false;
            scrollToBottom = false;
        }

        void OnScrollToTopUpdate()
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                return;

            if (!scrollToTop || scrollToBottom)
                return;

            if (scroller.value != null)
            {
                if (SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollToTopSpeedValue).value > 0)
                {
                    Vector2 scrollPosition = Vector2.Lerp(scroller.value.content.localPosition, Vector2.zero, (SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollToTopSpeedValue).value / (GetContentCount() / (GetContentCount() / 2))) * Time.smoothDeltaTime);
                    scroller.value.content.localPosition = scrollPosition;

                    float distance = ((Vector2)scroller.value.content.localPosition - Vector2.zero).sqrMagnitude;

                    if (distance < 0.1f)
                    {
                        scrollToTop = false;
                        scrollToBottom = false;
                        scroller.value.content.localPosition = Vector2.zero;
                    }
                }
                else
                    Debug.LogWarning("--> OnScrollToTop Failed : ScrollToTopSpeedValue Is Not Assigned.");
            }
            else
                Debug.LogWarning("--> OnScrollToTop Failed : Scroller Value Missing / Null.");
        }

        void OnScrollToBottomUpdate()
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                return;

            if (!scrollToBottom || scrollToTop)
                return;

            if (scroller.value != null)
            {
                if (SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollToTopSpeedValue).value > 0)
                {
                    Vector2 scrollPosition = Vector2.Lerp(scroller.value.content.localPosition, container.sizeDelta, (SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollToTopSpeedValue).value / (GetContentCount() / 2)) * Time.smoothDeltaTime);
                    scroller.value.content.localPosition = scrollPosition;

                    float distance = (orientation == AppData.OrientationType.Vertical) ? scroller.value.verticalNormalizedPosition : scroller.value.horizontalNormalizedPosition;

                    if (distance <= 0.0f)
                    {
                        scrollToTop = false;
                        scrollToBottom = false;
                    }
                }
                else
                    Debug.LogWarning("--> OnScrollToTop Failed : ScrollToTopSpeedValue Is Not Assigned.");
            }
            else
                Debug.LogWarning("--> OnScrollToTop Failed : Scroller Value Missing / Null.");
        }

        public void OnSnapToBottom()
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                return;

            if (SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollToTopSpeedValue).value > 0)
                scroller.value.content.localPosition = container.sizeDelta;
        }

        void OnContainerUpdate()
        {
            if (transitionToFocusPosition)
            {
                if (scroller.value != null)
                {
                    float scrollDistance = ((Vector2)scroller.value.content.localPosition - currentFocusPosition).sqrMagnitude;

                    if (scrollDistance > 0.5f)
                    {
                        #region Vertical Scroller

                        if (orientation == AppData.OrientationType.Vertical)
                        {
                            if (scroller.value.normalizedPosition.y > 0.0f && scroller.value.normalizedPosition.y <= 1.0f)
                            {
                                Vector2 focusedPosition = Vector2.Lerp(scroller.value.content.localPosition, currentFocusPosition, SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollToFocusedPositionSpeedValue).value * Time.smoothDeltaTime);
                                scroller.value.content.localPosition = focusedPosition;

                                float distance = ((Vector2)scroller.value.content.localPosition - currentFocusPosition).sqrMagnitude;

                                if (distance < 0.1f)
                                {
                                    transitionToFocusPosition = false;
                                    scroller.value.content.localPosition = focusedPosition;
                                }
                            }
                            else
                                transitionToFocusPosition = false;
                        }

                        #endregion

                        #region Horizontal Scroller

                        if (orientation == AppData.OrientationType.Horizontal)
                        {
                            if (scroller.value.normalizedPosition.x > 0.0f && scroller.value.normalizedPosition.x <= 1.0f)
                            {
                                Vector2 focusedPosition = Vector2.Lerp(scroller.value.content.localPosition, currentFocusPosition, SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollToFocusedPositionSpeedValue).value * Time.smoothDeltaTime);
                                scroller.value.content.localPosition = focusedPosition;

                                float distance = ((Vector2)scroller.value.content.localPosition - currentFocusPosition).sqrMagnitude;

                                if (distance < 0.1f)
                                {
                                    transitionToFocusPosition = false;
                                    scroller.value.content.localPosition = focusedPosition;
                                }
                            }
                            else
                                transitionToFocusPosition = false;
                        }

                        #endregion
                    }
                    else
                        transitionToFocusPosition = false;
                }
                else
                    Debug.LogWarning("--> OnContainerUpdate Failed : Scroller Value Missing / Null.");
            }
            else
                return;
        }

        void OnEdgeScrollingUpdate()
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                return;

            if (edgeScrolling)
            {
                if (scroller.value != null)
                {
                    if (edgeScrollDirection == AppData.DirectionType.Up)
                    {
                        if (scroller.value.normalizedPosition.y <= 1.0f)
                        {
                            Vector2 scrollPosition = Vector2.Lerp(scroller.value.content.localPosition, Vector2.zero, SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.EdgeScrollSpeedValue).value * Time.smoothDeltaTime);
                            scroller.value.content.localPosition = scrollPosition;

                            float distance = ((Vector2)scroller.value.content.localPosition - Vector2.zero).sqrMagnitude;

                            if (distance < 0.1f)
                            {
                                edgeScrolling = false;
                                scroller.value.content.localPosition = Vector2.zero;
                            }
                        }
                    }

                    if (edgeScrollDirection == AppData.DirectionType.Down)
                    {
                        if (scroller.value.normalizedPosition.y > 0.0f)
                        {
                            Vector2 targetPosition = new Vector2(0, 1000);
                            Vector2 scrollPosition = Vector2.Lerp(scroller.value.content.localPosition, targetPosition, (SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.EdgeScrollSpeedValue).value / 3) * Time.smoothDeltaTime);
                            scroller.value.content.localPosition = scrollPosition;

                            float distance = ((Vector2)scroller.value.content.localPosition - targetPosition).sqrMagnitude;

                            if (distance < 0.1f)
                            {
                                edgeScrolling = false;
                                scroller.value.content.localPosition = targetPosition;
                            }
                        }
                    }
                }
                else
                    Debug.LogWarning("--> OnContainerUpdate Failed : Scroller Value Missing / Null.");
            }
            else
                return;
        }

        void OnScrollbarFadeUpdate()
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                return;

            if (scroller.IsScrollBarEnabled())
            {
                if (scroller.GetFadeUIScrollBar())
                {
                    if (scroller.GetFadeUIScrollBar())
                    {
                        if (canFadeInScrollBar)
                            scroller.OnScrollbarFadeIn();
                        else
                            return;

                        if (canFadeOutScrollBar)
                        {
                            if (currentScrollBarFadeDelayDuration < SceneAssetsManager.Instance.GetDefaultExecutionValue(AppData.RuntimeValueType.ScrollBarFadeDelayDuration).value)
                                currentScrollBarFadeDelayDuration += 1.0f * Time.smoothDeltaTime;
                            else
                                scroller.OnScrollbarFadeOut();
                        }
                        else
                            return;
                    }
                }
                else
                    return;
            }
            else
                return;
        }

        #endregion

        public void SetFingerDragEvent()
        {
            if (scroller.IsScrollBarEnabled())
            {
                if (scroller.GetFadeUIScrollBar())
                {
                    scroller.GetUIScrollBarComponent().GetScrollbarFaderAlphaValue(scrollBarFaderCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(scrollBarFaderCallback.resultsCode))
                        {
                            if (scrollBarFaderCallback.data != 1.0f)
                            {
                                scroller.GetUIScrollBarComponent().SetIsFading();
                                canFadeOutScrollBar = false;
                                canFadeInScrollBar = true;
                            }
                        }
                        else
                            Debug.LogWarning($"--> SetFingerDragEvent's GetScrollbarFaderAlphaValue Failed With Results : {scrollBarFaderCallback.results}");
                    });
                }
                else
                    return;
            }
            else
                return;
        }

        public void SetFingerUpEvent()
        {
            if (scroller.IsScrollBarEnabled())
            {
                if (scroller.GetFadeUIScrollBar())
                {
                    scroller.GetUIScrollBarComponent().GetScrollbarFaderAlphaValue(scrollBarFaderCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(scrollBarFaderCallback.resultsCode))
                        {
                            if (scrollBarFaderCallback.data != 0.0f)
                            {
                                scroller.GetUIScrollBarComponent().SetIsFading();
                                currentScrollBarFadeDelayDuration = 0.0f;
                                canFadeInScrollBar = false;
                                canFadeOutScrollBar = true;
                            }
                        }
                        else
                            Debug.LogWarning($"--> SetFingerUpEvent's GetScrollbarFaderAlphaValue Failed With Results : {scrollBarFaderCallback.results}");
                    });
                }
                else
                    Debug.LogError("==> Finger Up Failed : GetFadeUIScrollBar Not True");
            }
            else
                Debug.LogError("==> Finger Up Failed : IsScrollBarEnabled Not True");
        }

        public void ScrollerPosition(Vector2 position)
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                return;

            UpdateAutoScrollWidget();

            //if (isFingerDown)
            //    scroller.OnScrollbarFadeIn();
            //else
            //    scroller.OnScrollbarFadeOut();

            //if (orientation == AppData.OrientationType.Vertical)
            //{
            //     if (ScreenUIManager.Instance != null)
            //     {
            //         if (position.y < ((layout.viewType == AppData.LayoutViewType.ItemView)? showScrollUpButtonDistanceItemView : showScrollUpButtonDistanceListView))
            //             ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Shown);
            //         else
            //             ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Hidden);
            //     }
            //     else
            //         Debug.LogWarning("--> ScrollerPosition Failed : Is Not Yet Initialized.");
            // }

            // if (orientation == AppData.OrientationType.Horizontal)
            // {
            //     if (ScreenUIManager.Instance != null)
            //     {
            //         if (position.x < ((layout.viewType == AppData.LayoutViewType.ItemView) ? showScrollUpButtonDistanceItemView : showScrollUpButtonDistanceListView))
            //             ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Shown);
            //         else
            //             ScreenUIManager.Instance.GetCurrentScreenData().value.SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Hidden);
            //     }
            //     else
            //         Debug.LogWarning("--> ScrollerPosition Failed : Is Not Yet Initialized.");
            // }

            //if (scroller.enableScrollBar && scroller.fadeUIScrollBar)
            //    scroller.OnScrollbarFadeIn();
        }

        void UpdateAutoScrollWidget()
        {
            if (orientation == AppData.OrientationType.Vertical)
            {
                if (scroller.value.verticalNormalizedPosition == 1.0f)
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Disabled);
                else
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Enabled);

                if (scroller.value.verticalNormalizedPosition == 0.0f)
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToBottomButton, AppData.InputUIState.Disabled);
                else
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToBottomButton, AppData.InputUIState.Enabled);
            }

            if (orientation == AppData.OrientationType.Horizontal)
            {
                if (scroller.value.horizontalNormalizedPosition == 1.0f)
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Disabled);
                else
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToTopButton, AppData.InputUIState.Enabled);

                if (scroller.value.horizontalNormalizedPosition == 0.0f)
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToBottomButton, AppData.InputUIState.Disabled);
                else
                    ScreenUIManager.Instance.GetCurrentScreenData().value.GetWidget(AppData.WidgetType.ScrollerNavigationWidget).SetActionButtonState(AppData.InputActionButtonType.ScrollToBottomButton, AppData.InputUIState.Enabled);
            }
        }

        public void OnEdgetScrolling(AppData.DirectionType scrollDirection)
        {
            if (scrollDirection != AppData.DirectionType.Default)
            {
                edgeScrollDirection = scrollDirection;
                edgeScrolling = true;
            }
            else
                edgeScrolling = false;
        }

        public AppData.OrientationType GetContainerOrientation()
        {
            return orientation;
        }

        #region Remove This - Check, Has Something To Do With Ambushed Selection Data

        //public void SetFocusedWidgetInfo(AppData.UIWidgetInfo folderWidgetInfo)
        //{
        //    focusedWidget.Clear();

        //    if (focusedWidget.Count == 0)
        //        focusedWidget.Enqueue(folderWidgetInfo);
        //}

        //public bool HasCurrentFocusedWidgetInfo()
        //{
        //    return currentFocusedWidget.Count == 1;
        //}

        //public void SetCurrentFocusedWidgetInfo(AppData.UIWidgetInfo folderWidgetInfo)
        //{
        //    currentFocusedWidget.Clear();

        //    if (currentFocusedWidget.Count == 0)
        //        currentFocusedWidget.Enqueue(folderWidgetInfo);
        //}

        //public int GetFocusedWidgetsCount()
        //{
        //    // focusedWidget.Count
        //    return 0;
        //}

        //public void GetFocusedWidgetInfoData(Action<AppData.CallbackData<AppData.UIWidgetInfo>> callback)
        //{
        //    AppData.CallbackData<AppData.UIWidgetInfo> callbackResults = new AppData.CallbackData<AppData.UIWidgetInfo>();

        //    if (focusedWidget.Count > 0)
        //    {
        //        AppData.UIWidgetInfo infoData = focusedWidget.Dequeue();

        //        AppData.UIWidgetInfo newInfoData = new AppData.UIWidgetInfo
        //        {
        //            widgetName = infoData.widgetName,
        //            dimensions = infoData.dimensions,
        //            position = infoData.position,
        //            selectionState = infoData.selectionState
        //        };

        //        focusedWidget.Enqueue(newInfoData);

        //        callbackResults.data = infoData;
        //        callbackResults.results = $"GetFocusedWidgetInfo Success : Focused To Widget : {callbackResults.data.GetWidgetName()}.";
        //        callbackResults.success = true;
        //    }
        //    else
        //    {
        //        callbackResults.results = "GetFocusedWidgetInfo Failed : There Is No Focused Widget Assigned.";
        //        callbackResults.data = default;
        //        callbackResults.success = false;
        //    }

        //    callback?.Invoke(callbackResults);
        //}

        //public void GetFocusedWidgetInfo(Action<AppData.CallbackData<AppData.UIWidgetInfo>> callback)
        //{
        //    AppData.CallbackData<AppData.UIWidgetInfo> callbackResults = new AppData.CallbackData<AppData.UIWidgetInfo>();

        //    if (focusedWidget.Count > 0)
        //    {
        //        callbackResults.data = focusedWidget.Dequeue();
        //        callbackResults.results = $"GetFocusedWidgetInfo Success : Focused To Widget : {callbackResults.data.GetWidgetName()}.";
        //        callbackResults.success = true;
        //    }
        //    else
        //    {
        //        callbackResults.results = "GetFocusedWidgetInfo Failed : There Is No Focused Widget Assigned.";
        //        callbackResults.data = default;
        //        callbackResults.success = false;
        //    }

        //    callback?.Invoke(callbackResults);
        //}

        //public void GetCurrentFocusedWidget(Action<AppData.CallbackData<AppData.UIWidgetInfo>> callback)
        //{
        //    AppData.CallbackData<AppData.UIWidgetInfo> callbackResults = new AppData.CallbackData<AppData.UIWidgetInfo>();

        //    if (currentFocusedWidget.Count > 0)
        //    {
        //        callbackResults.data = currentFocusedWidget.Dequeue();
        //        callbackResults.results = $"GetCurrentFocusedWidget Success : Focused To Widget : {callbackResults.data.GetWidgetName()}.";
        //        callbackResults.success = true;
        //    }
        //    else
        //    {
        //        callbackResults.results = "GetCurrentFocusedWidget Failed : There Is No Focused Widget Assigned.";
        //        callbackResults.data = default;
        //        callbackResults.success = false;
        //    }

        //    callback?.Invoke(callbackResults);
        //}

        //public void ClearAllFocusedWidgetInfo()
        //{
        //    focusedWidget.Clear();
        //    currentFocusedWidget.Clear();
        //}


        #endregion

        public void AddDynamicWidget(AppData.UIScreenWidget screenWidget, AppData.OrientationType orientation, bool keepWorldPosition, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (container != null)
            {
                if (screenWidget != null)
                {
                    ScreenUIManager.Instance.GetCurrentScreenData().value.ShowLoadingItem(AppData.LoadingItemType.Spinner, false);

                    screenWidget.gameObject.transform.SetParent(container, keepWorldPosition);

                    if (IsContainerActive())
                    {
                        if (containerUpdateRoutine != null)
                        {
                            StopCoroutine(containerUpdateRoutine);
                            containerUpdateRoutine = null;
                        }

                        if (containerUpdateRoutine == null)
                            containerUpdateRoutine = StartCoroutine(UpdatedContainerSizeAsync());
                    }

                    callbackResults.results = "AddDynamicWidget Success : Screen Widget Added Successfully.";
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = "AddDynamicWidget Failed : Screen Widget Is Missing / Null.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "AddDynamicWidget Failed : Content Container Is Missing / Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public bool IsContainerActive()
        {
            return gameObject.activeSelf && gameObject.activeInHierarchy;
        }

        public void UpdateContentOnRefresh()
        {
            if (container.childCount > 0)
            {
                List<AppData.UIScreenWidget> skyboxWidgetsList = new List<AppData.UIScreenWidget>();

                for (int i = 0; i < container.childCount; i++)
                {
                    AppData.UIScreenWidget widgetHandler = container.GetChild(i).GetComponent<AppData.UIScreenWidget>();

                    if (widgetHandler != null)
                    {
                        if (!skyboxWidgetsList.Contains(widgetHandler))
                            skyboxWidgetsList.Add(widgetHandler);
                    }
                    else
                    {
                        Debug.LogWarning("--> UpdateContentOnRefresh Failed : Widget Handler Component Not Found / Null.");
                        break;
                    }
                }

                switch (containerType)
                {
                    case AppData.ContentContainerType.SkyboxContent:

                        if (skyboxWidgetsList.Count > 0)
                        {
                            skyboxWidgetsList.Sort((widgetA, widgetB) => widgetA.GetSelectableType().CompareTo(widgetB.GetSelectableType()));

                            for (int i = 0; i < skyboxWidgetsList.Count; i++)
                                skyboxWidgetsList[i].SetContentContainerPositionIndex(i);
                        }
                        else
                            Debug.LogWarning("--> UpdateContentOnRefresh Failed : Widgets Dictionary Not Populated.");

                        break;
                }
            }
            else
                Debug.LogWarning("--> UpdateContentOnRefresh Failed : Container Content Is Empty / Null.");
        }

        IEnumerator UpdatedContainerSizeAsync()
        {
            yield return new WaitForEndOfFrame();

            UpdatedContainerSize();
        }

        void ResetScrollerState()
        {
            transitionToFocusPosition = false;
            scrollToTop = false;
            edgeScrolling = false;
        }

        public void DeselectAllContentWidgets()
        {
            GetContent(contentFound =>
            {
                if (AppData.Helpers.IsSuccessCode(contentFound.resultsCode))
                {
                    foreach (var content in contentFound.data)
                        content.OnSelectionFrameState(false, AppData.InputUIState.Normal, false);

                    Debug.LogError("==> Check, Has Something To Do With Ambushed Selection Data");
                    //currentFocusedWidget.Clear();
                    // SelectableManager.Instance.ClearFocusedWidgetInfo();

                    SelectableManager.Instance.GetProjectStructureSelectionSystem(projectSelectionCallbackResults => 
                    {
                        if (projectSelectionCallbackResults.Success())
                            projectSelectionCallbackResults.data.DeselectAll();
                        else
                            Log(projectSelectionCallbackResults.resultsCode, projectSelectionCallbackResults.results, this);
                    });
                }
                else
                    Debug.LogWarning($"--> DeselectAllContentWidgets Failed With Results : {contentFound.results}");
            });
        }

        void UpdatedContainerSize(Action<AppData.CallbackData<Vector2>> callback = null)
        {
            AppData.CallbackData<Vector2> callbackResults = new AppData.CallbackData<Vector2>();

            GetContent(contentFound =>
            {
                if (AppData.Helpers.IsSuccessCode(contentFound.resultsCode))
                {
                    containerRefreshed = false;

                #region Pager

                if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
                    {
                        int itemsPerPage = (GetLayout().viewType == AppData.LayoutViewType.ItemView) ? paginationComponent.itemView_ItemsPerPage : paginationComponent.listView_ItemsPerPage;

                        paginationComponent.Initialize();
                        paginationComponent.Paginate(contentFound.data, itemsPerPage);
                        paginationComponent.GoToPage(paginationComponent.CurrentPageIndex);
                        var currentPage = paginationComponent.GetCurrentPage();

                        Vector2 sizeDelta = container.sizeDelta;
                        int contentCount = currentPage.Count + 1;

                        if (orientation == AppData.OrientationType.Vertical)
                        {
                            if (layout.viewType == AppData.LayoutViewType.ItemView)
                            {
                                for (int i = 1; i < contentCount; i++)
                                {
                                    if (i % 2 == 0)
                                    {
                                        sizeDelta.y = (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) * i;
                                        sizeDelta.y /= 2;
                                    }
                                    else
                                    {
                                        sizeDelta.y = (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) * i;
                                        sizeDelta.y /= 2;

                                        sizeDelta.y += (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) / 2;
                                    }
                                }
                            }

                            if (layout.viewType == AppData.LayoutViewType.ListView)
                            {
                                sizeDelta.y = (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) * contentFound.data.Count - 1;
                            }
                        }

                        if (orientation == AppData.OrientationType.Horizontal)
                            sizeDelta.x = (layout.layout.itemViewSize.x + layout.layout.itemViewSpacing.x) * contentFound.data.Count;

                        container.sizeDelta = sizeDelta;

                        OnFocusedSelectionStateUpdate();
                    }

                #endregion

                #region Scroller

                if (GetPaginationViewType() == AppData.PaginationViewType.Scroller)
                    {
                        Vector2 sizeDelta = container.sizeDelta;
                        int contentCount = contentFound.data.Count + 1;

                        if (orientation == AppData.OrientationType.Vertical)
                        {
                            if (layout.viewType == AppData.LayoutViewType.ItemView)
                            {
                                for (int i = 1; i < contentCount; i++)
                                {
                                    if (i % 2 == 0)
                                    {
                                        sizeDelta.y = (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) * i;
                                        sizeDelta.y /= 2;
                                    }
                                    else
                                    {
                                        sizeDelta.y = (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) * i;
                                        sizeDelta.y /= 2;

                                        sizeDelta.y += (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) / 2;
                                    }
                                }
                            }

                            if (layout.viewType == AppData.LayoutViewType.ListView)
                            {
                                sizeDelta.y = (layout.layout.itemViewSize.y + layout.layout.itemViewSpacing.y) * contentFound.data.Count - 1;
                            }
                        }

                        if (orientation == AppData.OrientationType.Horizontal)
                            sizeDelta.x = (layout.layout.itemViewSize.x + layout.layout.itemViewSpacing.x) * contentFound.data.Count;

                        container.sizeDelta = sizeDelta;

                        scroller.Initialized(scrollerInitializedCallback =>
                        {
                            if (AppData.Helpers.IsSuccessCode(scrollerInitializedCallback.resultsCode))
                            {
                                if (scroller.GetFadeUIScrollBar())
                                    scroller.GetUIScrollBarComponent().Show();
                            }
                            else
                                LogWarning(scrollerInitializedCallback.results, this);
                        });

                        ResetScrollerState();

                        OnFocusedSelectionStateUpdate();

                        if (scroller.value != null)
                        {
                            if (scroller.IsScrollBarEnabled())
                                if (scroller.GetFadeUIScrollBar())
                                    scroller.Initialize();
                        }
                        else
                            LogWarning("Scroller.value Is Missing / Null.", this);
                    }

                #endregion
            }
                else
                    LogWarning(contentFound.results, this);
            });

            callback?.Invoke(callbackResults);
        }

        public void SetAssetsLoaded(bool loaded) => assetsLoaded = loaded;

        public bool GetAssetsLoaded()
        {
            return assetsLoaded;
        }

        public bool ContainerRefreshed()
        {
            return containerRefreshed;
        }

        public void OnFocusedSelectionStateUpdate()
        {
            if(focusedSelectionStateRoutine != null)
            {
                StopCoroutine(focusedSelectionStateRoutine);
                focusedSelectionStateRoutine = null;
            }

            focusedSelectionStateRoutine = StartCoroutine(OnSetSelectionStateAsync());

        }

        IEnumerator OnSetSelectionStateAsync()
        {
            yield return new WaitForEndOfFrame();

            SetAssetsLoaded(true);
            SetSelectionStateInfo();
        }

        void SetSelectionStateInfo()
        {
            if (IsSelectableContentContainer())
            {
                if (SelectableManager.Instance != null)
                {
                    if (SelectableManager.Instance.HasActiveSelection())
                    {
                        SelectableManager.Instance.GetFocusedSelectionData(focusedSelectionDataCallback =>
                        {
                            if (focusedSelectionDataCallback.Success())
                            {
                                GetSelectedWidgetList(focusedSelectionDataCallback.data, selectionCallback =>
                                {
                                    if (selectionCallback.Success())
                                    {
                                        SelectableManager.Instance.SetSelectionInfoState(selectionCallback.data, focusedSelectionDataCallback.data.selectionType, selectionSetCallback =>
                                        {
                                            if (AppData.Helpers.IsSuccessCode(selectionSetCallback.resultsCode))
                                                OnFocusToSelection(selectionSetCallback.data.GetWidgetAnchoredPosition());
                                            else
                                                LogError(selectionSetCallback.results, this, () => SetSelectionStateInfo());
                                        });
                                    }
                                    else
                                        Log(selectionCallback.resultsCode, selectionCallback.results, this);
                                });
                            }
                            else
                                LogError("Have Not Focused On Any Widget.", this, () => SetSelectionStateInfo());
                        });
                    }
                    else
                        LogError("Doesn't Have Focused Selection Data.", this, () => SetSelectionStateInfo());
                }
                else
                    LogError("Selectable Manager Instance Is Not Yet Initialized.", this, () => SetSelectionStateInfo());
            }
        }

        public bool IsSelectableContentContainer()
        {
            return selectableContentContainer;
        }

        public void GetSelectedWidgetList(AppData.FocusedSelectionData selectionData, Action<AppData.CallbackData<List<AppData.UIScreenWidget>>> callback)
        {
            AppData.CallbackData<List<AppData.UIScreenWidget>> callbackResults = new AppData.CallbackData<List<AppData.UIScreenWidget>>();

            if(GetContentCount() > 0)
            {
                GetContent(contentCallback =>
                {
                    if (contentCallback.Success())
                    {
                        List<AppData.UIScreenWidget> widgets = new List<AppData.UIScreenWidget>();

                        foreach (var selection in selectionData.selections)
                            widgets.Add(contentCallback.data.Find(widget => widget.name == selection.name));

                        if (widgets.Count == selectionData.selections.Count)
                        {
                            callbackResults.results = $"Found : {widgets.Count} Selected Widget(s).";
                            callbackResults.data = widgets;
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"Could'nt Find All Selected Widgets - {widgets.Count} Of {selectionData.selections.Count}.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = contentCallback.results;
                        callbackResults.data = default;
                        callbackResults.resultsCode = contentCallback.resultsCode;
                    }
                });
            }
            else
            {
                callbackResults.results = "There Are No Contents Loaded Yet. Possible Race Condition.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void OnFocusToSelection(Vector2 selectionPosition, bool transitionTo = false)
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Scroller)
            {
                if (scroller.value)
                {
                    if (selectionPosition != Vector2.zero)
                    {
                        if (transitionTo)
                        {
                            if (orientation == AppData.OrientationType.Vertical)
                            {
                                if (selectionPosition != Vector2.zero)
                                {
                                    currentFocusPosition = AppData.Helpers.GetScrollerSnapPosition(scroller.value, selectionPosition);
                                    currentFocusPosition.x = scroller.value.content.localPosition.x;
                                }
                            }

                            if (currentFocusPosition != Vector2.zero)
                                transitionToFocusPosition = true;
                        }
                        else
                        {
                            if (orientation == AppData.OrientationType.Vertical)
                            {
                                if (selectionPosition != Vector2.zero)
                                {
                                    Vector2 focusPosition = AppData.Helpers.GetScrollerSnapPosition(scroller.value, selectionPosition);
                                    focusPosition.x = scroller.value.content.localPosition.x;
                                    scroller.value.content.localPosition = focusPosition;
                                }
                            }

                            if (orientation == AppData.OrientationType.Horizontal)
                            {
                                if (selectionPosition != Vector2.zero)
                                {
                                    //Vector2 focusPosition = AppData.Helpers.GetScrollerSnapPosition(scroller.value, selectionPosition);
                                    //focusPosition.y = scroller.value.content.localPosition.y;
                                    //scroller.value.content.localPosition = focusPosition;
                                }
                                else
                                    Debug.LogWarning("--> Failed : Focus Widget Is Missing / Null.");
                            }
                        }
                    }
                    else
                        Debug.LogWarning("--> Failed : Focus Widget Position Is Not Assigned.");
                }
                else
                    Debug.LogWarning("--> Failed : Scroller Value Is Not Assigned.");
            }
        }

        public void OnFocusToSelection(string widgetName)
        {
            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
            {
                paginationComponent.GetItemPageIndex(widgetName, onItemPageIndexResults =>
                {
                    if (AppData.Helpers.IsSuccessCode(onItemPageIndexResults.resultsCode))
                    {
                        if (paginationComponent.CurrentPageIndex != onItemPageIndexResults.data)
                            Pagination_SelectPage(onItemPageIndexResults.data, true);
                        else
                            return;
                    }
                });
            }
        }

        public void OnFocusToWidget(AppData.UIScreenWidget focusToWidget, bool transitionTo = false, bool selectWidget = false)
        {
            if (orientation == AppData.OrientationType.Vertical)
            {
                if (focusToWidget != null)
                {
                    Vector2 focusPosition = AppData.Helpers.GetScrollerSnapPosition(scroller.value, focusToWidget.GetWidgetRect());
                    focusPosition.x = scroller.value.content.localPosition.x;
                    scroller.value.content.localPosition = focusPosition;

                    if (selectWidget)
                        focusToWidget.OnSelect();
                }
                else
                    LogWarning("Focus Widget Is Missing / Null.", this);
            }

            if (orientation == AppData.OrientationType.Horizontal)
            {
                if (focusToWidget != null)
                {
                    Vector2 focusPosition = AppData.Helpers.GetScrollerSnapPosition(scroller.value, focusToWidget.GetWidgetRect());
                    focusPosition.y = scroller.value.content.localPosition.y;
                    scroller.value.content.localPosition = focusPosition;

                    if (selectWidget)
                        focusToWidget.OnSelect();
                    else
                    {
                        //if (focusToWidget.IsSelected())
                        //    focusToWidget.OnDeselect();
                    }
                }
                else
                    LogWarning("Focus Widget Is Missing / Null.", this);
            }
        }

        public void OnUpdateSelectedWidgets(bool show, AppData.InputUIState state, bool showTint, bool async = false)
        {
            int selectionCount = SelectableManager.Instance.GetFocusedSelectionDataCount();

            if (selectionCount == 0)
                return;

            var currentPage = Pagination_GetCurrentPage();

            AppData.Helpers.ComponentValid(currentPage, validComponentCallbackResults => 
            {
                if (validComponentCallbackResults.Success())
                {
                    AppData.Helpers.ValueAssigned(currentPage.Count, valueAssignedCallbackResults => 
                    {
                        if (validComponentCallbackResults.Success())
                        {
                            SelectableManager.Instance.GetProjectStructureSelectionSystem(projectSelectionCallbackResults =>
                            {
                                if (projectSelectionCallbackResults.Success())
                                {
                                    foreach (var item in currentPage)
                                    {
                                        foreach (var selection in projectSelectionCallbackResults.data.GetCurrentSelections())
                                            if (item == selection)
                                                item.OnSelectionFrameState(show, state, showTint, async);
                                    }
                                }
                                else
                                    Log(projectSelectionCallbackResults.resultsCode, projectSelectionCallbackResults.results, this);
                            });
                        }
                        else
                            Log(valueAssignedCallbackResults.resultsCode, valueAssignedCallbackResults.results, this);
                    });
                }
                else
                    Log(validComponentCallbackResults.resultsCode, validComponentCallbackResults.results);
            });
        }

        public async void ClearWidgets(bool showSpinner = false, Action<AppData.Callback> callback = null)
        {
            try
            {
                AppData.Callback callbackResults = new AppData.Callback();

                if (ScreenUIManager.Instance.HasCurrentScreen())
                {
                    if (GetContentCount() > 0)
                    {
                        for (int i = 0; i < GetContentCount(); i++)
                        {
                            if (container.GetChild(i).GetComponent<AppData.UIScreenWidget>())
                            {
                                if (container.GetChild(i).GetComponent<AppData.UIScreenWidget>().GetSelectableWidgetType() != AppData.SelectableWidgetType.PlaceHolder)
                                    Destroy(container.GetChild(i).gameObject);
                                else
                                    LogError($"Widget : {container.GetChild(i).name} Is A Place Holde Component.", this);
                            }
                            else
                                LogError($"Widget : {container.GetChild(i).name} Doesn't Contain AppData.UIScreenWidget Component", this);
                        }

                        await AppData.Helpers.GetWaitForSecondsAsync(10);

                        if (container.childCount == 0)
                        {
                            if (showSpinner)
                                ScreenUIManager.Instance.GetCurrentScreenData().value.ShowWidget(AppData.WidgetType.LoadingWidget);

                            SceneAssetsManager.Instance.UnloadUnusedAssets();

                            callbackResults.results = "All Widgets Cleared.";
                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.results = $"{container.childCount} : Widgets Failed To Clear.";
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"No Widgets To Clear From Container : {gameObject.name}";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                }
                else
                {
                    callbackResults.results = $"Curent Screen Is Not Yet Initialized.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
            catch (Exception exception)
            {
                LogError(exception.Message, this);
                throw exception;
            }
        }

        public AppData.ContentContainerType GetContentContainerType()
        {
            return containerType;
        }

        public int GetContentCount()
        {
            return container.childCount;
        }

        public bool HasContent()
        {
            return GetContentCount() > 0;
        }

        public void GetContent(Action<AppData.CallbackData<List<AppData.UIScreenWidget>>> callback)
        {
            AppData.CallbackData<List<AppData.UIScreenWidget>> callbackResults = new AppData.CallbackData<List<AppData.UIScreenWidget>>();

            if (GetContentCount() > 0)
            {
                List<AppData.UIScreenWidget> widgetsList = new List<AppData.UIScreenWidget>();

                for (int i = 0; i < GetContentCount(); i++)
                {
                    AppData.UIScreenWidget widget = container.transform.GetChild(i).GetComponent<AppData.UIScreenWidget>();

                    if (widget != null)
                    {
                        if (!widgetsList.Contains(widget))
                            widgetsList.Add(widget);
                        else
                            LogWarning("Container Widget List Already Contains Widget.", this);
                    }
                    else
                        LogWarning("Widget component not found", this);
                }

                if (widgetsList.Count > 0)
                {
                    callbackResults.results = $"GetContent Success : {GetContentCount()} - Content Widgets Found Successfully.";
                    callbackResults.data = widgetsList;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"GetContent Failed : {GetContentCount()} - Content Widgets Couldn't Be Added To Widgets List. - Status Unknown.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "GetContent Failed : No Content Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SetWidgetListIndex(AppData.UIScreenWidget widget, int index, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (widget)
            {
                widget.SetContentSiblingIndexValue(index);

                callbackResults.results = $"Asset : {widget.name} Has Been Set To index : {index}.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = $"Set Asset List Index Value For : {widget} Missing / Not Assigned.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetSelectableWidgetType(List<AppData.UIScreenWidget> widgets, Action<AppData.CallbackData<AppData.SelectableWidgetType>> callback)
        {
            AppData.CallbackData<AppData.SelectableWidgetType> callbackResults = new AppData.CallbackData<AppData.SelectableWidgetType>();

            AppData.Helpers.ComponentValid(widgets, hasComponentsCallbackResults => 
            {
                callbackResults.results = hasComponentsCallbackResults.results;
                callbackResults.resultsCode = hasComponentsCallbackResults.resultsCode;

                if(callbackResults.Success())
                {
                    var selectableWidgetType = widgets[0].GetSelectableWidgetType();

                    foreach (var selectable in widgets)
                    {
                        if(selectable.GetSelectableWidgetType() != selectableWidgetType)
                        {
                            callbackResults.results = $"Widget Selection Type : {selectable.GetSelectableWidgetType()} Mismatch From Type : {selectableWidgetType}";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                            break;
                        }
                    }

                    if(callbackResults.Success())
                    {
                        callbackResults.results = $"Selectable Type Found : {selectableWidgetType}";
                        callbackResults.data = selectableWidgetType;
                    }
                }
            });

            callback.Invoke(callbackResults);
        }

        public void GetContent(AppData.UIScreenWidget widgetToExcludeFromList, Action<AppData.CallbackData<List<AppData.UIScreenWidget>>> callback)
        {
            AppData.CallbackData<List<AppData.UIScreenWidget>> callbackResults = new AppData.CallbackData<List<AppData.UIScreenWidget>>();

            if (GetContentCount() > 0)
            {
                List<AppData.UIScreenWidget> widgetsList = new List<AppData.UIScreenWidget>();

                for (int i = 0; i < GetContentCount(); i++)
                {
                    AppData.UIScreenWidget widget = container.transform.GetChild(i).GetComponent<AppData.UIScreenWidget>();

                    if (widget != null && widget.GetActive() && widget != widgetToExcludeFromList)
                    {
                        if (widget.GetSelectableWidgetType() == AppData.SelectableWidgetType.Folder || widget.GetSelectableWidgetType() == AppData.SelectableWidgetType.PlaceHolder)

                            if (!widgetsList.Contains(widget))
                                widgetsList.Add(widget);
                            else
                                LogWarning("Container Widget List Already Contains Widget.", this);
                    }
                    else
                        LogWarning("Widget component not found", this);
                }

                if (widgetsList.Count > 0)
                {
                    callbackResults.results = $"GetContent Success : {GetContentCount()} - Content Widgets Found Successfully.";
                    callbackResults.data = widgetsList;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"GetContent Failed : {GetContentCount()} - Content Widgets Couldn't Be Added To Widgets List. - Status Unknown.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "GetContent Failed : No Content Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetContent(string contentName, Action<AppData.CallbackData<AppData.UIScreenWidget>> callback)
        {
            AppData.CallbackData<AppData.UIScreenWidget> callbackResults = new AppData.CallbackData<AppData.UIScreenWidget>();

            GetContent(contentFound =>
            {
                if (AppData.Helpers.IsSuccessCode(contentFound.resultsCode))
                {
                    foreach (var content in contentFound.data)
                    {
                        if (content.name == contentName)
                        {
                            callbackResults.results = contentFound.results;
                            callbackResults.data = content;
                            callbackResults.resultsCode = contentFound.resultsCode;

                            break;
                        }
                        else
                            continue;
                    }
                }
                else
                {
                    callbackResults.results = contentFound.results;
                    callbackResults.data = default;
                    callbackResults.resultsCode = contentFound.resultsCode;
                }
            });

            callback?.Invoke(callbackResults);
        }

        public bool IsWidgetActive(string widgetName)
        {
            return GetWidgetNamed(widgetName).GetActive();
        }

        public bool IsWidgetSelected(string widgetName)
        {
            return GetWidgetNamed(widgetName).IsSelected();
        }

        public void OnWidgetSelectionState(AppData.FocusedSelectionInfo<AppData.SceneDataPackets> selection, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            var widget = GetWidgetNamed(selection.name);

            if(widget != null)
            {
                switch(selection.selectionInfoType)
                {
                    case AppData.FocusedSelectionType.SelectedItem:

                        widget.OnSelect();

                        break;

                    case AppData.FocusedSelectionType.Default:

                        widget.OnDeselect();

                        break;
                }

                callbackResults.results = $"Widget : {selection.name} Selected.";
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = $"Widget : {selection.name} Not Found. Get Widget Named : {selection.name} Failed.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void OnWidgetSelectionState(List<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> selections, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if(selections != null && selections.Count > 0)
            {
                List<AppData.UIScreenWidget> widgets = new List<AppData.UIScreenWidget>();

                foreach (var selection in selections)
                {
                    var widget = GetWidgetNamed(selection.name);

                    if (widget != null)
                    {
                        switch (selection.selectionInfoType)
                        {
                            case AppData.FocusedSelectionType.SelectedItem:

                                widget.OnSelect();

                                break;

                            case AppData.FocusedSelectionType.Default:

                                widget.OnDeselect();

                                break;
                        }

                        callbackResults.results = $"Widget : {selection.name} Selected.";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Widget : {selection.name} Not Found. Get Widget Named : {selection.name} Failed.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                        break;
                    }
                }
            }
            else
            {
                callbackResults.results = $"No Selections Found - Silection List Is Null / Empty.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void OnWidgetSelectionState(List<AppData.FocusedSelectionInfo<AppData.SceneDataPackets>> selections, AppData.FocusedSelectionType selectionType, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (selections != null && selections.Count > 0)
            {
                List<AppData.UIScreenWidget> widgets = new List<AppData.UIScreenWidget>();

                foreach (var selection in selections)
                {
                    var widget = GetWidgetNamed(selection.name);

                    if (widget != null)
                    {
                        switch (selectionType)
                        {
                            case AppData.FocusedSelectionType.SelectedItem:

                                widget.OnSelect();

                                break;

                            case AppData.FocusedSelectionType.Default:

                                widget.OnDeselect();

                                break;
                        }

                        callbackResults.results = $"Widget : {selection.name} Selected.";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = $"Widget : {selection.name} Not Found. Get Widget Named : {selection.name} Failed.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                        break;
                    }
                }
            }
            else
            {
                callbackResults.results = $"No Selections Found - Silection List Is Null / Empty.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void SelectAllWidgets(bool current_PageView = false, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (SelectableManager.Instance)
            {
                if (SelectableManager.Instance.HasActiveSelection())
                {
                    SelectableManager.Instance.CacheSelection(selectionCachedCallback =>
                    {
                        if (AppData.Helpers.IsSuccessCode(selectionCachedCallback.resultsCode))
                        {
                            List<string> newSelectionNameList = new List<string>();

                            if (current_PageView)
                            {
                                var selectionWidgets = Pagination_GetCurrentPage();

                                if(selectionWidgets != null && selectionWidgets.Count > 0)
                                {
                                    foreach (var widget in selectionWidgets)
                                    {
                                        if (!newSelectionNameList.Contains(widget.name))
                                            newSelectionNameList.Add(widget.name);
                                    }
                                }
                                else
                                {
                                    callbackResults.results = "There Are No Widgets Found In Current Page.";
                                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                                }
                            }
                            else
                            {
                                GetContent(foundWidgetsCallback => 
                                {
                                    if(AppData.Helpers.IsSuccessCode(foundWidgetsCallback.resultsCode))
                                    {
                                        foreach (var widget in foundWidgetsCallback.data)
                                        {
                                            if (!newSelectionNameList.Contains(widget.name))
                                                newSelectionNameList.Add(widget.name);
                                        }
                                    }
                                    else
                                    {
                                        callbackResults.results = foundWidgetsCallback.results;
                                        callbackResults.resultsCode = foundWidgetsCallback.resultsCode;
                                    }
                                });
                            }

                            if(newSelectionNameList.Count > 0)
                            {
                                if(SceneAssetsManager.Instance.GetFolderStructureData().InverseSelect())
                                {
                                    if(SelectableManager.Instance.HasCachedSelectionInfo())
                                    {
                                        SelectableManager.Instance.GetCachedSelectionInfoNameList(cachedSelectionInfoCallback => 
                                        {
                                            if(AppData.Helpers.IsSuccessCode(cachedSelectionInfoCallback.resultsCode))
                                            {
                                                foreach (var selection in cachedSelectionInfoCallback.data)
                                                    if (newSelectionNameList.Contains(selection))
                                                        newSelectionNameList.Remove(selection);
                                            }
                                            else
                                            {
                                                callbackResults.results = cachedSelectionInfoCallback.results;
                                                callbackResults.resultsCode = cachedSelectionInfoCallback.resultsCode;
                                            }
                                        });
                                    }
                                }

                                SelectableManager.Instance.Select(newSelectionNameList, AppData.FocusedSelectionType.SelectedItem, selectionCallback =>
                                {
                                    if(selectionCallback.Success())
                                        AppData.ActionEvents.OnAllWidgetsSelectionEvent(current_PageView);

                                    callbackResults.results = selectionCallback.results;
                                    callbackResults.resultsCode = selectionCallback.resultsCode;
                                });
                            }
                            else
                            {
                                callbackResults.results = "There Is No New Selection Name List Found.";
                                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                            }
                        }
                        else
                            callbackResults = selectionCachedCallback;
                    });
                }
                else
                {
                    callbackResults.results = "There Is No Active Selection";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "Selectable Manager Instance Is Not Yet Initialized";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void HasAllWidgetsSelected(Action<AppData.CallbackData<int>> callback)
        {
            AppData.CallbackData<int> callbackResults = new AppData.CallbackData<int>();

             
            if(SelectableManager.Instance.GetFocusedSelectionDataCount() == GetContentCount())
            {
                callbackResults.results = $"All : {GetContentCount()} Widgets Are Selected";
                callbackResults.data = GetContentCount();
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Not All Widgets Are Selected";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.WarningCode;
            }

            callback.Invoke(callbackResults);
        }

        public void AllWidgetsSelected(Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            GetContent(allContentCallback => 
            {
                if(AppData.Helpers.IsSuccessCode(allContentCallback.resultsCode))
                {
                    HasSelectedAll(allContentCallback.data, selectionCallback =>
                    {
                        callbackResults = selectionCallback;
                    });
                }
                else
                {
                    callbackResults.results = allContentCallback.results;
                    callbackResults.resultsCode = allContentCallback.resultsCode;
                }
            });

            callback.Invoke(callbackResults);
        }

        public void WidgetsInCurrentPageSelected(Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            var widgets = Pagination_GetCurrentPage();

            if(widgets != null && widgets.Count > 0)
            {
                HasSelectedAll(widgets, selectionCallback => 
                {
                    callbackResults = selectionCallback;
                });
            }
            else
            {
                callbackResults.results = "Current Page Widgets Not Found.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void HasSelectedAll(List<AppData.UIScreenWidget> widgetList, Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            foreach (var widget in widgetList)
            {
                SelectableManager.Instance.HasFocusedSelectionInfo(widget.name, selectionCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(selectionCallback.resultsCode) && widget.IsSelected() && widgetList.Count.Equals(SelectableManager.Instance.GetFocusedSelectionDataCount()))
                    {
                        callbackResults.results = "All Are Selected.";
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "Not All Are Selected.";
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        callback.Invoke(callbackResults);
                    }
                });
            }

            callback.Invoke(callbackResults);
        }

        public AppData.UIScreenWidget GetWidgetNamed(string widgetName)
        {
            AppData.UIScreenWidget widgetFound = null;

            if (GetContentCount() > 0)
            {
                GetContent(contentCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(contentCallback.resultsCode))
                    {
                        if (contentCallback.data != null && contentCallback.data.Count > 0)
                        {
                            foreach (var widget in contentCallback.data)
                            {
                                if (widgetFound == null && widget.name.Equals(widgetName))
                                {
                                    widgetFound = widget;
                                    break;
                                }

                                continue;
                            }
                        }
                        else
                            LogError(contentCallback.results, this);
                    }
                });
            }
            else
                LogError("There Are No Contents Found In Dynamic Container.", this);

            if(widgetFound == null)
                LogError($"Couldn't Find Widget Named : {widgetName}", this);

            return widgetFound;
        }

        public void GetItem<T>(string itemName, Action<AppData.CallbackData<AppData.PageItem<T>>> callback) where T : AppData.UIScreenWidget
        {
            AppData.CallbackData<AppData.PageItem<T>> callbackResults = new AppData.CallbackData<AppData.PageItem<T>>();

            if (GetContentCount() > 0)
            {
                GetContent(contentCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(contentCallback.resultsCode))
                    {
                        if (contentCallback.data != null && contentCallback.data.Count > 0)
                        {
                            foreach (var widget in contentCallback.data)
                            {
                                if (widget.name.Equals(itemName))
                                {
                                    AppData.PageItem<T> foundWidget = new AppData.PageItem<T>()
                                    {
                                        name = itemName,
                                        item = widget as T,
                                        pageID = Pagination_GetItemPageIndex(itemName)
                                    };

                                    callbackResults.results = $"Item Named : {itemName} Found.";
                                    callbackResults.data = foundWidget;
                                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;

                                    break;
                                }

                                continue;
                            }
                        }
                        else
                        {
                            callbackResults.results = "There Are No Content Found - Please Check Code Here Above";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = contentCallback.results;
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                });
            }
            else
                LogError("There Are No Contents Found In Dynamic Container.", this);

            callback.Invoke(callbackResults);
        }

        public void GetWidgetNamed(string widgetName, Action<AppData.CallbackData<AppData.UIScreenWidget>> callback)
        {
            AppData.CallbackData<AppData.UIScreenWidget> callbackResults = new AppData.CallbackData<AppData.UIScreenWidget>();

            if (GetContentCount() > 0)
            {
                GetContent(contentCallback =>
                {
                    if (AppData.Helpers.IsSuccessCode(contentCallback.resultsCode))
                    {
                        bool widgetFound = false;

                        foreach (var widget in contentCallback.data)
                        {
                            if (widget.name == widgetName)
                            {
                                widgetFound = true;

                                callbackResults.results = $"GetWidgetNamed : {widgetName} Success With Results : {contentCallback.results}.";
                                callbackResults.data = widget;
                                callbackResults.resultsCode = (widgetFound)? AppData.Helpers.SuccessCode : AppData.Helpers.ErrorCode;

                                break;
                            }
                        }

                        if (!widgetFound)
                        {
                            callbackResults.results = $"GetWidgetNamed : {widgetName} Failed - Widget : {widgetName} Not Found.";
                            callbackResults.data = default;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.results = $"GetWidgetNamed : {widgetName} Failed With Results : {contentCallback.results}.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                });
            }
            else
            {
                callbackResults.results = $"GetWidgetNamed : {widgetName} Failed : There Are No Contents Found.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void GetPlaceHolder(Action<AppData.CallbackData<AppData.WidgetPlaceHolder>> callback)
        {
            AppData.CallbackData<AppData.WidgetPlaceHolder> callbackResults = new AppData.CallbackData<AppData.WidgetPlaceHolder>();

            if (placeHolder.value && placeHolder.container)
            {
                callbackResults.results = "GetPlaceHolder Success";
                callbackResults.data = placeHolder;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Container / Value Missing Null.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public void GetWidgetSiblingIndex(AppData.UIScreenWidget widget, Action<AppData.CallbackData<int>> callback)
        {
            AppData.CallbackData<int> callbackResults = new AppData.CallbackData<int>();

            if (container.childCount > 0)
            {
                bool widgetFound = false;
                int siblingIndex = 0;

                for (int i = 0; i < container.childCount; i++)
                {
                    if (!widgetFound && container.GetChild(i).GetComponent<AppData.UIScreenWidget>() == widget)
                    {
                        siblingIndex = i;
                        widgetFound = true;
                        break;
                    }
                    else
                        continue;
                }

                if (widgetFound)
                {
                    callbackResults.results = $"Widget : {widget.name} Found Inside Dynamic Container.";
                    callbackResults.data = siblingIndex;
                    callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.results = $"Widget : {widget.name} Not Found Inside Dynamic Container.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "Dynamic Container Has No Widgets.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public FolderEmptyContentWidgetHandler GetEmptyFolderWidget()
        {
            return emptyFolderWidget;
        }

        public void SetViewLayout(AppData.FolderLayoutView layoutView)
        {
            layoutComponent.cellSize = layoutView.layout.itemViewSize;
            layoutComponent.spacing = layoutView.layout.itemViewSpacing;
            layoutComponent.padding = layoutView.padding;

            layout = layoutView;
        }

        public AppData.FolderLayoutView GetLayout()
        {
            return layout;
        }

        public void SetPaginationView(AppData.PaginationViewType paginationView)
        {
            switch (paginationView)
            {
                case AppData.PaginationViewType.Pager:

                    scroller.value.movementType = ScrollRect.MovementType.Clamped;
                    scroller.value.vertical = false;
                    scroller.value.horizontal = false;

                    break;

                case AppData.PaginationViewType.Scroller:

                    scroller.value.movementType = scroller.movementType;

                    if (orientation == AppData.OrientationType.Vertical)
                    {
                        scroller.value.vertical = true;
                        scroller.value.horizontal = false;
                    }

                    if (orientation == AppData.OrientationType.Horizontal)
                    {
                        scroller.value.vertical = false;
                        scroller.value.horizontal = true;
                    }

                    break;
            }

            currentPaginationView = paginationView;
        }

        public void OnPaginationActionButtonPressed(AppData.PaginationNavigationActionType actionType)
        {
            switch (actionType)
            {
                case AppData.PaginationNavigationActionType.GoToNextPage:

                    paginationComponent.NextPage();

                    break;

                case AppData.PaginationNavigationActionType.GoToPreviousPage:

                    paginationComponent.PreviousPage();

                    break;
            }

            UpdatedContainerSize();

            ScreenUIManager.Instance.Refresh();
        }

        public void Pagination_GoToPage(int pageNumber)
        {
            LogSuccess($"Page Has Active Selection - Go To Page :#{pageNumber}", this, () => Pagination_GoToPage(pageNumber));

            paginationComponent.GoToPage(pageNumber, true);

            UpdatedContainerSize();

            //ScreenUIManager.Instance.Refresh();
        }

        public void Pagination_SelectPage(int pageNumber, bool fromInput)
        {
            paginationComponent.GoToPage(pageNumber, fromInput);

            if (!fromInput)
                paginationComponent.CurrentPageIndex = pageNumber;

            UpdatedContainerSize();
        }

        public AppData.PaginationViewType GetPaginationViewType()
        {
            return paginationComponent.viewType;
        }

        public AppData.PaginationComponent GetPaginationComponent()
        {
            return paginationComponent;
        }

        public void Pagination_GoToItemPage(AppData.UIScreenWidget item)
        {
            paginationComponent.GetItemPageIndex(item.name, onItemPageIndexResults =>
            {
                if (AppData.Helpers.IsSuccessCode(onItemPageIndexResults.resultsCode))
                {
                    if (paginationComponent.CurrentPageIndex != onItemPageIndexResults.data)
                        Pagination_SelectPage(onItemPageIndexResults.data, false);
                    else
                        return;
                }
            });
        }

        public void Pagination_GoToItemPageAsync(AppData.UIScreenWidget item, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (item != null)
            {
                StartCoroutine(GoToItemPageAsync(item, goToPageCallback =>
                {
                    callbackResults = goToPageCallback;
                    callback?.Invoke(callbackResults);

                }));
            }
            else
            {
                callbackResults.results = "Couldn't Go To Items Page Because Item Is Null.";
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;

                callback?.Invoke(callbackResults);
            }
        }

        IEnumerator GoToItemPageAsync(AppData.UIScreenWidget item, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            yield return new WaitUntil(() => GetAssetsLoaded() == true);

            paginationComponent.GetItemPageIndex(item.name, onItemPageIndexResults =>
            {
                if (AppData.Helpers.IsSuccessCode(onItemPageIndexResults.resultsCode))
                {
                    if (paginationComponent.CurrentPageIndex != onItemPageIndexResults.data)
                        Pagination_SelectPage(onItemPageIndexResults.data, false);
                    else
                        return;
                }

                callbackResults.results = onItemPageIndexResults.results;
                callbackResults.resultsCode = onItemPageIndexResults.resultsCode;
            });

            callback?.Invoke(callbackResults);
        }

        public int Pagination_GetItemPageIndex(AppData.UIScreenWidget item)
        {
            int pageIndex = 0;

            paginationComponent.GetItemPageIndex(item.name, onItemPageIndexResults =>
            {
                if (AppData.Helpers.IsSuccessCode(onItemPageIndexResults.resultsCode))
                    pageIndex = onItemPageIndexResults.data;
            });

            return pageIndex;
        }

        public int Pagination_GetItemPageIndex(string itemName)
        {
            int pageIndex = 0;

            paginationComponent.GetItemPageIndex(itemName, onItemPageIndexResults =>
            {
                if (AppData.Helpers.IsSuccessCode(onItemPageIndexResults.resultsCode))
                    pageIndex = onItemPageIndexResults.data;
            });

            return pageIndex;
        }

        public bool Pagination_ItemExistInCurrentPage(AppData.UIScreenWidget item)
        {
            return paginationComponent.ItemExistInCurrentPage(item);
        }

        public bool Pagination_ItemExistInCurrentPage
            (string itemName)
        {
            return paginationComponent.ItemExistInCurrentPage(itemName);
        }

        public void Pagination_CurrentPageContainsSelection(Action<AppData.CallbackData<string>> callback)
        {
            AppData.CallbackData<string> callbackResults = new AppData.CallbackData<string>();

            if (SelectableManager.Instance != null)
            {
                var page = Pagination_GetCurrentPage();

                if (page != null && page.Count > 0)
                {
                    List<string> selectedItemsList = new List<string>();

                    foreach (var widget in page)
                    {
                        SelectableManager.Instance.HasFocusedSelectionInfo(widget.name, selectionCallback => 
                        {
                            if (AppData.Helpers.IsSuccessCode(selectionCallback.resultsCode))
                            {
                                if(!selectedItemsList.Contains(widget.name))
                                    selectedItemsList.Add(widget.name);
                            }
                        });
                    }

                    bool containsSelection = selectedItemsList.Count > 0;
                    string seletionName = (containsSelection)? selectedItemsList[0] : string.Empty;

                    if (containsSelection)
                    {
                        callbackResults.results = "Current Page Contains Selection.";
                        callbackResults.data = seletionName;
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.results = "Current Page Doesn't Contain Selection.";
                        callbackResults.data = default;
                        callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.results = "Current Page Not Found / Page Doesn't Have Items.";
                    callbackResults.data = default;
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }
            }
            else
            {
                callbackResults.results = "Selectable Manager Instance Is Not Yet Initialized.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        public List<AppData.UIScreenWidget> Pagination_GetCurrentPage()
        {
            return paginationComponent.GetCurrentPage();
        }

        public List<AppData.UIScreenWidget> Pagination_GetPage(int pageIndex)
        {
            return paginationComponent.GetPage(pageIndex);
        }

        public int Pagination_GetPageCount()
        {
            return paginationComponent.GetPageCount();
        }

        public void OnLayoutViewChangeSelection(string selectionName) => StartCoroutine(OnLayoutViewChangeSelectionAsync(selectionName));

        IEnumerator OnLayoutViewChangeSelectionAsync(string selectionName)
        {
            yield return new WaitUntil(() => GetAssetsLoaded() == true);

            yield return new WaitForEndOfFrame();

            GetItem<AppData.UIScreenWidget>(selectionName, selectionCallback =>
            {
                if (AppData.Helpers.IsSuccessCode(selectionCallback.resultsCode))
                {
                    LogSuccess($"Go To Selected Item : {selectionName}'s Page", this, () => OnLayoutViewChangeSelectionAsync(selectionName));

                    Pagination_GoToItemPage(selectionCallback.data.item);
                }
                else
                    LogError(selectionCallback.results, this, () => OnLayoutViewChangeSelectionAsync(selectionName));
            });
        }

        public void OnCreateNewPageWidget(Action<AppData.Callback> callback)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            int itemsPerPage = (GetLayout().viewType == AppData.LayoutViewType.ItemView) ? paginationComponent.itemView_ItemsPerPage : paginationComponent.listView_ItemsPerPage;

            if (GetPaginationViewType() == AppData.PaginationViewType.Pager)
            {
                paginationComponent.GetSlotAvailablePageNumber(itemsPerPage, slotAvailable =>
                {
                    if (AppData.Helpers.IsSuccessCode(slotAvailable.resultsCode))
                    {
                        Pagination_GoToPage(slotAvailable.data);

                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                        callbackResults.results = "Can Create New Widget Successfully.";
                    }
                    else
                    {
                        OnSetWidgetsVisibilityState(false);
                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                    }
                });


                callback?.Invoke(callbackResults);
            }

            if (GetPaginationViewType() == AppData.PaginationViewType.Scroller)
            {
                if (GetContentCount() > 0)
                {
                    GetContent(loadedContent =>
                    {
                        if (AppData.Helpers.IsSuccessCode(loadedContent.resultsCode))
                        {
                            int itemsPerPage = (GetLayout().viewType == AppData.LayoutViewType.ItemView) ? paginationComponent.itemView_ItemsPerPage : paginationComponent.listView_ItemsPerPage;

                            if (loadedContent.data.Count >= itemsPerPage)
                            {
                                paginationComponent.GetSlotAvailableOnScroller(loadedContent.data, isLoadAvailableCallback =>
                                {
                                    if (GetLayout().viewType == AppData.LayoutViewType.ItemView)
                                    {
                                        if (AppData.Helpers.IsSuccessCode(isLoadAvailableCallback.resultsCode))
                                        {
                                            OnSnapToBottom();

                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                        }
                                        else
                                        {
                                            Vector2 newContainerSize = container.sizeDelta;

                                            if (orientation == AppData.OrientationType.Horizontal)
                                                newContainerSize.x += GetLayout().layout.itemViewSize.x + (GetLayout().layout.itemViewSpacing.x * 2);

                                            if (orientation == AppData.OrientationType.Vertical)
                                                newContainerSize.y += GetLayout().layout.itemViewSize.y + (GetLayout().layout.itemViewSpacing.y * 2);

                                            container.sizeDelta = newContainerSize;

                                            OnSnapToBottom();

                                            callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                        }
                                    }

                                    if (GetLayout().viewType == AppData.LayoutViewType.ListView)
                                    {
                                        Vector2 newContainerSize = container.sizeDelta;

                                        if (orientation == AppData.OrientationType.Horizontal)
                                            newContainerSize.x += GetLayout().layout.itemViewSize.x + (GetLayout().layout.itemViewSpacing.x * 2);

                                        if (orientation == AppData.OrientationType.Vertical)
                                            newContainerSize.y += GetLayout().layout.itemViewSize.y + (GetLayout().layout.itemViewSpacing.y * 2);

                                        container.sizeDelta = newContainerSize;

                                        OnSnapToBottom();

                                        callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                                    }
                                }); ;
                            }
                            else
                            {
                                callbackResults.results = "Slot Available, No Need To Snap To Bottom.";
                                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
                            }
                        }
                        else
                        {
                            callbackResults.results = loadedContent.results;
                            callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                        }

                    });
                }
                else
                {
                    callbackResults.results = "No Content Fount.";
                    callbackResults.resultsCode = AppData.Helpers.ErrorCode;
                }

                callback?.Invoke(callbackResults);
            }
        }

        public void OnSetWidgetsVisibilityState(bool visible)
        {
            for (int i = 0; i < container.childCount; i++)
                if (container.GetChild(i).GetComponent<AppData.UIScreenWidget>().GetSelectableWidgetType() != AppData.SelectableWidgetType.PlaceHolder)
                    container.GetChild(i).gameObject.SetActive(visible);
        }

        public void SetScreenBounds(RectTransform bounds)
        {
            #region Width

            int width = (int)bounds.rect.width / 2;
            int widthSpacing = (int)layoutComponent.spacing.x / 2;
            int horizontalSpacing = (widthSpacing - (widthSpacing / 2));

            int left = (width + (widthSpacing / 2));
            int right = ((width + (widthSpacing / 2)) * 3) + (horizontalSpacing * 3);

            screenBounds.left = left;
            screenBounds.right = right;

            #endregion

            #region Height

            int height = (int)bounds.rect.height / 2;
            int heightSpacingDevided = (int)layoutComponent.spacing.y / 2;
            int heightSpacingMultiplied = (int)layoutComponent.spacing.y * 4;
            int heightSpacing = (int)layoutComponent.spacing.y;
            int verticalSpacingTop = (heightSpacingDevided - (heightSpacingDevided / 2));
            int verticalSpacingBottom = heightSpacingMultiplied + heightSpacing;

            int top = -(height + (verticalSpacingTop / 2));
            int layoutHeight = GetUILayoutDimension(SceneAssetsManager.Instance.GetFolderStructureData().GetLayoutViewType()).containerDimensions.height;
            int bottom = (-layoutHeight) - (-(height + verticalSpacingBottom));

            screenBounds.top = top;
            screenBounds.bottom = bottom;

            #endregion
        }

        public AppData.UILayoutDimensions GetUILayoutDimension(AppData.LayoutViewType layoutView)
        {
            return widgetsUILayoutDimensionList.Find(layoutDimensions => layoutDimensions.layoutView == layoutView);
        }

        public Vector2 GetLayoutSpacing()
        {
            return layoutComponent.spacing;
        }

        public AppData.ScreenBounds GetScreenBounds()
        {
            return screenBounds;
        }

        public Transform GetContentContainer()
        {
            return transform;
        }

        public Vector2 GetCurrentLayoutWidgetDimensions()
        {
            return GetLayout().layout.itemViewSize;
        }

        public int GetLastContentIndex()
        {
            return GetContentCount();
        }

        #endregion
    }
}
