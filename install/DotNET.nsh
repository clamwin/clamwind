;-----------------------------------------------------------------------------
; Name:        ClamWinInstaller.nsi
; Product:     ClamWin Free Antivirus Installer
;
; Author(s):   AnarkiNet[AnarkiNet at gmail dot com]
;              budtse [budtse at users dot sourceforge dot net]
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
;        Written by AnarkiNet(AnarkiNet@gmail.com)
; 28/06/2006 budtse : Adaptation for ClamWin
;
; NOTE: this .NSI script is designed for NSIS v2.17+, using HM NIS Edit, and
;       based on the NSIS sample scripts
; -----------------------------------------------------------------------------

; Downloads and runs the Microsoft .NET Framework version 2.0 Redistributable and runs it if the user does not have the correct version.
!macro CheckDotNET
!define DOTNET_URL "http://www.microsoft.com/downloads/info.aspx?na=90&p=&SrcDisplayLang=en&SrcCategoryId=&SrcFamilyId=0856eacb-4362-4b0d-8edd-aab15c5e04f5&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f5%2f6%2f7%2f567758a3-759e-473e-bf8f-52154438565a%2fdotnetfx.exe"
DetailPrint "Checking your .NET Framework version..."
Push $0
Push $1

System::Call "mscoree::GetCORVersion(w .r0, i ${NSIS_MAX_STRLEN}, *i) i .r1"
StrCmp $1 0 +2
	StrCpy $0 "not found"

Pop $1
Exch $0
Pop $0
${If} $0 == "not found"
	DetailPrint ".NET Framework not found, download is required for program to run!"
	Goto InvalidDotNET
${EndIf}

StrLen $1 ${DOTNET_VERSION}
StrCpy $0 $0 $1 1

${If} $0 != ${DOTNET_VERSION}
	DetailPrint ".NET Framework Version found: $0, but does not match required version: ${DOTNET_VERSION}"
	Goto InvalidDotNET
${EndIf}
DetailPrint ".NET Framework Version found: $0, matches required version: ${DOTNET_VERSION}."
Goto ValidDotNET

InvalidDotNET:
	MessageBox MB_YESNO|MB_ICONEXCLAMATION \
	"Your .NET Framework version: $0.$\nRequired Version: ${DOTNET_VERSION} or greater.$\nWould you like the installer to download the latest .NET Framework version from www.microsoft.com?$\nIf your version is newer, press No." \
	IDYES Download IDNO ValidDotNET

Download:
	DetailPrint "Beginning download of latest .NET Framework version."
	NSISDL::download ${DOTNET_URL} "$TEMP\dotnetfx.exe"
	DetailPrint "Completed download."
	Pop $R0
	StrCmp $R0 "success" +3
		MessageBox MB_OK "Download failed: $R0"
		Quit
	DetailPrint "Pausing installation while downloaded .NET Framework installer runs."
	ExecWait '"$TEMP\dotnetfx.exe"'
	DetailPrint "Completed .NET Framework install/update. Cleaning temporary files..."
	Delete "$TEMP\dotnetfx.exe"
	DetailPrint "Completed cleaning temporary files."

ValidDotNET:
	DetailPrint "Proceeding with remainder of installation..."
!macroend
