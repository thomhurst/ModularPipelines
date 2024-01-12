using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "clients", "get-public-cert")]
public record GcloudContainerAzureClientsGetPublicCertOptions(
[property: PositionalArgument] string Client,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }
}