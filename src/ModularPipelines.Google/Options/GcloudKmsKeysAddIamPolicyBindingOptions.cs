using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "keys", "add-iam-policy-binding")]
public record GcloudKmsKeysAddIamPolicyBindingOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Keyring,
[property: PositionalArgument] string Location,
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
}