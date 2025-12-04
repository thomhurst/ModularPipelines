using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "job", "deletion", "create")]
public record AzDtJobDeletionCreateOptions(
[property: CliOption("--dt-name")] string DtName
) : AzOptions
{
    [CliOption("--job-id")]
    public string? JobId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}