using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "create-service")]
public record AwsProtonCreateServiceOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--template-major-version")] string TemplateMajorVersion,
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--branch-name")]
    public string? BranchName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--repository-connection-arn")]
    public string? RepositoryConnectionArn { get; set; }

    [CommandSwitch("--repository-id")]
    public string? RepositoryId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--template-minor-version")]
    public string? TemplateMinorVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}