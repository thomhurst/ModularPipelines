using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "list-domain-names")]
public record AwsEsListDomainNamesOptions : AwsOptions
{
    [CliOption("--engine-type")]
    public string? EngineType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}