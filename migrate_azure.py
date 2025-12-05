#!/usr/bin/env python3
"""Migrate Azure option files from CliCommand to CliSubCommand."""
import os
import re

def migrate_file(filepath):
    """Migrate a single file from CliCommand to CliSubCommand."""
    with open(filepath, 'r', encoding='utf-8-sig') as f:
        content = f.read()

    # Skip files that don't have CliCommand or already have CliSubCommand
    if 'CliSubCommand' in content:
        return False
    if '[CliCommand(' not in content:
        return False

    # Skip base options file (AzOptions.cs) - it doesn't have CliCommand attribute
    if 'AzOptions.cs' in filepath and ': CommandLineToolOptions' in content:
        return False

    # Replace [CliCommand(...)] with [CliSubCommand(...)]
    new_content = re.sub(r'\[CliCommand\(', '[CliSubCommand(', content)

    if new_content != content:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(new_content)
        return True
    return False

def main():
    options_dir = r'C:\git\ModularPipelines\src\ModularPipelines.Azure\Options'

    migrated = 0
    skipped = 0

    for root, dirs, files in os.walk(options_dir):
        for filename in files:
            if filename.endswith('.cs'):
                filepath = os.path.join(root, filename)
                if migrate_file(filepath):
                    migrated += 1
                else:
                    skipped += 1

    print(f"Migrated: {migrated} files")
    print(f"Skipped: {skipped} files")

if __name__ == '__main__':
    main()
