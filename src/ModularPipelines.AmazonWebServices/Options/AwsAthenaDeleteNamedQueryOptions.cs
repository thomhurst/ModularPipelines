using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "delete-named-query")]
public record AwsAthenaDeleteNamedQueryOptions : AwsOptions
{
    [CommandSwitch("--named-query-id")]
    public string? NamedQueryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}