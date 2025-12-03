using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "create-connection")]
public record AwsEventsCreateConnectionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--authorization-type")] string AuthorizationType,
[property: CliOption("--auth-parameters")] string AuthParameters
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}