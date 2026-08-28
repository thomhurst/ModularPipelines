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
            "ModularPipelines.IBuildSystemDetector",
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
            ("ModularPipelines.Context", "IPipelineContext"),
            ("ModularPipelines.Context", "IModuleContext"),
            // Domain context interfaces
            ("ModularPipelines.Context.Domains", "IShellContext"),
            ("ModularPipelines.Context.Domains", "IFilesContext"),
            ("ModularPipelines.Context.Domains", "IDataContext"),
            ("ModularPipelines.Context.Domains", "IEnvironmentContext"),
            ("ModularPipelines.Context.Domains", "IInstallersContext"),
            ("ModularPipelines.Context.Domains", "INetworkContext"),
            ("ModularPipelines.Context.Domains", "ISecurityContext"),
            ("ModularPipelines.Context.Domains", "IServicesContext"),
            ("ModularPipelines.Context.Domains.Shell", "ICommandContext"),
            ("ModularPipelines.Context.Domains.Shell", "IBashContext"),
            ("ModularPipelines.Context.Domains.Shell", "IPowerShellContext"),
            ("ModularPipelines.Context.Domains.Files", "IZipContext"),
            ("ModularPipelines.Context.Domains.Files", "IChecksumContext"),
            ("ModularPipelines.Context.Domains.Data", "IJsonContext"),
            ("ModularPipelines.Context.Domains.Data", "IXmlContext"),
            ("ModularPipelines.Context.Domains.Data", "IYamlContext"),
            ("ModularPipelines.Context.Domains.Data", "IBase64Context"),
            ("ModularPipelines.Context.Domains.Data", "IHexContext"),
            ("ModularPipelines.Context.Domains.Environment", "IEnvironmentVariablesContext"),
            ("ModularPipelines.Context.Domains.Installers", "IWindowsInstallerContext"),
            ("ModularPipelines.Context.Domains.Installers", "ILinuxInstallerContext"),
            ("ModularPipelines.Context.Domains.Installers", "IMacInstallerContext"),
            ("ModularPipelines.Context.Domains.Installers", "IPredefinedInstallersContext"),
            ("ModularPipelines.Context.Domains.Network", "IHttpContext"),
            ("ModularPipelines.Context.Domains.Network", "IDownloaderContext"),
            ("ModularPipelines.Context.Domains.Security", "ICertificatesContext"),
            ("ModularPipelines.Context.Domains.Security", "IHasherContext"),
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
            "IEnvironmentContext",
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
            ("ModularPipelines.Interfaces", "IPipelineGlobalHooks"),
            ("ModularPipelines.Interfaces", "IModuleEventReceiver"),
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
