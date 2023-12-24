using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "create-fleet-advisor-collector")]
public record AwsDmsCreateFleetAdvisorCollectorOptions(
[property: CommandSwitch("--collector-name")] string CollectorName,
[property: CommandSwitch("--service-access-role-arn")] string ServiceAccessRoleArn,
[property: CommandSwitch("--s3-bucket-name")] string S3BucketName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}