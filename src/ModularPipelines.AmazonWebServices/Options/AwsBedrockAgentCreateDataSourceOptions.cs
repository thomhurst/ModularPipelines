using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "create-data-source")]
public record AwsBedrockAgentCreateDataSourceOptions(
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--data-source-configuration")] string DataSourceConfiguration
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--server-side-encryption-configuration")]
    public string? ServerSideEncryptionConfiguration { get; set; }

    [CommandSwitch("--vector-ingestion-configuration")]
    public string? VectorIngestionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}