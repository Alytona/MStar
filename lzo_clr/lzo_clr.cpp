#include "pch.h"

//#include <lzo\lzoconf.h>
//# include "conf.h"

# include "lzo_clr.h"
# include "FileHandlers.h"

extern "C" {
	int lzop_compress (const char* input_name, const char* output_name);
	int lzop_decompress (const char* input_name, const char* output_name);
}

namespace Lzo 
{
	bool Lzo::Compress ( String^ plainFileName, String^ compressedFileName ) 
	{
		CStrMarshaller plainFileNameMarshaller( plainFileName );
		CStrMarshaller compressedFileNameMarshaller( compressedFileName );
		return lzop_compress( plainFileNameMarshaller.c_str(), compressedFileNameMarshaller.c_str() ) != 0;
	}

	bool Lzo::Decompress ( String^ compressedFileName, String^ plainFileName ) 
	{
		CStrMarshaller compressedFileNameMarshaller( compressedFileName );
		CStrMarshaller decompressedFileNameMarshaller( plainFileName );
		return lzop_decompress( compressedFileNameMarshaller.c_str(), decompressedFileNameMarshaller.c_str() ) != 0;
	}
}