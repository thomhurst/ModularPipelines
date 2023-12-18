using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project", "task", "create")]
public record AzDmsProjectTaskCreateOptions(
[property: CommandSwitch("--database-options-json")] string DatabaseOptionsJson,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--source-connection-json")] string SourceConnectionJson,
[property: CommandSwitch("--target-connection-json")] string TargetConnectionJson
) : AzOptions
{
    [BooleanCommandSwitch("--enable-data-integrity-validation")]
    public bool? EnableDataIntegrityValidation { get; set; }

    [BooleanCommandSwitch("--enable-query-analysis-validation")]
    public bool? EnableQueryAnalysisValidation { get; set; }

    [BooleanCommandSwitch("--enable-schema-validation")]
    public bool? EnableSchemaValidation { get; set; }

    [CommandSwitch("--task-type")]
    public string? TaskType { get; set; }
}

