using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "get-sparql-stream")]
public record AwsNeptunedataGetSparqlStreamOptions : AwsOptions
{
    [CommandSwitch("--limit")]
    public long? Limit { get; set; }

    [CommandSwitch("--iterator-type")]
    public string? IteratorType { get; set; }

    [CommandSwitch("--commit-num")]
    public long? CommitNum { get; set; }

    [CommandSwitch("--op-num")]
    public long? OpNum { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}