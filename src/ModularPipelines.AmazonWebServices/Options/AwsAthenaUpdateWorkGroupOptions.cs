using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "update-work-group")]
public record AwsAthenaUpdateWorkGroupOptions(
[property: CliOption("--work-group")] string WorkGroup
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--configuration-updates")]
    public string? ConfigurationUpdates { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}