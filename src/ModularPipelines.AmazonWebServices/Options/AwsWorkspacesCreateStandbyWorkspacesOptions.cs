using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "create-standby-workspaces")]
public record AwsWorkspacesCreateStandbyWorkspacesOptions(
[property: CliOption("--primary-region")] string PrimaryRegion,
[property: CliOption("--standby-workspaces")] string[] StandbyWorkspaces
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}