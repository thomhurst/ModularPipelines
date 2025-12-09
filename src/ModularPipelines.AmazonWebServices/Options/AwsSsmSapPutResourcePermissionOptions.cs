using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-sap", "put-resource-permission")]
public record AwsSsmSapPutResourcePermissionOptions(
[property: CliOption("--action-type")] string ActionType,
[property: CliOption("--source-resource-arn")] string SourceResourceArn,
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}