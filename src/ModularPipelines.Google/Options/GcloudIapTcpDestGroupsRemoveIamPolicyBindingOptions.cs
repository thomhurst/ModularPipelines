using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "tcp", "dest-groups", "remove-iam-policy-binding")]
public record GcloudIapTcpDestGroupsRemoveIamPolicyBindingOptions(
[property: CliOption("--dest-group")] string DestGroup,
[property: CliOption("--member")] string Member,
[property: CliOption("--region")] string Region,
[property: CliOption("--role")] string Role
) : GcloudOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--condition")]
    public IEnumerable<KeyValue>? Condition { get; set; }

    [CliFlag("expression")]
    public bool? Expression { get; set; }

    [CliFlag("title")]
    public bool? Title { get; set; }

    [CliFlag("description")]
    public bool? Description { get; set; }

    [CliOption("--condition-from-file")]
    public string? ConditionFromFile { get; set; }
}