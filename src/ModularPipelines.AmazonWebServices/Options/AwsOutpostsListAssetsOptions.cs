using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "list-assets")]
public record AwsOutpostsListAssetsOptions(
[property: CommandSwitch("--outpost-identifier")] string OutpostIdentifier
) : AwsOptions
{
    [CommandSwitch("--host-id-filter")]
    public string[]? HostIdFilter { get; set; }

    [CommandSwitch("--status-filter")]
    public string[]? StatusFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}