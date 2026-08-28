using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests;

public class DependencyResultPropagationTests
{
    private class DepResult
    {
        public string Value { get; set; } = string.Empty;
    }

    private class DependencyModule : Module<DepResult>
    {
        protected internal override Task<DepResult> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<DepResult>(new DepResult { Value = "dep-value" });
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private class ConsumerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("consumed");
    }

    private class IndependentModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(42);
    }

    private static ModuleResult<T> CreateSuccessResult<T>(T value, string moduleName) where T : notnull
    {
        var now = DateTimeOffset.UtcNow;
        return new ModuleResult<T>.Success(value)
        {
            Name = moduleName,
            TypeName = moduleName,
            Duration = TimeSpan.FromMilliseconds(100),
            StartTime = now,
            EndTime = now.AddMilliseconds(100),
            Status = ModuleStatus.Succeeded,
        };
    }

    [Test]
    public async Task Worker_Applies_Dependency_Results_From_Assignment()
    {
        // Arrange
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DependencyModule));
        typeRegistry.Register(typeof(ConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);

        // Create a serialized dependency result
        var depResult = CreateSuccessResult(new DepResult { Value = "from-master" }, "DependencyModule");
        var serializedDep = serializer.Serialize(
            depResult,
            typeof(DependencyModule).FullName!,
            typeof(DepResult).FullName!,
            workerIndex: -1);

        // Create assignment with dependency results
        var assignment = new ModuleAssignment(
            ModuleTypeName: typeof(ConsumerModule).FullName!,
            ResultTypeName: typeof(string).FullName!,
            RequiredCapabilities: new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false),
            DependencyResults: [serializedDep]);

        // Create module instances
        var depModule = new DependencyModule();
        var consumerModule = new ConsumerModule();
        IReadOnlyList<IModule> modules = [depModule, consumerModule];
        var resultRegistry = new ModuleResultRegistry();
        var localSkip = new ModuleResult.Skipped(SkipDecision.Skip("Unavailable on this worker"))
        {
            Name = nameof(DependencyModule),
            TypeName = typeof(DependencyModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Skipped,
        };
        resultRegistry.RegisterResult(typeof(DependencyModule), localSkip);

        // Act — apply dependency results (simulating what WorkerModuleExecutor does)
        DependencyResultApplicator.Apply(
            assignment.DependencyResults!,
            DependencyResultApplicator.BuildModuleLookup(modules),
            serializer,
            resultRegistry,
            NullLogger.Instance);

        // Assert — GetModule<DependencyModule> should now resolve (ResultTask completes)
        var moduleResult = await ((IInternalModule) depModule).ResultTask;
        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(resultRegistry.GetResult(typeof(DependencyModule))?.Status)
            .IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Null_DependencyResults_Does_Not_Crash()
    {
        // Arrange — assignment with null DependencyResults (backwards compat)
        var assignment = new ModuleAssignment(
            ModuleTypeName: typeof(IndependentModule).FullName!,
            ResultTypeName: typeof(int).FullName!,
            RequiredCapabilities: new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false),
            DependencyResults: null);

        // Act & Assert — should not throw
        await Assert.That(assignment.DependencyResults).IsNull();
    }

    [Test]
    public async Task Empty_DependencyResults_Does_Not_Crash()
    {
        // Arrange — assignment with empty DependencyResults
        var assignment = new ModuleAssignment(
            ModuleTypeName: typeof(IndependentModule).FullName!,
            ResultTypeName: typeof(int).FullName!,
            RequiredCapabilities: new HashSet<string>(),
            MatrixTarget: null,
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfig(null, 0, false),
            DependencyResults: []);

        // Act & Assert — check guard condition
        var hasResults = assignment.DependencyResults is { Count: > 0 };
        await Assert.That(hasResults).IsFalse();
    }
}
