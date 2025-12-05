using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "add-iam-policy-binding")]
public record GcloudComputeImagesAddIamPolicyBindingOptions(
[property: CliArgument] string Image,
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