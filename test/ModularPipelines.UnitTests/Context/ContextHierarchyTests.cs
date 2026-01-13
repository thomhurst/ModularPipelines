using ModularPipelines.Context;
using ModularPipelines.Context.Domains;
using ModularPipelines.Logging;

namespace ModularPipelines.UnitTests.Context;

/// <summary>
/// Tests to verify the context interface hierarchy structure.
/// </summary>
public class ContextHierarchyTests
{
    [Test]
    public async Task IModuleContext_ShouldInheritFromIPipelineContext()
    {
        // IModuleContext should inherit from IPipelineContext
        await Assert.That(typeof(IModuleContext).GetInterfaces())
            .Contains(typeof(IPipelineContext));
    }

    [Test]
    public async Task IPipelineHookContext_ShouldInheritFromIPipelineContext()
    {
        // IPipelineHookContext should inherit from IPipelineContext
        await Assert.That(typeof(IPipelineHookContext).GetInterfaces())
            .Contains(typeof(IPipelineContext));
    }

    [Test]
    public async Task IPipelineContext_ShouldHaveExpectedDomainProperties()
    {
        var contextType = typeof(IPipelineContext);

        // Verify all expected domain properties exist
        var loggerProperty = contextType.GetProperty("Logger");
        await Assert.That(loggerProperty).IsNotNull()
            .Because("IPipelineContext should have Logger property");
        await Assert.That(loggerProperty!.PropertyType).IsEqualTo(typeof(IModuleLogger))
            .Because("Logger should be of type IModuleLogger");

        var shellProperty = contextType.GetProperty("Shell");
        await Assert.That(shellProperty).IsNotNull()
            .Because("IPipelineContext should have Shell property");
        await Assert.That(shellProperty!.PropertyType).IsEqualTo(typeof(IShellContext))
            .Because("Shell should be of type IShellContext");

        var filesProperty = contextType.GetProperty("Files");
        await Assert.That(filesProperty).IsNotNull()
            .Because("IPipelineContext should have Files property");
        await Assert.That(filesProperty!.PropertyType).IsEqualTo(typeof(IFilesContext))
            .Because("Files should be of type IFilesContext");

        var dataProperty = contextType.GetProperty("Data");
        await Assert.That(dataProperty).IsNotNull()
            .Because("IPipelineContext should have Data property");
        await Assert.That(dataProperty!.PropertyType).IsEqualTo(typeof(IDataContext))
            .Because("Data should be of type IDataContext");

        var environmentProperty = contextType.GetProperty("Environment");
        await Assert.That(environmentProperty).IsNotNull()
            .Because("IPipelineContext should have Environment property");
        await Assert.That(environmentProperty!.PropertyType).IsEqualTo(typeof(IEnvironmentDomainContext))
            .Because("Environment should be of type IEnvironmentDomainContext");

        var installersProperty = contextType.GetProperty("Installers");
        await Assert.That(installersProperty).IsNotNull()
            .Because("IPipelineContext should have Installers property");
        await Assert.That(installersProperty!.PropertyType).IsEqualTo(typeof(IInstallersContext))
            .Because("Installers should be of type IInstallersContext");

        var networkProperty = contextType.GetProperty("Network");
        await Assert.That(networkProperty).IsNotNull()
            .Because("IPipelineContext should have Network property");
        await Assert.That(networkProperty!.PropertyType).IsEqualTo(typeof(INetworkContext))
            .Because("Network should be of type INetworkContext");

        var securityProperty = contextType.GetProperty("Security");
        await Assert.That(securityProperty).IsNotNull()
            .Because("IPipelineContext should have Security property");
        await Assert.That(securityProperty!.PropertyType).IsEqualTo(typeof(ISecurityContext))
            .Because("Security should be of type ISecurityContext");

        var servicesProperty = contextType.GetProperty("Services");
        await Assert.That(servicesProperty).IsNotNull()
            .Because("IPipelineContext should have Services property");
        await Assert.That(servicesProperty!.PropertyType).IsEqualTo(typeof(IServicesContext))
            .Because("Services should be of type IServicesContext");
    }

    [Test]
    public async Task IModuleContext_ShouldHaveGetModuleMethods()
    {
        var moduleContextType = typeof(IModuleContext);

        // Check for module-specific members
        var getModuleMethod = moduleContextType.GetMethod("GetModule");
        await Assert.That(getModuleMethod).IsNotNull()
            .Because("IModuleContext should have GetModule method");

        var getModuleIfRegisteredMethod = moduleContextType.GetMethod("GetModuleIfRegistered");
        await Assert.That(getModuleIfRegisteredMethod).IsNotNull()
            .Because("IModuleContext should have GetModuleIfRegistered method");
    }

    [Test]
    public async Task IModuleContext_ShouldHaveSubModuleMethods()
    {
        var moduleContextType = typeof(IModuleContext);

        // Check for SubModule methods
        var subModuleMethods = moduleContextType.GetMethods().Where(m => m.Name == "SubModule").ToArray();
        await Assert.That(subModuleMethods.Length).IsGreaterThanOrEqualTo(1)
            .Because("IModuleContext should have SubModule methods");
    }
}
