using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("license-manager", "create-grant-version")]
public record AwsLicenseManagerCreateGrantVersionOptions(
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--grant-arn")] string GrantArn
) : AwsOptions
{
    [CommandSwitch("--grant-name")]
    public string? GrantName { get; set; }

    [CommandSwitch("--allowed-operations")]
    public string[]? AllowedOperations { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--status-reason")]
    public string? StatusReason { get; set; }

    [CommandSwitch("--source-version")]
    public string? SourceVersion { get; set; }

    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}