using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project", "task", "check-name")]
public record AzDmsProjectTaskCheckNameOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
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