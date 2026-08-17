# vault CLI reference

`ModularPipelines.Vault` provides strongly typed access to the `vault` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Vault
```

Import `ModularPipelines.Vault.Extensions`, then resolve the service with `context.Vault()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Vault.Extensions;

using ModularPipelines.Vault.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Vault().Delete(

            new VaultDeleteOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                                | Options record                               |
| ------------------------------------------ | -------------------------------------------- |
| `vault agent`                              | `VaultAgentOptions`                          |
| `vault agent generate-config`              | `VaultAgentGenerateConfigOptions`            |
| `vault audit disable`                      | `VaultAuditDisableOptions`                   |
| `vault audit enable`                       | `VaultAuditEnableOptions`                    |
| `vault audit list`                         | `VaultAuditListOptions`                      |
| `vault auth disable`                       | `VaultAuthDisableOptions`                    |
| `vault auth enable`                        | `VaultAuthEnableOptions`                     |
| `vault auth list`                          | `VaultAuthListOptions`                       |
| `vault auth move`                          | `VaultAuthMoveOptions`                       |
| `vault auth tune`                          | `VaultAuthTuneOptions`                       |
| `vault delete`                             | `VaultDeleteOptions`                         |
| `vault events subscribe`                   | `VaultEventsSubscribeOptions`                |
| `vault kv delete`                          | `VaultKvDeleteOptions`                       |
| `vault kv destroy`                         | `VaultKvDestroyOptions`                      |
| `vault kv enable-versioning`               | `VaultKvEnableVersioningOptions`             |
| `vault kv get`                             | `VaultKvGetOptions`                          |
| `vault kv list`                            | `VaultKvListOptions`                         |
| `vault kv metadata delete`                 | `VaultKvMetadataDeleteOptions`               |
| `vault kv metadata get`                    | `VaultKvMetadataGetOptions`                  |
| `vault kv metadata patch`                  | `VaultKvMetadataPatchOptions`                |
| `vault kv metadata put`                    | `VaultKvMetadataPutOptions`                  |
| `vault kv patch`                           | `VaultKvPatchOptions`                        |
| `vault kv put`                             | `VaultKvPutOptions`                          |
| `vault kv rollback`                        | `VaultKvRollbackOptions`                     |
| `vault kv undelete`                        | `VaultKvUndeleteOptions`                     |
| `vault lease lookup`                       | `VaultLeaseLookupOptions`                    |
| `vault lease renew`                        | `VaultLeaseRenewOptions`                     |
| `vault lease revoke`                       | `VaultLeaseRevokeOptions`                    |
| `vault list`                               | `VaultListOptions`                           |
| `vault login`                              | `VaultLoginOptions`                          |
| `vault monitor`                            | `VaultMonitorOptions`                        |
| `vault namespace create`                   | `VaultNamespaceCreateOptions`                |
| `vault namespace delete`                   | `VaultNamespaceDeleteOptions`                |
| `vault namespace list`                     | `VaultNamespaceListOptions`                  |
| `vault namespace lock`                     | `VaultNamespaceLockOptions`                  |
| `vault namespace lookup`                   | `VaultNamespaceLookupOptions`                |
| `vault namespace patch`                    | `VaultNamespacePatchOptions`                 |
| `vault namespace unlock`                   | `VaultNamespaceUnlockOptions`                |
| `vault operator diagnose`                  | `VaultOperatorDiagnoseOptions`               |
| `vault operator generate-root`             | `VaultOperatorGenerateRootOptions`           |
| `vault operator init`                      | `VaultOperatorInitOptions`                   |
| `vault operator key-status`                | `VaultOperatorKeyStatusOptions`              |
| `vault operator members`                   | `VaultOperatorMembersOptions`                |
| `vault operator migrate`                   | `VaultOperatorMigrateOptions`                |
| `vault operator raft autopilot get-config` | `VaultOperatorRaftAutopilotGetConfigOptions` |
| `vault operator raft autopilot set-config` | `VaultOperatorRaftAutopilotSetConfigOptions` |
| `vault operator raft autopilot state`      | `VaultOperatorRaftAutopilotStateOptions`     |
| `vault operator raft join`                 | `VaultOperatorRaftJoinOptions`               |
| `vault operator raft list-peers`           | `VaultOperatorRaftListPeersOptions`          |
| `vault operator raft remove-peer`          | `VaultOperatorRaftRemovePeerOptions`         |
| `vault operator raft snapshot inspect`     | `VaultOperatorRaftSnapshotInspectOptions`    |
| `vault operator raft snapshot restore`     | `VaultOperatorRaftSnapshotRestoreOptions`    |
| `vault operator raft snapshot save`        | `VaultOperatorRaftSnapshotSaveOptions`       |
| `vault operator rekey`                     | `VaultOperatorRekeyOptions`                  |
| `vault operator rotate`                    | `VaultOperatorRotateOptions`                 |
| `vault operator seal`                      | `VaultOperatorSealOptions`                   |
| `vault operator step-down`                 | `VaultOperatorStepDownOptions`               |
| `vault operator unseal`                    | `VaultOperatorUnsealOptions`                 |
| `vault operator usage`                     | `VaultOperatorUsageOptions`                  |
| `vault operator utilization`               | `VaultOperatorUtilizationOptions`            |
| `vault patch`                              | `VaultPatchOptions`                          |
| `vault path-help`                          | `VaultPathHelpOptions`                       |
| `vault pki health-check`                   | `VaultPkiHealthCheckOptions`                 |
| `vault pki issue`                          | `VaultPkiIssueOptions`                       |
| `vault pki list-intermediates`             | `VaultPkiListIntermediatesOptions`           |
| `vault pki verify-sign`                    | `VaultPkiVerifySignOptions`                  |
| `vault plugin deregister`                  | `VaultPluginDeregisterOptions`               |
| `vault plugin info`                        | `VaultPluginInfoOptions`                     |
| `vault plugin list`                        | `VaultPluginListOptions`                     |
| `vault plugin register`                    | `VaultPluginRegisterOptions`                 |
| `vault plugin reload`                      | `VaultPluginReloadOptions`                   |
| `vault plugin reload-status`               | `VaultPluginReloadStatusOptions`             |
| `vault plugin runtime deregister`          | `VaultPluginRuntimeDeregisterOptions`        |
| `vault plugin runtime info`                | `VaultPluginRuntimeInfoOptions`              |
| `vault plugin runtime list`                | `VaultPluginRuntimeListOptions`              |
| `vault plugin runtime register`            | `VaultPluginRuntimeRegisterOptions`          |
| `vault policy delete`                      | `VaultPolicyDeleteOptions`                   |
| `vault policy fmt`                         | `VaultPolicyFmtOptions`                      |
| `vault policy list`                        | `VaultPolicyListOptions`                     |
| `vault policy read`                        | `VaultPolicyReadOptions`                     |
| `vault policy write`                       | `VaultPolicyWriteOptions`                    |
| `vault proxy`                              | `VaultProxyOptions`                          |
| `vault read`                               | `VaultReadOptions`                           |
| `vault secrets disable`                    | `VaultSecretsDisableOptions`                 |
| `vault secrets enable`                     | `VaultSecretsEnableOptions`                  |
| `vault secrets list`                       | `VaultSecretsListOptions`                    |
| `vault secrets move`                       | `VaultSecretsMoveOptions`                    |
| `vault secrets tune`                       | `VaultSecretsTuneOptions`                    |
| `vault server`                             | `VaultServerOptions`                         |
| `vault ssh`                                | `VaultSshOptions`                            |
| `vault status`                             | `VaultStatusOptions`                         |
| `vault token capabilities`                 | `VaultTokenCapabilitiesOptions`              |
| `vault token create`                       | `VaultTokenCreateOptions`                    |
| `vault token lookup`                       | `VaultTokenLookupOptions`                    |
| `vault token renew`                        | `VaultTokenRenewOptions`                     |
| `vault token revoke`                       | `VaultTokenRevokeOptions`                    |
| `vault transform import`                   | `VaultTransformImportOptions`                |
| `vault transform import-version`           | `VaultTransformImportVersionOptions`         |
| `vault transit import`                     | `VaultTransitImportOptions`                  |
| `vault transit import-version`             | `VaultTransitImportVersionOptions`           |
| `vault unwrap`                             | `VaultUnwrapOptions`                         |
| `vault version-history`                    | `VaultVersionHistoryOptions`                 |
| `vault write`                              | `VaultWriteOptions`                          |
