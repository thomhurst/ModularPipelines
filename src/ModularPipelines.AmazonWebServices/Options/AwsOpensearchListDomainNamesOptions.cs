using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "list-domain-names")]
public record AwsOpensearchListDomainNamesOptions : AwsOptions
{
    [CliOption("--engine-type")]
    public string? EngineType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}