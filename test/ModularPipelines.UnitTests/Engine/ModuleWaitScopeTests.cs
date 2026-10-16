using ModularPipelines.Context;
using ModularPipelines.Engine;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleWaitScopeTests
{
    [Test]
    public async Task Wait_Attribution_Flows_To_Child_Tasks_And_Ends_With_Scope()
    {
        var scheduler = new Mock<IModuleScheduler>();
        var module = new TestModule();
        var dependency = new TestModule();
        var result = new TaskCompletionSource();
        using (new ModuleWaitScope(module, scheduler.Object))
        {
            await Task.Run(() => ModuleWaitScope.RegisterWait(dependency, result.Task));
        }

        ModuleWaitScope.RegisterWait(dependency, result.Task);
        scheduler.Verify(x => x.RegisterResultWait(module, dependency, result.Task), Times.Once);
    }

    [Test]
    public async Task Flowed_Scope_Becomes_Inactive_When_Owner_Exits()
    {
        var scheduler = new Mock<IModuleScheduler>();
        var module = new TestModule();
        var result = new TaskCompletionSource();
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task child;

        using (new ModuleWaitScope(module, scheduler.Object))
        {
            child = Task.Run(async () =>
            {
                await release.Task;
                ModuleWaitScope.RegisterWait(new TestModule(), result.Task);
            });
        }

        release.SetResult();
        await child.WaitAsync(TestHelpers.TestHostSettings.DefaultTestTimeout);
        scheduler.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Nested_Scope_Restores_Owner_And_Completed_Results_Do_Not_Register()
    {
        var outerScheduler = new Mock<IModuleScheduler>();
        var innerScheduler = new Mock<IModuleScheduler>();
        var outer = new TestModule();
        var inner = new TestModule();
        var dependency = new TestModule();
        var result = new TaskCompletionSource();

        using (new ModuleWaitScope(outer, outerScheduler.Object))
        {
            using (new ModuleWaitScope(inner, innerScheduler.Object))
            {
                ModuleWaitScope.RegisterWait(dependency, result.Task);
            }

            ModuleWaitScope.RegisterWait(dependency, result.Task);
            ModuleWaitScope.RegisterWait(dependency, Task.CompletedTask);
        }

        outerScheduler.Verify(x => x.RegisterResultWait(outer, dependency, result.Task), Times.Once);
        innerScheduler.Verify(x => x.RegisterResultWait(inner, dependency, result.Task), Times.Once);
        outerScheduler.VerifyNoOtherCalls();
        innerScheduler.VerifyNoOtherCalls();
        await Task.CompletedTask;
    }

    private sealed class TestModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }
}
