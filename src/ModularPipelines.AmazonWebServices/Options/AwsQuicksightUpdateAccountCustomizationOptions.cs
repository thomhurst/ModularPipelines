using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-account-customization")]
public record AwsQuicksightUpdateAccountCustomizationOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--account-customization")] string AccountCustomization
) : AwsOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}