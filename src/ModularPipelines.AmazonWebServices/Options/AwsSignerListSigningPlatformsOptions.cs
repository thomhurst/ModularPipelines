using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "list-signing-platforms")]
public record AwsSignerListSigningPlatformsOptions : AwsOptions
{
    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--partner")]
    public string? Partner { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}