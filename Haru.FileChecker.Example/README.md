# Haru.FileChecker.Example

A reimplementation of EFT's consistency checking.

## Usage

```sh
Haru.FileChecker.Example --mode=[mode] --path=[basepath] --config=[config]
```

Generate `ConsistencyInfo`:

```sh
Haru.FileChecker.Example --mode=generate --path=./client --config=./ConsistencyInfo
```

Validate `ConsistencyInfo`'s entries:

```sh
Haru.FileChecker.Example --mode=validate --path=./client --config=./client/ConsistencyInfo
```

## Implementation

### EFT

`FilesChecker.dll` contains all the logic to enable EFT to check if file sizes
are still correct, if the required files exist at all, and for some if the
checksum matches.

the file `ConsistencyInfo` is ignored by the client and exclusively used by the
`BsgLauncher`. Instead, the class `ConsistencyMetadataProvider` contains a list
with all the metadata.

The "critical" check differs between the two;

- `BsgLauncher` check the MD5 hash of the file
- `FilesChecker.dll` combines byte values (logic located inside
  `ConsistencyController`)

Priority modes determine which checks run:

**Priority**        | **Checks**
------------------- | --------------------------------------------------------
normal              | if the file exists, file size match
critical (client)   | if the file exists, file size match, file checksum match
critical (launcher) | if the file exists, file size match, file hash match

### Differences

There is a lot less code involved, and it's alot faster thanks to parallel
generation/validating of the files.
