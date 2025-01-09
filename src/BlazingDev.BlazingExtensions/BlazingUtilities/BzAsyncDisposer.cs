using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingDev.BlazingExtensions.BlazingUtilities;

public class BzAsyncDisposer : IAsyncDisposable
{
    public bool IsDisposed { get; private set; }
    private List<IDisposable> _syncDisposables = new();
    private List<IAsyncDisposable> _asyncDisposables = new();

    public void Add(IDisposable disposable)
    {
        ThrowIfDisposed();
        _syncDisposables.Add(disposable);
    }

    public void Add(IEnumerable<IDisposable> disposables)
    {
        ThrowIfDisposed();
        _syncDisposables.AddRange(disposables);
    }

    public void Add(Action disposeAction)
    {
        ThrowIfDisposed();
        _syncDisposables.Add(new BzDisposeAction(disposeAction));
    }

    public void Add(IAsyncDisposable disposable)
    {
        ThrowIfDisposed();
        _asyncDisposables.Add(disposable);
    }

    public void Add(IEnumerable<IAsyncDisposable> disposables)
    {
        ThrowIfDisposed();
        _asyncDisposables.AddRange(disposables);
    }

    public void Add(Func<Task> disposeAction)
    {
        ThrowIfDisposed();
        _asyncDisposables.Add(new BzAsyncDisposeAction(disposeAction));
    }

    public void Add<T>(T disposable) where T : IDisposable, IAsyncDisposable
    {
        ThrowIfDisposed();
        _asyncDisposables.Add(disposable);
    }

    private void ThrowIfDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(nameof(BzAsyncDisposer));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (IsDisposed)
        {
            return;
        }

        IsDisposed = true;

        foreach (var item in _syncDisposables)
        {
            item.Dispose();
        }

        foreach (var item in _asyncDisposables)
        {
            // Stryker disable once Boolean
            await item.DisposeAsync().ConfigureAwait(false);
        }
    }
}