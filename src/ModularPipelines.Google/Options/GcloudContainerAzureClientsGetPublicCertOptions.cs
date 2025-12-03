using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clients", "get-public-cert")]
public record GcloudContainerAzureClientsGetPublicCertOptions(
[property: CliArgument] string Client,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--output-file")]
    public string? OutputFile { get; set; }
}