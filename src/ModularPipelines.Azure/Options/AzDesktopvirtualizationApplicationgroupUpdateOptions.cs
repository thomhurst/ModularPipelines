using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("desktopvirtualization", "applicationgroup", "update")]
public record AzDesktopvirtualizationApplicationgroupUpdateOptions : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}