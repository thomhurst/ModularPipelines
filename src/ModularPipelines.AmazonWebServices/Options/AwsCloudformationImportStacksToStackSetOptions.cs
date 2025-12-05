using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "import-stacks-to-stack-set")]
public record AwsCloudformationImportStacksToStackSetOptions(
[property: CliOption("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CliOption("--stack-ids")]
    public string[]? StackIds { get; set; }

    [CliOption("--stack-ids-url")]
    public string? StackIdsUrl { get; set; }

    [CliOption("--organizational-unit-ids")]
    public string[]? OrganizationalUnitIds { get; set; }

    [CliOption("--operation-preferences")]
    public string? OperationPreferences { get; set; }

    [CliOption("--operation-id")]
    public string? OperationId { get; set; }

    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}