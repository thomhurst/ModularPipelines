using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "get-server-config")]
public record GcloudContainerAzureGetServerConfigOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}