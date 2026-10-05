# Buildah 1.33.7 inherited-option audit

Captured root help and 38 command/group help pages from Ubuntu 24.04 with
`buildah 1.33.7+ds1-1ubuntu0.24.04.3`, matching the generation workflow's package
installation. Only help/version commands were executed. The capture container
was removed. The current generated surface contains 37 commands because `info`
is excluded; version API changes remain owned by #5687.

Authoritative parser sources:
- https://github.com/containers/buildah/blob/v1.33.7/cmd/buildah/main.go
- https://github.com/containers/buildah/blob/v1.33.7/cmd/buildah/common.go
- https://github.com/containers/buildah/blob/v1.33.7/pkg/cli/common.go

The root registers 11 public PersistentFlags: cgroup manager, log level,
registries configuration file/directory, storage root/state/driver/options,
short-name alias file, and UID/GID maps. Hidden debug, CPU/memory profiling,
and default-mounts-file settings are not part of public help. Help and version
are root controls. The custom leaf usage template deliberately omits inherited
flags; leaf omissions do not mean these persistent settings are unavailable.

Storage options and UID/GID maps use StringSliceVar and accept repeated values.
UID/GID maps have custom help metavariables that hide their collection type.
The build and from commands redeclare UID/GID maps with command-local semantics.
Cobra TraverseChildren is disabled: each local map replaces its inherited flag
in the same parsing scope. Preserve local documentation and render the effective
value once, including assignments through the base options type.
