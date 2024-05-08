# Bethesda LOD File Generator

## About
A simple command-line drag-and-drop utility that allows for the creation and extraction of `.lod` (LODSettings) files for supported Bethesda games.

The creation of `.lod` files is the main reason I made this program, however it also supports extracting data from existing ones.
This is only useful if you want a readable version of the data stored within the file to see what settings are used, or if you want to make sure your `.lod` files were properly generated.

###### DISCLAIMER: This is not an official tool nor is it endorsed by Bethesda.

## Game Support
Bethesda games developed after the release of Skyrim use `.lod` files to help generate LOD meshes for worldspaces.

|    Game    |     Support     |
| ---------- | --------------- |
| Skyrim LE  |    Supported    |
| Skyrim SE  |    Supported    |
| Fallout 4  |    Supported    |
| Fallout 76 | Extraction Only |
| Starfield  |  Not Supported  |

> NOTES

###### Technically the creation of `.lod` files for Fallout 76 is supported, however there's no point adding support for the full `size` value of `512` as users are not allowed to create proper mods for the game.

###### Starfield support is planned as the files are very similar to older Creation Engine `.lod` files, however I need to look into where the settings stored in the files are gathered/calculated from. Until the Creation Kit is released I can't be certain the info I gather on these files is correct.

## LODSettings File Structure
The file structure for those that are interested in how the data is stored within `.lod` files. Use a program like [HxD](https://mh-nexus.de/en/hxd/) to open `.lod` files if you want to look at them.

### Skyrim/Fallout 4
Initial info gathered from [UESP](https://en.uesp.net/wiki/Skyrim_Mod:LOD_Settings_File_Format). Further info gathered by me during the creation of this program.
```
struct lodsetting
{
    signed short west;    // 00 - The westernmost cell of the worldspace
    signed short south;   // 02 - The southernmost cell of the worldspace
    signed int size;      // 04 - A power of 2 less than or equal to 256
			  //      (How many cells in the North/East direction to generate LOD for)
    signed int lowLOD;    // 08 - Lowest LOD level (4, 8, 16, 32; Default: 4)
    signed int highLOD;   // 0C - Highest LOD level (4, 8, 16, 32; Default: 32)
};
```

### Fallout 76
Same format as a Skyrim/Fallout 4 `.lod` file but can support `size` values up to `512`.
```
struct lodsetting
{
    signed short west;    // 00 - The westernmost cell of the worldspace
    signed short south;   // 02 - The southernmost cell of the worldspace
    signed int size;      // 04 - A power of 2 less than or equal to 512
			  //      (How many cells in the North/East direction to generate LOD for)
    signed int lowLOD;    // 08 - Lowest LOD level (4, 8, 16, 32; Default: 4)
    signed int highLOD;   // 0C - Highest LOD level (4, 8, 16, 32; Default: 32)
};
```

### Starfield
A slightly updated format compared to older Creation Engine `.lod` files.

I'm still gathering info on how exactly these files are structured.
```
struct lodsetting
{
    signed int objBoundMinX;  // 00 - The worldspace object bounds' Min X value
    signed int objBoundMinY;  // 04 - The worldspace object bounds' Min Y value
    signed int size;          // 08 - Difference between worldspace object bounds' Min/Max values?
    signed int unk1;          // 0C - Engine might ignore these values? 00000000
    signed int unk2;          // 10 - Engine might ignore these values? 00000000
};
```
