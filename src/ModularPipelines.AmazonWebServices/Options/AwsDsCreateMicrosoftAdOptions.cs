using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "create-microsoft-ad")]
public record AwsDsCreateMicrosoftAdOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--password")] string Password,
[property: CliOption("--vpc-settings")] string VpcSettings
) : AwsOptions
{
    [CliOption("--short-name")]
    public string? ShortName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}