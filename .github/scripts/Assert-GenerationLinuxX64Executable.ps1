param(
    [Parameter(Mandatory)]
    [string] $Path
)

$ErrorActionPreference = 'Stop'
$stream = [IO.File]::OpenRead([IO.Path]::GetFullPath($Path))
try {
    $header = [byte[]]::new(64)
    $length = $stream.ReadAtLeast($header, $header.Length, $false)
}
finally {
    $stream.Dispose()
}

# Ubuntu generation jobs execute the binary directly, without a shell fallback.
# In particular, pnpm's npm wrapper is a shebang-less placeholder until its
# lifecycle scripts replace it; archive integrity alone does not make it runnable.
if ($length -lt 64 -or
    $header[0] -ne 0x7f -or $header[1] -ne 0x45 -or $header[2] -ne 0x4c -or $header[3] -ne 0x46 -or
    $header[4] -ne 2 -or $header[5] -ne 1 -or $header[6] -ne 1 -or
    $header[7] -notin @(0, 3) -or
    $header[16] -notin @(2, 3) -or $header[17] -ne 0 -or
    $header[18] -ne 0x3e -or $header[19] -ne 0) {
    $prefix = [Convert]::ToHexString($header, 0, [Math]::Min($length, 20))
    throw "Invalid generation executable '$Path': expected a 64-bit little-endian Linux ELF executable for x86-64. First bytes: $prefix. Check the pinned platform archive and extraction path; npm wrapper packages may require install scripts."
}
