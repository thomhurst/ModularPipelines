using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "create-input")]
public record AwsMedialiveCreateInputOptions : AwsOptions
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

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--vpc")]
    public string? Vpc { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}