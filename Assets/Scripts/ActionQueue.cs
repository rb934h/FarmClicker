using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ActionQueue
{
    private readonly Queue<Func<UniTask>> _actions = new();
    private bool _isProcessing = false;

    public void Enqueue(Func<UniTask> action)
    {
        _actions.Enqueue(action);
        if (!_isProcessing)
            _ = ProcessQueue();
    }

    private async UniTask ProcessQueue()
    {
        _isProcessing = true;

        while (_actions.Count > 0)
        {
            var nextAction = _actions.Dequeue();
            await nextAction();
        }

        _isProcessing = false;
    }
}