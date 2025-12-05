using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "delete-resource-policy-statement")]
public record AwsLexv2ModelsDeleteResourcePolicyStatementOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--statement-id")] string StatementId
) : AwsOptions
{
    [CliOption("--expected-revision-id")]
    public string? ExpectedRevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}