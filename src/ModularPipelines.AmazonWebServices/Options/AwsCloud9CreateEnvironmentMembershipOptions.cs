using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud9", "create-environment-membership")]
public record AwsCloud9CreateEnvironmentMembershipOptions(
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--user-arn")] string UserArn,
[property: CliOption("--permissions")] string Permissions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}