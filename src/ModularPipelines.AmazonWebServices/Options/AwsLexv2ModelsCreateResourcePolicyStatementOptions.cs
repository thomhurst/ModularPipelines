using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-resource-policy-statement")]
public record AwsLexv2ModelsCreateResourcePolicyStatementOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--statement-id")] string StatementId,
[property: CommandSwitch("--effect")] string Effect,
[property: CommandSwitch("--principal")] string[] Principal,
[property: CommandSwitch("--action")] string[] Action
) : AwsOptions
{
    [CommandSwitch("--condition")]
    public IEnumerable<KeyValue>? Condition { get; set; }

    [CommandSwitch("--expected-revision-id")]
    public string? ExpectedRevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}