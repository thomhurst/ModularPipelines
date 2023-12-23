using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-azure-blob")]
public record AwsDatasyncCreateLocationAzureBlobOptions(
[property: CommandSwitch("--container-url")] string ContainerUrl,
[property: CommandSwitch("--authentication-type")] string AuthenticationType,
[property: CommandSwitch("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CommandSwitch("--sas-configuration")]
    public string? SasConfiguration { get; set; }

    [CommandSwitch("--blob-type")]
    public string? BlobType { get; set; }

    [CommandSwitch("--access-tier")]
    public string? AccessTier { get; set; }

    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}