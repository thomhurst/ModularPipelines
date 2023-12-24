using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qconnect", "delete-import-job")]
public record AwsQconnectDeleteImportJobOptions(
[property: CommandSwitch("--import-job-id")] string ImportJobId,
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}