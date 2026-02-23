using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class DistributedWorkPublisherTests
{
    private class DepResult
    {
        public string Value { get; set; } = string.Empty;
    }

    private class DependencyModule : Module<DepResult>
    {
        protected internal override Task<DepResult?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<DepResult?>(new DepResult { Value = "dep" });
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private class ConsumerModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("consumed");
    }

    private class IndependentModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(42);
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    [ModularPipelines.Attributes.DependsOn<IndependentModule>]
    private class MultiDepModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("multi");
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
        protected internal override Task<LargeResult?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<LargeResult?>(new LargeResult());
    }

    [ModularPipelines.Attributes.DependsOn<LargeResultModule>]
    private class ConsumerOfLargeModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("ok");
    }

    [Test]
    public async Task CreateAssignment_Strips_Value_When_DependencyResult_Exceeds_Size_Limit()
    {
        // Arrange — create a dependency result larger than 256 KB
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(LargeResultModule));
        typeRegistry.Register(typeof(ConsumerOfLargeModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();

        // Create a result with a payload > 256 KB
        var largePayload = new string('X', 300 * 1024);
        var depResult = CreateSuccessResult(new LargeResult { Payload = largePayload }, "LargeResultModule");
        resultRegistry.RegisterResult(typeof(LargeResultModule), depResult);

        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        // Act
        var module = new ConsumerOfLargeModule();
        var assignment = publisher.CreateAssignment(module);

        // Assert — dependency result is included but stripped to well under the original size
        await Assert.That(assignment.DependencyResults).IsNotNull();
        await Assert.That(assignment.DependencyResults!.Count).IsEqualTo(1);

        var strippedJson = assignment.DependencyResults[0].SerializedJson;
        await Assert.That(strippedJson.Length).IsLessThan(1024); // metadata-only should be tiny
        await Assert.That(strippedJson).Contains("\"$type\":\"Success\"");
        await Assert.That(strippedJson).Contains("\"Value\":null");
    }
}
