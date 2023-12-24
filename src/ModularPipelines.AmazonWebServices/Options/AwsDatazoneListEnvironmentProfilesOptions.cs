using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "list-environment-profiles")]
public record AwsDatazoneListEnvironmentProfilesOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CommandSwitch("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CommandSwitch("--aws-account-region")]
    public string? AwsAccountRegion { get; set; }

    [CommandSwitch("--environment-blueprint-identifier")]
    public string? EnvironmentBlueprintIdentifier { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--project-identifier")]
    public string? ProjectIdentifier { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}