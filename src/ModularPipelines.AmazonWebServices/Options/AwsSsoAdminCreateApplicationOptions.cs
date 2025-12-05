using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "create-application")]
public record AwsSsoAdminCreateApplicationOptions(
[property: CliOption("--application-provider-arn")] string ApplicationProviderArn,
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--portal-options")]
    public string? PortalOptions { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}