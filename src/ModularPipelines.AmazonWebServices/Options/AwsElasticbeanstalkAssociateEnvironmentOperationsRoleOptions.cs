using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "associate-environment-operations-role")]
public record AwsElasticbeanstalkAssociateEnvironmentOperationsRoleOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--operations-role")] string OperationsRole
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}