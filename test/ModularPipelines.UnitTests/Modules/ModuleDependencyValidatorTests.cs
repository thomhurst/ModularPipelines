using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Modules;

/// <summary>
/// Covers dependency validation over the dynamic (registration-event) dependency registry, which
/// the executors invoke against the runnable set once registration events have populated it.
/// </summary>
public class ModuleDependencyValidatorTests
{
    [Test]
    public async Task Validate_Dynamic_Dependency_On_Module_Outside_The_Set_Throws()
    {
        var consumer = new ConsumerModule();

        var dynamicRegistry = new ModuleDependencyRegistry();
        dynamicRegistry.AddDynamicDependency(typeof(ConsumerModule), typeof(ProducerModule));

        // The producer is not among the validated modules, so the required dynamic dependency is
        // unsatisfiable and must be reported eagerly (mirrors the DependencyWaiter failure).
        await Assert.That(() => ModuleDependencyValidator.Validate(
                new IModule[] { consumer },
                dynamicRegistry,
                metadataRegistry: null))
            .Throws<ModuleNotRegisteredException>();
    }

    [Test]
    public async Task Validate_Dynamic_Dependency_On_Module_In_The_Set_Does_Not_Throw()
    {
        var consumer = new ConsumerModule();
        var producer = new ProducerModule();

        var dynamicRegistry = new ModuleDependencyRegistry();
        dynamicRegistry.AddDynamicDependency(typeof(ConsumerModule), typeof(ProducerModule));

        await Assert.That(() => ModuleDependencyValidator.Validate(
                new IModule[] { consumer, producer },
                dynamicRegistry,
                metadataRegistry: null))
            .ThrowsNothing();
    }

    [Test]
    public async Task Validate_Long_Linear_Dependency_Graph_Does_Not_Overflow()
    {
        const int chainLength = 20_000;
        var moduleTypes = CreateGraphNodeTypes(chainLength);
        var dependencyGraph = moduleTypes.ToDictionary(
            moduleType => moduleType,
            _ => new HashSet<Type>());

        for (var index = 0; index < moduleTypes.Length - 1; index++)
        {
            dependencyGraph[moduleTypes[index]].Add(moduleTypes[index + 1]);
        }

        await Assert.That(() => ModuleDependencyValidator.ValidateCircularDependencies(dependencyGraph))
            .ThrowsNothing();
    }

    private static Type[] CreateGraphNodeTypes(int count)
    {
        Type[] argumentTypes =
        [
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(long),
            typeof(object),
            typeof(sbyte),
            typeof(short),
            typeof(string),
            typeof(uint),
            typeof(ulong),
            typeof(ushort),
            typeof(Guid),
        ];
        var genericType = typeof(GraphNode<,,,,,,,>);
        var moduleTypes = new Type[count];

        for (var index = 0; index < count; index++)
        {
            var value = index;
            var genericArguments = new Type[8];

            for (var argumentIndex = 0; argumentIndex < genericArguments.Length; argumentIndex++)
            {
                genericArguments[argumentIndex] = argumentTypes[value % argumentTypes.Length];
                value /= argumentTypes.Length;
            }

            moduleTypes[index] = genericType.MakeGenericType(genericArguments);
        }

        return moduleTypes;
    }

    private sealed class ProducerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("produced");
    }

    private sealed class ConsumerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("consumed");
    }

    private sealed class GraphNode<T1, T2, T3, T4, T5, T6, T7, T8>;
}
