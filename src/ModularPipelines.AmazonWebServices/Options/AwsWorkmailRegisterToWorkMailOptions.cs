using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "register-to-work-mail")]
public record AwsWorkmailRegisterToWorkMailOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--entity-id")] string EntityId,
[property: CommandSwitch("--email")] string Email
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}