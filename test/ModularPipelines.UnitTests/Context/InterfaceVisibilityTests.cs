using ModularPipelines.Context;
using System.Reflection;

namespace ModularPipelines.UnitTests.Context;

/// <summary>
/// Tests to verify that internal engine interfaces are not publicly exposed.
/// </summary>
public class InterfaceVisibilityTests
{
    [Test]
    public async Task EngineInterfaces_ShouldBeInternal()
    {
        var assembly = typeof(IModuleContext).Assembly;

        var engineInterfaces = assembly.GetTypes()
            .Where(t => t.IsInterface)
            .Where(t => t.Namespace?.Contains("Engine") == true)
            .Where(t => t.Name.StartsWith("IPipeline"))
            .ToList();

        foreach (var iface in engineInterfaces)
        {
            await Assert.That(iface.IsPublic).IsFalse()
                .Because($"{iface.Name} should be internal, not public");
        }
    }

    [Test]
    public async Task UserFacingContextInterfaces_ShouldBePublic()
    {
        var assembly = typeof(IModuleContext).Assembly;

        var expectedPublicInterfaces = new[]
        {
            "IPipelineHookContext",
            "IModuleContext",
            "IPipelineServices",
            "IPipelineLogging",
            "IPipelineTools",
            "IPipelineEncoding",
            "IPipelineFileSystem",
            "IPipelineEnvironment"
        };

        foreach (var interfaceName in expectedPublicInterfaces)
        {
            var iface = assembly.GetType($"ModularPipelines.Context.{interfaceName}");

            await Assert.That(iface).IsNotNull()
                .Because($"{interfaceName} should exist");
            await Assert.That(iface!.IsPublic).IsTrue()
                .Because($"{interfaceName} should be public");
        }
    }

    [Test]
    public async Task ExtensionPointInterfaces_ShouldBePublic()
    {
        var assembly = typeof(IModuleContext).Assembly;

        var extensionPointInterfaces = new[]
        {
            ("ModularPipelines.Host", "IPipelineHost"),
            ("ModularPipelines.Interfaces", "IPipelineGlobalHooks"),
            ("ModularPipelines.Interfaces", "IPipelineModuleHooks"),
            ("ModularPipelines.Requirements", "IPipelineRequirement")
        };

        foreach (var (ns, name) in extensionPointInterfaces)
        {
            var iface = assembly.GetType($"{ns}.{name}");

            await Assert.That(iface).IsNotNull()
                .Because($"{name} should exist");
            await Assert.That(iface!.IsPublic).IsTrue()
                .Because($"{name} should be public");
        }
    }

    [Test]
    public async Task IPipelineServiceContainerWrapper_ShouldBeInternal()
    {
        var assembly = typeof(IModuleContext).Assembly;

        var iface = assembly.GetType("ModularPipelines.DependencyInjection.IPipelineServiceContainerWrapper");

        await Assert.That(iface).IsNotNull()
            .Because("IPipelineServiceContainerWrapper should exist");
        await Assert.That(iface!.IsPublic).IsFalse()
            .Because("IPipelineServiceContainerWrapper should be internal");
    }
}
