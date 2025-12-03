using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage-mover", "job-run", "show")]
public record AzStorageMoverJobRunShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--job-definition-name")]
    public string? JobDefinitionName { get; set; }

    [CliOption("--job-run-name")]
    public string? JobRunName { get; set; }

    [CliOption("--project-name")]
    public string? ProjectName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-mover-name")]
    public string? StorageMoverName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}