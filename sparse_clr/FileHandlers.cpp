#include "pch.h"

#include <io.h>
#include <fcntl.h>
#include <stdio.h>
#include <sys\stat.h>

#include "FileHandlers.h"

namespace Compress
{
	errno_t InputFileHandle::open()
	{
		errno_t errorCode = _sopen_s( &Handle, FilenameMarshaller->c_str(), O_RDONLY | O_BINARY, _SH_DENYWR, _S_IREAD | _S_IWRITE );
		if (Handle < 0) {
			fprintf( stderr, "Cannot open input file %s (error code = %d)\n", FilenameMarshaller->c_str(), errorCode );
			return errorCode;
		}
		Opened = true;
		return 0;
	}

	errno_t OutputFileHandle::open()
	{
		errno_t errorCode = _sopen_s( &Handle, FilenameMarshaller->c_str(), O_WRONLY | O_CREAT | O_TRUNC | O_BINARY, _SH_DENYRW, _S_IREAD | _S_IWRITE );
		if (Handle < 0) {
			fprintf( stderr, "Cannot open output file %s (error code = %d)\n", FilenameMarshaller->c_str(), errorCode );
			return errorCode;
		}
		Opened = true;
		return 0;
	}
}