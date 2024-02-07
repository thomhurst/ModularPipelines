using System.Text;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests;

public class UnusedModuleDetectorTests
{
    private readonly UnusedModuleDetector _unusedModuleDetector;
    private readonly Mock<IPipelineServiceContainerWrapper> _serviceContainerWrapper;
    private readonly Mock<IAssemblyLoadedTypesProvider> _assemblyLoadedTypesProvider;
    private readonly StringBuilder _sb;

    public UnusedModuleDetectorTests()
    {
        _sb = new StringBuilder();

        _serviceContainerWrapper = new Mock<IPipelineServiceContainerWrapper>();

        _assemblyLoadedTypesProvider = new Mock<IAssemblyLoadedTypesProvider>();

        _unusedModuleDetector = new UnusedModuleDetector(
            _assemblyLoadedTypesProvider.Object,
            _serviceContainerWrapper.Object,
            new StringLogger<UnusedModuleDetector>(_sb)
            );
    }

    [Test]
    public async Task Logs_Unregisted_Modules_Correctly()
    {
        _assemblyLoadedTypesProvider.Setup(x => x.GetLoadedTypesAssignableTo(typeof(ModuleBase)))
            .Returns([
                typeof(Module1),
                typeof(Module2),
                typeof(Module3),
                typeof(Module4),
                typeof(Module5)
            ]);

        var serviceCollection = new ServiceCollection()
            .AddModule<Module1>()
            .AddModule<Module3>()
            .AddModule<Module4>();

        _serviceContainerWrapper.Setup(x => x.ServiceCollection)
            .Returns(serviceCollection);

        _unusedModuleDetector.Log();
        await Assert.That(_sb.ToString()).Is.Not.Empty();
        await Assert.That(_sb.ToString().Trim()).Is.EqualTo("""
Unregistered Modules: ModularPipelines.UnitTests.UnusedModuleDetectorTests+Module2
ModularPipelines.UnitTests.UnusedModuleDetectorTests+Module5
""");
    }

    private class Module1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module4 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module5 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }
}