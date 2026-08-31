# gh CLI reference

`ModularPipelines.GitHub` provides strongly typed access to the `gh` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `gh` executable. Install it separately and ensure `gh` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.GitHub
```

Resolve the service with `context.Tools.Gh`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.GitHub.Services.IGh>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.GitHub.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Gh.Config.ListAsync(

            new GhConfigListOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                     | Options record                      |
| ------------------------------- | ----------------------------------- |
| `gh agent-task`                 | `GhAgentTaskOptions`                |
| `gh agent-task create`          | `GhAgentTaskCreateOptions`          |
| `gh agent-task list`            | `GhAgentTaskListOptions`            |
| `gh agent-task view`            | `GhAgentTaskViewOptions`            |
| `gh api`                        | `GhApiOptions`                      |
| `gh attestation`                | `GhAttestationOptions`              |
| `gh attestation download`       | `GhAttestationDownloadOptions`      |
| `gh attestation trusted-root`   | `GhAttestationTrustedRootOptions`   |
| `gh attestation verify`         | `GhAttestationVerifyOptions`        |
| `gh auth`                       | `GhAuthOptions`                     |
| `gh auth login`                 | `GhAuthLoginOptions`                |
| `gh auth logout`                | `GhAuthLogoutOptions`               |
| `gh auth refresh`               | `GhAuthRefreshOptions`              |
| `gh auth setup-git`             | `GhAuthSetupGitOptions`             |
| `gh auth status`                | `GhAuthStatusOptions`               |
| `gh auth switch`                | `GhAuthSwitchOptions`               |
| `gh auth token`                 | `GhAuthTokenOptions`                |
| `gh browse`                     | `GhBrowseOptions`                   |
| `gh cache`                      | `GhCacheOptions`                    |
| `gh cache delete`               | `GhCacheDeleteOptions`              |
| `gh cache list`                 | `GhCacheListOptions`                |
| `gh codespace`                  | `GhCodespaceOptions`                |
| `gh codespace code`             | `GhCodespaceCodeOptions`            |
| `gh codespace cp`               | `GhCodespaceCpOptions`              |
| `gh codespace create`           | `GhCodespaceCreateOptions`          |
| `gh codespace delete`           | `GhCodespaceDeleteOptions`          |
| `gh codespace edit`             | `GhCodespaceEditOptions`            |
| `gh codespace jupyter`          | `GhCodespaceJupyterOptions`         |
| `gh codespace list`             | `GhCodespaceListOptions`            |
| `gh codespace logs`             | `GhCodespaceLogsOptions`            |
| `gh codespace ports`            | `GhCodespacePortsOptions`           |
| `gh codespace ports forward`    | `GhCodespacePortsForwardOptions`    |
| `gh codespace ports visibility` | `GhCodespacePortsVisibilityOptions` |
| `gh codespace rebuild`          | `GhCodespaceRebuildOptions`         |
| `gh codespace ssh`              | `GhCodespaceSshOptions`             |
| `gh codespace stop`             | `GhCodespaceStopOptions`            |
| `gh codespace view`             | `GhCodespaceViewOptions`            |
| `gh config`                     | `GhConfigOptions`                   |
| `gh config clear-cache`         | `GhConfigClearCacheOptions`         |
| `gh config get`                 | `GhConfigGetOptions`                |
| `gh config list`                | `GhConfigListOptions`               |
| `gh config set`                 | `GhConfigSetOptions`                |
| `gh copilot`                    | `GhCopilotOptions`                  |
| `gh discussion`                 | `GhDiscussionOptions`               |
| `gh discussion comment`         | `GhDiscussionCommentOptions`        |
| `gh discussion create`          | `GhDiscussionCreateOptions`         |
| `gh discussion edit`            | `GhDiscussionEditOptions`           |
| `gh discussion list`            | `GhDiscussionListOptions`           |
| `gh discussion view`            | `GhDiscussionViewOptions`           |
| `gh extension`                  | `GhExtensionOptions`                |
| `gh extension browse`           | `GhExtensionBrowseOptions`          |
| `gh extension create`           | `GhExtensionCreateOptions`          |
| `gh extension install`          | `GhExtensionInstallOptions`         |
| `gh extension list`             | `GhExtensionListOptions`            |
| `gh extension remove`           | `GhExtensionRemoveOptions`          |
| `gh extension search`           | `GhExtensionSearchOptions`          |
| `gh extension upgrade`          | `GhExtensionUpgradeOptions`         |
| `gh gist`                       | `GhGistOptions`                     |
| `gh gist clone`                 | `GhGistCloneOptions`                |
| `gh gist create`                | `GhGistCreateOptions`               |
| `gh gist delete`                | `GhGistDeleteOptions`               |
| `gh gist edit`                  | `GhGistEditOptions`                 |
| `gh gist list`                  | `GhGistListOptions`                 |
| `gh gist rename`                | `GhGistRenameOptions`               |
| `gh gist view`                  | `GhGistViewOptions`                 |
| `gh gpg-key`                    | `GhGpgKeyOptions`                   |
| `gh gpg-key add`                | `GhGpgKeyAddOptions`                |
| `gh gpg-key delete`             | `GhGpgKeyDeleteOptions`             |
| `gh gpg-key list`               | `GhGpgKeyListOptions`               |
| `gh issue`                      | `GhIssueOptions`                    |
| `gh issue close`                | `GhIssueCloseOptions`               |
| `gh issue comment`              | `GhIssueCommentOptions`             |
| `gh issue create`               | `GhIssueCreateOptions`              |
| `gh issue delete`               | `GhIssueDeleteOptions`              |
| `gh issue develop`              | `GhIssueDevelopOptions`             |
| `gh issue edit`                 | `GhIssueEditOptions`                |
| `gh issue list`                 | `GhIssueListOptions`                |
| `gh issue lock`                 | `GhIssueLockOptions`                |
| `gh issue pin`                  | `GhIssuePinOptions`                 |
| `gh issue reopen`               | `GhIssueReopenOptions`              |
| `gh issue status`               | `GhIssueStatusOptions`              |
| `gh issue transfer`             | `GhIssueTransferOptions`            |
| `gh issue unlock`               | `GhIssueUnlockOptions`              |
| `gh issue unpin`                | `GhIssueUnpinOptions`               |
| `gh issue view`                 | `GhIssueViewOptions`                |
| `gh label`                      | `GhLabelOptions`                    |
| `gh label clone`                | `GhLabelCloneOptions`               |
| `gh label create`               | `GhLabelCreateOptions`              |
| `gh label delete`               | `GhLabelDeleteOptions`              |
| `gh label edit`                 | `GhLabelEditOptions`                |
| `gh label list`                 | `GhLabelListOptions`                |
| `gh licenses`                   | `GhLicensesOptions`                 |
| `gh org`                        | `GhOrgOptions`                      |
| `gh org list`                   | `GhOrgListOptions`                  |
| `gh pr`                         | `GhPrOptions`                       |
| `gh pr checkout`                | `GhPrCheckoutOptions`               |
| `gh pr checks`                  | `GhPrChecksOptions`                 |
| `gh pr close`                   | `GhPrCloseOptions`                  |
| `gh pr comment`                 | `GhPrCommentOptions`                |
| `gh pr create`                  | `GhPrCreateOptions`                 |
| `gh pr diff`                    | `GhPrDiffOptions`                   |
| `gh pr edit`                    | `GhPrEditOptions`                   |
| `gh pr list`                    | `GhPrListOptions`                   |
| `gh pr lock`                    | `GhPrLockOptions`                   |
| `gh pr merge`                   | `GhPrMergeOptions`                  |
| `gh pr ready`                   | `GhPrReadyOptions`                  |
| `gh pr reopen`                  | `GhPrReopenOptions`                 |
| `gh pr revert`                  | `GhPrRevertOptions`                 |
| `gh pr review`                  | `GhPrReviewOptions`                 |
| `gh pr status`                  | `GhPrStatusOptions`                 |
| `gh pr unlock`                  | `GhPrUnlockOptions`                 |
| `gh pr update-branch`           | `GhPrUpdateBranchOptions`           |
| `gh pr view`                    | `GhPrViewOptions`                   |
| `gh preview`                    | `GhPreviewOptions`                  |
| `gh preview prompter`           | `GhPreviewPrompterOptions`          |
| `gh project`                    | `GhProjectOptions`                  |
| `gh project close`              | `GhProjectCloseOptions`             |
| `gh project copy`               | `GhProjectCopyOptions`              |
| `gh project create`             | `GhProjectCreateOptions`            |
| `gh project delete`             | `GhProjectDeleteOptions`            |
| `gh project edit`               | `GhProjectEditOptions`              |
| `gh project field-create`       | `GhProjectFieldCreateOptions`       |
| `gh project field-delete`       | `GhProjectFieldDeleteOptions`       |
| `gh project field-list`         | `GhProjectFieldListOptions`         |
| `gh project item-add`           | `GhProjectItemAddOptions`           |
| `gh project item-archive`       | `GhProjectItemArchiveOptions`       |
| `gh project item-create`        | `GhProjectItemCreateOptions`        |
| `gh project item-delete`        | `GhProjectItemDeleteOptions`        |
| `gh project item-edit`          | `GhProjectItemEditOptions`          |
| `gh project item-list`          | `GhProjectItemListOptions`          |
| `gh project link`               | `GhProjectLinkOptions`              |
| `gh project list`               | `GhProjectListOptions`              |
| `gh project mark-template`      | `GhProjectMarkTemplateOptions`      |
| `gh project unlink`             | `GhProjectUnlinkOptions`            |
| `gh project view`               | `GhProjectViewOptions`              |
| `gh release`                    | `GhReleaseOptions`                  |
| `gh release create`             | `GhReleaseCreateOptions`            |
| `gh release delete`             | `GhReleaseDeleteOptions`            |
| `gh release delete-asset`       | `GhReleaseDeleteAssetOptions`       |
| `gh release download`           | `GhReleaseDownloadOptions`          |
| `gh release edit`               | `GhReleaseEditOptions`              |
| `gh release list`               | `GhReleaseListOptions`              |
| `gh release upload`             | `GhReleaseUploadOptions`            |
| `gh release verify`             | `GhReleaseVerifyOptions`            |
| `gh release verify-asset`       | `GhReleaseVerifyAssetOptions`       |
| `gh release view`               | `GhReleaseViewOptions`              |
| `gh repo`                       | `GhRepoOptions`                     |
| `gh repo archive`               | `GhRepoArchiveOptions`              |
| `gh repo autolink`              | `GhRepoAutolinkOptions`             |
| `gh repo autolink create`       | `GhRepoAutolinkCreateOptions`       |
| `gh repo autolink delete`       | `GhRepoAutolinkDeleteOptions`       |
| `gh repo autolink list`         | `GhRepoAutolinkListOptions`         |
| `gh repo autolink view`         | `GhRepoAutolinkViewOptions`         |
| `gh repo clone`                 | `GhRepoCloneOptions`                |
| `gh repo create`                | `GhRepoCreateOptions`               |
| `gh repo delete`                | `GhRepoDeleteOptions`               |
| `gh repo deploy-key`            | `GhRepoDeployKeyOptions`            |
| `gh repo deploy-key add`        | `GhRepoDeployKeyAddOptions`         |
| `gh repo deploy-key delete`     | `GhRepoDeployKeyDeleteOptions`      |
| `gh repo deploy-key list`       | `GhRepoDeployKeyListOptions`        |
| `gh repo edit`                  | `GhRepoEditOptions`                 |
| `gh repo fork`                  | `GhRepoForkOptions`                 |
| `gh repo gitignore`             | `GhRepoGitignoreOptions`            |
| `gh repo gitignore list`        | `GhRepoGitignoreListOptions`        |
| `gh repo gitignore view`        | `GhRepoGitignoreViewOptions`        |
| `gh repo license`               | `GhRepoLicenseOptions`              |
| `gh repo license list`          | `GhRepoLicenseListOptions`          |
| `gh repo license view`          | `GhRepoLicenseViewOptions`          |
| `gh repo list`                  | `GhRepoListOptions`                 |
| `gh repo read-dir`              | `GhRepoReadDirOptions`              |
| `gh repo read-file`             | `GhRepoReadFileOptions`             |
| `gh repo rename`                | `GhRepoRenameOptions`               |
| `gh repo set-default`           | `GhRepoSetDefaultOptions`           |
| `gh repo sync`                  | `GhRepoSyncOptions`                 |
| `gh repo unarchive`             | `GhRepoUnarchiveOptions`            |
| `gh repo view`                  | `GhRepoViewOptions`                 |
| `gh ruleset`                    | `GhRulesetOptions`                  |
| `gh ruleset check`              | `GhRulesetCheckOptions`             |
| `gh ruleset list`               | `GhRulesetListOptions`              |
| `gh ruleset view`               | `GhRulesetViewOptions`              |
| `gh run`                        | `GhRunOptions`                      |
| `gh run cancel`                 | `GhRunCancelOptions`                |
| `gh run delete`                 | `GhRunDeleteOptions`                |
| `gh run download`               | `GhRunDownloadOptions`              |
| `gh run list`                   | `GhRunListOptions`                  |
| `gh run rerun`                  | `GhRunRerunOptions`                 |
| `gh run view`                   | `GhRunViewOptions`                  |
| `gh run watch`                  | `GhRunWatchOptions`                 |
| `gh search`                     | `GhSearchOptions`                   |
| `gh search code`                | `GhSearchCodeOptions`               |
| `gh search commits`             | `GhSearchCommitsOptions`            |
| `gh search issues`              | `GhSearchIssuesOptions`             |
| `gh search prs`                 | `GhSearchPrsOptions`                |
| `gh search repos`               | `GhSearchReposOptions`              |
| `gh secret`                     | `GhSecretOptions`                   |
| `gh secret delete`              | `GhSecretDeleteOptions`             |
| `gh secret list`                | `GhSecretListOptions`               |
| `gh secret set`                 | `GhSecretSetOptions`                |
| `gh skill`                      | `GhSkillOptions`                    |
| `gh skill install`              | `GhSkillInstallOptions`             |
| `gh skill list`                 | `GhSkillListOptions`                |
| `gh skill preview`              | `GhSkillPreviewOptions`             |
| `gh skill publish`              | `GhSkillPublishOptions`             |
| `gh skill search`               | `GhSkillSearchOptions`              |
| `gh skill update`               | `GhSkillUpdateOptions`              |
| `gh ssh-key`                    | `GhSshKeyOptions`                   |
| `gh ssh-key add`                | `GhSshKeyAddOptions`                |
| `gh ssh-key delete`             | `GhSshKeyDeleteOptions`             |
| `gh ssh-key list`               | `GhSshKeyListOptions`               |
| `gh stack`                      | `GhStackOptions`                    |
| `gh status`                     | `GhStatusOptions`                   |
| `gh variable`                   | `GhVariableOptions`                 |
| `gh variable delete`            | `GhVariableDeleteOptions`           |
| `gh variable get`               | `GhVariableGetOptions`              |
| `gh variable list`              | `GhVariableListOptions`             |
| `gh variable set`               | `GhVariableSetOptions`              |
| `gh workflow`                   | `GhWorkflowOptions`                 |
| `gh workflow disable`           | `GhWorkflowDisableOptions`          |
| `gh workflow enable`            | `GhWorkflowEnableOptions`           |
| `gh workflow list`              | `GhWorkflowListOptions`             |
| `gh workflow run`               | `GhWorkflowRunOptions`              |
| `gh workflow view`              | `GhWorkflowViewOptions`             |
