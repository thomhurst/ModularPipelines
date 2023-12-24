using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-identity-propagation-config")]
public record AwsQuicksightUpdateIdentityPropagationConfigOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--service")] string Service
) : AwsOptions
{
    [CommandSwitch("--authorized-targets")]
    public string[]? AuthorizedTargets { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}