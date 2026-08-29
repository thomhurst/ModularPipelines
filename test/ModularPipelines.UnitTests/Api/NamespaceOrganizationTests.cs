using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Logging;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;

namespace ModularPipelines.UnitTests.Api;

public class NamespaceOrganizationTests
{
    [Test]
    public async Task PublicApis_AreInTheirFeatureNamespaces()
    {
        (Type Type, string Namespace)[] expected =
        [
            (typeof(Module<>), "ModularPipelines"),
            (typeof(IModuleContext), "ModularPipelines"),
            (typeof(IPipelineContext), "ModularPipelines"),
            (typeof(ModuleResult<>), "ModularPipelines"),
            (typeof(CommandResult), "ModularPipelines"),
            (typeof(None), "ModularPipelines"),
            (typeof(SkipDecision), "ModularPipelines"),
            (typeof(ModuleStatus), "ModularPipelines"),
            (typeof(ModulePriority), "ModularPipelines"),
            (typeof(DependsOnAttribute), "ModularPipelines"),
            (typeof(RunIfAllAttribute), "ModularPipelines"),
            (typeof(SkipIfAttribute), "ModularPipelines"),
            (typeof(OnWindows), "ModularPipelines"),
            (typeof(IsCI), "ModularPipelines"),
            (typeof(ModuleConfigurationBuilder), "ModularPipelines"),
            (typeof(IShellContext), "ModularPipelines.Context"),
            (typeof(IHttpContext), "ModularPipelines.Context"),
            (typeof(IPipelineEventHandler), "ModularPipelines.Events"),
            (typeof(IModuleStartHandler), "ModularPipelines.Events"),
            (typeof(IModuleHookContext), "ModularPipelines.Events"),
            (typeof(ISecretRegistry), "ModularPipelines.Secrets"),
            (typeof(SecretMaskingOptions), "ModularPipelines.Secrets"),
            (typeof(SecretValueAttribute), "ModularPipelines.Secrets"),
            (typeof(PipelineRunReport), "ModularPipelines.Reporting"),
            (typeof(IRunHistoryStore), "ModularPipelines.Reporting"),
            (typeof(IModuleResultRepository), "ModularPipelines.Reporting"),
            (typeof(RunReportOptions), "ModularPipelines.Reporting"),
            (typeof(IConsoleWriter), "ModularPipelines.Logging"),
        ];

        foreach (var (type, expectedNamespace) in expected)
        {
            await Assert.That(type.Namespace).IsEqualTo(expectedNamespace);
        }
    }

    [Test]
    public async Task PreviousTypeKindNamespaces_DoNotExposeMovedTypes()
    {
        var assembly = typeof(Module<>).Assembly;
        string[] removedTypeNames =
        [
            "ModularPipelines.Modules.Module`1",
            "ModularPipelines.Context.IModuleContext",
            "ModularPipelines.Models.ModuleResult`1",
            "ModularPipelines.Enums.ModuleStatus",
            "ModularPipelines.Attributes.DependsOnAttribute",
            "ModularPipelines.Conditions.OnWindows",
            "ModularPipelines.Engine.ISecretRegistry",
            "ModularPipelines.Engine.IRunHistoryStore",
        ];

        foreach (var typeName in removedTypeNames)
        {
            await Assert.That(assembly.GetType(typeName)).IsNull();
        }
    }
}
