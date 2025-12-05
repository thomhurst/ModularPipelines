using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clients", "create")]
public record GcloudContainerAzureClientsCreateOptions(
[property: CliArgument] string Client,
[property: CliArgument] string Location,
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--tenant-id")] string TenantId
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}