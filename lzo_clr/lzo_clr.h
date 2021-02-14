#pragma once

using namespace System;

namespace Lzo {

	public ref class Lzo
	{
	public:
		static bool Compress ( String^ plainFileName, String^ compressedFileName );
		static bool Decompress ( String^ compressedFileName, String^ plainFileName );
	};
}
