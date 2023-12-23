using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "get-file-upload-url")]
public record AwsMturkGetFileUploadUrlOptions(
[property: CommandSwitch("--assignment-id")] string AssignmentId,
[property: CommandSwitch("--question-identifier")] string QuestionIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}