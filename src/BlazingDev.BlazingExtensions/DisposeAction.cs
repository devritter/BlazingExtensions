using System;

namespace BlazingDev.BlazingExtensions;

/// <summary>
/// Executes the passed action on disposal
/// </summary>
public class DisposeAction(Action action) : IDisposable
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