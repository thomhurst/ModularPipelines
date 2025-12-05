using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "delete-app-version-resource")]
public record AwsResiliencehubDeleteAppVersionResourceOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-region")]
    public string? AwsRegion { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--logical-resource-id")]
    public string? LogicalResourceId { get; set; }

    [CliOption("--physical-resource-id")]
    public string? PhysicalResourceId { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}