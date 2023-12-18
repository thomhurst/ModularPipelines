using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "wait")]
public record AzImageWaitOptions : AzOptions
{
    [BooleanCommandSwitch("--created")]
    public bool? Created { get; set; }

    [CommandSwitch("--custom")]
    public string? Custom { get; set; }

    [BooleanCommandSwitch("--deleted")]
    public bool? Deleted { get; set; }

    [BooleanCommandSwitch("--exists")]
    public bool? Exists { get; set; }

    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--image-name")]
    public string? ImageName { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--updated")]
    public bool? Updated { get; set; }
}

