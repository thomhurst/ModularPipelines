using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspace-add-ons", "get-authorization")]
public record GcloudWorkspaceAddOnsGetAuthorizationOptions : GcloudOptions;