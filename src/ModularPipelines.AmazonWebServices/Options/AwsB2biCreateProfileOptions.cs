using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "create-profile")]
public record AwsB2biCreateProfileOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--phone")] string Phone,
[property: CliOption("--business-name")] string BusinessName,
[property: CliOption("--logging")] string Logging
) : AwsOptions
{
    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}