using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "delete-environment-blueprint-configuration")]
public record AwsDatazoneDeleteEnvironmentBlueprintConfigurationOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--environment-blueprint-identifier")] string EnvironmentBlueprintIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}