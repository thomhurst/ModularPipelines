using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "update-rules-of-ip-group")]
public record AwsWorkspacesUpdateRulesOfIpGroupOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--user-rules")] string[] UserRules
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}