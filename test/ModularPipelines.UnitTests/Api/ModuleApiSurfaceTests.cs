using ModularPipelines.Configuration;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Api;

public class ModuleApiSurfaceTests
{
    private sealed class DirectModule : IModule
    {
        IInternalModule IModule.AsInternalModule() => throw new NotSupportedException();

        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration => ModuleConfiguration.Default;
    }

    [Test]
    public async Task IModuleOnlyExposesAuthoringMetadata()
    {
        var publicProperties = typeof(IModule)
            .GetProperties()
            .Select(property => property.Name);

        await Assert.That(publicProperties)
            .IsEquivalentTo([nameof(IModule.ResultType), nameof(IModule.Configuration)]);
        await Assert.That(typeof(IModule).GetMethods().Where(method => !method.IsSpecialName))
            .IsEmpty();
    }

    [Test]
    public async Task ModuleExecutionContractIsInternal()
    {
        await Assert.That(typeof(IInternalModule).IsNotPublic).IsTrue();
        await Assert.That(typeof(IInternalModule).GetProperty(nameof(IInternalModule.ResultTask)))
            .IsNotNull();
        await Assert.That(typeof(IInternalModule).GetMethod(nameof(IInternalModule.TrySetDistributedResult)))
            .IsNotNull();
        await Assert.That(typeof(IModule).Assembly.GetType("ModularPipelines.Models.ModuleRunType"))
            .IsNull();
    }

    [Test]
    public async Task ExecutionBackendContractIsPublicAndResultReturning()
    {
        var executeMethod = typeof(IExecutionBackend).GetMethod(nameof(IExecutionBackend.ExecuteAsync));

        using (Assert.Multiple())
        {
            await Assert.That(typeof(IExecutionBackend).IsPublic).IsTrue();
            await Assert.That(typeof(IExecutionBackendContext).IsPublic).IsTrue();
            await Assert.That(typeof(IExecutionBackend).GetProperty(nameof(IExecutionBackend.OwnsEntirePlan)))
                .IsNotNull();
            await Assert.That(executeMethod).IsNotNull();
            await Assert.That(executeMethod!.ReturnType)
                .IsEqualTo(typeof(Task<IReadOnlyList<IModuleResult>>));
            await Assert.That(executeMethod.GetParameters().Select(parameter => parameter.ParameterType))
                .IsEquivalentTo([
                    typeof(ExecutionBackendRequest),
                    typeof(CancellationToken),
                ]);
            await Assert.That(typeof(ExecutionBackendRequest).GetProperty(nameof(ExecutionBackendRequest.EstimatedDurations))!.PropertyType)
                .IsEqualTo(typeof(IReadOnlyDictionary<ModularPipelines.Distributed.ModuleId, TimeSpan>));
        }
    }

    [Test]
    public async Task IModuleCannotBeImplementedOutsideTheAssembly()
    {
        var hiddenAbstractMembers = typeof(IModule)
            .GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .Where(method => method.IsAbstract && method.IsAssembly)
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(hiddenAbstractMembers).IsNotEmpty();
            await Assert.That(typeof(IInternalModule).IsAssignableFrom(typeof(Module<string>))).IsTrue();
        }
    }

    [Test]
    public async Task RuntimeTypeRegistrationOfNonModuleTypesFailsWithGuidance()
    {
        var builder = Pipeline.CreateBuilder();

        var runtimeException = Assert.Throws<InvalidOperationException>(
            () => builder.AddModules(typeof(DirectModule)));
        var assemblyScanException = Assert.Throws<InvalidOperationException>(
            () => builder.AddModulesFromAssembly(typeof(DirectModule).Assembly));

        foreach (var exception in new[] { runtimeException, assemblyScanException })
        {
            await Assert.That(exception.Message).Contains("must derive from Module<T> or SyncModule<T>");
        }
    }
}
