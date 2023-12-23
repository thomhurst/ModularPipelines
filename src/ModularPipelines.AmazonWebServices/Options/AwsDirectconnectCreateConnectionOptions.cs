using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-connection")]
public record AwsDirectconnectCreateConnectionOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--bandwidth")] string Bandwidth,
[property: CommandSwitch("--connection-name")] string ConnectionName
) : AwsOptions
{
    [CommandSwitch("--lag-id")]
    public string? LagId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}