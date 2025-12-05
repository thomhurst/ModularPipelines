using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "list-things")]
public record AwsIotListThingsOptions : AwsOptions
{
    [CliOption("--attribute-name")]
    public string? AttributeName { get; set; }

    [CliOption("--attribute-value")]
    public string? AttributeValue { get; set; }

    [CliOption("--thing-type-name")]
    public string? ThingTypeName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}