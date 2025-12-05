using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "keys", "add-iam-policy-binding")]
public record GcloudKmsKeysAddIamPolicyBindingOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Keyring,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions
{
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