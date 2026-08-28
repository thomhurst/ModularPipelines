[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $RepositoryRoot,

    [Parameter(Mandatory)]
    [string[]] $ManifestPath,

    [string[]] $AllowedPath = @(),

    [string[]] $AllowedPathFile = @(),

    [string[]] $AllowedOversizedPath = @(),

    [ValidateRange(1, [long]::MaxValue)]
    [long] $MaximumFileSizeBytes = 5MB
)

$ErrorActionPreference = 'Stop'
$forbiddenExtensions = [System.Collections.Generic.HashSet[string]]::new(
    [string[]] @(
        '.7z', '.bin', '.cab', '.deb', '.dll', '.dylib', '.exe', '.gz',
        '.msi', '.msix', '.nupkg', '.rar', '.rpm', '.so', '.tar', '.tgz',
        '.xz', '.zip'),
    [System.StringComparer]::OrdinalIgnoreCase)
$pathComparer = if ($IsWindows) {
    [System.StringComparer]::OrdinalIgnoreCase
}
else {
    [System.StringComparer]::Ordinal
}
$comparison = if ($IsWindows) {
    [System.StringComparison]::OrdinalIgnoreCase
}
else {
    [System.StringComparison]::Ordinal
}

$repositoryPath = [System.IO.Path]::GetFullPath($RepositoryRoot)
if (-not (Test-Path -LiteralPath $repositoryPath -PathType Container)) {
    throw "Repository root does not exist: $repositoryPath"
}

$repositoryPrefix = $repositoryPath.TrimEnd(
    [System.IO.Path]::DirectorySeparatorChar,
    [System.IO.Path]::AltDirectorySeparatorChar) +
    [System.IO.Path]::DirectorySeparatorChar
$allowedPaths = [System.Collections.Generic.HashSet[string]]::new($pathComparer)
$allowedOversizedPaths = [System.Collections.Generic.HashSet[string]]::new($pathComparer)

function ConvertTo-SafeRelativePath([string] $Path) {
    if ([string]::IsNullOrWhiteSpace($Path) -or [System.IO.Path]::IsPathRooted($Path)) {
        throw "Generated path must be a non-empty repository-relative path: '$Path'"
    }

    $fullPath = [System.IO.Path]::GetFullPath(
        (Join-Path $repositoryPath $Path))
    if (-not $fullPath.StartsWith($repositoryPrefix, $comparison)) {
        throw "Generated path escapes the repository: '$Path'"
    }

    return [System.IO.Path]::GetRelativePath($repositoryPath, $fullPath)
        .Replace('\', '/')
}

foreach ($path in $AllowedPath) {
    $null = $allowedPaths.Add((ConvertTo-SafeRelativePath $path))
}

foreach ($path in $AllowedOversizedPath) {
    $relativePath = ConvertTo-SafeRelativePath $path
    $null = $allowedOversizedPaths.Add($relativePath)
}

foreach ($pathFile in @($ManifestPath) + @($AllowedPathFile)) {
    if (-not (Test-Path -LiteralPath $pathFile -PathType Leaf)) {
        throw "Generated path manifest does not exist: $pathFile"
    }

    foreach ($path in Get-Content -LiteralPath $pathFile) {
        if (-not [string]::IsNullOrWhiteSpace($path)) {
            $null = $allowedPaths.Add((ConvertTo-SafeRelativePath $path))
        }
    }
}

$changedPaths = [System.Collections.Generic.HashSet[string]]::new($pathComparer)
$pathsToStage = [System.Collections.Generic.HashSet[string]]::new($pathComparer)
$gitPathQueries = @(
    @{ Arguments = @('diff', '--name-only'); NeedsStaging = $true },
    @{ Arguments = @('diff', '--cached', '--name-only'); NeedsStaging = $false },
    @{ Arguments = @('ls-files', '--others', '--exclude-standard'); NeedsStaging = $true }
)
foreach ($query in $gitPathQueries) {
    $arguments = $query.Arguments
    $paths = & git -C $repositoryPath -c core.quotePath=false @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "git $($arguments -join ' ') failed."
    }

    foreach ($path in $paths) {
        if (-not [string]::IsNullOrWhiteSpace($path)) {
            $relativePath = ConvertTo-SafeRelativePath $path
            $null = $changedPaths.Add($relativePath)
            if ($query.NeedsStaging) {
                $null = $pathsToStage.Add($relativePath)
            }
        }
    }
}

foreach ($file in Get-ChildItem -LiteralPath $repositoryPath -File) {
    if ($forbiddenExtensions.Contains($file.Extension) -or
        $file.Length -gt $MaximumFileSizeBytes) {
        throw "Unexpected root artifact '$($file.Name)' ($($file.Length) bytes). " +
            'CLI downloads and extraction must stay under runner temp.'
    }
}

foreach ($path in $changedPaths) {
    if (-not $allowedPaths.Contains($path)) {
        throw "Unexpected checkout change '$path'. Only generator-manifest paths may be staged."
    }

    $fullPath = Join-Path $repositoryPath $path
    if (-not (Test-Path -LiteralPath $fullPath -PathType Leaf)) {
        continue
    }

    $file = Get-Item -LiteralPath $fullPath
    if ($forbiddenExtensions.Contains($file.Extension)) {
        throw "Generated change '$path' uses forbidden artifact extension '$($file.Extension)'."
    }

    if ($file.Length -gt $MaximumFileSizeBytes -and
        -not $allowedOversizedPaths.Contains($path)) {
        throw "Generated change '$path' is $($file.Length) bytes; limit is $MaximumFileSizeBytes bytes."
    }
}

if ($changedPaths.Count -eq 0) {
    return
}

if ($pathsToStage.Count -gt 0) {
    $pathspecFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllLines(
            $pathspecFile,
            [string[]] $pathsToStage,
            [System.Text.UTF8Encoding]::new($false))
        & git -C $repositoryPath add "--pathspec-from-file=$pathspecFile"
        if ($LASTEXITCODE -ne 0) {
            throw 'Failed to stage generated paths.'
        }
    }
    finally {
        Remove-Item -LiteralPath $pathspecFile -Force -ErrorAction SilentlyContinue
    }
}

$stagedPaths = & git -C $repositoryPath -c core.quotePath=false diff --cached --name-only
if ($LASTEXITCODE -ne 0) {
    throw 'Failed to inspect staged generated paths.'
}

foreach ($path in $stagedPaths) {
    $relativePath = ConvertTo-SafeRelativePath $path
    if (-not $allowedPaths.Contains($relativePath)) {
        throw "Staging escaped the generator manifest: '$relativePath'."
    }
}
