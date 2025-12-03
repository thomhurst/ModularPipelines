using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "disassociate-environment-operations-role")]
public record AwsElasticbeanstalkDisassociateEnvironmentOperationsRoleOptions(
[property: CliOption("--environment-name")] string EnvironmentName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}