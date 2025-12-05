using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "start-import-job")]
public record AwsQconnectStartImportJobOptions(
[property: CliOption("--import-job-type")] string ImportJobType,
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--upload-id")] string UploadId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--external-source-configuration")]
    public string? ExternalSourceConfiguration { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}