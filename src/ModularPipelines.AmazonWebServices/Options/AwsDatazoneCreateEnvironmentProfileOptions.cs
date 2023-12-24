using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-environment-profile")]
public record AwsDatazoneCreateEnvironmentProfileOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--environment-blueprint-identifier")] string EnvironmentBlueprintIdentifier,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CommandSwitch("--aws-account-id")]
    public string? AwsAccountId { get; set; }

    [CommandSwitch("--aws-account-region")]
    public string? AwsAccountRegion { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--user-parameters")]
    public string[]? UserParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}