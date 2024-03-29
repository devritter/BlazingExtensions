using BlazingDev.BlazingExtensions.BlazingUtilities;

namespace BlazingDev.BlazingExtensions.Tests.BlazingUtilities;

public class BzDisposeActionTests
{
    [Fact]
    public void ActionIsCalledOnDispose()
    {
        var called = false;
        using (new BzDisposeAction(() => called = true))
        {
        }

        Assert.True(called);
    }

    [Fact]
    public void ActionIsOnlyCalledOnce()
    {
        var count = 0;
        var disposable = new BzDisposeAction(() => count++);
        disposable.Dispose();
        disposable.Dispose();
        Assert.Equal(1, count);
    }
}