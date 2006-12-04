;-----------------------------------------------------------------------------
; Name:        WinVer.nsh
; Product:     ClamWin Free Antivirus Installer
;
; Author(s):      budtse [budtse at users dot sourceforge dot net]
;
; Based on Yazno's function, http://yazno.tripod.com/powerpimpit/,
;       updated by Joost Verburg, then adapted for ClamWin by budtse
;
; Created:     2006/06/28
; Copyright:   Copyright ClamWin Pty Ltd (c) 2006
; Licence:
;   This program is free software; you can redistribute it and/or modify
;   it under the terms of the GNU General Public License as published by
;   the Free Software Foundation; either version 2 of the License, or
;   (at your option) any later version.
;
;   This program is distributed in the hope that it will be useful,
;   but WITHOUT ANY WARRANTY; without even the implied warranty of
;   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
;   GNU General Public License for more details.
;
;   You should have received a copy of the GNU General Public License
;   along with this program; if not, write to the Free Software
;   Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA
; -----------------------------------------------------------------------------
; History
; 28/06/2006 budtse : Adaptation for ClamWin
;
; NOTE: this .NSI script is designed for NSIS v2.17+, using HM NIS Edit, and
;       based on the NSIS sample scripts
; -----------------------------------------------------------------------------

; GetWindowsVersion
;
; Returns on top of stack
;
; Windows Version (95, 98, ME, NT x.x, 2000, XP, 2003)
; or
; '' (Unknown Windows Version)
;
; Usage:
;   Call GetWindowsVersion
;   Pop $R0
;   ; at this point $R0 is "NT 4.0" or whatnot

!ifndef WINVER
!define WINVER

  !ifndef UN
    !define UN ""
  !endif

  !macro FUNC_WINVER un

    Push $R0
    Push $R1

    ClearErrors

    ReadRegStr $R0 HKLM \
      'SOFTWARE\Microsoft\Windows NT\CurrentVersion' CurrentVersion

    IfErrors 0 lbl_winnt

    ; we are not NT
    ReadRegStr $R0 HKLM \
      'SOFTWARE\Microsoft\Windows\CurrentVersion' VersionNumber

    StrCpy $R1 $R0 1
    StrCmp $R1 '4' 0 lbl_error

    StrCpy $R1 $R0 3

    StrCmp $R1 '4.0' lbl_win32_95
    StrCmp $R1 '4.9' lbl_win32_ME lbl_win32_98

  lbl_win32_95:
    StrCpy $R0 '95'
    Goto lbl_done

  lbl_win32_98:
    StrCpy $R0 '98'
    Goto lbl_done

  lbl_win32_ME:
    StrCpy $R0 'ME'
    Goto lbl_done

  lbl_winnt:

    StrCpy $R1 $R0 1

    StrCmp $R1 '3' lbl_winnt_x
    StrCmp $R1 '4' lbl_winnt_x

    StrCpy $R1 $R0 3

    StrCmp $R1 '5.0' lbl_winnt_2000
    StrCmp $R1 '5.1' lbl_winnt_XP
    StrCmp $R1 '5.2' lbl_winnt_2003 lbl_error

  lbl_winnt_x:
    StrCpy $R0 "NT $R0" 6
    Goto lbl_done

  lbl_winnt_2000:
    Strcpy $R0 '2000'
    Goto lbl_done

  lbl_winnt_XP:
    Strcpy $R0 'XP'
    Goto lbl_done

  lbl_winnt_2003:
    Strcpy $R0 '2003'
    Goto lbl_done

  lbl_error:
    Strcpy $R0 ''
  lbl_done:

    Pop $R1
    Exch $R0

  !macroend

  Function GetWindowsVersion
    !insertmacro FUNC_WINVER ""
  FunctionEnd

  Function un.GetWindowsVersion
    !insertmacro FUNC_WINVER "un."
  FunctionEnd

!endif ; WINVER_NSH_INCLUDED
