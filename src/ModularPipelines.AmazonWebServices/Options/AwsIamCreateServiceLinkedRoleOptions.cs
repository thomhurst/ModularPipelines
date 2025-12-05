using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-service-linked-role")]
public record AwsIamCreateServiceLinkedRoleOptions(
[property: CliOption("--aws-service-name")] string AwsServiceName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--custom-suffix")]
    public string? CustomSuffix { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}