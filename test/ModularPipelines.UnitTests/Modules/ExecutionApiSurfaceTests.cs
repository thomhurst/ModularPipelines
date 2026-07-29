using System.Reflection;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Modules;

public class ExecutionApiSurfaceTests
{
    [Test]
    public async Task Module_Execution_Uses_One_Name_Per_Programming_Model()
    {
        var assembly = typeof(Module<>).Assembly;
        var moduleExecuteAsync = typeof(Module<>).GetMethod(
            "ExecuteAsync",
            BindingFlags.Instance | BindingFlags.NonPublic);
        var syncModuleMethods = typeof(SyncModule<>).GetMethods(
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        var syncExecute = syncModuleMethods.SingleOrDefault(static method => method.Name == "Execute");
        var duplicateSyncHooks = syncModuleMethods
            .Where(static method => method.Name is
                "OnBeforeExecute" or
                "OnAfterExecute" or
                "OnSkipped" or
                "OnFailed")
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(assembly.GetType("ModularPipelines.Modules.Module")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Modules.SyncModule")).IsNull();
            await Assert.That(moduleExecuteAsync).IsNotNull();
            await Assert.That(moduleExecuteAsync!.IsAbstract).IsTrue();
            await Assert.That(syncExecute).IsNotNull();
            await Assert.That(syncExecute!.IsAbstract).IsTrue();
            await Assert.That(duplicateSyncHooks).IsEmpty();
        }
    }
}
