#pragma once

using namespace System;

namespace Compress 
{
	public ref class Sparse
	{
	public:
		static bool Compress(String^ imgFile, unsigned int blockSize, unsigned int maxSize);
		static bool Decompress ( array<String^>^ sparseFiles, String^ imgFile );
	};
}
