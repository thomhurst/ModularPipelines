using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "start-calculation-execution")]
public record AwsAthenaStartCalculationExecutionOptions(
[property: CliOption("--session-id")] string SessionId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--calculation-configuration")]
    public string? CalculationConfiguration { get; set; }

    [CliOption("--code-block")]
    public string? CodeBlock { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}