
#ifndef _LIBSPARSE_SPARSE_CRC32_H_
#define _LIBSPARSE_SPARSE_CRC32_H_

#include <stdint.h>

uint32_t sparse_crc32(uint32_t crc, const void* buf, size_t size);

#endif
