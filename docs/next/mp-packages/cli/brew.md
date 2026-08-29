# brew CLI reference

`ModularPipelines.Homebrew` provides strongly typed access to the `brew` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `brew` executable. Install it separately and ensure `brew` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Homebrew
```

Resolve the service with `context.Tools.Brew`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Homebrew.Services.IBrew>()` instead.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var brew = context.Tools.Brew;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                       | Options record                        |
| --------------------------------- | ------------------------------------- |
| `brew alias`                      | `BrewAliasOptions`                    |
| `brew as-console-user`            | `BrewAsConsoleUserOptions`            |
| `brew audit`                      | `BrewAuditOptions`                    |
| `brew autoremove`                 | `BrewAutoremoveOptions`               |
| `brew bottle`                     | `BrewBottleOptions`                   |
| `brew bump`                       | `BrewBumpOptions`                     |
| `brew bump-cask-pr`               | `BrewBumpCaskPrOptions`               |
| `brew bump-compatibility-version` | `BrewBumpCompatibilityVersionOptions` |
| `brew bump-formula-pr`            | `BrewBumpFormulaPrOptions`            |
| `brew bump-revision`              | `BrewBumpRevisionOptions`             |
| `brew bump-unversioned-casks`     | `BrewBumpUnversionedCasksOptions`     |
| `brew bundle`                     | `BrewBundleOptions`                   |
| `brew bundle add`                 | `BrewBundleAddOptions`                |
| `brew bundle check`               | `BrewBundleCheckOptions`              |
| `brew bundle cleanup`             | `BrewBundleCleanupOptions`            |
| `brew bundle dump`                | `BrewBundleDumpOptions`               |
| `brew bundle edit`                | `BrewBundleEditOptions`               |
| `brew bundle env`                 | `BrewBundleEnvOptions`                |
| `brew bundle exec`                | `BrewBundleExecOptions`               |
| `brew bundle install`             | `BrewBundleInstallOptions`            |
| `brew bundle list`                | `BrewBundleListOptions`               |
| `brew bundle remove`              | `BrewBundleRemoveOptions`             |
| `brew bundle sh`                  | `BrewBundleShOptions`                 |
| `brew cat`                        | `BrewCatOptions`                      |
| `brew cleanup`                    | `BrewCleanupOptions`                  |
| `brew command`                    | `BrewCommandOptions`                  |
| `brew command-not-found-init`     | `BrewCommandNotFoundInitOptions`      |
| `brew completions`                | `BrewCompletionsOptions`              |
| `brew completions link`           | `BrewCompletionsLinkOptions`          |
| `brew completions state`          | `BrewCompletionsStateOptions`         |
| `brew completions unlink`         | `BrewCompletionsUnlinkOptions`        |
| `brew config`                     | `BrewConfigOptions`                   |
| `brew contributions`              | `BrewContributionsOptions`            |
| `brew create`                     | `BrewCreateOptions`                   |
| `brew debugger`                   | `BrewDebuggerOptions`                 |
| `brew deps`                       | `BrewDepsOptions`                     |
| `brew desc`                       | `BrewDescOptions`                     |
| `brew developer`                  | `BrewDeveloperOptions`                |
| `brew developer off`              | `BrewDeveloperOffOptions`             |
| `brew developer on`               | `BrewDeveloperOnOptions`              |
| `brew developer state`            | `BrewDeveloperStateOptions`           |
| `brew docs`                       | `BrewDocsOptions`                     |
| `brew doctor`                     | `BrewDoctorOptions`                   |
| `brew edit`                       | `BrewEditOptions`                     |
| `brew exec`                       | `BrewExecOptions`                     |
| `brew extract`                    | `BrewExtractOptions`                  |
| `brew fetch`                      | `BrewFetchOptions`                    |
| `brew formula`                    | `BrewFormulaOptions`                  |
| `brew generate-man-completions`   | `BrewGenerateManCompletionsOptions`   |
| `brew generate-zap`               | `BrewGenerateZapOptions`              |
| `brew gist-logs`                  | `BrewGistLogsOptions`                 |
| `brew home`                       | `BrewHomeOptions`                     |
| `brew info`                       | `BrewInfoOptions`                     |
| `brew install`                    | `BrewInstallOptions`                  |
| `brew install-bundler-gems`       | `BrewInstallBundlerGemsOptions`       |
| `brew irb`                        | `BrewIrbOptions`                      |
| `brew leaves`                     | `BrewLeavesOptions`                   |
| `brew lgtm`                       | `BrewLgtmOptions`                     |
| `brew link`                       | `BrewLinkOptions`                     |
| `brew linkage`                    | `BrewLinkageOptions`                  |
| `brew list`                       | `BrewListOptions`                     |
| `brew livecheck`                  | `BrewLivecheckOptions`                |
| `brew log`                        | `BrewLogOptions`                      |
| `brew migrate`                    | `BrewMigrateOptions`                  |
| `brew missing`                    | `BrewMissingOptions`                  |
| `brew nodenv-sync`                | `BrewNodenvSyncOptions`               |
| `brew options`                    | `BrewOptionsOptions`                  |
| `brew outdated`                   | `BrewOutdatedOptions`                 |
| `brew pin`                        | `BrewPinOptions`                      |
| `brew postinstall`                | `BrewPostinstallOptions`              |
| `brew prof`                       | `BrewProfOptions`                     |
| `brew pyenv-sync`                 | `BrewPyenvSyncOptions`                |
| `brew rbenv-sync`                 | `BrewRbenvSyncOptions`                |
| `brew readall`                    | `BrewReadallOptions`                  |
| `brew reinstall`                  | `BrewReinstallOptions`                |
| `brew ruby`                       | `BrewRubyOptions`                     |
| `brew rubydoc`                    | `BrewRubydocOptions`                  |
| `brew sandbox-exec`               | `BrewSandboxExecOptions`              |
| `brew search`                     | `BrewSearchOptions`                   |
| `brew services`                   | `BrewServicesOptions`                 |
| `brew services cleanup`           | `BrewServicesCleanupOptions`          |
| `brew services info`              | `BrewServicesInfoOptions`             |
| `brew services kill`              | `BrewServicesKillOptions`             |
| `brew services list`              | `BrewServicesListOptions`             |
| `brew services restart`           | `BrewServicesRestartOptions`          |
| `brew services run`               | `BrewServicesRunOptions`              |
| `brew services start`             | `BrewServicesStartOptions`            |
| `brew services stop`              | `BrewServicesStopOptions`             |
| `brew sh`                         | `BrewShOptions`                       |
| `brew source`                     | `BrewSourceOptions`                   |
| `brew style`                      | `BrewStyleOptions`                    |
| `brew tab`                        | `BrewTabOptions`                      |
| `brew tap`                        | `BrewTapOptions`                      |
| `brew tap-info`                   | `BrewTapInfoOptions`                  |
| `brew tap-new`                    | `BrewTapNewOptions`                   |
| `brew test`                       | `BrewTestOptions`                     |
| `brew test-bot`                   | `BrewTestBotOptions`                  |
| `brew tests`                      | `BrewTestsOptions`                    |
| `brew trust`                      | `BrewTrustOptions`                    |
| `brew typecheck`                  | `BrewTypecheckOptions`                |
| `brew unalias`                    | `BrewUnaliasOptions`                  |
| `brew unbottled`                  | `BrewUnbottledOptions`                |
| `brew uninstall`                  | `BrewUninstallOptions`                |
| `brew unlink`                     | `BrewUnlinkOptions`                   |
| `brew unpack`                     | `BrewUnpackOptions`                   |
| `brew unpin`                      | `BrewUnpinOptions`                    |
| `brew untap`                      | `BrewUntapOptions`                    |
| `brew untrust`                    | `BrewUntrustOptions`                  |
| `brew update`                     | `BrewUpdateOptions`                   |
| `brew update-perl-resources`      | `BrewUpdatePerlResourcesOptions`      |
| `brew update-python-resources`    | `BrewUpdatePythonResourcesOptions`    |
| `brew update-reset`               | `BrewUpdateResetOptions`              |
| `brew update-test`                | `BrewUpdateTestOptions`               |
| `brew upgrade`                    | `BrewUpgradeOptions`                  |
| `brew uses`                       | `BrewUsesOptions`                     |
| `brew vendor-gems`                | `BrewVendorGemsOptions`               |
| `brew verify`                     | `BrewVerifyOptions`                   |
| `brew version-install`            | `BrewVersionInstallOptions`           |
| `brew vulns`                      | `BrewVulnsOptions`                    |
| `brew which-formula`              | `BrewWhichFormulaOptions`             |
| `brew which-update`               | `BrewWhichUpdateOptions`              |
