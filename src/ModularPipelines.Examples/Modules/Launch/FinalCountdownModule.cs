using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Examples.Modules.Approvals;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Launch;

[DependsOn<FlightDirectorApprovalModule>]
[DependsOn<SafetyOfficerApprovalModule>]
[ModuleCategory("Launch")]
public class FinalCountdownModule : Module<CountdownStatus>
{
    protected override async Task<CountdownStatus?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var flightDirector = (await context.GetModule<FlightDirectorApprovalModule>()).ValueOrDefault!;
        var safetyOfficer = (await context.GetModule<SafetyOfficerApprovalModule>()).ValueOrDefault!;

        if (!flightDirector.Approved)
        {
            context.Logger.LogError("Launch aborted: Flight Director denied approval - {Reason}",
                flightDirector.Reason);
            return new CountdownStatus(0, true, "HOLD");
        }

        if (!safetyOfficer.Approved)
        {
            context.Logger.LogError("Launch aborted: Safety Officer denied approval - {Reason}",
                safetyOfficer.Reason);
            return new CountdownStatus(0, true, "HOLD");
        }

        context.Logger.LogInformation("All approvals received - initiating final countdown sequence");
        context.Logger.LogDebug("Transferring to internal power...");
        context.Logger.LogDebug("Ground power disconnect confirmed");

        // Simulate countdown with SubModules for visibility
        await context.SubModule("T-10 seconds", async () =>
        {
            context.Logger.LogInformation("T-10... Main engine start sequence initiated");
            context.Logger.LogDebug("Fuel prevalves opening...");
            await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
            context.Logger.LogDebug("Oxidizer prevalves opening...");
            await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
            context.Logger.LogDebug("Ignition system armed");
            context.Logger.LogInformation("T-8... Engine ignition sequence start");
            await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
            context.Logger.LogDebug("Spark igniters activated");
            context.Logger.LogInformation("T-6... Engine 1 ignition confirmed");
            await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
            context.Logger.LogInformation("T-6... Engine 2 ignition confirmed");
            context.Logger.LogInformation("T-6... Engine 3 ignition confirmed");
            await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
            return true;
        });

        await context.SubModule("T-5 seconds", async () =>
        {
            context.Logger.LogInformation("T-5... All engines at full thrust");
            context.Logger.LogDebug("Engine 1 thrust: 100%");
            context.Logger.LogDebug("Engine 2 thrust: 100%");
            context.Logger.LogDebug("Engine 3 thrust: 100%");
            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
            context.Logger.LogInformation("T-4... Thrust nominal, vehicle is GO");
            context.Logger.LogDebug("Verifying hold-down clamp status...");
            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
            context.Logger.LogInformation("T-3... Final flight computer check");
            context.Logger.LogDebug("Flight computer A: GO");
            context.Logger.LogDebug("Flight computer B: GO");
            context.Logger.LogDebug("Flight computer C: GO");
            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
            context.Logger.LogInformation("T-2... Vehicle is in startup");
            await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
            context.Logger.LogInformation("T-1... Guidance is internal");
            return true;
        });

        await context.SubModule("T-0 seconds", async () =>
        {
            context.Logger.LogInformation("T-0... LIFTOFF!");
            context.Logger.LogDebug("Hold-down clamps released");
            await Task.Delay(TimeSpan.FromMilliseconds(300), cancellationToken);
            context.Logger.LogInformation("Tower cleared!");
            context.Logger.LogDebug("Beginning roll program...");
            await Task.Delay(TimeSpan.FromMilliseconds(300), cancellationToken);
            context.Logger.LogInformation("Roll program complete, vehicle is on course");
            context.Logger.LogDebug("Throttling down for Max Q...");
            await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
            return true;
        });

        return new CountdownStatus(
            SecondsRemaining: 0,
            HoldCalled: false,
            Phase: "LIFTOFF");
    }
}
