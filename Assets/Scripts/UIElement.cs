using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    [SerializeField]
    private float _showDelay;
    [SerializeField]
    private float _hideDelay;

    private UIPanel _parentPanel;

    private UITransition _transition;
    private WaitForSeconds _waitForShowDelay;
    private WaitForSeconds _waitForHideDelay;

    public void Init(UIPanel container)
    {
        _parentPanel = container;

        _transition = GetComponent<UITransition>();

        if (_transition == null)
        {
            transform.localScale = Vector3.zero;
        }
        _transition.Init(container);

        _waitForHideDelay = new WaitForSeconds(_hideDelay);
        _waitForShowDelay = new WaitForSeconds(_showDelay);
    }

    public void Show()
    {
        StartCoroutine(IE_Show());
    }

    public void Hide()
    {
        StartCoroutine(IE_Hide());
    }

    private IEnumerator IE_Show()
    {
        _transition?.PreShowSetup();
        yield return _waitForShowDelay;
        if (_transition == null)
        {
            transform.localScale = Vector3.one;

            _parentPanel.NotifyElementShowed();
            yield break;
        }

        _transition.ShowTransition();
    }

    private IEnumerator IE_Hide()
    {
        yield return _waitForHideDelay;
        if (_transition == null)
        {
            transform.localScale = Vector3.zero;

            _parentPanel.NotifyElementHided();
            yield break;
        }

        _transition.HideTransition();
    }
}
