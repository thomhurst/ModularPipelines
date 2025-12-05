using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "start-session")]
public record AwsSsmStartSessionOptions(
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--document-name")]
    public string? DocumentName { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}