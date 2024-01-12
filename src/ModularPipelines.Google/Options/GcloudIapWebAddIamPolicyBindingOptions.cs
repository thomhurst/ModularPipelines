using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "web", "add-iam-policy-binding")]
public record GcloudIapWebAddIamPolicyBindingOptions(
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions
{
    [CommandSwitch("--condition")]
    public IEnumerable<KeyValue>? Condition { get; set; }

    [BooleanCommandSwitch("expression")]
    public bool? Expression { get; set; }

    [BooleanCommandSwitch("title")]
    public bool? Title { get; set; }

    [BooleanCommandSwitch("description")]
    public bool? Description { get; set; }

    [CommandSwitch("--condition-from-file")]
    public string? ConditionFromFile { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}