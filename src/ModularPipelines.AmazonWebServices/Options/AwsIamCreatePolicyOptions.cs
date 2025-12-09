using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-policy")]
public record AwsIamCreatePolicyOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}