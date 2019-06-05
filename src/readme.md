# PGN Tools
A set of tools and libraries to read [PGN Files](https://en.wikipedia.org/wiki/Portable_Game_Notation).

* PGNTools - Common Library supporting all the PGN tools.

* PGNQuery - Information about PGN files/database
  * Sorting functionality for resulting output games
  * Option to output as JSON

* PGN2JSON - Standard alone console application that only works against file system PGN (no PGN Library support, see PGN Query for that) and produces JSON output.

* PGNLibraryCreate - To create a new PGN Library database

* PGNLibraryImport - import PGN files in to a previously created PGN Library DB (see PGNLibraryCreate)

## PGNQuery

Queries a "database" of PGN files, outputting the files that match the specified criteriea, the "database" is either an actual PGN Library database (see PGNLibraryCreate and PGNLibraryImport) or any collection(s) of PGN files on the file system.

## PGNReader

NET Standard library component that deserialises the PGN files in to C# objects.

## PGN-tools.Common
Code common to all the PGN tools.


# BUILDING THE PROJECT
*NOTE* - the command line projects are dependent on my own simple command line argument processor [YACLAP](https://github.com/Chrislee187/yaclap), which is pulled in as a NUGET dependency, I haven't got around to releasing the processor on any public NUGET gallery yet so currently this component pulls the package from a local build folder (added locally to my machine as an additional NUGET package source)

# TODO
## Common Command Lines Arguments

* DONE ~~arguments - are always files or paths,
     	if file exists it is added to the input sources
    	if path exists (and has no *) in it, paths\*.pgn are added to input sources.
     recursive path searches may be specified by the command line options --recurse (see below)~~
* if paths exists and has a * in it wildcards are resolved and files added to input source

## Common Command Lines Options

* DONE ~~`--recurse`, if any arguments are valid paths (optionally recurse through subfolders as well~~
* `--output filename` redirect stdout to the specified filename
* `--use-db` "connectionstring" use the specified database as the input source

