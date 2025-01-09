using BlazingDev.BlazingExtensions.BlazingUtilities;

namespace BlazingDev.BlazingExtensions.Tests.BlazingUtilities;

public class BzAsyncDisposerTests
{
    [Fact]
    public async Task DisposesPassedItems()
    {
        // Arrange
        var disposer = new BzAsyncDisposer();
        var disposable1 = new DummyDisposable();
        var disposable2 = new DummyAsyncDisposable();
        var action1Count = 0;
        var action2Count = 0;
        var action1 = () =>
        {
            action1Count++;
        };
        var action2 = () =>
        {
            action2Count++;
            return Task.CompletedTask;
        };

        // Act
        disposer.Add(disposable1);
        disposer.Add(disposable2);
        disposer.Add(action1);
        disposer.Add(action2);
        await disposer.DisposeAsync();

        disposable1.IsDisposed.Should().BeTrue();
        disposable2.IsDisposed.Should().BeTrue();
        action1Count.Should().Be(1);
        action2Count.Should().Be(1);
    }

    [Fact]
    public void Add_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var disposer = new BzAsyncDisposer();
        disposer.DisposeAsync().AsTask().Wait();

        // Act & Assert
        Assert(() => disposer.Add(new DummyDisposable()));
        Assert(() => disposer.Add([new DummyDisposable()]));
        Assert(() => disposer.Add(new DummyAsyncDisposable()));
        Assert(() => disposer.Add([new DummyAsyncDisposable()]));
        Assert(() => disposer.Add(() => Console.WriteLine()));
        Assert(() => disposer.Add(() => Task.FromResult(true)));
        Assert(() => disposer.Add(Stream.Null));

        void Assert(Action action)
        {
            action.Should().Throw<ObjectDisposedException>().WithMessage(nameof(BzAsyncDisposer).Wrap("*"));
        }
    }

    [Fact]
    public async Task DisposeAsync_CalledMultipleTimes_ShouldOnlyDisposeOnce()
    {
        // Arrange
        var disposer = new BzAsyncDisposer();
        var disposable = new DummyDisposable();
        disposer.Add(disposable);

        // Act
        await disposer.DisposeAsync();
        disposer.IsDisposed.Should().BeTrue();
        await disposer.DisposeAsync();

        // Assert
        disposable.DisposeCount.Should().Be(1);
    }

    [Fact]
    public async Task MultipleItemsCanBeAddedAtOnce()
    {
        // Arrange
        var disposer = new BzAsyncDisposer();
        var syncDisposable = new DummyDisposable();
        var asyncDisposable = new DummyAsyncDisposable();
        disposer.Add([syncDisposable, syncDisposable, syncDisposable]);
        disposer.Add([asyncDisposable, asyncDisposable, asyncDisposable]);

        // Act
        await disposer.DisposeAsync();

        // Assert
        syncDisposable.DisposeCount.Should().Be(3);
        asyncDisposable.DisposeCount.Should().Be(3);
    }

    [Fact]
    public async Task ObjectsImplementingBothInterfacesCanBeAdded()
    {
        // Arrange
        var disposer = new BzAsyncDisposer();
        var stream = new DummyStream();
        disposer.Add(stream);

        // Act
        await disposer.DisposeAsync();

        // Assert
        stream.AsyncDisposeCalled.Should().BeTrue();
    }

    private class DummyDisposable : IDisposable
    {
        public bool IsDisposed { get; private set; }
        public int DisposeCount { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
            DisposeCount++;
        }
    }

    private class DummyAsyncDisposable : IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }
        public int DisposeCount { get; private set; }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            DisposeCount++;
            return ValueTask.CompletedTask;
        }
    }

    private class DummyStream : MemoryStream
    {
        public bool AsyncDisposeCalled { get; set; }

        public override ValueTask DisposeAsync()
        {
            AsyncDisposeCalled = true;
            return base.DisposeAsync();
        }
    }
}