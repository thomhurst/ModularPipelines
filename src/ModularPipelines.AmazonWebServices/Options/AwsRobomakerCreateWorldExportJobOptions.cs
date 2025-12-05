using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("robomaker", "create-world-export-job")]
public record AwsRobomakerCreateWorldExportJobOptions(
[property: CliOption("--worlds")] string[] Worlds,
[property: CliOption("--output-location")] string OutputLocation,
[property: CliOption("--iam-role")] string IamRole
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}