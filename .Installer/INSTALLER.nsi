;-------------------------------------------------------------------------------
; Includes
!include "MUI2.nsh"
!include "LogicLib.nsh"
!include "WinVer.nsh"
!include "x64.nsh"

Function .onInit
  Exec "$INSTDIR\Uninstall.exe"
FunctionEnd

;-------------------------------------------------------------------------------
; Constants
SetCompressor "bzip2"
!define PRODUCT_NAME "XtendedMenu"
!define PRODUCT_DESCRIPTION "XtendedMenu for Windows"
!define COMPANYNAME "xCONFLiCTiONx"
!define COPYRIGHT "Copyright © 2022 ${COMPANYNAME}"
!define PRODUCT_VERSION 1.4.0.0
!define SETUP_VERSION 1.4.0.0
!define /date MyTIMESTAMP "%Y%m%d"

;-------------------------------------------------------------------------------
; Attributes
Name "XtendedMenu"
OutFile "XtendedMenu_Setup.exe"
InstallDir "$PROGRAMFILES64\XtendedMenu" ; LOCALAPPDATA|APPDATA|$PROGRAMFILES|$PROGRAMFILES32|$PROGRAMFILES64
InstallDirRegKey HKLM "Software\xCONFLiCTiONx\XtendedMenu" ""
RequestExecutionLevel admin ; user|highest|admin

;-------------------------------------------------------------------------------
; Version Info
VIProductVersion "${PRODUCT_VERSION}"
VIAddVersionKey "ProductName" "${PRODUCT_NAME}"
VIAddVersionKey "ProductVersion" "${PRODUCT_VERSION}"
VIAddVersionKey "FileDescription" "${PRODUCT_DESCRIPTION}"
VIAddVersionKey "CompanyName" "${COMPANYNAME}"
VIAddVersionKey "LegalCopyright" "${COPYRIGHT}"
VIAddVersionKey "FileVersion" "${SETUP_VERSION}"

;-------------------------------------------------------------------------------
; Modern UI Appearance
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Header\orange.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\orange.bmp"

;-------------------------------------------------------------------------------
; Installer Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "LICENSE.rtf"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_TEXT "Start XtendedMenu Settings"
!define MUI_FINISHPAGE_RUN_FUNCTION "LaunchSettings"
!insertmacro MUI_PAGE_FINISH

;-------------------------------------------------------------------------------
; Uninstaller Pages
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;-------------------------------------------------------------------------------
; Languages
!insertmacro MUI_LANGUAGE "English"

;-------------------------------------------------------------------------------
; Installer Sections
Section "${PRODUCT_NAME}"
	SetOutPath $INSTDIR
	File "${PRODUCT_NAME}.exe"
	File "${PRODUCT_NAME}.dll"
  File "Deleter.exe"
  File "EasyLogger.dll"
  File "EasyLogger.xml"
  File "SharpShell.dll"
  File "SharpShell.xml"
  File "TAFactory.IconPack.dll"  
  File "LICENSE.rtf"
;write uninstall information to the registry
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayIcon" "$INSTDIR\${PRODUCT_NAME}.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString" "$INSTDIR\Uninstall.exe"  
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "InstallDate" "${MyTIMESTAMP}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "InstallLocation" "$INSTDIR"
  WriteRegDword HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "Language" 0x00000409
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "Publisher" "${COMPANYNAME}"
	WriteUninstaller "$INSTDIR\Uninstall.exe"
  ;create desktop shortcut
  SetShellVarContext all
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME} Settings.lnk" "$INSTDIR\${PRODUCT_NAME}.exe" ""
  SetOutPath "$INSTDIR"
  nsExec::Exec '"$WINDIR\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe" "$INSTDIR\XtendedMenu.dll" "-codebase"'
SectionEnd

Function LaunchSettings
  SetOutPath "$INSTDIR"
  Exec '"$INSTDIR\${PRODUCT_NAME}.exe" "/settings"'
  Sleep 1000
FunctionEnd

;-------------------------------------------------------------------------------
; Uninstaller Sections
Section "Uninstall"
  SetShellVarContext all
  nsExec::Exec '"$WINDIR\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe" "-unregister" "$INSTDIR\XtendedMenu.dll"'
  Delete "$SMPROGRAMS\${PRODUCT_NAME} Settings.lnk"
	DeleteRegKey HKLM "Software\xCONFLiCTiONx\${PRODUCT_NAME}"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  MessageBox MB_YESNO "Would you like to restart explorer.exe? You will need to reboot or manually restart explorer yourself to completely remove the shell extension." IDYES true IDNO false
true:
  FindWindow $R0 "Shell_TrayWnd"
  nsExec::Exec '"taskkill" "/F" "/im" "explorer.exe"'
  nsRestartExplorer::nsRestartExplorer start ignore
  Goto next
false:
next:
  Delete "$INSTDIR\*"
	RMDir "$INSTDIR"
SectionEnd
