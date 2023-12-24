using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-interconnect")]
public record AwsDirectconnectCreateInterconnectOptions(
[property: CommandSwitch("--interconnect-name")] string InterconnectName,
[property: CommandSwitch("--bandwidth")] string Bandwidth,
[property: CommandSwitch("--location")] string Location
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