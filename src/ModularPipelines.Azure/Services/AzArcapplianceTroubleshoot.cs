using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance")]
public class AzArcapplianceTroubleshoot
{
    public AzArcapplianceTroubleshoot(
        AzArcapplianceTroubleshootCommand command
    )
    {
        Command = command;
    }

    public AzArcapplianceTroubleshootCommand Command { get; }
}