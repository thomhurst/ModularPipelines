using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "enable")]
public record AwsInspector2EnableOptions(
[property: CliOption("--resource-types")] string[] ResourceTypes
) : AwsOptions
{
    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}