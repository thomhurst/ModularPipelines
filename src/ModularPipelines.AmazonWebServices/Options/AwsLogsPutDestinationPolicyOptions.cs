using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-destination-policy")]
public record AwsLogsPutDestinationPolicyOptions(
[property: CommandSwitch("--destination-name")] string DestinationName,
[property: CommandSwitch("--access-policy")] string AccessPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}