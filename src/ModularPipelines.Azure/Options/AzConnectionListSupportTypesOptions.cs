using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connection", "list-support-types")]
public record AzConnectionListSupportTypesOptions : AzOptions
{
    [CliOption("--target-type")]
    public string? TargetType { get; set; }
}