using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "list-tags-for-resource")]
public record AwsSsoAdminListTagsForResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CliOption("--instance-arn")]
    public string? InstanceArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}