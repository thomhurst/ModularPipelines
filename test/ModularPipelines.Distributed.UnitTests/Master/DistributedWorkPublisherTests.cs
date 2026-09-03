using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class DistributedWorkPublisherTests
{
    private class DepResult
    {
        public string Value { get; set; } = string.Empty;
    }

    private class DependencyModule : Module<DepResult>
    {
        protected internal override Task<DepResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<DepResult>(new DepResult { Value = "dep" });
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

    private class FluentConsumerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .DependsOn<DependencyModule>();

        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("consumed");
    }

    private abstract class SelectorDependencyBase : Module<DepResult>;

    private class SelectorDependencyModule : SelectorDependencyBase
    {
        protected internal override Task<DepResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<DepResult>(new DepResult { Value = "selected" });
    }

    [ModularPipelines.DependsOnAllModulesInheritingFrom<SelectorDependencyBase>]
    private class SelectorConsumerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("consumed");
    }

    private class TaggedDependencyModule : Module<DepResult>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithTags("distributed");

        protected internal override Task<DepResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<DepResult>(new DepResult { Value = "tagged" });
    }

    [DependsOnModulesWithTag("distributed")]
    private class TaggedConsumerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("consumed");
    }

    [ModularPipelines.DependsOn<DependencyModule>]
    [ModularPipelines.DependsOn<IndependentModule>]
    private class MultiDepModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("multi");
    }

    [RunIfAny<OnLinux, FalseCondition>]
    private sealed class MixedGenericAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [RunIfAny<OnLinux, WorkerOnlyCondition>]
    private sealed class MixedWorkerOnlyAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [RunIf<CustomUnixConditionGroup>]
    private sealed class CustomUnixConditionGroupModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    private sealed class CustomUnixConditionGroup : ConditionGroup, IPlanningRunCondition
    {
        public override IReadOnlyList<IRunCondition> Conditions => [new OnLinux(), new OnMacOS()];

        public override ConditionLogic Logic => ConditionLogic.Any;
    }

    [RunIf<WorkerOnlyConditionGroup>]
    private sealed class WorkerOnlyConditionGroupModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    private sealed class WorkerOnlyConditionGroup : ConditionGroup
    {
        public WorkerOnlyConditionGroup() =>
            throw new InvalidOperationException("Worker-only group was constructed by publisher");

        public override IReadOnlyList<IRunCondition> Conditions => [];

        public override ConditionLogic Logic => ConditionLogic.All;
    }

    [RequiresCapability("linux")]
    [RunIfAny<OnWindows, OnMacOS>]
    private sealed class ConflictingExplicitOperatingSystemModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [RequiresCapability(Capability.Names.Linux, Capability.Names.Docker)]
    private sealed class MultipleCapabilitiesModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [RunIfAny<OnLinux, FalseCondition>]
    [RunIf<OnWindows>]
    private sealed class ConflictingMixedGenericAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [RunIfAny<OnLinux, FalseCondition>]
    [RunIfAny<OnWindows, FalseCondition>]
    private sealed class ConflictingConditionalMixedAlternativeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    private sealed class FalseCondition : IPlanningRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(false);
    }

    private sealed class WorkerOnlyCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
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
    public async Task CreateAssignment_Includes_DependencyResults_When_Deps_Are_Registered()
    {
        // Arrange
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DependencyModule));
        typeRegistry.Register(typeof(ConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();

        // Register the dependency result (simulating master collected it)
        var depResult = CreateSuccessResult(new DepResult { Value = "from-dep" }, "DependencyModule");
        resultRegistry.RegisterResult(typeof(DependencyModule), depResult);

        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        // Act
        var consumerModule = new ConsumerModule();
        var assignment = publisher.CreateAssignment(consumerModule);

        // Assert
        await Assert.That(assignment.DependencyResults).IsNotNull();
        await Assert.That(assignment.DependencyResults!.Count).IsEqualTo(1);
        await Assert.That(assignment.DependencyResults[0].ModuleTypeName).IsEqualTo(typeof(DependencyModule).FullName!);
    }

    [Test]
    public async Task CreateAssignment_Preserves_Dependency_Failure_Worker_Index()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DependencyModule));
        typeRegistry.Register(typeof(ConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var now = DateTimeOffset.UtcNow;
        ModuleResult<DepResult> depResult = new ModuleResult<DepResult>.Failure(
            new RemoteModuleException("WorkerException", "worker failed", "remote stack", workerIndex: 3))
        {
            Name = nameof(DependencyModule),
            TypeName = typeof(DependencyModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = now,
            EndTime = now,
            Status = ModuleStatus.Failed,
            WorkerIndex = 3,
        };
        resultRegistry.RegisterResult(typeof(DependencyModule), depResult);
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry);

        var assignment = publisher.CreateAssignment(new ConsumerModule());

        await Assert.That(assignment.DependencyResults).IsNotNull();
        var serializedDependency = assignment.DependencyResults!.Single();
        await Assert.That(serializedDependency.WorkerIndex).IsEqualTo(3);
        var relayedResult = serializer.Deserialize(serializedDependency);
        var relayedException = relayedResult!.ExceptionOrDefault as RemoteModuleException;
        await Assert.That(relayedException).IsNotNull();
        await Assert.That(relayedException!.WorkerIndex).IsEqualTo(3);
    }

    [Test]
    public async Task CreateAssignment_Preserves_System_Dependency_Failure_Worker_Index()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DependencyModule));
        typeRegistry.Register(typeof(ConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var now = DateTimeOffset.UtcNow;
        ModuleResult<DepResult> depResult = new ModuleResult<DepResult>.Failure(
            new InvalidOperationException("worker failed"))
        {
            Name = nameof(DependencyModule),
            TypeName = typeof(DependencyModule).FullName,
            Duration = TimeSpan.Zero,
            StartTime = now,
            EndTime = now,
            Status = ModuleStatus.Failed,
            WorkerIndex = 3,
        };
        resultRegistry.RegisterResult(typeof(DependencyModule), depResult);
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry);

        var assignment = publisher.CreateAssignment(new ConsumerModule());

        var serializedDependency = assignment.DependencyResults!.Single();
        await Assert.That(serializedDependency.WorkerIndex).IsEqualTo(3);
        var relayedResult = serializer.Deserialize(serializedDependency);
        await Assert.That(relayedResult!.ExceptionOrDefault)
            .IsTypeOf<InvalidOperationException>();
        await Assert.That(((ModuleResult) relayedResult).WorkerIndex).IsEqualTo(3);
    }

    [Test]
    public async Task CreateAssignment_Includes_Fluent_DependencyResults()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DependencyModule));
        typeRegistry.Register(typeof(FluentConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var depResult = CreateSuccessResult(new DepResult { Value = "from-dep" }, "DependencyModule");
        resultRegistry.RegisterResult(typeof(DependencyModule), depResult);
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new FluentConsumerModule());

        await Assert.That(assignment.DependencyResults).IsNotNull();
        await Assert.That(assignment.DependencyResults!).HasSingleItem();
        await Assert.That(assignment.DependencyResults[0].ModuleTypeName).IsEqualTo(typeof(DependencyModule).FullName!);
    }

    [Test]
    public async Task CreateAssignment_Includes_Selector_DependencyResults()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(SelectorDependencyModule));
        typeRegistry.Register(typeof(SelectorConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var dependencyResult = CreateSuccessResult(new DepResult { Value = "selected" }, "SelectorDependencyModule");
        resultRegistry.RegisterResult(typeof(SelectorDependencyModule), dependencyResult);
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new SelectorConsumerModule());

        await Assert.That(assignment.DependencyResults).IsNotNull();
        await Assert.That(assignment.DependencyResults!).HasSingleItem();
        await Assert.That(assignment.DependencyResults[0].ModuleTypeName)
            .IsEqualTo(typeof(SelectorDependencyModule).FullName!);
    }

    [Test]
    public async Task CreateAssignment_Includes_TagSelector_DependencyResults()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(TaggedDependencyModule));
        typeRegistry.Register(typeof(TaggedConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var dependencyResult = CreateSuccessResult(new DepResult { Value = "tagged" }, "TaggedDependencyModule");
        resultRegistry.RegisterResult(typeof(TaggedDependencyModule), dependencyResult);
        var dependencyRegistry = new ModuleDependencyRegistry();
        var metadataRegistry = new ModuleMetadataRegistry(new ModuleAttributeEventService());
        var dependencyModule = new TaggedDependencyModule();
        var consumerModule = new TaggedConsumerModule();
        metadataRegistry.FinalizeMetadata(typeof(TaggedDependencyModule), dependencyModule);
        metadataRegistry.FinalizeMetadata(typeof(TaggedConsumerModule), consumerModule);
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry,
            dependencyRegistry,
            metadataRegistry);

        var assignment = publisher.CreateAssignment(consumerModule);

        await Assert.That(assignment.DependencyResults).IsNotNull();
        await Assert.That(assignment.DependencyResults!).HasSingleItem();
        await Assert.That(assignment.DependencyResults[0].ModuleTypeName)
            .IsEqualTo(typeof(TaggedDependencyModule).FullName!);
    }

    [Test]
    public async Task CreateAssignment_Returns_Null_DependencyResults_When_No_Deps()
    {
        // Arrange
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(IndependentModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        // Act
        var module = new IndependentModule();
        var assignment = publisher.CreateAssignment(module);

        // Assert — no dependencies, so DependencyResults should be null
        await Assert.That(assignment.DependencyResults).IsNull();
    }

    [Test]
    public async Task CreateAssignment_Includes_MasterSatisfied_Condition_Groups()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(IndependentModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var routingOptions = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
        var conditionRouting = new DistributedConditionRouting(
            routingOptions,
            new ModularPipelines.Distributed.Configuration.RoleDetector(routingOptions));
        var module = new IndependentModule();
        conditionRouting.MarkConditionGroupSatisfied(module, typeof(DistributedWorkPublisherTests));
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry,
            executionLocationContext: conditionRouting);

        var assignment = publisher.CreateAssignment(module);

        await Assert.That(assignment.SatisfiedConditionGroups)
            .Contains(typeof(DistributedWorkPublisherTests).AssemblyQualifiedName!);
    }

    [Test]
    public async Task CreateAssignment_Leaves_Planning_Safe_Mixed_Alternative_Unrestricted()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(MixedGenericAlternativeModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new MixedGenericAlternativeModule());

        await Assert.That(assignment.RequiredCapabilities).DoesNotContain("linux");
    }

    [Test]
    public async Task CreateAssignment_Leaves_Worker_Only_Mixed_Alternative_Unrestricted()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(MixedWorkerOnlyAlternativeModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new MixedWorkerOnlyAlternativeModule());

        await Assert.That(assignment.RequiredCapabilities).DoesNotContain("linux");
    }

    [Test]
    public async Task CreateAssignment_Routes_Custom_Operating_System_Condition_Group()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(CustomUnixConditionGroupModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new CustomUnixConditionGroupModule());
        var requiredCapability = assignment.RequiredCapabilities.Single();

        using (Assert.Multiple())
        {
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Linux))
                .Contains(requiredCapability);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.MacOS))
                .Contains(requiredCapability);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Windows))
                .DoesNotContain(requiredCapability);
        }
    }

    [Test]
    public async Task CreateAssignment_Does_Not_Construct_Worker_Only_Condition_Group()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(WorkerOnlyConditionGroupModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new WorkerOnlyConditionGroupModule());

        await Assert.That(assignment.RequiredCapabilities).IsEmpty();
    }

    [Test]
    public async Task CreateAssignment_Flattens_Multiple_Capabilities_From_One_Attribute()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(MultipleCapabilitiesModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry);

        var assignment = publisher.CreateAssignment(new MultipleCapabilitiesModule());

        await Assert.That(assignment.RequiredCapabilities)
            .IsEquivalentTo([Capability.Linux, Capability.Docker]);
    }

    [Test]
    public async Task CreateAssignment_Rejects_Conflicting_Explicit_Operating_System()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(ConflictingExplicitOperatingSystemModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            publisher.CreateAssignment(new ConflictingExplicitOperatingSystemModule()));

        await Assert.That(exception.Message).Contains("incompatible operating-system");
    }

    [Test]
    public async Task CreateAssignment_Does_Not_Combine_Conflicting_Conditional_Os_Route()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(ConflictingMixedGenericAlternativeModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry);

        var assignment = publisher.CreateAssignment(
            new ConflictingMixedGenericAlternativeModule());

        using (Assert.Multiple())
        {
            await Assert.That(assignment.RequiredCapabilities).Contains("windows");
            await Assert.That(assignment.RequiredCapabilities).DoesNotContain("linux");
        }
    }

    [Test]
    public async Task CreateAssignment_Drops_Conflicting_Conditional_Os_Routes()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(ConflictingConditionalMixedAlternativeModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry);

        var assignment = publisher.CreateAssignment(
            new ConflictingConditionalMixedAlternativeModule());

        using (Assert.Multiple())
        {
            await Assert.That(assignment.RequiredCapabilities).DoesNotContain("windows");
            await Assert.That(assignment.RequiredCapabilities).DoesNotContain("linux");
        }
    }

    [Test]
    public async Task CreateAssignment_Includes_Multiple_DependencyResults()
    {
        // Arrange
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DependencyModule));
        typeRegistry.Register(typeof(IndependentModule));
        typeRegistry.Register(typeof(MultiDepModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();

        // Register both dependency results
        var depResult = CreateSuccessResult(new DepResult { Value = "dep" }, "DependencyModule");
        resultRegistry.RegisterResult(typeof(DependencyModule), depResult);

        var indResult = CreateSuccessResult(42, "IndependentModule");
        resultRegistry.RegisterResult(typeof(IndependentModule), indResult);

        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        // Act
        var module = new MultiDepModule();
        var assignment = publisher.CreateAssignment(module);

        // Assert
        await Assert.That(assignment.DependencyResults).IsNotNull();
        await Assert.That(assignment.DependencyResults!.Count).IsEqualTo(2);

        var depTypeNames = assignment.DependencyResults.Select(d => d.ModuleTypeName).ToHashSet();
        await Assert.That(depTypeNames).Contains(typeof(DependencyModule).FullName!);
        await Assert.That(depTypeNames).Contains(typeof(IndependentModule).FullName!);
    }

    [Test]
    public async Task CreateAssignment_Skips_Deps_Not_In_Registry()
    {
        // Arrange — dependency result not registered (e.g., optional dependency that didn't run)
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DependencyModule));
        typeRegistry.Register(typeof(ConsumerModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        // Intentionally NOT registering DependencyModule result

        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        // Act
        var module = new ConsumerModule();
        var assignment = publisher.CreateAssignment(module);

        // Assert — no results available, so DependencyResults should be null
        await Assert.That(assignment.DependencyResults).IsNull();
    }

    private class LargeResult
    {
        public string Payload { get; set; } = string.Empty;
    }

    private class LargeResultModule : Module<LargeResult>
    {
        protected internal override Task<LargeResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<LargeResult>(new LargeResult());
    }

    [ModularPipelines.DependsOn<LargeResultModule>]
    private class ConsumerOfLargeModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("ok");
    }

    [Test]
    public async Task CreateAssignment_Compresses_Large_DependencyResults()
    {
        // Arrange — create a dependency result larger than 64 KB compression threshold
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(LargeResultModule));
        typeRegistry.Register(typeof(ConsumerOfLargeModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();

        // Create a result with a payload > 64 KB (repetitive text compresses well)
        var largePayload = new string('X', 300 * 1024);
        var depResult = CreateSuccessResult(new LargeResult { Payload = largePayload }, "LargeResultModule");
        resultRegistry.RegisterResult(typeof(LargeResultModule), depResult);

        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        // Act
        var module = new ConsumerOfLargeModule();
        var assignment = publisher.CreateAssignment(module);

        // Assert — dependency result is included and compressed
        await Assert.That(assignment.DependencyResults).IsNotNull();
        await Assert.That(assignment.DependencyResults!.Count).IsEqualTo(1);

        var compressedJson = assignment.DependencyResults[0].SerializedJson;
        await Assert.That(compressedJson).StartsWith(DistributedWorkPublisher.GzipPrefix);
        // Compressed should be much smaller than the 300 KB original
        await Assert.That(compressedJson.Length).IsLessThan(100 * 1024);
    }

    [Test]
    public async Task CompressJson_DecompressJson_Roundtrip()
    {
        var original = "{\"$type\":\"Success\",\"Value\":\"" + new string('A', 100_000) + "\"}";

        var compressed = DistributedWorkPublisher.CompressJson(original);
        await Assert.That(compressed).StartsWith(DistributedWorkPublisher.GzipPrefix);
        await Assert.That(compressed.Length).IsLessThan(original.Length);

        var decompressed = DistributedWorkPublisher.DecompressJson(compressed);
        await Assert.That(decompressed).IsEqualTo(original);
    }
}
