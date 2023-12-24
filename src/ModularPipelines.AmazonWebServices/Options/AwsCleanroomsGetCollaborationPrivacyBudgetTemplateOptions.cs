using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "get-collaboration-privacy-budget-template")]
public record AwsCleanroomsGetCollaborationPrivacyBudgetTemplateOptions(
[property: CommandSwitch("--collaboration-identifier")] string CollaborationIdentifier,
[property: CommandSwitch("--privacy-budget-template-identifier")] string PrivacyBudgetTemplateIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}