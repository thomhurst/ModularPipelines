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
        await Assert.That(_sb.ToString()).IsNotEmpty();

        // Normalize line endings for cross-platform compatibility
        var actual = _sb.ToString().Trim().Replace("\r\n", "\n");
        var expected = "⚠ Unregistered Modules:\n  • Module2\n  • Module5";

        await Assert.That(actual).IsEqualTo(expected);
    }

    private class Module1 : ModuleNew
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module2 : ModuleNew
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module3 : ModuleNew
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module4 : ModuleNew
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }

    private class Module5 : ModuleNew
    {
        public override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return null;
        }
    }
}