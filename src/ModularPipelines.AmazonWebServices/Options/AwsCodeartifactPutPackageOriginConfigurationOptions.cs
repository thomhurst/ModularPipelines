using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "put-package-origin-configuration")]
public record AwsCodeartifactPutPackageOriginConfigurationOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--repository")] string Repository,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--package")] string Package,
[property: CommandSwitch("--restrictions")] string Restrictions
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}