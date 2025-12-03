using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "update-resource-policy")]
public record AwsLexv2ModelsUpdateResourcePolicyOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--expected-revision-id")]
    public string? ExpectedRevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}