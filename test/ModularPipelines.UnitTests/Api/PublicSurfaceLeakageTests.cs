using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Tracing;

namespace ModularPipelines.UnitTests.Api;

public class PublicSurfaceLeakageTests
{
    [Test]
    public async Task ImplementationTypesAreNotPublic()
    {
        var assembly = typeof(Pipeline).Assembly;
        var internalTypeNames = new[]
        {
            "ModularPipelines.Context.Domains.Implementations.InstallersContext",
            "ModularPipelines.Context.HashContext",
            "ModularPipelines.Engine.ILogoPrinter",
            "ModularPipelines.Enums.DependencyType",
            "ModularPipelines.Extensions.StreamExtensions",
            "ModularPipelines.Helpers.Disposer",
            "ModularPipelines.Helpers.FileHelper",
            "ModularPipelines.Helpers.IDependencyCollisionDetector",
            "ModularPipelines.Http.IHttpLogger",
            "ModularPipelines.Interfaces.IBuildSystemPipelineFileWriter",
            "ModularPipelines.Interfaces.IScopeDisposer",
            "ModularPipelines.Logging.ICommandLogger",
            "ModularPipelines.Logging.IExceptionOutputFormatter",
            "ModularPipelines.Models.DeclaredDependency",
            "ModularPipelines.Modules.ModuleDependencyValidator",
            "ModularPipelines.Serialization.FilePathJsonConverter",
            "ModularPipelines.Serialization.FolderPathJsonConverter",
            "ModularPipelines.Tracing.ModuleActivityTracing",
            "ModularPipelines.Validation.IDependencyValidator",
            "ModularPipelines.Validation.IModuleConfigurationValidator",
            "ModularPipelines.Validation.IOptionsValidator",
        };

        foreach (var typeName in internalTypeNames)
        {
            var type = assembly.GetType(typeName);
            await Assert.That(type).IsNotNull().Because($"{typeName} should still exist internally");
            await Assert.That(type!.IsNotPublic).IsTrue().Because($"{typeName} is an implementation detail");
        }
    }

    [Test]
    public async Task DeadTypesAreDeleted()
    {
        var assembly = typeof(Pipeline).Assembly;
        var deletedTypeNames = new[]
        {
            "ModularPipelines.Attributes.ExcludeFromCodeCoverageAttributeChanger",
            "ModularPipelines.Context.CommandServiceBase",
            "ModularPipelines.Context.FileInstaller",
            "ModularPipelines.Context.Linux.AptGet",
            "ModularPipelines.Context.Linux.IAptGet",
            "ModularPipelines.Context.PredefinedInstallers",
            "ModularPipelines.Context.Domains.Installers.ILinuxInstallerContext",
            "ModularPipelines.Context.Domains.Installers.IMacInstallerContext",
            "ModularPipelines.Context.Domains.Installers.IPredefinedInstallersContext",
            "ModularPipelines.Context.Domains.Installers.IWindowsInstallerContext",
            "ModularPipelines.Context.Checksum",
            "ModularPipelines.Context.Hasher",
            "ModularPipelines.Context.HashType",
            "ModularPipelines.Context.Domains.Files.IChecksumContext",
            "ModularPipelines.Context.Domains.Security.IHasherContext",
            "ModularPipelines.Enums.WaitResult",
            "ModularPipelines.OperatingSystemHelper",
            "ModularPipelines.Options.Linux.AptGet.AptGetOptions",
            "ModularPipelines.Options.Linux.DpkgInstallOptions",
            "ModularPipelines.Options.Mac.MacBrewOptions",
            "ModularPipelines.Options.Windows.ExeInstallerOptions",
            "ModularPipelines.Options.Windows.MsiInstallerOptions",
            "ModularPipelines.Options.Windows.WindowsInstallerOptionsBase",
            "ModularPipelines.Plugins.PluginTestHelper",
        };

        foreach (var typeName in deletedTypeNames)
        {
            await Assert.That(assembly.GetType(typeName)).IsNull();
        }
    }

    [Test]
    public async Task PublicSurfaceKeepsOnlySupportedEntryPoints()
    {
        var builderMethods = typeof(PipelineBuilderExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);
        var summaryType = typeof(PipelineSummary);

        using (Assert.Multiple())
        {
            await Assert.That(typeof(PipelineTelemetry).IsPublic).IsTrue();
            await Assert.That(builderMethods.Any(method => method.Name == "AddValidator")).IsTrue();
            await Assert.That(builderMethods.Any(method => method.Name == "AddPipelineFileWriter")).IsFalse();
            await Assert.That(builderMethods.Any(method => method.Name == "AddSingleton")).IsFalse();
            await Assert.That(builderMethods.Any(method => method.Name == "Configure" && method.IsGenericMethod)).IsFalse();
            await Assert.That(summaryType.GetProperty("Modules", BindingFlags.Public | BindingFlags.Instance)).IsNull();
            await Assert.That(summaryType.GetMethod("GetModule", BindingFlags.Public | BindingFlags.Instance)).IsNull();
        }
    }
}
