using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "list-cross-account-resources")]
public record AwsGlobalacceleratorListCrossAccountResourcesOptions(
[property: CliOption("--resource-owner-aws-account-id")] string ResourceOwnerAwsAccountId
) : AwsOptions
{
    [CliOption("--accelerator-arn")]
    public string? AcceleratorArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}