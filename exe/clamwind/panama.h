/* panama.h */

/**************************************************************************+
*
*  PANAMA high-performance reference C-code, based on the description in
*  the paper 'Fast Hashing and Stream Encryption with PANAMA', presented
*  at the Fast Software Encryption Workshop, Paris, 1998, see "Fast
*  Software Encryption - 5th International Workshop, FSE'98", edited by
*  Serge Vaudenay, LNCS-1372, Springer-Verlag, 1998, pp 60-74, also
*  available on-line at http://standard.pictel.com/ftp/research/security
*
*  Algorithm design by Joan Daemen and Craig Clapp
*
*  panama.h  -  Header file for Panama C-code implementation.
*
*
*  History:
*
*  29-Oct-98  Craig Clapp  Implemention for Dr. Dobbs, Dec. 1998 issue,
*                          based on earlier performance-benchmark code.
*
*
*  Notes:  This code is supplied for the purposes of evaluating the
*          performance of the Panama stream/hash module and as a
*          reference implementation for generating test vectors for
*          compatibility / interoperability verification.
*
*
+**************************************************************************/

#ifndef __PANAMA_H_
#define __PANAMA_H_

#ifndef NULL
#define NULL 0
#endif

#define WORDLENGTH   32
#define ONES         0xffffffffL

typedef  unsigned long  ULONG;


/****** platform specific definitions ******/

#include <limits.h>

#if (UINT_MAX == ONES)
    typedef unsigned int   UINT32;
#elif (ULONG_MAX == ONES)
    typedef unsigned long  UINT32;
#elif (ULONG_MAX > ONES)
    typedef unsigned long  UINT32;
#define CROP(a)  ((a) & ONES)
#else
#error "Unable to identify 32-bit compatible data type for target CPU"
#endif


#if defined(__TCS__)  /* TriMedia compiler driver tmcc defines __TCS__ */

#include "custom_defs.h"  /* TriMedia TM1 custom operators */
#define ROTL32(a,shift) ROLI(shift, a)  /* built-in TriMedia macro */

#elif defined(_MSC_VER)  /* Microsoft compiler defines _MSC_VER */

#include <stdlib.h>   /* Microsoft library includes rotate instructions */
#define restrict /* 'restrict' keyword is not part of Microsoft C++, null it out */
#define ROTL32(a,shift) _lrotl(a,shift)  /* built-in Microsoft library function */

#else

/* standard C idioms for Microsoft and TriMedia compiler features */
#define restrict /* 'restrict' keyword is not part of ANSI C, null it out */
#define ROTL32(a,shift)  (((a) << (shift)) | ((a) >> (WORDLENGTH - (shift))))

#endif


/* allow for the possibility that UINT32 data type is bigger than 32 bits */
#ifdef CROP
#undef ROTL32
#define ROTL32(a,shift)  (CROP(((a) << (shift)) | ((a) >> (WORDLENGTH - (shift)))))
#endif


/****** structure definitions ******/

#define PAN_STAGE_SIZE   8
#define PAN_STAGES       32
#define PAN_STATE_SIZE   17


typedef struct {UINT32    word[PAN_STAGE_SIZE];} PAN_STAGE;

typedef struct {PAN_STAGE stage[PAN_STAGES];
                int       tap_0;               } PAN_BUFFER;

typedef struct {UINT32    word[PAN_STATE_SIZE];} PAN_STATE;


/****** function prototypes ******/

void pan_pull(UINT32     *restrict In,      /* input array                    */
              UINT32     *restrict Out,     /* output array                   */
              long        pan_blocks,       /* number of blocks to be Pulled  */
              PAN_BUFFER *restrict buffer,  /* LFSR buffer                    */
              PAN_STATE  *restrict state);  /* 17-word finite-state machine   */

void pan_push(UINT32     *restrict In,      /* input array                    */
              long        pan_blocks,       /* number of blocks to be Pushed  */
              PAN_BUFFER *restrict buffer,  /* LFSR buffer                    */
              PAN_STATE  *restrict state);  /* 17-word finite-state machine   */

void pan_reset(PAN_BUFFER *buffer, PAN_STATE *state);

void pan_hash(UINT32     *sourcetext,  /* input array                         */
              UINT32     *hashcode,    /* 256-bit hash result                 */
              long        bitlength);  /* length to be hashed, in bits        */

void pan_crypt(UINT32    *source_buf,  /* input array                         */
               UINT32    *dest_buf,    /* output array                        */
               PAN_STAGE *key,         /* 256-bit (max) key                   */
               PAN_STAGE *init_vec,    /* 256-bit (max) initialization vector */
               long       bitlength);  /* length to be encrypted, in bits     */

#endif // __PANAMA_H_
