using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Logging;
using ModularPipelines.Reporting;
using ModularPipelines.Requirements;
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
            (typeof(IModule), "ModularPipelines"),
            (typeof(ModuleConfiguration), "ModularPipelines"),
            (typeof(ICancellableInitializer), "ModularPipelines"),
            (typeof(IModularPipelinesPlugin), "ModularPipelines"),
            (typeof(PipelinePlan), "ModularPipelines"),
            (typeof(PipelinePlanModule), "ModularPipelines"),
            (typeof(PipelinePlanWave), "ModularPipelines"),
            (typeof(RequirementDecision), "ModularPipelines.Requirements"),
            (typeof(IModuleContext), "ModularPipelines"),
            (typeof(IPipelineContext), "ModularPipelines"),
            (typeof(ModuleResult<>), "ModularPipelines"),
            (typeof(CommandResult), "ModularPipelines"),
            (typeof(None), "ModularPipelines"),
            (typeof(SkipDecision), "ModularPipelines"),
            (typeof(ModuleStatus), "ModularPipelines"),
            (typeof(ModulePriority), "ModularPipelines"),
            (typeof(DependsOnAttribute), "ModularPipelines"),
            (typeof(RunIfAttribute<,>), "ModularPipelines"),
            (typeof(SkipIfAttribute<>), "ModularPipelines"),
            (typeof(RunConditionAttribute), "ModularPipelines"),
            (typeof(IPlanningSafe), "ModularPipelines"),
            (typeof(OnWindows), "ModularPipelines"),
            (typeof(OnCI), "ModularPipelines"),
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
            "ModularPipelines.Modules.IModule",
            "ModularPipelines.Configuration.ModuleConfiguration",
            "ModularPipelines.DependencyInjection.ICancellableInitializer",
            "ModularPipelines.Plugins.IModularPipelinesPlugin",
            "ModularPipelines.Models.PipelinePlan",
            "ModularPipelines.Models.PipelinePlanModule",
            "ModularPipelines.Models.PipelinePlanWave",
            "ModularPipelines.Models.RequirementDecision",
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
