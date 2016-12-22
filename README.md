# JpegExifRenamer
Simple but usefull console tool to sort and rename jpeg files by EXIF info

#How to use?

Simple put binary files to directory, where the jpeg files stored. Next, run.

Program will collect all jpeg files in the directory, and try to read EXIF info. If it's possible, it determine date and time of
image, and move file to such location:

.\Year\Month\Day\HourMinuteSecond_{originalFileName.originalExtension}

So, you can simple drop your jpeg files to directory, copy here binary files, and run.

#Thanks

Tool uses ExifLib, installed as NuGet package, thanks to Simon McKenzie.

https://www.codeproject.com/Articles/36342/ExifLib-A-Fast-Exif-Data-Extractor-for-NET
