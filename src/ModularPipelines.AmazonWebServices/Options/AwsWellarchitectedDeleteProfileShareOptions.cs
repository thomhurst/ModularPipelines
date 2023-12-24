using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "delete-profile-share")]
public record AwsWellarchitectedDeleteProfileShareOptions(
[property: CommandSwitch("--share-id")] string ShareId,
[property: CommandSwitch("--profile-arn")] string ProfileArn
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}