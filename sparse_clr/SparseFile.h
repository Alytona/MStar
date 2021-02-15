#pragma once

#include "sparse.h"

namespace Compress {

	class SparseFile
	{
		sparse_file* SparseFileCookie = nullptr;

		void destroy()
		{
			if (SparseFileCookie != nullptr)
				sparse_file_destroy( SparseFileCookie );
		}

	public:
		SparseFile()
		{
		}
		SparseFile( sparse_file* sparseFileCookie )
		{
			SparseFileCookie = sparseFileCookie;
		}
		~SparseFile()
		{
			destroy();
		}

		bool create ( int fileHandle, unsigned int blockSize );
		bool read ( int fileHandle );
		bool import ( int fileHandle );
		bool write ( int fileHandle, bool sparse );

		int getResparseFilesQuantity( unsigned int maxSize );
		int resparse( sparse_file** out_s, unsigned int maxSize, int filesQuantity );
	};
}
