# ModularPipelines

Read [CLAUDE.md](../../../../CLAUDE.md) for guarded build/test commands, formatting, documentation builds, and generated-option constraints.

- Run local .NET commands through `scripts/Invoke-AgentDotNet.ps1` using in-process PowerShell array binding. Respect its time/memory limits; never build/format the all-project solution or execute the build pipeline without explicit authorization.
- Change generators/scrapers and regenerate options; do not hand-edit generated output or preserve obsolete generated APIs with shims.
- Stop only services created for the current worktree; shared lock Redis must remain running.
