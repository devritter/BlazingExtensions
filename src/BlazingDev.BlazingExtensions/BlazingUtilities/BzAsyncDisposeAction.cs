using System;
using System.Threading.Tasks;

namespace BlazingDev.BlazingExtensions.BlazingUtilities;

public class BzAsyncDisposeAction(Func<Task> disposeAction) : IAsyncDisposable
{
    private bool _isDisposed;

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }
        
        await disposeAction();
        _isDisposed = true;
    }
}