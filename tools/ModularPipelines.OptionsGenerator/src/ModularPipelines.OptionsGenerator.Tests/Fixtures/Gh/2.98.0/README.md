# GitHub CLI 2.98.0 global-option audit

Captured with official `gh version 2.98.0 (2026-08-20)` on Windows using `gh <command> --help`; root uses `gh --help`. A temporary empty GH_CONFIG_DIR excludes user aliases and extensions. Only help was executed. Line endings, trailing spaces, and final blank lines are normalized; option content is unchanged.

The [root manual](https://cli.github.com/manual/gh) and root fixture expose help/version control actions only. The [issue list manual](https://cli.github.com/manual/gh_issue_list) lists `--repo`/`-R` among inherited flags because the issue command group provides it. It is absent from auth status, config get, and api help, so it is not a universal GhOptions property. The existing command records correctly retain repository selection on supported commands.

Disposition: no production or generated API change. Captured-help and package rendering tests protect the current scope, scalar arity, alias, ordering, and lack of duplicate flags. Existing command/version APIs and generated snapshots remain unchanged; regeneration is unnecessary for this no-change audit.
