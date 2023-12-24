using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "delete-resource-policy-statement")]
public record AwsLexv2ModelsDeleteResourcePolicyStatementOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--statement-id")] string StatementId
) : AwsOptions
{
    [CommandSwitch("--expected-revision-id")]
    public string? ExpectedRevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}