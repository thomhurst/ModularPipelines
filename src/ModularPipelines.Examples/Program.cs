using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines;
using ModularPipelines.Examples.Modules.Approvals;
using ModularPipelines.Examples.Modules.Launch;
using ModularPipelines.Examples.Modules.PreFlight;
using ModularPipelines.Examples.Modules.Propulsion;
using ModularPipelines.Examples.Modules.Systems;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

var builder = Pipeline.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Options.ExecutionMode = ExecutionMode.StopOnFirstException;

builder.Services

    // Layer 1: Pre-Flight Checks (No Dependencies - Run in Parallel)
    .AddModule<WeatherAnalysisModule>()
    .AddModule<CrewReadinessModule>()
    .AddModule<MissionControlOnlineModule>()
    .AddModule<SatelliteTrackingModule>()

    // Layer 2: System Validations (Single Dependencies)
    .AddModule<FuelSystemCheckModule>()
    .AddModule<NavigationSystemModule>()
    .AddModule<LifeSupportCheckModule>()
    .AddModule<CommunicationsCheckModule>()

    // Layer 3: Deep Dependency Chain (Propulsion - 4 Levels with SubModules)
    .AddModule<PropulsionDiagnosticsModule>()
    .AddModule<EngineIgnitionSequenceModule>()
    .AddModule<ThrustVectorCalibrationModule>()
    .AddModule<LaunchTrajectoryCalculationModule>()

    // Layer 4: Fan-In Approvals (Multiple Dependencies)
    .AddModule<FlightDirectorApprovalModule>()
    .AddModule<SafetyOfficerApprovalModule>()

    // Layer 5: Final Launch Sequence
    .AddModule<FinalCountdownModule>()
    .AddModule<LaunchModule>()
    .AddModule<TelemetryStreamModule>()
    .AddModule<MissionStatusReportModule>();

await builder.Build().RunAsync();
