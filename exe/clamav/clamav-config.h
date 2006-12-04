/* Define if your snprintf is busted */

/* "build clamd" */
/* #define BUILD_CLAMD 1 */

/* name of the clamav user */
#define CLAMAVUSER "Win32User"

/* enable experimental code */
/* #define CL_EXPERIMENTAL 1 */

/* thread safe */
#define CL_THREAD_SAFE 1

/* where to look for the config file */
#define CONFDIR "."

/* Path to virus database directory. */
#define DATADIR "."

/* Name of the main database */
#define DB1NAME "main.cvd"

/* Name of the daily database */
#define DB2NAME "daily.cvd"

/* "default FD_SETSIZE value" */
#define DEFAULT_FD_SETSIZE 64

/* file i/o buffer size */
#define FILEBUFF 8192

/* have bzip2 */
#define HAVE_BZLIB_H 1

/* have gmp installed */
#define HAVE_GMP 1

/* Define to 1 if you have the <inttypes.h> header file. */
#define HAVE_INTTYPES_H 1

/* in_addr_t is defined */
#define HAVE_IN_ADDR_T 1

/* in_port_t is defined */
#define HAVE_IN_PORT_T 1

/* Define to 1 if you have the <limits.h> header file. */
#define HAVE_LIMITS_H 1

/* Define to 1 if you have the <malloc.h> header file. */
#define HAVE_MALLOC_H 1

/* Define to 1 if you have the `memcpy' function. */
#define HAVE_MEMCPY 1

/* Define to 1 if you have the <memory.h> header file. */
#define HAVE_MEMORY_H 1

/* Define to 1 if you have a working `mmap' system call. */
#define HAVE_MMAP 1

/* Define to 1 if you have the <sys/mman.h> header file. */
#define HAVE_SYS_MMAN_H 1

/* "pragma pack" */
#define HAVE_PRAGMA_PACK 1

/* Define to 1 if you have the <regex.h> header file. */
#define HAVE_REGEX_H 1

/* Define to 1 if you have the `snprintf' function. */
/* #undef HAVE_SNPRINTF */

/* Define to 1 if you have the <stdlib.h> header file. */
#define HAVE_STDLIB_H 1

/* Define to 1 if you have the <string.h> header file. */
#define HAVE_STRING_H 1

/* Define to 1 if you have the <sys/stat.h> header file. */
#define HAVE_SYS_STAT_H 1

/* Define to 1 if you have the <sys/types.h> header file. */
#define HAVE_SYS_TYPES_H 1

/* Define to 1 if you have the <unistd.h> header file. */
#define HAVE_UNISTD_H 1

/* zlib installed */
#define HAVE_ZLIB_H 1

/* "no fd_set" */
#define NO_FD_SET 1

/* Name of package */
#define PACKAGE "ClamWin"

/* scan buffer size */
#define SCANBUFF 131072

/* The number of bytes in type int */
#define SIZEOF_INT 4

/* The number of bytes in type long */
#define SIZEOF_LONG 4

/* The number of bytes in type long long */
#define SIZEOF_LONG_LONG 8

/* The number of bytes in type short */
#define SIZEOF_SHORT 2

/* Define to 1 if you have the ANSI C header files. */
#define STDC_HEADERS 1

/* Version number of package */
#define VERSION "devel-msvc - "__DATE__

/* use libcurl in mbox code */
/* #define WITH_CURL 1 */
/* #define CURL_STATICLIB */

/* endianess */
#define WORDS_BIGENDIAN 0

/* thread safe */
/* #undef _REENTRANT */

/* Define to `__inline__' or `__inline' if that's what the C compiler
   calls it, or to nothing if 'inline' is not supported under any name.  */
#define inline _inline

#include <osdeps.h>
