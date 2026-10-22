$ErrorActionPreference = 'Stop'
$testRoot = Join-Path ([IO.Path]::GetTempPath()) "generation-elf-tests-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $testRoot | Out-Null

try {
    $executable = Join-Path $testRoot 'pnpm'
    $validHeader = [byte[]]::new(64)
    [Array]::Copy([byte[]] @(0x7f, 0x45, 0x4c, 0x46, 2, 1, 1, 0), $validHeader, 8)
    $validHeader[16] = 3 # ET_DYN (PIE)
    $validHeader[18] = 0x3e # EM_X86_64

    function Assert-Rejected([byte[]] $bytes, [string] $scenario) {
        [IO.File]::WriteAllBytes($executable, $bytes)
        try {
            & "$PSScriptRoot/Assert-GenerationLinuxX64Executable.ps1" -Path $executable
            throw "Accepted $scenario."
        }
        catch {
            if ($_.Exception.Message -notlike 'Invalid generation executable*expected a 64-bit little-endian Linux ELF executable for x86-64*Check the pinned platform archive*') {
                throw
            }
        }
    }

    foreach ($type in @(2, 3)) {
        foreach ($abi in @(0, 3)) {
            $validHeader[16] = $type
            $validHeader[7] = $abi
            [IO.File]::WriteAllBytes($executable, $validHeader)
            & "$PSScriptRoot/Assert-GenerationLinuxX64Executable.ps1" -Path $executable
        }
    }

    Assert-Rejected ([Text.Encoding]::UTF8.GetBytes("# pnpm's native binary replaces this file during installation`nexec node bin/pnpm.mjs")) 'the pnpm npm placeholder'
    Assert-Rejected ([Text.Encoding]::UTF8.GetBytes('<html>download failed</html>')) 'an error page'
    Assert-Rejected ([byte[]]::new(0)) 'an empty download'
    Assert-Rejected $validHeader[0..19] 'a truncated ELF header'
    foreach ($case in @(
        @{ Index = 0; Value = 0; Name = 'invalid ELF magic' },
        @{ Index = 4; Value = 1; Name = '32-bit ELF' },
        @{ Index = 5; Value = 2; Name = 'big-endian ELF' },
        @{ Index = 6; Value = 0; Name = 'invalid ELF version' },
        @{ Index = 7; Value = 9; Name = 'FreeBSD ELF' },
        @{ Index = 16; Value = 1; Name = 'a relocatable object' },
        @{ Index = 17; Value = 1; Name = 'invalid ELF type' },
        @{ Index = 18; Value = 0xb7; Name = 'ARM64 ELF' },
        @{ Index = 19; Value = 1; Name = 'invalid ELF machine' }
    )) {
        $invalidHeader = $validHeader.Clone()
        $invalidHeader[$case.Index] = $case.Value
        Assert-Rejected $invalidHeader $case.Name
    }
    Write-Host 'Linux x64 generation executable tests passed.'
}
finally {
    if (-not ([IO.Path]::GetFullPath($testRoot)).StartsWith([IO.Path]::GetFullPath([IO.Path]::GetTempPath()), [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Test cleanup path is outside the temporary directory.'
    }
    Remove-Item -LiteralPath $testRoot -Recurse -Force
}
