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
