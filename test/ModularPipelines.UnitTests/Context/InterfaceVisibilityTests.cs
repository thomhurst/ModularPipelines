using ModularPipelines.Events;
using System.Reflection;
using ModularPipelines.Context;

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
    public async Task EnginePlumbing_ShouldNotBePublic()
    {
        var assembly = typeof(IModuleContext).Assembly;
        string[] internalTypeNames =
        [
            "ModularPipelines.Modules.SubModuleBase",
            "ModularPipelines.Context.ICommandLineBuilder",
            "ModularPipelines.Engine.LogoPrinter",
            "ModularPipelines.Context.IBuildSystemDetector",
        ];

        foreach (var typeName in internalTypeNames)
        {
            var type = assembly.GetType(typeName);
            await Assert.That(type).IsNotNull();
            await Assert.That(type!.IsPublic).IsFalse()
                .Because($"{typeName} should be internal");
        }

        await Assert.That(assembly.GetType("ModularPipelines.Modules.SubModule`1")).IsNull();

        var failingModuleName = assembly
            .GetType("ModularPipelines.Exceptions.DependencyFailedException")!
            .GetProperty("FailingModuleName")!;
        await Assert.That(failingModuleName.SetMethod).IsNull();
    }

    [Test]
    public async Task UserFacingContextInterfaces_ShouldBePublic()
    {
        var assembly = typeof(IModuleContext).Assembly;

        // v2.0 role-based context interfaces
        var expectedPublicInterfaces = new[]
        {
            // Core context interfaces
            ("ModularPipelines", "IPipelineContext"),
            ("ModularPipelines", "IModuleContext"),
            // Domain context interfaces
            ("ModularPipelines.Context", "IShellContext"),
            ("ModularPipelines.Context", "IFilesContext"),
            ("ModularPipelines.Context", "IDataContext"),
            ("ModularPipelines.Context", "IEnvironmentContext"),
            ("ModularPipelines.Context", "IInstallersContext"),
            ("ModularPipelines.Context", "INetworkContext"),
            ("ModularPipelines.Context", "ISecurityContext"),
            ("ModularPipelines.Context", "IServicesContext"),
            ("ModularPipelines.Context", "ICommandContext"),
            ("ModularPipelines.Context", "IBashContext"),
            ("ModularPipelines.Context", "IPowerShellContext"),
            ("ModularPipelines.Context", "IZipContext"),
            ("ModularPipelines.Context", "IJsonContext"),
            ("ModularPipelines.Context", "IXmlContext"),
            ("ModularPipelines.Context", "IYamlContext"),
            ("ModularPipelines.Context", "IBase64Context"),
            ("ModularPipelines.Context", "IHexContext"),
            ("ModularPipelines.Context", "IEnvironmentVariablesContext"),
            ("ModularPipelines.Context", "IHttpContext"),
            ("ModularPipelines.Context", "IDownloaderContext"),
            ("ModularPipelines.Context", "ICertificatesContext"),
            ("ModularPipelines.Context", "IHashContext"),
        };

        foreach (var (ns, interfaceName) in expectedPublicInterfaces)
        {
            var iface = assembly.GetType($"{ns}.{interfaceName}");

            await Assert.That(iface).IsNotNull()
                .Because($"{interfaceName} should exist");
            await Assert.That(iface!.IsPublic).IsTrue()
                .Because($"{interfaceName} should be public");
        }
    }

    [Test]
    public async Task LegacyContextInterfaces_ShouldNotExist()
    {
        var assembly = typeof(IModuleContext).Assembly;
        var removedInterfaces = new[]
        {
            "ICommand",
            "IBash",
            "IPowershell",
            "IZip",
            "IChecksum",
            "IJson",
            "IXml",
            "IYaml",
            "IBase64",
            "IHex",
            "IEnvironmentVariables",
            "IWindowsInstaller",
            "ILinuxInstaller",
            "IMacInstaller",
            "IPredefinedInstallers",
            "IDownloader",
            "ICertificates",
            "IHasher",
            "IPipelineHookContext",
            "IFileSystemContext",
            "IInstaller",
        };

        foreach (var interfaceName in removedInterfaces)
        {
            await Assert.That(assembly.GetType($"ModularPipelines.Context.{interfaceName}")).IsNull();
        }

        await Assert.That(assembly.GetType("ModularPipelines.Http.IHttp")).IsNull();
    }

    [Test]
    public async Task ExtensionPointInterfaces_ShouldBePublic()
    {
        var assembly = typeof(IModuleContext).Assembly;

        var extensionPointInterfaces = new[]
        {
            ("ModularPipelines", "IPipeline"),
            ("ModularPipelines.Events", "IEventHandler"),
            ("ModularPipelines.Events", "IPipelineEventHandler"),
            ("ModularPipelines.Events", "IModuleEventHandler"),
            ("ModularPipelines.Events", "IModuleReadyHandler"),
            ("ModularPipelines.Events", "IModuleStartHandler"),
            ("ModularPipelines.Events", "IModuleEndHandler"),
            ("ModularPipelines.Events", "IModuleFailureHandler"),
            ("ModularPipelines.Events", "IModuleSkippedHandler"),
            ("ModularPipelines.Events", "IModuleRegistrationHandler"),
            ("ModularPipelines.Events", "IPlanningSafeModuleRegistrationHandler"),
            ("ModularPipelines.Events", "IModuleRegistrationContext"),
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

        await Assert.That(assembly.GetType("ModularPipelines.Interfaces.IPipelineGlobalHooks")).IsNull();
        await Assert.That(assembly.GetType("ModularPipelines.Interfaces.IModuleEventReceiver")).IsNull();
        await Assert.That(assembly.GetType("ModularPipelines.Attributes.Events.IModuleStartHandler")).IsNull();
        await Assert.That(assembly.GetType("ModularPipelines.Attributes.Events.IModuleRegistrationEventReceiver")).IsNull();
    }

    [Test]
    public async Task ModuleEventHandler_Composes_One_Shared_Handler_Family()
    {
        var modulePhaseHandlers = new[]
        {
            typeof(IModuleReadyHandler),
            typeof(IModuleStartHandler),
            typeof(IModuleEndHandler),
            typeof(IModuleFailureHandler),
            typeof(IModuleSkippedHandler),
            typeof(IModuleRegistrationHandler),
        };

        foreach (var handlerType in modulePhaseHandlers.Append(typeof(IPipelineEventHandler)))
        {
            await Assert.That(typeof(IEventHandler).IsAssignableFrom(handlerType)).IsTrue();
        }

        foreach (var handlerType in modulePhaseHandlers.Take(5))
        {
            await Assert.That(handlerType.IsAssignableFrom(typeof(IModuleEventHandler))).IsTrue();
        }

        await Assert.That(typeof(IEventHandler).GetProperty(nameof(IEventHandler.ContinueOnError))).IsNotNull();
        await Assert.That(typeof(IEventHandler).GetProperty(nameof(IEventHandler.Priority))).IsNotNull();
        await Assert.That(typeof(IModuleRegistrationHandler)
            .IsAssignableFrom(typeof(IPlanningSafeModuleRegistrationHandler))).IsTrue();
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
