# ChannelMerger

>*You can call it Merger-chan*

A utility to merge up to four grayscale images into an RGB one.
___
___

## Features

- Can process 8bit and 16bit images
- Can process images in basically any format
- Outputs in basically any format desired
- Supports both RGB and RGBA output

## Usage

- Select resolution of images you want to process, or enter your own
- Load grayscale images into the channels you need
- Select colour fill forchannels where no iamges will be loaded (or don't - default is white)
- Select an output folder
- Enter file name and extension
- Click ``MERGE``
- ???
- PROFIT

## Notes

- Resolution format **must** be ``123x456`` where ``123`` is width and ``456`` is height
- It's best to load up all four images of the selected resolution. That's because:
  - Images larger than selected resolution will be cropped to those dimensions
  - Weird shit will happen if you load up smaller images. Don't.

## Known bugs

- Could it be..? None..?

## Todo

- Add image scaling, so an image too big won't need to be cropped, and image too small wan't cause... Whatever it is it's causing now.
- Add themes, 'cause why not ¯\\\_(ツ)_/¯
- Add 32bit processing..?
- Maybe some basic image manipulation stuff too, like invert, etc.
- Fix bugs
- Add new bugs