using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-azure-blob")]
public record AwsDatasyncCreateLocationAzureBlobOptions(
[property: CliOption("--container-url")] string ContainerUrl,
[property: CliOption("--authentication-type")] string AuthenticationType,
[property: CliOption("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CliOption("--sas-configuration")]
    public string? SasConfiguration { get; set; }

    [CliOption("--blob-type")]
    public string? BlobType { get; set; }

    [CliOption("--access-tier")]
    public string? AccessTier { get; set; }

    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}