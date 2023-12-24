using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "batch-get-schema")]
public record AwsCleanroomsBatchGetSchemaOptions(
[property: CommandSwitch("--collaboration-identifier")] string CollaborationIdentifier,
[property: CommandSwitch("--names")] string[] Names
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}