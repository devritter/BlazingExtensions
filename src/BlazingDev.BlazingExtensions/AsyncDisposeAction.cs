using System;
using System.Threading.Tasks;

namespace BlazingDev.BlazingExtensions;

public class AsyncDisposeAction : IAsyncDisposable
{
    private readonly Func<Task> _taskAction;
    private bool _isDisposed;

    public AsyncDisposeAction(Func<Task> disposeAction)
    {
        _taskAction = disposeAction;
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }
        
        await _taskAction();
        _isDisposed = true;
    }
}