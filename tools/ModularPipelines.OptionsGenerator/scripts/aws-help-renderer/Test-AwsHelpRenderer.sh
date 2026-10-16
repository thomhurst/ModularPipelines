#!/usr/bin/env bash
set -euo pipefail
renderer_dir="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
export MODULARPIPELINES_REAL_GROFF="$(command -v groff)"
chmod +x "$renderer_dir/groff"
output="$("$renderer_dir/groff" -m man -T ascii "$renderer_dir/wrapped-help.man")"
for token in \
  describe-local-gateway-route-table-virtual-interface-group-associations \
  describe-local-gateway-route-table-virtual-interface-group-associations-with-extra-command-tokens \
  compatibility Overrides automatic direct-ConnectGatewayId; do
  if ! grep -Fq -- "$token" <<< "$output"; then
    printf 'AWS help renderer split token: %s\n' "$token" >&2
    exit 1
  fi
done
printf 'AWS help renderer preserves command names and description words.\n'
