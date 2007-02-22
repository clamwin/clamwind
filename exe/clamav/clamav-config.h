/* clamav-config.h.  Generated by Sherpya  */

/* enable bind8 compatibility */
/* #undef BIND_8_COMPAT */

/* Define if your snprintf is busted */
/* #undef BROKEN_SNPRINTF */

/* "build clamd" */
/* #undef BUILD_CLAMD */

/* name of the clamav user */
#define CLAMAVUSER "ClamWin"

/* enable experimental code */
/* #undef CL_EXPERIMENTAL */

/* thread safe */
#define CL_THREAD_SAFE 1

/* where to look for the config file */
#define CONFDIR "."

/* Path to virus database directory. */
#define DATADIR "."

/* "default FD_SETSIZE value" */
#define DEFAULT_FD_SETSIZE 64

/* file i/o buffer size */
#define FILEBUFF 8192

/* enable workaround for broken DNS servers */
/* #undef FRESHCLAM_DNS_FIX */

/* use "Cache-Control: no-cache" in freshclam */
/* #undef FRESHCLAM_NO_CACHE */

/* access rights in msghdr */
/* #undef HAVE_ACCRIGHTS_IN_MSGHDR */

/* attrib aligned */
/* undef HAVE_ATTRIB_ALIGNED */

/* attrib packed */
/* #define HAVE_ATTRIB_PACKED */

/* have bzip2 */
#define HAVE_BZLIB_H 1

/* ancillary data style fd pass */
/* #undef HAVE_CONTROL_IN_MSGHDR */

/* Define to 1 if you have the `ctime_r' function. */
/* #undef HAVE_CTIME_R */

/* ctime_r takes 2 arguments */
/* #undef HAVE_CTIME_R_2 */

/* ctime_r takes 3 arguments */
/* #undef HAVE_CTIME_R_3 */

/* Define to 1 if you have the <dlfcn.h> header file. */
/* #undef HAVE_DLFCN_H */

/* Define to 1 if fseeko (and presumably ftello) exists and is declared. */
/* #undef HAVE_FSEEKO */

/* gethostbyname_r takes 3 arguments */
/* #undef HAVE_GETHOSTBYNAME_R_3 */

/* gethostbyname_r takes 5 arguments */
/* #undef HAVE_GETHOSTBYNAME_R_5 */

/* gethostbyname_r takes 6 arguments */
/* #undef HAVE_GETHOSTBYNAME_R_6 */

/* Define to 1 if you have the `getpagesize' function. */
/* #undef HAVE_GETPAGESIZE */

/* have gmp installed */
#define HAVE_GMP 1

/* Define to 1 if you have the <iconv.h> header file. */
/* #undef HAVE_ICONV_H */

/* Define to 1 if you have the `inet_ntop' function. */
/* #undef HAVE_INET_NTOP */

/* Define to 1 if you have the `initgroups' function. */
/* #undef HAVE_INITGROUPS */

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

/* Define to 1 if you have the `mkstemp' function. */
#define HAVE_MKSTEMP 1

/* Define to 1 if you have a working `mmap' system call. */
#define HAVE_MMAP 1

/* Support for NodalCore acceleration */
/* #undef HAVE_NCORE */

/* Define to 1 if you have the <ndir.h> header file. */
/* #undef HAVE_NDIR_H */

/* Define to 1 if you have the `poll' function. */
/* #undef HAVE_POLL */

/* Define to 1 if you have the <poll.h> header file. */
/* #undef HAVE_POLL_H */

/* "pragma pack" */
#define HAVE_PRAGMA_PACK 1

/* readdir_r takes 2 arguments */
/* #undef HAVE_READDIR_R_2 */

/* readdir_r takes 3 arguments */
/* #undef HAVE_READDIR_R_3 */

/* Define to 1 if you have the `recvmsg' function. */
/* #undef HAVE_RECVMSG */

/* Define to 1 if you have the <regex.h> header file. */
#define HAVE_REGEX_H 1

/* have resolv.h */
/* #undef HAVE_RESOLV_H */

/* Define to 1 if you have the `sendmsg' function. */
/* #undef HAVE_SENDMSG */

/* Define to 1 if you have the `setgroups' function. */
/* #undef HAVE_SETGROUPS */

/* Define to 1 if you have the `setsid' function. */
/* #undef HAVE_SETSID */

/* Define to 1 if you have the `snprintf' function. */
/* #undef HAVE_SNPRINTF */

/* Define to 1 if you have the <stdbool.h> header file. */
/* #define HAVE_STDBOOL_H */

/* Define to 1 if you have the <stdint.h> header file. */
#define HAVE_STDINT_H 1

/* Define to 1 if you have the <stdlib.h> header file. */
#define HAVE_STDLIB_H 1

/* Define to 1 if you have the `strerror_r' function. */
/* #undef HAVE_STRERROR_R */

/* Define to 1 if you have the <strings.h> header file. */
/* HAVE_STRINGS_H */

/* Define to 1 if you have the <string.h> header file. */
#define HAVE_STRING_H 1

/* Define to 1 if you have the `strlcat' function. */
/* #undef HAVE_STRLCAT */

/* Define to 1 if you have the `strlcpy' function. */
/* #undef HAVE_STRLCPY */

/* Define to 1 if you have the <sys/filio.h> header file. */
/* #undef HAVE_SYS_FILIO_H */

/* Define to 1 if you have the <sys/inttypes.h> header file. */
/* #undef HAVE_SYS_INTTYPES_H */

/* Define to 1 if you have the <sys/int_types.h> header file. */
/* #undef HAVE_SYS_INT_TYPES_H */

/* Define to 1 if you have the <sys/mman.h> header file. */
#define HAVE_SYS_MMAN_H 1

/* Define to 1 if you have the <sys/param.h> header file. */
/* #undef HAVE_SYS_PARAM_H */

/* "have <sys/select.h>" */
/* #undef HAVE_SYS_SELECT_H */

/* Define to 1 if you have the <sys/stat.h> header file. */
#define HAVE_SYS_STAT_H 1

/* Define to 1 if you have the <sys/types.h> header file. */
#define HAVE_SYS_TYPES_H 1

/* Define to 1 if you have the <sys/uio.h> header file. */
/* #undef HAVE_SYS_UIO_H */

/* Define to 1 if you have the <tcpd.h> header file. */
/* #undef HAVE_TCPD_H */

/* Define to 1 if you have the <termios.h> header file. */
/* #undef HAVE_TERMIOS_H */

/* Define to 1 if you have the <unistd.h> header file. */
#define HAVE_UNISTD_H 1

/* Define to 1 if you have the `vsnprintf' function. */
/* #undef HAVE_VSNPRINTF */

/* zlib installed */
#define HAVE_ZLIB_H 1

/* Early Linux doesn't set cmsg fields */
/* #undef INCOMPLETE_CMSG */

/* bzip funtions do not have bz2 prefix */
/* #undef NOBZ2PREFIX */

/* "no fd_set" */
#define NO_FD_SET 1

/* Name of package */
#define PACKAGE "ClamWin"

/* scan buffer size */
#define SCANBUFF 131072

/* location of Sendmail binary */
/* #undef SENDMAIL_BIN */

/* major version of Sendmail */
/* #undef SENDMAIL_VERSION_A */

/* minor version of Sendmail */
/* #undef SENDMAIL_VERSION_B */

/* subversion of Sendmail */
/* #undef SENDMAIL_VERSION_C */

/* Define to 1 if the `setpgrp' function takes no argument. */
#define SETPGRP_VOID 1

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

/* use syslog */
/* #undef USE_SYSLOG */

/* Version number of package */
#define VERSION "devel-msvc - "__DATE__

/* use libcurl in mbox code */
/* #define WITH_CURL 1 */
/* #define CURL_STATICLIB */

/* endianess */
#define WORDS_BIGENDIAN 0

/* Define to 1 to make fseeko visible on some hosts (e.g. glibc 2.2). */
/* #undef _LARGEFILE_SOURCE */

/* POSIX compatibility */
/* #undef _POSIX_PII_SOCKET */

/* thread safe */
/* #undef _REENTRANT */

/* Define to empty if `const' does not conform to ANSI C. */
/* #undef const */

/* Define to `__inline__' or `__inline' if that's what the C compiler
   calls it, or to nothing if 'inline' is not supported under any name.  */

#ifndef __cplusplus
#define inline _inline
#endif

#include <osdeps.h>
