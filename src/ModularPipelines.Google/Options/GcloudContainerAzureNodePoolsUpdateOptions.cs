using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "node-pools", "update")]
public record GcloudContainerAzureNodePoolsUpdateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CommandSwitch("--node-version")]
    public string? NodeVersion { get; set; }

    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--clear-annotations")]
    public bool? ClearAnnotations { get; set; }

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }
}