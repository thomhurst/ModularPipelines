using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.PreFlight;

[ModuleCategory("PreFlight")]
public class WeatherAnalysisModule : Module<WeatherReport>
{
    protected override async Task<WeatherReport?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Analyzing weather conditions at launch site...");
        context.Logger.LogDebug("Connecting to meteorological data service...");

        await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
        context.Logger.LogInformation("Retrieving surface wind data from anemometer array...");
        context.Logger.LogDebug("Wind sensor 1: 8.1 mph NE");
        context.Logger.LogDebug("Wind sensor 2: 8.5 mph NE");
        context.Logger.LogDebug("Wind sensor 3: 8.2 mph NE");

        await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
        context.Logger.LogInformation("Measuring atmospheric temperature at multiple altitudes...");
        context.Logger.LogDebug("Ground level: 72.5F");
        context.Logger.LogDebug("500ft: 70.2F");
        context.Logger.LogDebug("1000ft: 68.1F");
        context.Logger.LogDebug("5000ft: 55.3F");

        await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
        context.Logger.LogInformation("Checking visibility conditions...");
        context.Logger.LogDebug("Horizontal visibility: 10+ statute miles");
        context.Logger.LogDebug("Cloud ceiling: 25,000ft scattered");

        await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
        context.Logger.LogInformation("Analyzing upper atmosphere wind shear data...");
        context.Logger.LogDebug("Max Q wind shear: Within acceptable limits");
        context.Logger.LogDebug("Jet stream position: 180nm north of launch site");

        await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
        context.Logger.LogInformation("Checking for lightning within 20nm radius...");
        context.Logger.LogDebug("Lightning detection: NEGATIVE");
        context.Logger.LogDebug("Anvil cloud presence: NEGATIVE");

        var report = new WeatherReport(
            Temperature: 72.5,
            WindSpeed: 8.3,
            Visibility: 10.0,
            IsLaunchSafe: true);

        context.Logger.LogInformation("Weather analysis complete: {Temp}F, Wind {Wind}mph, Visibility {Vis}mi - {Status}",
            report.Temperature, report.WindSpeed, report.Visibility,
            report.IsLaunchSafe ? "SAFE" : "UNSAFE");

        return report;
    }
}
