using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "create-profile-share")]
public record AwsWellarchitectedCreateProfileShareOptions(
[property: CliOption("--profile-arn")] string ProfileArn,
[property: CliOption("--shared-with")] string SharedWith
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}