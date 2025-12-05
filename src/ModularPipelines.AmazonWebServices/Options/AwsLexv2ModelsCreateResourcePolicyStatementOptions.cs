using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-resource-policy-statement")]
public record AwsLexv2ModelsCreateResourcePolicyStatementOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--statement-id")] string StatementId,
[property: CliOption("--effect")] string Effect,
[property: CliOption("--principal")] string[] Principal,
[property: CliOption("--action")] string[] Action
) : AwsOptions
{
    [CliOption("--condition")]
    public IEnumerable<KeyValue>? Condition { get; set; }

    [CliOption("--expected-revision-id")]
    public string? ExpectedRevisionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}