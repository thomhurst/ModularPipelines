using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "connection", "list-support-types")]
public record AzWebappConnectionListSupportTypesOptions : AzOptions
{
    [CliOption("--target-type")]
    public string? TargetType { get; set; }
}