using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "authorize-ip-rules")]
public record AwsWorkspacesAuthorizeIpRulesOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--user-rules")] string[] UserRules
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}