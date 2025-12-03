using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amlfs", "archive")]
public record AzAmlfsArchiveOptions : AzOptions
{
    [CliOption("--amlfs-name")]
    public string? AmlfsName { get; set; }

    [CliOption("--filesystem-path")]
    public string? FilesystemPath { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}