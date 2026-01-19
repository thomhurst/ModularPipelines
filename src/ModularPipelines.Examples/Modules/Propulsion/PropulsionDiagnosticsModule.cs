using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Propulsion;

[ModuleCategory("Propulsion")]
public class PropulsionDiagnosticsModule : Module<PropulsionStatus>
{
    protected override async Task<PropulsionStatus?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Running propulsion diagnostics on all engines...");
        context.Logger.LogDebug("Initializing engine diagnostic interface...");
        context.Logger.LogDebug("Loading engine telemetry baseline parameters...");

        // Use SubModules to check each engine individually
        var engine1 = await context.SubModule("Engine 1 Diagnostics", async () =>
        {
            context.Logger.LogInformation("Engine 1: Starting diagnostic sequence...");
            context.Logger.LogDebug("Engine 1: Checking fuel injector pressure...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 1: Fuel injector pressure: 2847 PSI - NOMINAL");
            context.Logger.LogDebug("Engine 1: Checking oxidizer flow rate...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 1: Oxidizer flow rate: 1250 kg/s - NOMINAL");
            context.Logger.LogDebug("Engine 1: Verifying turbopump RPM...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 1: Turbopump RPM: 35,420 - NOMINAL");
            context.Logger.LogInformation("Engine 1: Diagnostic sequence complete - PASS");
            return new EngineStatus(1, true, 25.0, 101.3);
        });

        var engine2 = await context.SubModule("Engine 2 Diagnostics", async () =>
        {
            context.Logger.LogInformation("Engine 2: Starting diagnostic sequence...");
            context.Logger.LogDebug("Engine 2: Checking fuel injector pressure...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 2: Fuel injector pressure: 2851 PSI - NOMINAL");
            context.Logger.LogDebug("Engine 2: Checking oxidizer flow rate...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 2: Oxidizer flow rate: 1248 kg/s - NOMINAL");
            context.Logger.LogDebug("Engine 2: Verifying turbopump RPM...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 2: Turbopump RPM: 35,380 - NOMINAL");
            context.Logger.LogInformation("Engine 2: Diagnostic sequence complete - PASS");
            return new EngineStatus(2, true, 24.8, 101.1);
        });

        var engine3 = await context.SubModule("Engine 3 Diagnostics", async () =>
        {
            context.Logger.LogInformation("Engine 3: Starting diagnostic sequence...");
            context.Logger.LogDebug("Engine 3: Checking fuel injector pressure...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 3: Fuel injector pressure: 2845 PSI - NOMINAL");
            context.Logger.LogDebug("Engine 3: Checking oxidizer flow rate...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 3: Oxidizer flow rate: 1252 kg/s - NOMINAL");
            context.Logger.LogDebug("Engine 3: Verifying turbopump RPM...");
            await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
            context.Logger.LogDebug("Engine 3: Turbopump RPM: 35,445 - NOMINAL");
            context.Logger.LogInformation("Engine 3: Diagnostic sequence complete - PASS");
            return new EngineStatus(3, true, 25.2, 101.5);
        });

        // Additional diagnostic delay
        context.Logger.LogDebug("Running cross-engine synchronization check...");
        await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        context.Logger.LogDebug("Engine synchronization delta: 0.02ms - WITHIN TOLERANCE");
        context.Logger.LogDebug("Verifying gimbal actuator response times...");
        await Task.Delay(TimeSpan.FromMilliseconds(250), cancellationToken);
        context.Logger.LogDebug("Gimbal response time: 45ms - NOMINAL");

        var engines = new[] { engine1, engine2, engine3 };
        var allNominal = engines.All(e => e.Healthy);

        foreach (var engine in engines)
        {
            context.Logger.LogInformation("Engine {Id}: {Status}, Temp: {Temp}C, Pressure: {Pressure}kPa",
                engine.EngineId,
                engine.Healthy ? "HEALTHY" : "FAULT",
                engine.Temperature,
                engine.Pressure);
        }

        var status = new PropulsionStatus(engines, allNominal);
        context.Logger.LogInformation("Propulsion system: {Status}", allNominal ? "ALL NOMINAL" : "FAULT DETECTED");

        return status;
    }
}
