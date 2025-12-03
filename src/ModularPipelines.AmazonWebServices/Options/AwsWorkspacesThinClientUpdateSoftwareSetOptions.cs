using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-thin-client", "update-software-set")]
public record AwsWorkspacesThinClientUpdateSoftwareSetOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--validation-status")] string ValidationStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}