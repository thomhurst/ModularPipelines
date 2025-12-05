#!/usr/bin/env python3
"""
Migration script to convert old CLI attributes to new CLI attributes in Docker, Helm, and Kubernetes packages.
"""

import os
import re
import glob

def migrate_file(filepath):
    """Migrate a single file to use new CLI attributes."""
    with open(filepath, 'r', encoding='utf-8-sig') as f:
        content = f.read()

    original = content

    # 1. Replace CommandPrecedingArguments with CliCommand
    # [CommandPrecedingArguments("docker", "config", "create")]
    content = re.sub(
        r'\[CommandPrecedingArguments\(([^)]+)\)\]',
        r'[CliCommand(\1)]',
        content
    )

    # 2. Replace BooleanCommandSwitch with CliFlag
    # [BooleanCommandSwitch("--force")] -> [CliFlag("--force")]
    content = re.sub(
        r'\[BooleanCommandSwitch\(([^)]+)\)\]',
        r'[CliFlag(\1)]',
        content
    )

    # 3. Replace CommandEqualsSeparatorSwitch with CliOption with Format
    # [CommandEqualsSeparatorSwitch("--option")] -> [CliOption("--option", Format = OptionFormat.EqualsSeparated)]
    content = re.sub(
        r'\[CommandEqualsSeparatorSwitch\("([^"]+)"\)\]',
        r'[CliOption("\1", Format = OptionFormat.EqualsSeparated)]',
        content
    )

    # 4. Replace CommandSwitch with CliOption
    # [CommandSwitch("--option")] -> [CliOption("--option")]
    content = re.sub(
        r'\[CommandSwitch\(([^)]+)\)\]',
        r'[CliOption(\1)]',
        content
    )

    # 5. Replace PositionalArgument with CliArgument
    # [PositionalArgument(Position = Position.AfterSwitches)] -> [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    # [PositionalArgument(Position = Position.BeforeSwitches)] -> [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    # [PositionalArgument] -> [CliArgument]
    content = re.sub(
        r'\[PositionalArgument\(Position\s*=\s*Position\.AfterSwitches\)\]',
        r'[CliArgument(Placement = ArgumentPlacement.AfterOptions)]',
        content
    )
    content = re.sub(
        r'\[PositionalArgument\(Position\s*=\s*Position\.BeforeSwitches\)\]',
        r'[CliArgument(Placement = ArgumentPlacement.BeforeOptions)]',
        content
    )
    content = re.sub(
        r'\[PositionalArgument\]',
        r'[CliArgument]',
        content
    )
    # Handle PlaceholderName pattern
    content = re.sub(
        r'\[PositionalArgument\(PlaceholderName\s*=\s*"([^"]+)"\)\]',
        r'[CliArgument(Name = "\1")]',
        content
    )

    # 6. Add OptionFormat using if CliOption with Format is used
    if 'OptionFormat.EqualsSeparated' in content and 'using ModularPipelines.Options;' not in content:
        # Find the last using statement and add after it
        content = re.sub(
            r'(using [^;]+;)\n(\nnamespace)',
            r'\1\nusing ModularPipelines.Options;\2',
            content
        )

    if content != original:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
        return True
    return False

def migrate_package(package_path):
    """Migrate all files in a package."""
    migrated = 0
    for filepath in glob.glob(os.path.join(package_path, '**', '*.cs'), recursive=True):
        # Skip Generated folder
        if 'Generated' in filepath or 'obj' in filepath or 'bin' in filepath:
            continue
        if migrate_file(filepath):
            print(f"Migrated: {filepath}")
            migrated += 1
    return migrated

def main():
    packages = [
        r'C:\git\ModularPipelines\src\ModularPipelines.Azure',
    ]

    total = 0
    for package in packages:
        print(f"\n=== Migrating {os.path.basename(package)} ===")
        count = migrate_package(package)
        total += count
        print(f"Migrated {count} files")

    print(f"\n=== Total: {total} files migrated ===")

if __name__ == '__main__':
    main()
