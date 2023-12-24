using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "describe-loa")]
public record AwsDirectconnectDescribeLoaOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }

    [CommandSwitch("--loa-content-type")]
    public string? LoaContentType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}