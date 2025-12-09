using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "update-location-azure-blob")]
public record AwsDatasyncUpdateLocationAzureBlobOptions(
[property: CliOption("--location-arn")] string LocationArn
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--sas-configuration")]
    public string? SasConfiguration { get; set; }

    [CliOption("--blob-type")]
    public string? BlobType { get; set; }

    [CliOption("--access-tier")]
    public string? AccessTier { get; set; }

    [CliOption("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}