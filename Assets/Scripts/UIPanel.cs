using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private List<UIElement> _elements;
    [SerializeField]
    private bool _showFromStart;

    [Header("UnityEvents")]
    [SerializeField]
    private UnityEvent _onStartShow;
    [SerializeField]
    private UnityEvent _onAllElementsShown;
    [SerializeField]
    private UnityEvent _onAllElementsHided;

    private UIController _uiController;
    private Canvas _canvas;
    private GraphicRaycaster _rayCaster;

    public bool IsOpening { get; set; }

    [SerializeField]
    private int _showedElements = 0;

    private UIPanel _showNextAfterThisHide;

    public bool ShowFromStart
    {
        get
        {
            return _showFromStart;
        }
    }

    public void Init(UIController controller)
    {
        _canvas = GetComponent<Canvas>();
        if (_canvas == null)
        {
            Debug.LogError($"Missing canvas of [{gameObject.name}]", this);
            return;
        }

        _rayCaster = GetComponent<GraphicRaycaster>();
        if (_rayCaster == null)
        {
            Debug.LogWarning($"Missing graphic raycaster of [{gameObject.name}]", this);
        }
        else
        {
            _rayCaster.enabled = false;
        }

        _uiController = controller;

        if (!ValidateElements())
        {
            return;
        }

        _canvas.enabled = false;

        for (int i = 0; i < _elements.Count; i++)
        {
            _elements[i].Init(this);
        }
    }

    public void Open()
    {
        if (IsOpening)
        {
            return;
        }

        if (!ValidateElements())
        {
            return;
        }

        _canvas.enabled = true;
        transform.SetAsLastSibling();

        _onStartShow.Invoke();
        for (int i = 0; i < _elements.Count; i++)
        {
            _elements[i].Show();
        }

        IsOpening = true;
        _uiController?.PushToStack(this);
    }

    public void Close()
    {
        if (!IsOpening)
        {
            return;
        }

        if (!ValidateElements())
        {
            return;
        }

        // panel is on top, pop it.
        // if it is not on top, just close and it will be pop later
        if (transform.GetSiblingIndex() == transform.parent.childCount - 1)
        {
            _uiController?.PopFromStack();
        }

        _rayCaster.enabled = false;

        for (int i = 0; i < _elements.Count; i++)
        {
            _elements[i].Hide();
        }

        IsOpening = false;
    }

    public void CloseAndOpenOther(UIPanel other)
    {
        Close();

        _showNextAfterThisHide = other;
    }

    public void NotifyElementShowed()
    {
        _showedElements++;
        if (_showedElements == _elements.Count)
        {
            _rayCaster.enabled = true;
            _onAllElementsShown?.Invoke();
        }
    }

    public void NotifyElementHided()
    {
        _showedElements--;
        if (_showedElements != 0)
        {
            return;
        }

        _canvas.enabled = false;
        _onAllElementsHided?.Invoke();
        transform.SetAsFirstSibling();

        if (_showNextAfterThisHide == null)
        {
            return;
        }

        _showNextAfterThisHide.Open();
        _showNextAfterThisHide = null;
    }

    public void SetInteractable(bool interactable)
    {
        _rayCaster.enabled = interactable;
    }

    public void SetMoveToFront()
    {
        transform.SetAsLastSibling();
    }

    private bool ValidateElements()
    {
        if (_elements.Count == 0)
        {
            Debug.LogWarning($"No element found in {gameObject.name}", this);
            return false;
        }

        return true;
    }
}
