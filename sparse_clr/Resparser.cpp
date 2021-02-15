#include "pch.h"

#include <stdio.h>
#include <stdlib.h>

#include "FileHandlers.h"
#include "Resparser.h"

namespace Compress 
{
	void Resparser::clearCookies() {
		if (OutputSparseFileCookies != nullptr) {
			for (int i = 0; i < FilesQuantity; i++) {
				if (OutputSparseFileCookies[i] != nullptr)
					sparse_file_destroy( OutputSparseFileCookies[i] );
			}
			delete[] OutputSparseFileCookies;
			OutputSparseFileCookies = nullptr;
		}
		FilesQuantity = 0;
	}

	bool Resparser::resparse( String^ chunkFilename, unsigned int maxSize )
	{
		clearCookies();

		int filesQuantity = InputSparseFile->getResparseFilesQuantity( maxSize );
		if (filesQuantity < 0) {
			return false;
		}

		OutputSparseFileCookies = new struct sparse_file* [filesQuantity];

		if (!OutputSparseFileCookies) {
			fprintf( stderr, "Failed to allocate sparse file array\n" );
			return false;
		}

		FilesQuantity = filesQuantity;
		for (int i = 0; i < filesQuantity; i++)
			OutputSparseFileCookies[i] = nullptr;

		int resparseResult = InputSparseFile->resparse( OutputSparseFileCookies, maxSize, FilesQuantity );
		if (resparseResult < 0) {
			return false;
		}

		for (int i = 0; i < FilesQuantity; i++)
		{
			OutputFileHandle outputFile( chunkFilename + "." + i );
			if (outputFile.open() != 0)
				return false;

			int ret = sparse_file_write( OutputSparseFileCookies[i], outputFile.getHandle(), true, false );
			if (ret) {
				fprintf( stderr, "Failed to write sparse file\n" );
				return false;
			}
		}
		clearCookies();
		return true;
	}
}