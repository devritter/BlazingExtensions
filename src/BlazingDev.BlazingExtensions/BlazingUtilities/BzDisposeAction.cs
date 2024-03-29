using System;

namespace BlazingDev.BlazingExtensions.BlazingUtilities;

/// <summary>
/// Executes the passed action on disposal
/// </summary>
public class BzDisposeAction(Action action) : IDisposable
{
    private bool _isDisposed;

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }
        
        action();
        _isDisposed = true;
    }
}