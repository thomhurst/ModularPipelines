using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "repositories", "update")]
public record GcloudArtifactsRepositoriesUpdateOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--immutable-tags")]
    public bool? ImmutableTags { get; set; }

    [CommandSwitch("--remote-password-secret-version")]
    public string? RemotePasswordSecretVersion { get; set; }

    [CommandSwitch("--remote-username")]
    public string? RemoteUsername { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--upstream-policy-file")]
    public string? UpstreamPolicyFile { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}