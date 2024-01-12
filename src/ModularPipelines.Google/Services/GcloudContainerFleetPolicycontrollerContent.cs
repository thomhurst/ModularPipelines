using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "policycontroller")]
public class GcloudContainerFleetPolicycontrollerContent
{
    public GcloudContainerFleetPolicycontrollerContent(
        GcloudContainerFleetPolicycontrollerContentBundles bundles,
        GcloudContainerFleetPolicycontrollerContentTemplates templates
    )
    {
        Bundles = bundles;
        Templates = templates;
    }

    public GcloudContainerFleetPolicycontrollerContentBundles Bundles { get; }

    public GcloudContainerFleetPolicycontrollerContentTemplates Templates { get; }
}