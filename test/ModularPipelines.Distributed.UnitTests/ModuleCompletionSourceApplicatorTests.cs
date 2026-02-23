using ModularPipelines.Distributed;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests;

public class ModuleCompletionSourceApplicatorTests
{
    private class SimpleResult
    {
        public string Message { get; set; } = string.Empty;
    }

    private class TestModule : Module<SimpleResult>
    {
        protected internal override Task<SimpleResult?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<SimpleResult?>(new SimpleResult { Message = "test" });
    }

    private static ModuleResult<T> CreateSuccessResult<T>(T value, string moduleName) where T : notnull
    {
        var now = DateTimeOffset.UtcNow;
        return new ModuleResult<T>.Success(value)
        {
            ModuleName = moduleName,
            ModuleTypeName = moduleName,
            ModuleDuration = TimeSpan.FromMilliseconds(100),
            ModuleStart = now,
            ModuleEnd = now.AddMilliseconds(100),
            ModuleStatus = Status.Successful,
        };
    }

    [Test]
    public async Task TryApply_Sets_Result_On_Module_CompletionSource()
    {
        // Arrange
        var module = new TestModule();
        var result = CreateSuccessResult(new SimpleResult { Message = "applied" }, "TestModule");

        // Act
        var applied = ModuleCompletionSourceApplicator.TryApply(module, result);

        // Assert
        await Assert.That(applied).IsTrue();

        // The ResultTask should now be completed with the applied result
        var moduleResult = await ((IModule)module).ResultTask;
        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.IsSuccess).IsTrue();
    }

    [Test]
    public async Task TryApply_Is_Idempotent_On_Second_Call()
    {
        // Arrange
        var module = new TestModule();
        var result1 = CreateSuccessResult(new SimpleResult { Message = "first" }, "TestModule");
        var result2 = CreateSuccessResult(new SimpleResult { Message = "second" }, "TestModule");

        // Act — first apply succeeds
        var applied1 = ModuleCompletionSourceApplicator.TryApply(module, result1);

        // Second apply — TrySetResult is idempotent, should not throw
        var applied2 = ModuleCompletionSourceApplicator.TryApply(module, result2);

        // Assert — both calls return true (property + method found), but only first value sticks
        await Assert.That(applied1).IsTrue();
        await Assert.That(applied2).IsTrue();

        var moduleResult = await ((IModule)module).ResultTask;
        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.ModuleName).IsEqualTo("TestModule");
    }
}
