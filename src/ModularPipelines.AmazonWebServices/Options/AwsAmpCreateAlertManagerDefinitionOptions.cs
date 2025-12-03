using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amp", "create-alert-manager-definition")]
public record AwsAmpCreateAlertManagerDefinitionOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--data")] string Data
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}