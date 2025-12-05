using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-sap", "update-application-settings")]
public record AwsSsmSapUpdateApplicationSettingsOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--credentials-to-add-or-update")]
    public string[]? CredentialsToAddOrUpdate { get; set; }

    [CliOption("--credentials-to-remove")]
    public string[]? CredentialsToRemove { get; set; }

    [CliOption("--backint")]
    public string? Backint { get; set; }

    [CliOption("--database-arn")]
    public string? DatabaseArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}