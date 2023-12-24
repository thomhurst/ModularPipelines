using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "list-nodes")]
public record AwsPanoramaListNodesOptions : AwsOptions
{
    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--package-name")]
    public string? PackageName { get; set; }

    [CommandSwitch("--package-version")]
    public string? PackageVersion { get; set; }

    [CommandSwitch("--patch-version")]
    public string? PatchVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}