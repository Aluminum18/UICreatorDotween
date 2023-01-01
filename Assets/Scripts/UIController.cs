using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private bool _showInitUIFromStart = true;
    [SerializeField]
    private List<UIPanel> _UIPanels;

    // Store UI panel following opening order
    private Stack<UIPanel> _panelStack = new Stack<UIPanel>();

    public void ShowInitPanels()
    {
        InitAllUIPanels().Forget();
    }

    public bool IsOnTop(UIPanel panel)
    {
        if (_panelStack.Count == 0)
        {
            return false;
        }

        var topPanel = _panelStack.Peek();

        return panel.GetInstanceID() == topPanel.GetInstanceID();
    }

    public void PushToStack(UIPanel panel)
    {
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
        if (recentPanel.Status == UIPanel.PanelStatus.IsOpening)
        {
            return;
        }
        PopFromStack();
    }

    public void CloseAllButInitPanels()
    {
        while (_panelStack.Count > 0)
        {
            var panel = _panelStack.Peek();
            if (panel.ShowFromStart)
            {
                break;
            }

            panel.Close();
        }
    }

    private void Start()
    {
        if (!_showInitUIFromStart)
        {
            return;
        }

        InitAllUIPanels().Forget();
    }

    private async UniTaskVoid InitAllUIPanels()
    {
        for (int i = 0; i < _UIPanels.Count; i++)
        {
            var panel = _UIPanels[i];
            if (panel == null)
            {
                Debug.LogError($"Panel at index [{i}] is null");
                continue;
            }

            panel.Init(this);

            if (panel.ShowFromStart)
            {
                // the init frame handles very heavy logic, showing animation from beginning often causes lagging
                await UniTask.DelayFrame(2);
                panel.Open();
            }
        }
    }
}
