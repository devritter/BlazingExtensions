namespace BlazingDev.BlazingExtensions.Tests;

public class DisposeActionTests
{
    [Fact]
    public void ActionIsCalledOnDispose()
    {
        var called = false;
        using (new DisposeAction(() => called = true))
        {
        }
        Assert.True(called);
    }

    [Fact]
    public void ActionIsOnlyCalledOnce()
    {
        var count = 0;
        var disposable = new DisposeAction(() => count++);
        disposable.Dispose();
        disposable.Dispose();
        Assert.Equal(1, count);
    }
}