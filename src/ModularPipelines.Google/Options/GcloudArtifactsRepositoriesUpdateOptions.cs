using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "update")]
public record GcloudArtifactsRepositoriesUpdateOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--immutable-tags")]
    public bool? ImmutableTags { get; set; }

    [CliOption("--remote-password-secret-version")]
    public string? RemotePasswordSecretVersion { get; set; }

    [CliOption("--remote-username")]
    public string? RemoteUsername { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--upstream-policy-file")]
    public string? UpstreamPolicyFile { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}