$ErrorActionPreference = 'Stop'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) "generation-winget-tests-$([guid]::NewGuid().ToString('N'))"
$dependencyRoot = Join-Path $testRoot 'dependencies with spaces'
$x64 = Join-Path $dependencyRoot 'x64'
New-Item -ItemType Directory -Path $x64 -Force | Out-Null

try {
    $bundle = Join-Path $testRoot 'installer.msixbundle'
    $expectedPaths = @('runtime.appx', 'desktop.msix') | ForEach-Object { Join-Path $x64 $_ }
    foreach ($path in @($bundle) + $expectedPaths + @(Join-Path $x64 'ignore.txt')) {
        Set-Content -LiteralPath $path -Value ''
    }
    $registration = @{ Calls = 0 }
    function Add-AppxPackage {
        param([string] $Path, [string[]] $DependencyPath, [switch] $ForceUpdateFromAnyVersion)
        $registration.Calls++
        if ($Path -ne $bundle -or -not $ForceUpdateFromAnyVersion -or
            $DependencyPath.Count -ne $expectedPaths.Count -or
            (Compare-Object $expectedPaths $DependencyPath)) {
            throw 'WinGet registration did not receive the bundle and individual dependency paths.'
        }
    }

    & "$PSScriptRoot/Install-GenerationWinGet.ps1" -Bundle $bundle -DependencyRoot $dependencyRoot
    if ($registration.Calls -ne 1) { throw 'WinGet registration was not invoked.' }

    $expectedPaths | ForEach-Object { Remove-Item -LiteralPath $_ }
    try {
        & "$PSScriptRoot/Install-GenerationWinGet.ps1" -Bundle $bundle -DependencyRoot $dependencyRoot
        throw 'Empty dependency directory was accepted.'
    }
    catch {
        if ($_.Exception.Message -ne 'No x64 WinGet dependencies found in the verified archive.') { throw }
    }
    if ($registration.Calls -ne 1) { throw 'Empty dependencies reached registration.' }
    Write-Host 'WinGet registration tests passed.'
}
finally {
    if (-not ([IO.Path]::GetFullPath($testRoot)).StartsWith([IO.Path]::GetFullPath([IO.Path]::GetTempPath()), [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Test cleanup path is outside the temporary directory.'
    }
    Remove-Item -LiteralPath $testRoot -Recurse -Force
}
