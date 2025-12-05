using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "get-file-upload-url")]
public record AwsMturkGetFileUploadUrlOptions(
[property: CliOption("--assignment-id")] string AssignmentId,
[property: CliOption("--question-identifier")] string QuestionIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}