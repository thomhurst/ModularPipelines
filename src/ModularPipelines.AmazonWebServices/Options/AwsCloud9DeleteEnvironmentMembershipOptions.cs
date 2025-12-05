using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud9", "delete-environment-membership")]
public record AwsCloud9DeleteEnvironmentMembershipOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--user-arn")] string UserArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}