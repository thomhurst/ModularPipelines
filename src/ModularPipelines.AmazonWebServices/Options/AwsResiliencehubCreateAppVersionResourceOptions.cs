using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "create-app-version-resource")]
public record AwsResiliencehubCreateAppVersionResourceOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--app-components")] string[] AppComponents,
[property: CliOption("--logical-resource-id")] string LogicalResourceId,
[property: CliOption("--physical-resource-id")] string PhysicalResourceId,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--additional-info")]
    public IEnumerable<KeyValue>? AdditionalInfo { get; set; }

    [CliOption("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CliOption("--aws-region")]
    public string? AwsRegion { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}