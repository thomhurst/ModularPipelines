using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "connection", "list-support-types")]
public record AzSpringConnectionListSupportTypesOptions : AzOptions
{
    [CliOption("--target-type")]
    public string? TargetType { get; set; }
}