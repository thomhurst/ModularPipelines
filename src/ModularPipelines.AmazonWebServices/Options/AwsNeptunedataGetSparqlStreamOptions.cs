using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "get-sparql-stream")]
public record AwsNeptunedataGetSparqlStreamOptions : AwsOptions
{
    [CliOption("--limit")]
    public long? Limit { get; set; }

    [CliOption("--iterator-type")]
    public string? IteratorType { get; set; }

    [CliOption("--commit-num")]
    public long? CommitNum { get; set; }

    [CliOption("--op-num")]
    public long? OpNum { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}