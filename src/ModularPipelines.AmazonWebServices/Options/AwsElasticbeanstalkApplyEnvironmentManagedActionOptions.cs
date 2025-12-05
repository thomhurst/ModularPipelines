using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "apply-environment-managed-action")]
public record AwsElasticbeanstalkApplyEnvironmentManagedActionOptions(
[property: CliOption("--action-id")] string ActionId
) : AwsOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}