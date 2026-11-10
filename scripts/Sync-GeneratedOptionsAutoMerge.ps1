# Revalidate generated options PRs on review activity, including already-armed PRs.
# Only trusted default-branch code calls this script. Never execute PR code here.
[CmdletBinding()]
param(
    [Parameter(Mandatory)][int]$Pr,
    [Parameter(Mandatory)][string]$Repo,
    [Parameter(Mandatory)][string]$DefaultBranch
)

$ErrorActionPreference = 'Stop'
$raw = gh pr view $Pr --repo $Repo --json state,isDraft,isCrossRepository,headRefName,baseRefName,headRefOid,autoMergeRequest
if ($LASTEXITCODE -ne 0) { throw 'Could not read generated options PR state.' }
$view = ($raw -join "`n") | ConvertFrom-Json
if ($view.state -ne 'OPEN' -or $view.isCrossRepository -or
    $view.headRefName -notlike 'automated/update-cli-options-*' -or
    $view.baseRefName -ne $DefaultBranch) {
    Write-Output "::notice::#$Pr is not an eligible generated options PR."
    exit 0
}

# Disarm first: a missing review, a new finding, or even a failed gate request must
# not leave earlier clearance active. Re-arm below only after every gate passes.
if ($null -ne $view.autoMergeRequest) {
    gh pr merge $Pr --repo $Repo --disable-auto
    if ($LASTEXITCODE -ne 0) { throw 'Could not disarm generated options auto-merge.' }
}
if ($view.isDraft) { exit 0 }

& pwsh -NoProfile -File (Join-Path $PSScriptRoot 'Assert-CodexReviewClear.ps1') `
    -Pr $Pr -Repo $Repo -HeadSha $view.headRefOid -RequireConversationClearance
if ($LASTEXITCODE -ne 0) {
    Write-Output "::notice::#$Pr remains unarmed; Codex or conversation clearance is pending."
    exit 0
}

& pwsh -NoProfile -File (Join-Path $PSScriptRoot 'Enable-AutoMerge.ps1') `
    -Pr $Pr -Repo $Repo -ExpectedHeadSha $view.headRefOid
if ($LASTEXITCODE -ne 0) {
    Write-Output "::notice::#$Pr remains unarmed; the merge gate denied clearance."
}
exit 0
