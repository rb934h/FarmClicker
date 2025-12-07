using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ActionQueue
{
    private readonly MonoBehaviour _owner;
    private readonly Queue<Func<CancellationToken, UniTask>> _queue = new();
    
    private CancellationTokenSource _currentCts;
    private bool _isExecuting;

    public ActionQueue(MonoBehaviour owner)
    {
        _owner = owner;
    }

    public void Enqueue(Func<CancellationToken, UniTask> action)
    {
        _queue.Enqueue(action);

        if (!_isExecuting)
            RunNext().Forget();
    }

    public void CancelCurrent()
    {
        _currentCts?.Cancel();
    }

    public void Clear()
    {
        _queue.Clear();
        CancelCurrent();
    }

    private async UniTaskVoid RunNext()
    {
        _isExecuting = true;

        while (_queue.Count > 0)
        {
            var action = _queue.Dequeue();
            
            _currentCts?.Dispose();
            _currentCts = new CancellationTokenSource();
            
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                _currentCts.Token,
                _owner.GetCancellationTokenOnDestroy()
            );
            var token = linkedCts.Token;

            try
            {
                await action(token);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Token was cancelled");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                linkedCts.Dispose();
            }
        }

        _isExecuting = false;
    }
}