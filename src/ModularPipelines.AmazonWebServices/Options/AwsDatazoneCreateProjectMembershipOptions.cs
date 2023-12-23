using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-project-membership")]
public record AwsDatazoneCreateProjectMembershipOptions(
[property: CommandSwitch("--designation")] string Designation,
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--project-identifier")] string ProjectIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}