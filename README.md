# UnitySVG

## Overview

UnitySVG is a library for reading SVG files and producing `Texture2D` objects from them at runtime.  The intent is to allow some of one's art assets to be reduced considerably in terms of the net time needed before the user can begin playing the game.

In order for this to be viable, three things must be true:

* The net download size of the SVG code + SVG assets (or whatever preprocessed format we want to use) + the time needed to render the SVGs and compress them must be smaller than the download time using DXT textures directly when happening at some "average-user bandwidth" (undefined here, of course).
* The resultant visual quality must be acceptable for a significant set of visual styles.  (Both with regard to the complexity of SVG image that can be used and the result of runtime texture compression.)
* The net user experience must be positive.  (Long pauses == bad, swap thrashing == bad, etc.)

On unity versions 2018.1 or later try using SVG importer by unity which provides more functionality; it can be installed into unity project by going into package manager -> Vector Graphics. More details can be found [here](https://docs.unity3d.com/Packages/com.unity.vectorgraphics@2.0/manual/index.html#using-vector-graphics)

## Compatibility

A *very* limited subset of SVG is supported, specifically *excluding*:

* Animation
* Text
* Fonts
* ICC color profiles
* Bindings
* Compound documents
* CSS
* foreignObjects

Essentially, only things involved in rendering static images are supported, excluding features related to clarity of the XML, "reuse", or whatnot.  The principle workflow I had envisioned for this centered around (semi-)auto-generated conversions from raster to vector format, so some of the more high-level features of SVG were simply not relevant.


## Performance

Pretty awful right now.  See [benchmarks.txt](benchmarks.txt) for specifics.

I strongly suggest installing and using [SVGO](https://github.com/svg/svgo) to optimize SVG files before use.


## History

The initial version of the code is a modified version of [SavageSVG](https://github.com/codebutler/savagesvg)

This fork offer major improvements in memory usage (as much as 500:1 in some extreme cases), though I have yet to appreciably improve either parsing or rendering times on hardware that isn't heavily memory-bound.


## License

See [LICENSE](LICENSE) for more information.


## TODO

Please note that it is unlikely _I_ will have time to do much of this, these are merely the things I see as necessary to achieving the goals above.  I post this with my minor improvements for the daring to dabble with, and am willing to accept pull requests.

* Serious performance optimizations, and further work on memory reduction.  Some operations seem disproportionately slow, such as rendering of arc segments.
* See about reusing relevant Unity classes (Vector2, Rect) where appropriate.
* Document which variant/version of the SVG spec we most closely match, and significant deviations from it.
* Rigorous regression/compatibility/performance testing (automated, of course).  See:
    * [http://wiki.svg.org/index.php/Main_Page](http://wiki.svg.org/index.php/Main_Page)
    * [http://www.linuxrising.org/svg_test/test_big.html](http://www.linuxrising.org/svg_test/test_big.html)
    * [http://croczilla.com/bits_and_pieces/svg/samples/](http://croczilla.com/bits_and_pieces/svg/samples/)
* Refactor the document model code to be cleanly / easily usable independent of an actual XML document.
* Separate XML (and attribute!) parser that uses the SVG DOM API we construct.
* Some support for foreignObjects to allow blending in noise textures and whatnot.  (Definitely want to use GPU compositing for this...)
* Use coroutines or threads (or a combination thereof) to make the rendering process be non-blocking.

More ambitious ideas include:

* See about using GPU for some operations?  (Compositing, etc...)
* See about implementing primitives like rendering of path segments and such on the GPU?


## Developing UnitySVG

### Workstation Setup

#### OS X

You need Mono.  The best way to ensure you get this, plus have paths set up properly is to use [HomeBrew](http://brew.sh).

```bash
brew install astyle mono
```

Make sure you add this to your `.profile` (or whatever file is appropriate to your shell), and restart your terminal:

```bash
export MONO_GAC_PREFIX="/usr/local"
```
