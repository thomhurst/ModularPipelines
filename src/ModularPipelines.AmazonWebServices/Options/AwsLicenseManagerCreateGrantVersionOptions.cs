using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("license-manager", "create-grant-version")]
public record AwsLicenseManagerCreateGrantVersionOptions(
[property: CliOption("--client-token")] string ClientToken,
[property: CliOption("--grant-arn")] string GrantArn
) : AwsOptions
{
    [CliOption("--grant-name")]
    public string? GrantName { get; set; }

    [CliOption("--allowed-operations")]
    public string[]? AllowedOperations { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--status-reason")]
    public string? StatusReason { get; set; }

    [CliOption("--source-version")]
    public string? SourceVersion { get; set; }

    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}