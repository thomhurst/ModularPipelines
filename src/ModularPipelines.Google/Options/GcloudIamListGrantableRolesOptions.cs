using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "list-grantable-roles")]
public record GcloudIamListGrantableRolesOptions(
[property: CliArgument] string Resource
) : GcloudOptions;