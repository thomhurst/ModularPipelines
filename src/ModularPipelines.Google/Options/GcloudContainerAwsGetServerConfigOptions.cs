using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "aws", "get-server-config")]
public record GcloudContainerAwsGetServerConfigOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}