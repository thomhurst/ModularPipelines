using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dms", "project", "task", "create")]
public record AzDmsProjectTaskCreateOptions(
[property: CliOption("--database-options-json")] string DatabaseOptionsJson,
[property: CliOption("--name")] string Name,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--source-connection-json")] string SourceConnectionJson,
[property: CliOption("--target-connection-json")] string TargetConnectionJson
) : AzOptions
{
    [CliFlag("--enable-data-integrity-validation")]
    public bool? EnableDataIntegrityValidation { get; set; }

    [CliFlag("--enable-query-analysis-validation")]
    public bool? EnableQueryAnalysisValidation { get; set; }

    [CliFlag("--enable-schema-validation")]
    public bool? EnableSchemaValidation { get; set; }

    [CliOption("--task-type")]
    public string? TaskType { get; set; }
}