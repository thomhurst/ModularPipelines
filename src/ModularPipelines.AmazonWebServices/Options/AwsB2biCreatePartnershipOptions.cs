using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "create-partnership")]
public record AwsB2biCreatePartnershipOptions(
[property: CliOption("--profile-id")] string ProfileId,
[property: CliOption("--name")] string Name,
[property: CliOption("--email")] string Email
) : AwsOptions
{
    [CliOption("--phone")]
    public string? Phone { get; set; }

    [CliOption("--capabilities")]
    public string[]? Capabilities { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}