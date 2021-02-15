#pragma once

#include "sparse.h"

#include "SparseFile.h"

namespace Compress {

	class Resparser
	{
		SparseFile* InputSparseFile;
		sparse_file** OutputSparseFileCookies = nullptr;
		int FilesQuantity;

		void clearCookies();

	public:
		Resparser( SparseFile* inputFile )
		{
			InputSparseFile = inputFile;
			FilesQuantity = 0;
		}
		~Resparser()
		{
			clearCookies();
		}
		bool resparse( String^ chunkFilename, unsigned int maxSize );
	};
}
