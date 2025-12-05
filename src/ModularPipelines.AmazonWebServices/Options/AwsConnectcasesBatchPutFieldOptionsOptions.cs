using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "batch-put-field-options")]
public record AwsConnectcasesBatchPutFieldOptionsOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--field-id")] string FieldId,
[property: CliOption("--options")] string[] Options
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}