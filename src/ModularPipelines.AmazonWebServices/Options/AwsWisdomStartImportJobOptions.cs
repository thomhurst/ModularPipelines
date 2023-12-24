using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wisdom", "start-import-job")]
public record AwsWisdomStartImportJobOptions(
[property: CommandSwitch("--import-job-type")] string ImportJobType,
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId,
[property: CommandSwitch("--upload-id")] string UploadId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--external-source-configuration")]
    public string? ExternalSourceConfiguration { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}