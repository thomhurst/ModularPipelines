using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "extend-schema")]
public record GcloudActiveDirectoryDomainsExtendSchemaOptions(
[property: PositionalArgument] string Domain,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--ldif-file")] string LdifFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}