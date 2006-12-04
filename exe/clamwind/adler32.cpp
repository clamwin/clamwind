/* adler32.c -- compute the Adler-32 checksum of a data stream
 * Copyright (C) 1995-2004 Mark Adler
 * For conditions of distribution and use, see copyright notice in zlib.h
 */

#include <clamwind.h>
#define BUFSIZE 8192

#define BASE 65521    /* largest prime smaller than 65536 */
#define NMAX 5552
/* NMAX is the largest n such that 255n(n+1)/2 + (n+1)(BASE-1) <= 2^32-1 */

#define DO1(buf,i)  {adler += (buf)[i]; sum2 += adler;}
#define DO2(buf,i)  DO1(buf,i); DO1(buf,i+1);
#define DO4(buf,i)  DO2(buf,i); DO2(buf,i+2);
#define DO8(buf,i)  DO4(buf,i); DO4(buf,i+4);
#define DO16(buf)   DO8(buf,0); DO8(buf,8);

#define MOD(a) a %= BASE
#define MOD4(a) a %= BASE

/* ========================================================================= */
uint32_t adler32(uint32_t adler, unsigned char *buf, size_t len)
{
    uint32_t sum2;
    uint32_t n;

    /* split Adler-32 into component sums */
    sum2 = (adler >> 16) & 0xffff;
    adler &= 0xffff;

    /* in case user likes doing a byte at a time, keep it fast */
    if (len == 1)
    {
        adler += buf[0];
        if (adler >= BASE)
            adler -= BASE;
        sum2 += adler;
        if (sum2 >= BASE)
            sum2 -= BASE;
        return (adler | (sum2 << 16));
    }

    /* in case short lengths are provided, keep it somewhat fast */
    if (len < 16)
    {
        while (len--)
        {
            adler += *buf++;
            sum2 += adler;
        }
        if (adler >= BASE)
            adler -= BASE;
        MOD4(sum2);             /* only added so many BASE's */
        return (adler | (sum2 << 16));
    }

    /* do length NMAX blocks -- requires just one modulo operation */
    while (len >= NMAX)
    {
        len -= NMAX;
        n = NMAX / 16;          /* NMAX is divisible by 16 */
        do {
            DO16(buf);          /* 16 sums unrolled */
            buf += 16;
        } while (--n);
        MOD(adler);
        MOD(sum2);
    }

    /* do remaining bytes (less than NMAX, still just one modulo) */
    if (len)                  /* avoid modulos if none remaining */
    {
        while (len >= 16)
        {
            len -= 16;
            DO16(buf);
            buf += 16;
        }
        while (len--)
        {
            adler += *buf++;
            sum2 += adler;
        }
        MOD(adler);
        MOD(sum2);
    }

    /* return recombined sums */
    return (adler | (sum2 << 16));
}


uint32_t compute_checksum(HANDLE hFile)
{
    uint32_t adler = 1;
    DWORD count = 0;
    unsigned char buffer[BUFSIZE];

    SetFilePointer(hFile, 0, NULL, FILE_BEGIN);

    while (ReadFile(hFile, buffer, sizeof(buffer), &count, NULL) && count)
    {
        adler = adler32(adler, buffer, count);
        if (count != BUFSIZE) break;
    }
    SetFilePointer(hFile, 0, NULL, FILE_BEGIN);
    return adler;
}
