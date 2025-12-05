using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-identity-propagation-config")]
public record AwsQuicksightUpdateIdentityPropagationConfigOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--service")] string Service
) : AwsOptions
{
    [CliOption("--authorized-targets")]
    public string[]? AuthorizedTargets { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}