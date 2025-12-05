using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clients", "list")]
public record GcloudContainerAzureClientsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}