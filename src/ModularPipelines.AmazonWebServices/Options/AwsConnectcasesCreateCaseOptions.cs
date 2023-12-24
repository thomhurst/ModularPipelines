using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "create-case")]
public record AwsConnectcasesCreateCaseOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--fields")] string[] Fields,
[property: CommandSwitch("--template-id")] string TemplateId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}