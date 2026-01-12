using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Context;

/// <summary>
/// Tests to verify the context interface hierarchy structure.
/// </summary>
public class ContextHierarchyTests
{
    [Test]
    public async Task IModuleContext_ShouldInheritFromIPipelineHookContext()
    {
        // IModuleContext should inherit from IPipelineHookContext
        await Assert.That(typeof(IModuleContext).GetInterfaces())
            .Contains(typeof(IPipelineHookContext));
    }

    [Test]
    public async Task IModuleContext_ShouldProvideAllCapabilities()
    {
        var moduleContextType = typeof(IModuleContext);

        // Should have access to core capabilities through inheritance
        await Assert.That(typeof(IPipelineServices).IsAssignableFrom(moduleContextType)).IsTrue()
            .Because("IModuleContext should implement IPipelineServices");

        await Assert.That(typeof(IPipelineLogging).IsAssignableFrom(moduleContextType)).IsTrue()
            .Because("IModuleContext should implement IPipelineLogging");

        await Assert.That(typeof(IPipelineTools).IsAssignableFrom(moduleContextType)).IsTrue()
            .Because("IModuleContext should implement IPipelineTools");

        await Assert.That(typeof(IPipelineEncoding).IsAssignableFrom(moduleContextType)).IsTrue()
            .Because("IModuleContext should implement IPipelineEncoding");

        await Assert.That(typeof(IPipelineFileSystem).IsAssignableFrom(moduleContextType)).IsTrue()
            .Because("IModuleContext should implement IPipelineFileSystem");

        await Assert.That(typeof(IPipelineEnvironment).IsAssignableFrom(moduleContextType)).IsTrue()
            .Because("IModuleContext should implement IPipelineEnvironment");
    }

    [Test]
    public async Task IModuleContext_ShouldHaveModuleSpecificMethods()
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
    public async Task IPipelineHookContext_ShouldBeTheBaseCompositeInterface()
    {
        var hookContextType = typeof(IPipelineHookContext);
        var interfaces = hookContextType.GetInterfaces();

        await Assert.That(interfaces).Contains(typeof(IPipelineServices))
            .Because("IPipelineHookContext should inherit from IPipelineServices");

        await Assert.That(interfaces).Contains(typeof(IPipelineLogging))
            .Because("IPipelineHookContext should inherit from IPipelineLogging");

        await Assert.That(interfaces).Contains(typeof(IPipelineTools))
            .Because("IPipelineHookContext should inherit from IPipelineTools");

        await Assert.That(interfaces).Contains(typeof(IPipelineEncoding))
            .Because("IPipelineHookContext should inherit from IPipelineEncoding");

        await Assert.That(interfaces).Contains(typeof(IPipelineFileSystem))
            .Because("IPipelineHookContext should inherit from IPipelineFileSystem");

        await Assert.That(interfaces).Contains(typeof(IPipelineEnvironment))
            .Because("IPipelineHookContext should inherit from IPipelineEnvironment");
    }
}
