using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectparticipant", "describe-view")]
public record AwsConnectparticipantDescribeViewOptions(
[property: CommandSwitch("--view-token")] string ViewToken,
[property: CommandSwitch("--connection-token")] string ConnectionToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}