using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot")]
public class AzIotDu
{
    public AzIotDu(
        AzIotDuAccount account,
        AzIotDuDevice device,
        AzIotDuInstance instance,
        AzIotDuUpdate update
    )
    {
        Account = account;
        Device = device;
        Instance = instance;
        Update = update;
    }

    public AzIotDuAccount Account { get; }

    public AzIotDuDevice Device { get; }

    public AzIotDuInstance Instance { get; }

    public AzIotDuUpdate Update { get; }
}