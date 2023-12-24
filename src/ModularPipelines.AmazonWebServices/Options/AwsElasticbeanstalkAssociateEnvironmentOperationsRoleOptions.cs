using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "associate-environment-operations-role")]
public record AwsElasticbeanstalkAssociateEnvironmentOperationsRoleOptions(
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--operations-role")] string OperationsRole
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}