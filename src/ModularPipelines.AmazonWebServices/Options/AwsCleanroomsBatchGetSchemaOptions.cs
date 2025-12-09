using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "batch-get-schema")]
public record AwsCleanroomsBatchGetSchemaOptions(
[property: CliOption("--collaboration-identifier")] string CollaborationIdentifier,
[property: CliOption("--names")] string[] Names
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}