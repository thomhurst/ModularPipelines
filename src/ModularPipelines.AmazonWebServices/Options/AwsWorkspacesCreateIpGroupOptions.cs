using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "create-ip-group")]
public record AwsWorkspacesCreateIpGroupOptions(
[property: CliOption("--group-name")] string GroupName
) : AwsOptions
{
    [CliOption("--group-desc")]
    public string? GroupDesc { get; set; }

    [CliOption("--user-rules")]
    public string[]? UserRules { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}