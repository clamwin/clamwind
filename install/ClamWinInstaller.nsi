;-----------------------------------------------------------------------------
; Name:        ClamWinInstaller.nsi
; Product:     ClamWin Free Antivirus Installer
;
; Author(s):      budtse [budtse at users dot sourceforge dot net]
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
; 28/06/2006 budtse : First draft
;
; NOTE: this .NSI script is designed for NSIS v2.17+, using HM NIS Edit, and
;       based on the NSIS sample scripts
; -----------------------------------------------------------------------------

;--------------------------------
;Standard NSIS libraries

  !include 'MUI.nsh'
  !include 'LogicLib.nsh'
  !include 'Sections.nsh'

;--------------------------------
;Other NSIS libraries

  !include 'WinVer.nsh'
  !include 'DotNET.nsh'
  !include 'ServiceLib.nsh'
  !include 'driver.nsh'

;--------------------------------
;General

!define PRODUCT_NAME 'ClamWin Free Antivirus'
!define PRODUCT_SHORT 'ClamWin'
!define PRODUCT_VERSION '0.0.1'
!define PRODUCT_PUBLISHER 'Alch'
!define PRODUCT_WEB_SITE 'http://www.clamwin.com'
!define DOTNET_VERSION '2.0.50727'
!define WEBSITE 'http://www.clamwin.com'
!define WEBSITE_UPDATE 'http://www.clamwin.com/index.php?option=content&task=view&id=40&Itemid=60'
!define PROJECT 'The ClamWin Project'
  ;Name and file
  Name 'ClamWin Free Antivirus'
  OutFile '${PRODUCT_SHORT}-${PRODUCT_VERSION}-install.exe'

  ;Default installation folder
  InstallDir '$PROGRAMFILES\${PRODUCT_NAME}'

  ;Get installation folder from registry if available
  InstallDirRegKey HKLM 'Software\${PRODUCT_NAME}' 'InstallLocation'

;--------------------------------
;Interface Configuration

  !define MUI_ICON 'TrayIcon.ico'
  !define MUI_UNICON 'TrayIcon.ico'
  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP 'logo1.bmp'
  !define MUI_HEADERIMAGE_BITMAP_NOSTRETCH
  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  Var STARTMENU_FOLDER

  !insertmacro MUI_PAGE_LICENSE '..\clamwindsvc_license.txt'
  !insertmacro MUI_PAGE_LICENSE '..\CWMonitor_License.txt'
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_STARTMENU 'PRODUCT_NAME' $STARTMENU_FOLDER
  !insertmacro MUI_PAGE_INSTFILES

  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE 'English'

;--------------------------------
;Installer Sections

  var WINDOWSVERSION
  var PRE_XP

Section 'ClamWin' secClamWin
  SectionIn RO ; Mark read-only, so this can't be de-selected

  ; Get Windows Version
  StrCpy $PRE_XP 'false'
  Call GetWindowsVersion
  Pop $R0
  strCpy $WINDOWSVERSION $R0
  ; WINDOWSVERSION can be : 95, 98, ME, NT x.x, 2000, XP, 2003)
  ${If} $WINDOWSVERSION = '95'
    StrCpy $PRE_XP 'true'
  ${ElseIf} $WINDOWSVERSION = '98'
    StrCpy $PRE_XP 'true'
  ${ElseIf} $WINDOWSVERSION = 'ME'
    StrCpy $PRE_XP 'true'
  ${ElseIf} $WINDOWSVERSION = 'NT 4.0'
    StrCpy $PRE_XP 'true'
  ${Else}
    StrCpy $PRE_XP 'false'
  ${EndIf}

  ; Check if DOTNET 2.0 is installed, and offer to download it if not
  !insertmacro CheckDotNET

  ; Detect if the service is installed
  !insertmacro SERVICE 'installed' 'ClamWind' ''
  Pop $0
  ${If} $0 == 'true'
    DetailPrint 'ClamWind service detected !'
    Call StopService
  ${EndIf}

  ${If} $PRE_XP = 'true'
    ; on xp and greater VC80 CRT needs to be installed in Microsoft.VC80.CRT
    SetOutPath '$INSTDIR\bin\Microsoft.VC80.CRT'
    File ..\exe\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest  ; DestDir: {app}\bin\Microsoft.VC80.CRT; Components: ClamAV; Check: IsXPOrLater
    File ..\exe\Microsoft.VC80.CRT\msvcm80.dll  ; DestDir: {app}\bin\Microsoft.VC80.CRT; Components: ClamAV; Flags: restartreplace uninsrestartdelete; Check: IsXPOrLater
    File ..\exe\Microsoft.VC80.CRT\msvcr80.dll  ; DestDir: {app}\bin\Microsoft.VC80.CRT; Components: ClamAV; Flags: restartreplace uninsrestartdelete; Check: IsXPOrLater
    File ..\exe\Microsoft.VC80.CRT\msvcp80.dll  ; DestDir: {app}\bin\Microsoft.VC80.CRT; Components: ClamAV; Flags: restartreplace uninsrestartdelete; Check: IsXPOrLater
  ${Else}
    ; on 2000 and 98 VC80 CRT needs to be installed in the same dir; no manifest
    SetOutPath '$INSTDIR\bin'
    File ..\exe\Microsoft.VC80.CRT\msvcm80.dll  ; DestDir: {app}\bin; Components: ClamAV; Flags: restartreplace uninsrestartdelete; Check: NotIsXPOrLater
    File ..\exe\Microsoft.VC80.CRT\msvcr80.dll  ; DestDir: {app}\bin; Components: ClamAV; Flags: restartreplace uninsrestartdelete; Check: NotIsXPOrLater
    File ..\exe\Microsoft.VC80.CRT\msvcp80.dll  ; DestDir: {app}\bin; Components: ClamAV; Flags: restartreplace uninsrestartdelete; Check: NotIsXPOrLater
  ${EndIf}

  ; ClamWind service
  SetOutPath '$INSTDIR\exe'
  File ..\exe\clamwind.exe
  File ..\exe\libclamav.dll
  File ..\exe\main.cvd
  File ..\exe\daily.cvd

  ; Register the service
  Call RegisterStartService

  ; ClamWind GUI
  SetOutPath '$INSTDIR\gui'
  File ..\gui\TaskScheduler.xml
  File ..\gui\System.Data.SQLite.xml
  File ..\gui\System.Data.SQLite.DLL.x64
  File ..\gui\System.Data.SQLite.DLL.x32
  File ..\gui\ClamWinApp.exe.config
  File ..\gui\TaskScheduler.dll
  File ..\gui\System.Data.SQLite.DLL
  File ..\gui\PanelsEx.dll
  File ..\gui\ICSharpCode.SharpZipLib.dll
  File ..\gui\ClamWinApp.exe
  File 'TrayIcon.ico'

  SetOutPath '$INSTDIR\gui\Data'
  File ..\gui\Data\clamwin.db

  SetOutPath '$INSTDIR\gui\ru'
  File ..\gui\ru\ClamWinApp.resources.dll

  SetOutPath '$INSTDIR\gui\en'
  File ..\gui\en\ClamWinApp.resources.dll

  ; On-Access Scanning Driver
  SetOutPath '$INSTDIR\sys'
  File ..\sys\cwfilmonxp.inf
  SetOutPath '$INSTDIR\sys\bin\wxp\fre\i386'
  File ..\sys\bin\wxp\fre\i386\cwfilmon.sys

  ; Install the ClamWin Free Antivirus Monitor
  Detailprint 'Installing the ClamWin Free Antivirus Monitor...'
  Call RegisterDriver

  ; Create the quarantine folder
  CreateDirectory '$INSTDIR\gui\Quarantine'

  ; Set Start Menu icons
  SetShellVarContext "all"
  SetOutPath "$SMPROGRAMS\${PRODUCT_NAME}"

  CreateShortCut '$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk' \
                 '$INSTDIR\gui\ClamWinApp.exe'

  ;Store installation folder to the registry
  WriteRegStr HKLM 'Software\${PRODUCT_NAME}' 'InstallLocation' $INSTDIR
  WriteRegStr HKLM 'Software\${PRODUCT_NAME}' 'Version' ${PRODUCT_VERSION}

  ; write out uninstaller
  WriteUninstaller '$INSTDIR\${PRODUCT_SHORT}-uninst.exe'

  ; Show the uninstaller in Add/Remove Programs
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "DisplayName" "${PRODUCT_NAME} ${PRODUCT_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "UninstallString" "$INSTDIR\${PRODUCT_SHORT}-uninst.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "NoModify" "1"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "NoRepair" "1"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "URLInfoAbout" "${WEBSITE}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "URLUpdateInfo" "${WEBSITE_UPDATE}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "Publisher" "${PROJECT}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "DisplayIcon" "$INSTDIR\gui\TrayIcon.ico"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}" \
                 "InstallLocation" "$INSTDIR"



SectionEnd ; end of default section

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_secClamWin ${LANG_ENGLISH} 'The ClamWin Free Antivirus program files.'
;  LangString DESC_secClamAV ${LANG_ENGLISH} 'The Great Clam AV Virus Scanning Engine.'
;  LangString DESC_secOutlookAddin ${LANG_ENGLISH} 'An addin for MS Outlook that automatically scans incoming mail for viruses'
;  LangString DESC_secExplorerShell ${LANG_ENGLISH} 'An extension to Windows Explorer which allows you to scan files by right-clicking on them'
;  LangString DESC_secEnglishManual ${LANG_ENGLISH} 'The original ClamWin Free Antivirus Manual'
;  LangString DESC_secRussianManual ${LANG_ENGLISH} 'A Russian translation of the ClamWin Free Antivirus Manual'
;  LangString DESC_secFrenchManual ${LANG_ENGLISH} 'A French translation of the ClamWin Free Antivirus Manual'
;  LangString DESC_secItalianManual ${LANG_ENGLISH} 'An Italian translation of the ClamWin Free Antivirus Manual'
;
  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${secClamWin} $(DESC_secClamWin)
;    !insertmacro MUI_DESCRIPTION_TEXT ${secClamAV} $(DESC_secClamAV)
;    !insertmacro MUI_DESCRIPTION_TEXT ${secOutlookAddin} $(DESC_secOutlookAddin)
;    !insertmacro MUI_DESCRIPTION_TEXT ${secExplorerShell} $(DESC_secExplorerShell)
;    !insertmacro MUI_DESCRIPTION_TEXT ${secEnglishManual} $(DESC_secEnglishManual)
;    !insertmacro MUI_DESCRIPTION_TEXT ${secRussianManual} $(DESC_secRussianManual)
;    !insertmacro MUI_DESCRIPTION_TEXT ${secFrenchManual} $(DESC_secFrenchManual)
;    !insertmacro MUI_DESCRIPTION_TEXT ${secItalianManual} $(DESC_secItalianManual)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

Section 'Uninstall'

  ; Stop and Deregister the service
  DetailPrint 'Deregistering clamwind.exe service...'
  Call un.DeRegisterService

  ; De-install the ClamWin Free Antivirus Monitor
  DetailPrint 'De-installing the ClamWin Free Antivirus Monitor...'
  Call un.DeRegisterDriver

  ; On-Access Scanning Driver
  Delete '$INSTDIR\sys\bin\wxp\fre\i386\cwfilmon.sys'
  Delete '$INSTDIR\sys\cwfilmonxp.inf'
  RMDir '$INSTDIR\sys\bin\wxp\fre\i386'
  RMDir '$INSTDIR\sys\bin\wxp\fre'
  RMDir '$INSTDIR\sys\bin\wxp'
  RMDir '$INSTDIR\sys\bin'
  RMDir '$INSTDIR\sys'

  ; Localisation files
  Delete '$INSTDIR\gui\en\ClamWinApp.resources.dll'
  RMDir '$INSTDIR\gui\en'
  Delete '$INSTDIR\gui\ru\ClamWinApp.resources.dll'
  RMDir '$INSTDIR\gui\ru'

  ; Database files
  Delete '$INSTDIR\gui\Data\clamwin.db'
  RMDir '$INSTDIR\gui\Data'

  ; Quarantine (if empty)
  RMDir '$INSTDIR\gui\Quarantine'

  ; ClamWind GUI
  Delete '$INSTDIR\gui\TaskScheduler.xml'
  Delete '$INSTDIR\gui\System.Data.SQLite.xml'
  Delete '$INSTDIR\gui\System.Data.SQLite.DLL.x64'
  Delete '$INSTDIR\gui\System.Data.SQLite.DLL.x32'
  Delete '$INSTDIR\gui\ClamWinApp.exe.config'
  Delete '$INSTDIR\gui\TaskScheduler.dll'
  Delete '$INSTDIR\gui\System.Data.SQLite.DLL'
  Delete '$INSTDIR\gui\PanelsEx.dll'
  Delete '$INSTDIR\gui\ICSharpCode.SharpZipLib.dll'
  Delete '$INSTDIR\gui\ClamWinApp.exe'
  RMDir '$INSTDIR\gui'

  ; VC80 CRT
  Delete '$INSTDIR\bin\Microsoft.VC80.CRT\Microsoft.VC80.CRT.manifest'
  Delete '$INSTDIR\bin\Microsoft.VC80.CRT\msvcm80.dll'
  Delete '$INSTDIR\bin\Microsoft.VC80.CRT\msvcr80.dll'
  Delete '$INSTDIR\bin\Microsoft.VC80.CRT\msvcp80.dll'
  RMDir '$INSTDIR\bin\Microsoft.VC80.CRT'

  Delete '$INSTDIR\bin\msvcm80.dll'
  Delete '$INSTDIR\bin\msvcr80.dll'
  Delete '$INSTDIR\bin\msvcp80.dll'
  RMDir '$INSTDIR\bin'

  ; ClamWind service
  Delete '$INSTDIR\exe\clamwind.exe'
  Delete '$INSTDIR\exe\libclamav.dll'
  Delete '$INSTDIR\exe\main.cvd'
  Delete '$INSTDIR\exe\daily.cvd'
  ; Also delete the logfile
  Delete '$INSTDIR\exe\clamwind.log'
  RMDir '$INSTDIR\exe'

  ; Uninstaller
  Delete '$INSTDIR\${PRODUCT_SHORT}-${PRODUCT_VERSION}-uninst.exe'

  ; Main directory
  RMDir '$INSTDIR'

  ; Delete the shortcuts from the Program menu
  SetShellVarContext "all"
  Delete '$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk'
  RMDir '$SMPROGRAMS\${PRODUCT_NAME}'

  ; Clean up the registry settings
  DeleteRegKey HKLM 'Software\${PRODUCT_NAME}'

  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_SHORT}"

SectionEnd

;--------------------------------
; Functions
Function .onInit

  ; Create a mutex to indicate the installer is running.  If an other
  ;  instance of the installer is already running, the creation of the
  ;  mutex will fail.
  System::Call 'kernel32::CreateMutexA(i 0, i 0, t "${PRODUCT_NAME}") i .r0 ?e'
  Pop $0                ; grab return value from CreateMutex
  StrCmp $0 0 launch    ; jump to 'launch' if mutex creation was successful
  ; If another instance was running, abort
      ; TODO : Find the other window and bring it to foreground
  loop:
    FindWindow $1 '#32770' '' 0 $1      ; get window name and store into $1
    ${If} $1 = 0
      goto launch
    ${Else}
      System::Call 'user32::GetWindowText(i r1, t .r2, i 30) i.'
      ${If} $2 = "ClamWin Free Antivirus Setup"
        System::Call 'user32::SetForegroundWindow(i r1) i.'   ; yes? bring it to the front  Abort
        Abort
      ${EndIf}
    ${EndIf}
    goto loop
  Abort

launch:
  ; TODO : If a previous version is installed, should we de-install or just install over it ?
  ReadRegStr $0 HKLM 'Software\${PRODUCT_NAME}' 'InstallLocation'
  ReadRegStr $1 HKLM 'Software\${PRODUCT_NAME}' 'Version'
  DetailPrint 'Previous version $1 of ${PRODUCT_NAME} found at "$0" !'


FunctionEnd


!macro STOP_SERVICE un

  !insertmacro SERVICE 'status' 'ClamWind' ''
  pop $0
  StrCmp $0 'running' lbl_StopService 0
  StrCmp $0 'start_pending' lbl_StopService 0
  StrCmp $0 'stop_pending' lbl_WaitStopService 0
  StrCmp $0 'stopped' lbl_ServiceStopped 0
  StrCmp $0 'continue_pending' lbl_StopService 0
  StrCmp $0 'pause_pending' lbl_StopService 0
  StrCmp $0 'unknown' lbl_StopService 0
  ; If not one of these, an error occurred, so just go on
  goto lbl_ServiceStopped

lbl_StopService:
  DetailPrint 'Stopping service ClamWind...'
  !insertmacro SERVICE 'stop' 'ClamWind' ''

lbl_WaitStopService:
  sleep 100
  !insertmacro SERVICE 'status' 'ClamWind' ''
  pop $0
  StrCmp $0 'stopped' 0 lbl_WaitStopService
lbl_ServiceStopped:
  sleep 500
!macroend

Function StopService
  !insertmacro STOP_SERVICE ''
FunctionEnd

Function Un.StopService
  !insertmacro STOP_SERVICE 'un.'
FunctionEnd

Function RegisterStartService
  DetailPrint 'Registering ClamWind Service...'
  ; Call Service function of ServiceLib.nsh
  Push 'create'           ; Action
  Push 'ClamWind'         ; Service Name
  ; Parameters : User = 'NULL', so the System user will be used
  Push 'path="$INSTDIR\exe\clamwind.exe";autostart=1;interact=0;machine=localhost;user=NULL;password=NULL'
  Call Service
  Pop $0 ;response

  ${If} $0 = 'true'
    ; Now try to start the service
    DetailPrint 'Starting ClamWind Service...'
    Push 'start'
    Push 'ClamWind'
    Push '' ; No exra parameters
    Call Service
    Pop $0 ; response
    ${If} $0 != 'true'
      DetailPrint '............................FAILED'
    ${EndIf}
  ${Else}
    DetailPrint '.................................FAILED!'
  ${EndIf}
FunctionEnd

Function un.DeRegisterService
  Call un.StopService
  DetailPrint 'Deregistering ClamWind Service...'
  ; Call Service function of ServiceLib.nsh
  Push 'delete'           ; Action
  Push 'ClamWind'         ; Service Name
  Push ''                 ; No extra parameters
  Call un.Service
  Pop $0 ;response

  ${If} $0 != "true"
    DetailPrint '.................................FAILED!'
  ${EndIf}

FunctionEnd

Function RegisterDriver
  ; the directory of the .inf file
  Push '$INSTDIR\sys'
  ; the filepath of the .inf file (directory + filename)
  Push '$INSTDIR\sys\cwfilmonxp.inf'
  ; the HID (Hardware ID) of your device
  Push 'Clamwin Free Antivirus File Monitor Driver'
  ; Call the Install function
  Call InstallUpgradeDriver
FunctionEnd

Function un.DeRegisterDriver
  ; the filepath of the .inf file (directory + filename)
  Push '$INSTDIR\sys\cwfilmonxp.inf'
  ; the HID (Hardware ID) of your device
  Push 'Clamwin Free Antivirus File Monitor Driver'
  ; Call the UnInstall function
  Call un.UninstallDriver
FunctionEnd
