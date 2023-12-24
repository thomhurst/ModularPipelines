using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-environment")]
public record AwsDatazoneCreateEnvironmentOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--environment-profile-identifier")] string EnvironmentProfileIdentifier,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--glossary-terms")]
    public string[]? GlossaryTerms { get; set; }

    [CommandSwitch("--user-parameters")]
    public string[]? UserParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}