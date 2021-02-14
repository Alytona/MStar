#pragma once
#include <io.h>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Lzo
{
	class CStrMarshaller
	{
		void* DataPtr;

	public:
		CStrMarshaller( String^ source )
		{
			DataPtr = (void*)(Marshal::StringToHGlobalAnsi( source ));
		}
		~CStrMarshaller()
		{
			Marshal::FreeHGlobal( (IntPtr)DataPtr );
		}
		const char* c_str()
		{
			return (const char*)DataPtr;
		}
	};

	class FileHandle
	{
	protected:
		int Handle;
		bool Opened;
		CStrMarshaller* FilenameMarshaller;

	public:
		FileHandle( String^ filename )
		{
			Opened = false;
			FilenameMarshaller = new CStrMarshaller( filename );
		}
		~FileHandle()
		{
			delete FilenameMarshaller;
			if (Opened)
				_close( Handle );
		}

		int getHandle()
		{
			return Handle;
		}
		virtual errno_t open() = 0;
	};

	class InputFileHandle : public FileHandle
	{
	public:
		InputFileHandle( String^ filename ) : FileHandle ( filename ) {}
		virtual errno_t open();
	};
	class OutputFileHandle : public FileHandle
	{
	public:
		OutputFileHandle( String^ filename ) : FileHandle ( filename ) {}
		virtual errno_t open();
	};
}