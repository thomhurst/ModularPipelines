using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "describe-workspace-bundles")]
public record AwsWorkspacesDescribeWorkspaceBundlesOptions : AwsOptions
{
    [CommandSwitch("--bundle-ids")]
    public string[]? BundleIds { get; set; }

    [CommandSwitch("--owner")]
    public string? Owner { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}