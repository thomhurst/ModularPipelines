using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "operations", "list")]
public record GcloudBigtableOperationsListOptions : GcloudOptions
{
    [CliOption("--instance")]
    public string? Instance { get; set; }
}