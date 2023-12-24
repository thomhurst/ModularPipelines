using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "update-application-settings")]
public record AwsSsmSapUpdateApplicationSettingsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--credentials-to-add-or-update")]
    public string[]? CredentialsToAddOrUpdate { get; set; }

    [CommandSwitch("--credentials-to-remove")]
    public string[]? CredentialsToRemove { get; set; }

    [CommandSwitch("--backint")]
    public string? Backint { get; set; }

    [CommandSwitch("--database-arn")]
    public string? DatabaseArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}