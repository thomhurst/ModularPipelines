using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "update-app-version-resource")]
public record AwsResiliencehubUpdateAppVersionResourceOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--additional-info")]
    public IEnumerable<KeyValue>? AdditionalInfo { get; set; }

    [CliOption("--app-components")]
    public string[]? AppComponents { get; set; }

    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-region")]
    public string? AwsRegion { get; set; }

    [CliOption("--logical-resource-id")]
    public string? LogicalResourceId { get; set; }

    [CliOption("--physical-resource-id")]
    public string? PhysicalResourceId { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}