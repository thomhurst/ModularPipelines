using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-location-azure-blob")]
public record AwsDatasyncUpdateLocationAzureBlobOptions(
[property: CommandSwitch("--location-arn")] string LocationArn
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--sas-configuration")]
    public string? SasConfiguration { get; set; }

    [CommandSwitch("--blob-type")]
    public string? BlobType { get; set; }

    [CommandSwitch("--access-tier")]
    public string? AccessTier { get; set; }

    [CommandSwitch("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}