using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connection", "list-support-types")]
public record AzContainerappConnectionListSupportTypesOptions : AzOptions
{
    [CliOption("--target-type")]
    public string? TargetType { get; set; }
}