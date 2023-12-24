using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "remove-permission")]
public record AwsLambdaRemovePermissionOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--statement-id")] string StatementId
) : AwsOptions
{
    [CommandSwitch("--qualifier")]
    public string? Qualifier { get; set; }

    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}