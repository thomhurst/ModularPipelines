using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "list-signing-profiles")]
public record AwsSignerListSigningProfilesOptions : AwsOptions
{
    [CliOption("--platform-id")]
    public string? PlatformId { get; set; }

    [CliOption("--statuses")]
    public string[]? Statuses { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}