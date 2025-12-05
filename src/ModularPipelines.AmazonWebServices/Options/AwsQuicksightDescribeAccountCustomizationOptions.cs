using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-account-customization")]
public record AwsQuicksightDescribeAccountCustomizationOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId
) : AwsOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}