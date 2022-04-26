﻿using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(UIElement))]
public class UITransition : MonoBehaviour
{
    [SerializeField]
    private TransitionType _transitionType;

    [SerializeField]
    private float _showDelay = 0f;
    [SerializeField]
    private Vector3 _showFrom;
    [SerializeField]
    private Vector3 _showTo;
    [SerializeField]
    private float _showTransitionTime;
    [SerializeField]
    private Ease _showEaseType;

    [SerializeField]
    private float _hideDelay = 0f;
    [SerializeField]
    private Vector3 _hideFrom;
    [SerializeField]
    private Vector3 _hideTo;
    [SerializeField]
    private float _hideTransitionTime;
    [SerializeField]
    private Ease _hideEaseType;

    [SerializeField]
    private UnityEvent _onStartShow;
    [SerializeField]
    private UnityEvent _onFinishShow;
    [SerializeField]
    private UnityEvent _onStartHide;
    [SerializeField]
    private UnityEvent _onFinishHide;

    private UIPanel _parentPanel;

    private CanvasGroup _canvasGroup;

    public void Init(UIPanel parent)
    {
        _parentPanel = parent;
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void PreShowSetup()
    {
        switch (_transitionType)
        {
            case TransitionType.Fade:
                {
                    if (_canvasGroup == null)
                    {
                        Debug.LogWarning("In order to use Fade Transition, add CanvasGroup Component to this object", this);
                        return;
                    }

                    _canvasGroup.alpha = _showFrom.x;
                    break;
                }
            case TransitionType.Move:
                {
                    transform.localPosition = _showFrom;
                    break;
                }
            case TransitionType.Zoom:
                {
                    transform.localScale = _showFrom;
                    break;
                }
            default:
                {
                    return;
                }
        }
    }

    public async void ShowTransition()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_showDelay));
        _onStartShow.Invoke();

        switch (_transitionType)
        {
            case TransitionType.Fade:
                {
                    if (_canvasGroup == null)
                    {
                        Debug.LogWarning("In order to use Fade Transition, add CanvasGroup Component to this object", this);
                        return;
                    }

                    _canvasGroup.DOFade(_showTo.x, _showTransitionTime).SetEase(_showEaseType).onComplete = NotifyFinishShow;
                    break;
                }
            case TransitionType.Move:
                {
                    transform.DOLocalMove(_showTo, _showTransitionTime).SetEase(_showEaseType).onComplete = NotifyFinishShow;
                    break;
                }
            case TransitionType.Zoom:
                {
                    transform.DOScale(_showTo.x, _showTransitionTime).SetEase(_showEaseType).onComplete = NotifyFinishShow;
                    break;
                }
            default:
                {
                    NotifyFinishShow();
                    return;
                }
        }
    }

    public async void HideTransition()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_hideDelay));
        _onStartHide.Invoke();
        
        switch (_transitionType)
        {
            case TransitionType.Fade:
                {
                    if (_canvasGroup == null)
                    {
                        Debug.LogWarning("In order to use Fade Transition, add CanvasGroup Component to this object", this);
                        return;
                    }

                    //hideDescription = LeanTween.alphaCanvas(_canvasGroup, _from.x, _hideTransitionTime);
                    _canvasGroup.DOFade(_hideTo.x, _hideTransitionTime).SetEase(_hideEaseType).onComplete = NotifyFinishHide;
                    break;
                }
            case TransitionType.Move:
                {
                    transform.DOLocalMove(_hideTo, _hideTransitionTime).SetEase(_hideEaseType).onComplete = NotifyFinishHide;
                    break;
                }
            case TransitionType.Zoom:
                {
                    transform.DOScale(_hideTo, _hideTransitionTime).SetEase(_hideEaseType).onComplete = NotifyFinishHide;
                    break;
                }
            default:
                {
                    NotifyFinishHide();
                    return;
                }
        }
    }

    private void NotifyFinishShow()
    {
        _onFinishShow.Invoke();
        _parentPanel.NotifyElementShowed();
    }

    private void NotifyFinishHide()
    {
        _onFinishHide.Invoke();
        _parentPanel.NotifyElementHided();
    }
}

public enum TransitionType
{
    None = 0,
    Move = 1,
    Zoom = 2,
    Fade = 3
}
