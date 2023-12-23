using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-listing-change-set")]
public record AwsDatazoneCreateListingChangeSetOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--entity-identifier")] string EntityIdentifier,
[property: CommandSwitch("--entity-type")] string EntityType
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--entity-revision")]
    public string? EntityRevision { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}