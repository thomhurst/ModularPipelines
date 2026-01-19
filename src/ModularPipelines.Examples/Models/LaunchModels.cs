namespace ModularPipelines.Examples.Models;

// PreFlight
public record WeatherReport(double Temperature, double WindSpeed, double Visibility, bool IsLaunchSafe);

public record CrewStatus(int CrewCount, bool AllHealthy, bool AllTrained);

public record GroundSystemStatus(bool Online, int ActiveStations, DateTime LastHeartbeat);

public record SatelliteLink(string SatelliteId, double SignalStrength, bool Locked);

// Systems
public record FuelSystemStatus(double FuelLevel, double OxidizerLevel, bool PressureNominal);

public record NavigationData(double Latitude, double Longitude, double Heading, bool GpsLocked);

public record LifeSupportStatus(double OxygenLevel, double CO2Level, double CabinPressure, bool Nominal);

public record CommsStatus(bool UplinkActive, bool DownlinkActive, double Bandwidth);

// Propulsion
public record EngineStatus(int EngineId, bool Healthy, double Temperature, double Pressure);

public record PropulsionStatus(EngineStatus[] Engines, bool AllNominal);

public record IgnitionStatus(bool IgnitionReady, DateTime IgnitionTime);

public record ThrustVectorData(double Pitch, double Yaw, double Roll, bool Calibrated);

public record TrajectoryData(double Azimuth, double Inclination, double ApogeeKm, bool Calculated);

// Approvals
public record ApprovalResult(bool Approved, string ApproverRole, DateTime Timestamp, string? Reason);

// Launch
public record CountdownStatus(int SecondsRemaining, bool HoldCalled, string Phase);

public record LaunchResult(bool Successful, DateTime LaunchTime, string MissionId);

public record TelemetryData(double AltitudeKm, double VelocityMps, double FuelRemaining, TimeSpan MissionElapsed);

public record MissionReport(string MissionId, bool Success, TimeSpan TotalDuration, string Summary);
