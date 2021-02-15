#include "pch.h"

#include <io.h>
#include <stdio.h>

#include "SparseFile.h"

namespace Compress
{
	bool SparseFile::create ( int fileHandle, unsigned int blockSize )
	{
		destroy();

		int64_t len = _lseeki64( fileHandle, 0, SEEK_END );
		_lseeki64( fileHandle, 0, SEEK_SET );

		SparseFileCookie = sparse_file_new( blockSize, len );
		if (SparseFileCookie == nullptr) {
			fprintf( stderr, "Failed to create sparse file\n" );
			return false;
		}
		return true;
	}
	bool SparseFile::read ( int fileHandle )
	{
		sparse_file_verbose( SparseFileCookie );
		if (sparse_file_read( SparseFileCookie, fileHandle, false, false ) != 0) {
			fprintf( stderr, "Failed to read file\n" );
			return false;
		}
		return true;
	}

	bool SparseFile::import ( int fileHandle )
	{
		destroy();

		SparseFileCookie = sparse_file_import( fileHandle, true, false );
		if (SparseFileCookie == nullptr) {
			fprintf( stderr, "Failed to import sparse file\n" );
			return false;
		}
		return true;
	}

	bool SparseFile::write ( int fileHandle, bool sparse )
	{
		int result = sparse_file_write( SparseFileCookie, fileHandle, sparse, false );
		if (sparse && result != 0 || !sparse && result < 0)
		{
			fprintf( stderr, "Cannot write output file\n" );
			return false;
		}
		return true;
	}

	int SparseFile::getResparseFilesQuantity( unsigned int maxSize )
	{
		int files = sparse_file_resparse( SparseFileCookie, maxSize, nullptr, 0 );
		if (files < 0) {
			fprintf( stderr, "Failed to resparse\n" );
		}
		return files;
	}
	int SparseFile::resparse( sparse_file** out_s, unsigned int maxSize, int filesQuantity )
	{
		int files = sparse_file_resparse( SparseFileCookie, maxSize, out_s, filesQuantity );
		if (files < 0) {
			fprintf( stderr, "Failed to resparse\n" );
		}
		return files;
	}
}