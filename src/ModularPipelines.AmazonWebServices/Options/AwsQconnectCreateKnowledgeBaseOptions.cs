using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "create-knowledge-base")]
public record AwsQconnectCreateKnowledgeBaseOptions(
[property: CliOption("--knowledge-base-type")] string KnowledgeBaseType,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--rendering-configuration")]
    public string? RenderingConfiguration { get; set; }

    [CliOption("--server-side-encryption-configuration")]
    public string? ServerSideEncryptionConfiguration { get; set; }

    [CliOption("--source-configuration")]
    public string? SourceConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}