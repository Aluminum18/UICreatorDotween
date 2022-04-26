using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private List<UIPanel> _UIPanels;

    // Store UI panel following opening order
    private Stack<UIPanel> _panelStack = new Stack<UIPanel>();

    public void PushToStack(UIPanel panel)
    {
        if (_panelStack.Count > 0)
        {
            var recentPanel = _panelStack.Peek();
            recentPanel.SetInteractable(!recentPanel.IsOpening);
        }

        _panelStack.Push(panel);
    }

    public void PopFromStack()
    {
        if (_panelStack.Count == 0)
        {
            return;
        }

        _panelStack.Pop();

        if (_panelStack.Count == 0)
        {
            return;
        }

        var recentPanel = _panelStack.Peek();
        if (recentPanel.IsOpening)
        {
            recentPanel.SetInteractable(true);
        }
        else
        {
            PopFromStack();
        }
    }

    private void Start()
    {
        InitAllUIPanels();
    }

    private void InitAllUIPanels()
    {
        for (int i = 0; i < _UIPanels.Count; i++)
        {
            var panel = _UIPanels[i];
            if (panel == null)
            {
                // Log
                continue;
            }

            panel.Init(this);

            if (panel.ShowFromStart)
            {
                // the init frame handles very 
                Observable.TimerFrame(2).Subscribe(_ =>
                {
                    panel.Open();
                });
            }
        }
    }
}
