# flyway CLI reference

`ModularPipelines.Flyway` provides strongly typed access to the `flyway` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `flyway` executable. Install it separately and ensure `flyway` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Flyway
```

Resolve the service with `context.Tools.Flyway`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Flyway.Services.IFlyway>()` instead.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var flyway = context.Tools.Flyway;
```

## Global options[​](#global-options "Direct link to Global options")

Global options are rendered before the selected subcommand.

| CLI option                      | Property                       | Availability | Description                                                                                     |
| ------------------------------- | ------------------------------ | ------------ | ----------------------------------------------------------------------------------------------- |
| `-baselineDescription`          | `BaselineDescription`          | All editions | Description to tag schema with when executing baseline                                          |
| `-baselineOnMigrate`            | `BaselineOnMigrate`            | All editions | Baseline on migrate against uninitialized non-empty schema                                      |
| `-baselineVersion`              | `BaselineVersion`              | All editions | Version to tag schema with when executing baseline                                              |
| `-batch`                        | `Batch`                        | Flyway Teams | \[teams] Batch SQL statements when executing them                                               |
| `-callbacks`                    | `Callbacks`                    | All editions | Comma-separated list of FlywayCallback classes, or locations to scan for FlywayCallback classes |
| `-cherryPick`                   | `CherryPick`                   | Flyway Teams | \[teams] Comma separated list of migrations that Flyway should consider when migrating          |
| `-cleanDisabled`                | `CleanDisabled`                | All editions | Whether to disable clean                                                                        |
| `-cleanOnValidationError`       | `CleanOnValidationError`       | All editions | \[Deprecated] Automatically clean on a validation error                                         |
| `-color`                        | `Color`                        | All editions | Whether to colorize output. Values: always, never, or auto (default)                            |
| `-configFileEncoding`           | `ConfigFileEncoding`           | All editions | Encoding to use when loading the config files                                                   |
| `-configFiles`                  | `ConfigFiles`                  | All editions | Comma-separated list of config files to use                                                     |
| `-connectRetries`               | `ConnectRetries`               | All editions | Maximum number of retries when attempting to connect to the database                            |
| `-createSchemas`                | `CreateSchemas`                | All editions | Whether Flyway should attempt to create the schemas specified in the schemas property           |
| `-detectEncoding`               | `DetectEncoding`               | Flyway Teams | \[teams] Whether Flyway should try to automatically detect SQL migration file encoding          |
| `-driver`                       | `Driver`                       | All editions | Fully qualified classname of the JDBC driver                                                    |
| `-dryRunOutput`                 | `DryRunOutput`                 | Flyway Teams | \[teams] File where to output the SQL statements of a migration dry run                         |
| `-encoding`                     | `Encoding`                     | All editions | Encoding of SQL migrations                                                                      |
| `-errorOverrides`               | `ErrorOverrides`               | Flyway Teams | \[teams] Rules to override specific SQL states and errors codes                                 |
| `-executeInTransaction`         | `ExecuteInTransaction`         | All editions | Whether SQL should execute within a transaction                                                 |
| `-failOnMissingLocations`       | `FailOnMissingLocations`       | All editions | Whether to fail if a location specified in the flyway.locations option doesn't exist            |
| `-ignoreMigrationPatterns`      | `IgnoreMigrationPatterns`      | All editions | Patterns of migrations and states to ignore during validate                                     |
| `-initSql`                      | `InitSql`                      | All editions | SQL statements to run to initialize a new database connection                                   |
| `-installedBy`                  | `InstalledBy`                  | All editions | Username that will be recorded in the schema history table                                      |
| `-jarDirs`                      | `JarDirs`                      | All editions | Comma-separated list of dirs for Jdbc drivers & Java migrations                                 |
| `-jdbcProperties.`              | `JdbcProperties`               | All editions | Properties to pass to the JDBC driver object                                                    |
| `-licenseKey`                   | `LicenseKey`                   | Flyway Teams | \[teams] Your Flyway license key                                                                |
| `-locations`                    | `Locations`                    | All editions | Classpath locations to scan recursively for migrations                                          |
| `-lockRetryCount`               | `LockRetryCount`               | All editions | The maximum number of retries when trying to obtain a lock                                      |
| `-mixed`                        | `Mixed`                        | All editions | Allow mixing transactional and non-transactional statements                                     |
| `-n`                            | `NonInteractive`               | All editions | Suppress prompting for a user and password                                                      |
| `-outOfOrder`                   | `OutOfOrder`                   | All editions | Allows migrations to be run "out of order"                                                      |
| `-outputFile`                   | `OutputFile`                   | All editions | Send output to the specified file alongside the console                                         |
| `-outputType`                   | `OutputType`                   | All editions | Serialise the output in the given format, Values: json                                          |
| `-password`                     | `Password`                     | All editions | Password to use to connect to the database                                                      |
| `-placeholderPrefix`            | `PlaceholderPrefix`            | All editions | Prefix of every placeholder                                                                     |
| `-placeholderReplacement`       | `PlaceholderReplacement`       | All editions | Whether placeholders should be replaced                                                         |
| `-placeholders.`                | `Placeholders`                 | All editions | Placeholders to replace in sql migrations                                                       |
| `-placeholderSuffix`            | `PlaceholderSuffix`            | All editions | Suffix of every placeholder                                                                     |
| `-q`                            | `Quiet`                        | All editions | Suppress all output, except for errors and warnings                                             |
| `-repeatableSqlMigrationPrefix` | `RepeatableSqlMigrationPrefix` | All editions | File name prefix for repeatable SQL migrations                                                  |
| `-resolvers`                    | `Resolvers`                    | All editions | Comma-separated list of custom MigrationResolvers                                               |
| `-schemas`                      | `Schemas`                      | All editions | Comma-separated list of the schemas managed by Flyway                                           |
| `-scriptPlaceholderPrefix`      | `ScriptPlaceholderPrefix`      | All editions | Prefix of every script placeholder                                                              |
| `-scriptPlaceholderSuffix`      | `ScriptPlaceholderSuffix`      | All editions | Suffix of every script placeholder                                                              |
| `-skipDefaultCallbacks`         | `SkipDefaultCallbacks`         | All editions | Skips default callbacks (sql)                                                                   |
| `-skipDefaultResolvers`         | `SkipDefaultResolvers`         | All editions | Skips default resolvers (jdbc, sql and Spring-jdbc)                                             |
| `-skipExecutingMigrations`      | `SkipExecutingMigrations`      | All editions | Whether Flyway should skip actually executing the contents of the migrations                    |
| `-sqlMigrationPrefix`           | `SqlMigrationPrefix`           | All editions | File name prefix for versioned SQL migrations                                                   |
| `-sqlMigrationSeparator`        | `SqlMigrationSeparator`        | All editions | File name separator for SQL migrations                                                          |
| `-sqlMigrationSuffixes`         | `SqlMigrationSuffixes`         | All editions | Comma-separated list of file name suffixes for SQL migrations                                   |
| `-stream`                       | `Stream`                       | Flyway Teams | \[teams] Stream SQL migrations when executing them                                              |
| `-table`                        | `Table`                        | All editions | Name of Flyway's schema history table                                                           |
| `-target`                       | `Target`                       | All editions | Target version up to which Flyway should use migrations                                         |
| `-undoSqlMigrationPrefix`       | `UndoSqlMigrationPrefix`       | Flyway Teams | \[teams] File name prefix for undo SQL migrations                                               |
| `-url`                          | `Url`                          | All editions | Jdbc url to use to connect to the database                                                      |
| `-user`                         | `User`                         | All editions | User to use to connect to the database                                                          |
| `-validateMigrationNaming`      | `ValidateMigrationNaming`      | All editions | Validate file names of SQL migrations (including callbacks)                                     |
| `-validateOnMigrate`            | `ValidateOnMigrate`            | All editions | Validate when running migrate                                                                   |
| `-X`                            | `Debug`                        | All editions | Print debug output                                                                              |

## Commands[​](#commands "Direct link to Commands")

| CLI command           | Options record             |
| --------------------- | -------------------------- |
| `flyway add`          | `FlywayAddOptions`         |
| `flyway auth`         | `FlywayAuthOptions`        |
| `flyway baseline`     | `FlywayBaselineOptions`    |
| `flyway check`        | `FlywayCheckOptions`       |
| `flyway clean`        | `FlywayCleanOptions`       |
| `flyway deploy`       | `FlywayDeployOptions`      |
| `flyway diff`         | `FlywayDiffOptions`        |
| `flyway diffApply`    | `FlywayDiffApplyOptions`   |
| `flyway diffText`     | `FlywayDiffTextOptions`    |
| `flyway generate`     | `FlywayGenerateOptions`    |
| `flyway info`         | `FlywayInfoOptions`        |
| `flyway init`         | `FlywayInitOptions`        |
| `flyway list-engines` | `FlywayListEnginesOptions` |
| `flyway migrate`      | `FlywayMigrateOptions`     |
| `flyway prepare`      | `FlywayPrepareOptions`     |
| `flyway repair`       | `FlywayRepairOptions`      |
| `flyway snapshot`     | `FlywaySnapshotOptions`    |
| `flyway undo`         | `FlywayUndoOptions`        |
| `flyway validate`     | `FlywayValidateOptions`    |
