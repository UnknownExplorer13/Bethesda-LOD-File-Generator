<div align="center">
<h1><u>Bethesda LOD File Generator</u></h1>
<img src="Bethesda LOD File Generator/Resources/Icon.ico" />
<p style="font-size:20px"><i>Take in the sights of your world!</i></p>
</div>

<h2>About</h2>
<p>
A simple command-line drag-and-drop utility that allows for the creation and extraction of <code>.lod</code> (LODSettings) files for supported Bethesda games.

The creation of <code>.lod</code> files is the main reason I made this program, however it also supports extracting data from existing ones.
This is only useful if you want a readable version of the data stored within the file to see what settings are used, or if you want to make sure your <code>.lod</code> files were properly generated.
</p>

<h6>DISCLAIMER: This is not an official tool nor is it endorsed by Bethesda.</h6>

<h2>Game Support</h2>
<p>
Bethesda games developed after the release of Skyrim use <code>.lod</code> files to help generate LOD meshes for worldspaces.
</p>

<table>
    <tr>
        <th>Game</th>
        <th>Creation</th>
        <th>Extraction</th>
    </tr>
    <tr>
        <td>Skyrim LE</td>
        <td>Supported</td>
        <td>Supported</td>
    </tr>
    <tr>
        <td>Skyrim SE</td>
        <td>Supported</td>
        <td>Supported</td>
    </tr>
    <tr>
        <td>Fallout 4</td>
        <td>Supported</td>
        <td>Supported</td>
    </tr>
    <tr>
        <td>Fallout 76</td>
        <td>Not Supported</td>
        <td>Supported</td>
    </tr>
    <tr>
        <td>Starfield</td>
        <td>Not Supported</td>
        <td>Not Supported</td>
    </tr>
</table>

<h4>NOTES</h4>
<p>
Technically the creation of <code>.lod</code> files for Fallout 76 is supported but there's no point adding support for the full <code>size</code> value of <code>512</code> as users are not allowed to create proper mods for the game.

Starfield support is planned as the files are very similar to older Creation Engine <code>.lod</code> files, however I need to look into where the settings stored in the files are gathered/calculated from. Until the Creation Kit is released I can't be certain the info I gather on these files is correct.
</p>

<h2>LODSettings File Structure</h2>
<p>
The file structure for those that are interested in how the data is stored within the <code>.lod</code> files. Use a program like <a href="https://mh-nexus.de/en/hxd/">HxD</a> if you want to look at the files yourself.
</p>

<h3>Skyrim/Fallout 4</h3>
<p>
Initial info gathered from <a href="https://en.uesp.net/wiki/Skyrim_Mod:LOD_Settings_File_Format">UESP</a>. Further info gathered by me during the creation of this program.
</p>
<pre><code>struct lodsetting
{
    signed short west;    // 00 - The westernmost cell of the worldspace
    signed short south;   // 02 - The southernmost cell of the worldspace
    signed int size;      // 04 - A power of 2 less than or equal to 256
                          //      (How many cells in the North/East direction to generate LOD for)
    signed int lowLOD;    // 08 - Lowest LOD level (4, 8, 16, 32; Default: 4)
    signed int highLOD;   // 0C - Highest LOD level (4, 8, 16, 32; Default: 32)
};
</code></pre>

<h4>Example - DLC01SoulCairn.lod</h4>
<pre><code>CC FF CD FF 00 01 00 00 04 00 00 00 20 00 00 00</code></pre>
<table>
    <tr>
        <th></th>
        <th>West</th>
        <th>South</th>
        <th>Size</th>
        <th>Lowest LOD</th>
        <th>Highest LOD</th>
    </tr>
    <tr>
        <td><b>Bytes</b></td>
        <td>CC FF</td>
        <td>CD FF</td>
        <td>00 01 00 00</td>
        <td>04 00 00 00</td>
        <td>20 00 00 00</td>
    </tr>
    <tr>
        <td><b>Value</b></td>
        <td>-52</td>
        <td>-51</td>
        <td>256</td>
        <td>4</td>
        <td>32</td>
    </tr>
</table>

<h3>Fallout 76</h3>
<p>
Same format as a Skyrim/Fallout 4 <code>.lod</code> file but can support <code>size</code> values up to <code>512</code>.
</p>
<pre><code>struct lodsetting
{
    signed short west;    // 00 - The westernmost cell of the worldspace
    signed short south;   // 02 - The southernmost cell of the worldspace
    signed int size;      // 04 - A power of 2 less than or equal to 512
                          //      (How many cells in the North/East direction to generate LOD for)
    signed int lowLOD;    // 08 - Lowest LOD level (4, 8, 16, 32; Default: 4)
    signed int highLOD;   // 0C - Highest LOD level (4, 8, 16, 32; Default: 32)
};
</code></pre>

<h4>Example - Appalachia.lod</h4>
<pre><code>42 FF 23 FF 00 02 00 00 04 00 00 00 20 00 00 00</code></pre>
<table>
    <tr>
        <th></th>
        <th>West</th>
        <th>South</th>
        <th>Size</th>
        <th>Lowest LOD</th>
        <th>Highest LOD</th>
    </tr>
    <tr>
        <td><b>Bytes</b></td>
        <td>42 FF</td>
        <td>23 FF</td>
        <td>00 02 00 00</td>
        <td>04 00 00 00</td>
        <td>20 00 00 00</td>
    </tr>
    <tr>
        <td><b>Value</b></td>
        <td>-190</td>
        <td>-221</td>
        <td>512</td>
        <td>4</td>
        <td>32</td>
    </tr>
</table>

<h3>Starfield</h3>
<p>
A slightly updated format compared to older Creation Engine <code>.lod</code> files.

I'm still gathering info on how exactly these files are structured.
</p>
<pre><code>struct lodsetting
{
    signed int objBoundMinX;  // 00 - The worldspace object bounds' Min X value
    signed int objBoundMinY;  // 04 - The worldspace object bounds' Min Y value
    signed int size;          // 08 - Using Math.Abs(a - b) using the two largest values from Min/Max
                              //      gives about the right results but there are inconsistencies in
                              //      in some .lod files that makes me doubt this is 100% correct
    signed int unk1;          // 0C - Engine might ignore these values? 00000000
    signed int unk2;          // 10 - Engine might ignore these values? 00000000
};
</code></pre>

<h4>Example - NewAtlantis.lod</h4>
<pre><code>DD FF FF FF C5 FF FF FF 88 00 00 00 00 00 00 00 00 00 00 00</code></pre>
<table>
    <tr>
        <th></th>
        <th>objBound Min X</th>
        <th>objBound Min Y</th>
        <th>Size</th>
        <th>unk1</th>
        <th>unk2</th>
    </tr>
    <tr>
        <td><b>Bytes</b></td>
        <td>DD FF FF FF</td>
        <td>C5 FF FF FF</td>
        <td>88 00 00 00</td>
        <td>00 00 00 00</td>
        <td>00 00 00 00</td>
    </tr>
    <tr>
        <td><b>Value</b></td>
        <td>-35</td>
        <td>-59</td>
        <td>136</td>
        <td>0</td>
        <td>0</td>
    </tr>
</table>
