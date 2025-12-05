using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "create-collection")]
public record AwsOpensearchserverlessCreateCollectionOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--standby-replicas")]
    public string? StandbyReplicas { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}