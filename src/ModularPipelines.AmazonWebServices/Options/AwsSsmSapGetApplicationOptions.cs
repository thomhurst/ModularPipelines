using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-sap", "get-application")]
public record AwsSsmSapGetApplicationOptions : AwsOptions
{
    [CliOption("--application-id")]
    public string? ApplicationId { get; set; }

    [CliOption("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CliOption("--app-registry-arn")]
    public string? AppRegistryArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}