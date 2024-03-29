using BlazingDev.BlazingExtensions.BlazingUtilities;

namespace BlazingDev.BlazingExtensions.Tests;

public class BzAsyncDisposeActionTests
{
    [Fact]
    public async Task ActionIsCalledOnDispose()
    {
        var called = false;
        await using (new BzAsyncDisposeAction(async () => called = true))
        {
        }
        Assert.True(called);
    }

    [Fact]
    public async Task ActionIsOnlyCalledOnce()
    {
        var count = 0;
        var disposable = new BzAsyncDisposeAction(async () => count++);
        await disposable.DisposeAsync();
        await disposable.DisposeAsync();
        Assert.Equal(1, count);
    }
}