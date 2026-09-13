[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidatePattern('^[a-z0-9][a-z0-9-]*$')][string]$Tool,
    [Parameter(Mandatory)][string]$SourceRef,
    [Parameter(Mandatory)][string]$DefaultBranch
)

$branch = "automated/update-cli-options-$Tool"
if ($SourceRef -cne $DefaultBranch) {
    # Hash the complete, case-sensitive ref so slashes cannot create ref-directory conflicts
    # and distinct names such as feature/foo and feature-foo cannot overwrite each other.
    $bytes = [Text.Encoding]::UTF8.GetBytes($SourceRef)
    $digest = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
    $branch += "-ref-$digest"
}

$branch
