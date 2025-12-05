using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-ops-item")]
public record AwsSsmGetOpsItemOptions(
[property: CliOption("--ops-item-id")] string OpsItemId
) : AwsOptions
{
    [CliOption("--ops-item-arn")]
    public string? OpsItemArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}