using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Moq;

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
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<DepResult>(new DepResult { Value = "dep-value" });
    }

    [ModularPipelines.DependsOn<DependencyModule>]
    private class ConsumerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("consumed");
    }

    private class IndependentModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
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
    public async Task Worker_Fetches_And_Applies_Dependency_Result_Reference_Once_Per_Run()
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

        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        coordinator
            .Setup(x => x.WaitForResultAsync(typeof(DependencyModule).FullName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serializedDep);

        var assignment = new ModuleAssignment(
            ModuleTypeName: typeof(ConsumerModule).FullName!,
            ResultTypeName: typeof(string).FullName!,
            RequiredCapabilities: new HashSet<Capability>(),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false),
            DependencyResultReferences:
            [
                new DependencyResultReference(typeof(DependencyModule).FullName!, IsAvailable: true),
            ]);

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

        var resultCache = new DependencyResultCache(coordinator.Object, CancellationToken.None);
        var moduleLookup = DependencyResultApplicator.BuildModuleLookup(modules);

        await DependencyResultApplicator.FetchAndApplyAsync(
            assignment.DependencyResultReferences!,
            resultCache,
            moduleLookup,
            serializer,
            resultRegistry,
            NullLogger.Instance);
        await DependencyResultApplicator.FetchAndApplyAsync(
            assignment.DependencyResultReferences!,
            resultCache,
            moduleLookup,
            serializer,
            resultRegistry,
            NullLogger.Instance);

        // Assert — GetModule<DependencyModule> should now resolve (ResultTask completes)
        var moduleResult = await ((IInternalModule) depModule).ResultTask;
        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(resultRegistry.GetResult(typeof(DependencyModule))?.Status)
            .IsEqualTo(ModuleStatus.Succeeded);
        coordinator.Verify(
            x => x.WaitForResultAsync(typeof(DependencyModule).FullName!, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Null_Dependency_Result_References_Does_Not_Crash()
    {
        // Arrange — assignment with null DependencyResults (backwards compat)
        var assignment = new ModuleAssignment(
            ModuleTypeName: typeof(IndependentModule).FullName!,
            ResultTypeName: typeof(int).FullName!,
            RequiredCapabilities: new HashSet<Capability>(),
            AssignedAt: DateTimeOffset.UtcNow,
            Configuration: new ModuleAssignmentConfiguration(null, false),
            DependencyResultReferences: null);

        // Act & Assert — should not throw
        await Assert.That(assignment.DependencyResultReferences).IsNull();
    }

    [Test]
    public async Task Unavailable_Dependency_Result_Reference_Does_Not_Fetch()
    {
        var coordinator = new Mock<IDistributedWorkerCoordinator>();

        await DependencyResultApplicator.FetchAndApplyAsync(
            [new DependencyResultReference(typeof(DependencyModule).FullName!, IsAvailable: false)],
            new DependencyResultCache(coordinator.Object, CancellationToken.None),
            DependencyResultApplicator.BuildModuleLookup([new DependencyModule()]),
            new ModuleResultSerializer(new ModuleTypeRegistry()),
            new ModuleResultRegistry(),
            NullLogger.Instance);

        coordinator.Verify(
            x => x.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
