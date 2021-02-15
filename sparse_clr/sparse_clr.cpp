#include "pch.h"

#include <stdio.h>

#include "FileHandlers.h"
#include "Resparser.h"
#include "SparseFile.h"

#include "sparse_clr.h"

using namespace Compress;

bool Sparse::Decompress ( array<String^>^ sparseFiles, String^ imgFile )
{
	OutputFileHandle outputFile( imgFile );
	if (outputFile.open() == 0) 
	{
		for each (String ^ sparseFile in sparseFiles)
		{
			InputFileHandle inputFile( sparseFile );
			if (inputFile.open() != 0) 
				return false;

			SparseFile sparseFileHolder;
			if (!sparseFileHolder.import( inputFile.getHandle() ))
				return false;

			if (_lseek( outputFile.getHandle(), 0, SEEK_SET ) == -1) {
				perror( "lseek failed" );
				return false;
			}
			if (!sparseFileHolder.write( outputFile.getHandle(), false ))
				return false;
		}
		return true;
	}
    return false;
}

bool imgToSimg(String^ imgFile, String^ sparseFile, unsigned int blockSize)
{
	if (blockSize < 1024) {
		fprintf(stderr, "Block size would be greater than 1024 or equal.\n");
		return false;
	}
	if (blockSize % 4 != 0) {
		fprintf(stderr, "Block size would be aligned to 4.\n");
		return false;
	}

	InputFileHandle inputFile(imgFile);
	OutputFileHandle outputFile(sparseFile);
	if (inputFile.open() == 0 && outputFile.open() == 0)
	{
		SparseFile sparseFileHolder;
		if (sparseFileHolder.create(inputFile.getHandle(), blockSize) && sparseFileHolder.read(inputFile.getHandle()))
		{
			return sparseFileHolder.write(outputFile.getHandle(), true);
		}
	}
	return false;
}

bool Sparse::Compress(String^ imgFile, unsigned int blockSize, unsigned int maxSize) 
{
	if (imgToSimg(imgFile, imgFile + ".sparse", blockSize))
	{
		InputFileHandle inputFile( imgFile + ".sparse" );
		if (inputFile.open() != 0)
			return false;

		SparseFile sparseFileHolder;
		if (!sparseFileHolder.import(inputFile.getHandle()))
			return false;

		Resparser resparser(&sparseFileHolder);
		return resparser.resparse(imgFile + ".chunk", maxSize );
	}
	return false;
}



