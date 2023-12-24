using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "import-stacks-to-stack-set")]
public record AwsCloudformationImportStacksToStackSetOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CommandSwitch("--stack-ids")]
    public string[]? StackIds { get; set; }

    [CommandSwitch("--stack-ids-url")]
    public string? StackIdsUrl { get; set; }

    [CommandSwitch("--organizational-unit-ids")]
    public string[]? OrganizationalUnitIds { get; set; }

    [CommandSwitch("--operation-preferences")]
    public string? OperationPreferences { get; set; }

    [CommandSwitch("--operation-id")]
    public string? OperationId { get; set; }

    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}