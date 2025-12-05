using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "operations", "list")]
public record GcloudContainerAwsOperationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}