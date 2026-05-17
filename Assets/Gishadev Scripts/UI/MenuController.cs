using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace gishadev.tools.UI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Page initialPage;
        [SerializeField] private GameObject firstFocusItem;
        [SerializeField] private bool zeroPagesAllowed;
        [SerializeField] private bool popOnSamePagePush = true;

        private Stack<Page> _pageStack = new();
        private Canvas _rootCanvas;

        protected virtual void Awake()
        {
            _rootCanvas = GetComponent<Canvas>();
        }

        protected virtual void Start()
        {
            if (firstFocusItem != null)
                EventSystem.current.SetSelectedGameObject(firstFocusItem);

            if (initialPage != null)
                PushPage(initialPage);
        }

        private void OnCancel()
        {
            if (_rootCanvas.enabled && _rootCanvas.gameObject.activeInHierarchy)
                if (_pageStack.Count != 0)
                    PopPage();
        }

        public bool IsPageInStack(Page page)
        {
            return _pageStack.Contains(page);
        }

        public bool IsPageOnTopOfStack(Page page)
        {
            return _pageStack.Count > 0 && page == _pageStack.Peek();
        }

        public void PushPage(Page page)
        {
            if (_pageStack.Count > 0)
            {
                Page currentPage = _pageStack.Peek();

                if (currentPage == page && popOnSamePagePush)
                {
                    page.Enter();
                    PopPage();
                    return;
                }

                if (currentPage.ExitOnNewPagePush)
                    PopPage();
            }

            page.Enter();
            _pageStack.Push(page);
        }

        public void PopPage()
        {
            if (_pageStack.Count == 0)
                return;

            if (_pageStack.Count > 1 || zeroPagesAllowed)
            {
                Page page = _pageStack.Pop();
                page.Exit();


                // Page newCurrentPage = _pageStack.Peek();
                // if (newCurrentPage.ExitOnNewPagePush)
                //     newCurrentPage.Enter();
            }
            else
                Debug.LogWarning("Trying to pop a page but only 1 page remains in the stack!");
        }

        public void PopAllPages()
        {
            for (int i = 1; i < _pageStack.Count; i++)
                PopPage();
        }
    }
}