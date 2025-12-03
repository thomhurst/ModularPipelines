using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-input")]
public record AwsMedialiveUpdateInputOptions(
[property: CliOption("--input-id")] string InputId
) : AwsOptions
{
    [CliOption("--destinations")]
    public string[]? Destinations { get; set; }

    [CliOption("--input-devices")]
    public string[]? InputDevices { get; set; }

    [CliOption("--input-security-groups")]
    public string[]? InputSecurityGroups { get; set; }

    [CliOption("--media-connect-flows")]
    public string[]? MediaConnectFlows { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}