using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "clients", "create")]
public record GcloudContainerAzureClientsCreateOptions(
[property: PositionalArgument] string Client,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--tenant-id")] string TenantId
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}