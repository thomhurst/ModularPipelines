### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
AwaitThis | Usage | Error | Prohibit await this in modules
StatefulModule | Design | Warning | Detects mutable instance fields in modules that could leak state between executions
MPDEP001 | Usage | Error | DependsOn type does not implement IModule
MPDEP002 | Usage | Error | Circular dependency detected between modules (renamed from ConflictingDependsOnAttribute)
MPDEP003 | Usage | Error | Module depends on itself
MPCLI001 | Usage | Error | CliFlag property must be bool? or int?
MPCLI002 | Usage | Error | Value-less bool? CliOption should use CliFlag
MPCLI003 | Usage | Error | Multiple CLI attributes applied to one property
MPCLI004 | Usage | Error | Duplicate CLI switch in an options hierarchy
MPCLI005 | Usage | Error | Duplicate CLI argument position in a command phase
MPCLI006 | Usage | Error | CLI attributes used outside CommandLineToolOptions
MPCLI007 | Usage | Error | Conflicting CliTool identity in an options hierarchy
MPCLI008 | Usage | Warning | Static subcommand options hierarchy has no CliTool

### Removed Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|--------------------
ConflictingDependsOnAttribute | Usage | Error | Renamed to MPDEP002
