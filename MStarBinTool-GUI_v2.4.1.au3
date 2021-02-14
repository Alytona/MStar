#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_Icon=bin\App.ico
#AutoIt3Wrapper_Outfile=MStarBinTool-GUI.exe
#AutoIt3Wrapper_Outfile_x64=MStarBinTool-GUI64.exe
#AutoIt3Wrapper_Compression=4
#AutoIt3Wrapper_UseX64=n
#AutoIt3Wrapper_Res_Comment=This tool used for work with MStar SmartTV\Android firmwares
#AutoIt3Wrapper_Res_Description=MStarBinTool-GUI utility
#AutoIt3Wrapper_Res_Fileversion=2.4.1.0
#AutoIt3Wrapper_Res_ProductName=MStarBinTool-GUI
#AutoIt3Wrapper_Res_ProductVersion=2.4.1.0
#AutoIt3Wrapper_Res_CompanyName=4PDA users
#AutoIt3Wrapper_Res_LegalCopyright=free
#AutoIt3Wrapper_Res_LegalTradeMarks=free
#AutoIt3Wrapper_Res_Language=1049
#AutoIt3Wrapper_Res_requestedExecutionLevel=requireAdministrator
#AutoIt3Wrapper_Res_Compatibility=Vista,Windows7,win7,win8,win81,win10
#AutoIt3Wrapper_Res_Field=Coded by|Alexandr-Pessimist
#AutoIt3Wrapper_Res_Field=Build date|%date%
#AutoIt3Wrapper_Res_Field=AutoIt Version|%AutoItVer%
#AutoIt3Wrapper_Add_Constants=n
#AutoIt3Wrapper_AU3Check_Stop_OnWarning=y
#AutoIt3Wrapper_AU3Check_Parameters=-q -d
#Tidy_Parameters=/rel
#AutoIt3Wrapper_Run_Au3Stripper=y
#Au3Stripper_Parameters=/so /rm /rsln
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
#Region Include
#include <AutoItConstants.au3>
#include <ButtonConstants.au3>
#include <ComboConstants.au3>
#include <Constants.au3>
#include <Date.au3>
#include <EditConstants.au3>
#include <File.au3>
#include <FileConstants.au3>
#include <GuiComboBox.au3>
#include <GUIConstantsEx.au3>
#include <GuiListView.au3>
#include <ListViewConstants.au3>
#include <Math.au3>
#include <MsgBoxConstants.au3>
#include <StaticConstants.au3>
#include <String.au3>
#include <StringConstants.au3>
#include <TabConstants.au3>
#include <WindowsConstants.au3>
#EndRegion
#Region Options
Opt('GUICloseOnESC', 1)
Opt('GUIOnEventMode', 1)
Opt('MustDeclareVars', 1)
Opt('TrayIconDebug', 0)
Opt('TrayIconHide', 1)
Opt('TrayMenuMode', 1)
Opt('TrayOnEventMode', 0)
Opt('WinSearchChildren', 1)
#EndRegion
#Region Global
Global $aAppInfo[3] = ['MStarBinTool-GUI', '2.4.1', 'Russian'], $aAppCfg[4] = [@ScriptDir & '\bin\Settings.ini', @ScriptDir & '\bin\lang\', @ScriptDir & '\bin\win32\', '']
Global $aGuiForm[3], $aMenuFile[2], $aMenuOper[7], $aMenuOpt[2], $aMenuHelp[5], $aMainTabs[7], $iStBar, $aLngCtrl[3], $aInfoCtrl[2]
Global $aTb1Ctrl[30], $aTb2Ctrl[28], $aTb3Ctrl[4], $aTb4Ctrl[14], $aTb5Ctrl[5], $aTb6Ctrl[8]
Global Const $aYesNo[2] = ['Yes', 'No'], $aFirmCfg[7] = ['FirmDirPath', 'ExternalName', 'InternalName', 'FactoryInit', 'UseHexPrefix', 'TypeCRC', 'RamAddress']
Global Const $aPartTable[3] = ['Name', 'Size', 'Erase'], $aImgList[9] = ['Name', 'Size', 'Compress', 'Sparse', 'Split', 'EmptySkip', 'Type', 'Path', 'Chunks(o,s;)']
Global Const $aRiskImg[5] = ['rboot', 'sboot', 'MBOOT', 'MBOOTBAK', 'MPOOL'], $aImgType[5] = ['RomBoot', 'SecureBoot', 'Partition', 'SecureInfo', 'NuttxConfig']
Global Const $aXmlTag[15] = ['Firmware', 'Configuration', 'UserPartTable', 'Partition', 'ImageList', 'Image', 'Environments', 'PreEnv', 'PostEnv', 'EmmcPartTable', _
	'Block', 'AppList', 'Count', 'App', 'Package']
Global Const $aMMC[29] = ['mmc', 'slc 0 1', 'rmgpt', 'create', 'erase.p', 'filepartload', 'write.p', 'write.p.cont', 'write.p.continue', 'write.boot', _
	'sparse_write', 'unlzo', 'unlzo.cont', 'unlzo.continue', 'store_secure_info', 'store_nuttx_config', 'factory_init', 'setenv', 'imageSize', 'imageOffset', _
	'dont_overwrite_init', 'open_DDR_size', 'DDR', 'EMMC', 'CEnv_UpgradeCRC_Tmp', 'CEnv_UpgradeCRC_Val', 'printenv', 'cleanallenv', 'saveenv']
Global Const $auOS[22][2] = [['00', 'UNKNOWN'], ['01', 'OpenBSD'], ['02', 'NetBSD'], ['03', 'FreeBSD'], ['04', '4.4BSD'], ['05', 'Linux'], ['06', 'SVR4'], ['07', 'Esix'], _
	['08', 'Solaris'], ['09', 'Irix'], ['10', 'SCO'], ['11', 'Dell'], ['12', 'NCR'], ['13', 'LynxOS'], ['14', 'VxWorks'], ['15', 'pSOS'], ['16', 'QNX'], ['17', 'U-Boot Firmware'], _
	['18', 'RTEMS'], ['19', 'Unity OS'], ['20', 'INTEGRITY'], ['21', 'OSE']]
Global Const $auArch[20][2] = [['00', 'UNKNOWN'], ['01', 'Alpha'], ['02', 'ARM'], ['03', 'Intel x86'], ['04', 'IA64'], ['05', 'MIPS'], ['06', 'MIPS64'], ['07', 'PowerPC'], _
	['08', 'IBM S390'], ['09', 'SuperH'], ['10', 'Sparc'], ['11', 'Sparc64'], ['12', 'M68k'], ['13', 'MicroBlaze'], ['14', 'Nios-II'], ['15', 'Blackfin'], ['16', 'AVR32'], ['17', 'ST200'], _
	['18', 'NDS32'], ['19', 'OpenRISC 1000']]
Global Const $auType[15][2] = [['00', 'UNKNOWN'], ['01', 'Standalone'], ['02', 'Kernel'], ['03', 'RAMDisk'], ['04', 'Multi-File'], ['05', 'Firmware'], ['06', 'Script'], _
	['07', 'Filesystem'], ['08', 'Flat Device Tree Blob'], ['09', 'Kirkwood Boot'], ['10', 'Freescale IMXBoot'], ['11', 'Davinci UBL'], ['12', 'OMAP'], ['13', 'Davinci AIS'], _
	['14', 'Kernel (any load address)']]
Global Const $auCmpr[5][2] = [['00', 'none'], ['01', 'gzip'], ['02', 'bzip2'], ['03', 'lzma'], ['04', 'lzo']]
Global $auCfg[15][4] = [['4', '27051956', 'MAGIC', 'lv_magic'], ['4', '', 'HCRC', 'lv_header_crc'], ['4', '', 'BT', 'lv_build_time'], ['4', '', 'DS', 'lv_data_size'], _
	['4', '', 'LA', 'lv_load_addr'], ['4', '', 'EP', 'lv_entry_point'], ['4', '', 'DCRC', 'lv_data_crc'], ['1', $auOS[5][1], 'OS', 'lv_op_sys'], _
	['1', $auArch[2][1], 'ARCH', 'lv_arch'], ['1', $auType[4][1], 'IT', 'lv_img_type'], ['1', $auCmpr[0][1], 'CT', 'lv_compr_type'], ['32', '', 'IN', 'lv_img_name'], _
	['4', '', 'KS', 'lv_kernel_size'], ['4', '', 'RS', 'lv_ramdisk_size'], ['MStar-linux', 'MStar-linux(recovery)', 'DP', 'lv_dir_path']]
Global $aAdbCfg[2] = ['0', '0.0.0.0']; ConState, LocIP
#EndRegion
#Region StartGui
_MainForm()
#EndRegion

Func _MainForm($MF_W = 550, $MF_H = 340)
	$aAppInfo[2] = IniRead($aAppCfg[0], $aAppInfo[0], 'Language', $aAppInfo[2]); get\set lang
	$aAppCfg[3] = _ReadInpFile($aAppCfg[1] & $aAppInfo[2] & '.xml', 0, 0, 1); read lang file
	$aGuiForm[0] = GUICreate($aAppInfo[0], $MF_W, $MF_H)
	GUISetFont(8.5, 400, 0, 'Segoe UI', $aGuiForm[0], 5)
	GUISetOnEvent($GUI_EVENT_CLOSE, '_GuiCtrl_Events', $aGuiForm[0])
	#Region Menu
	$aMenuFile[0] = GUICtrlCreateMenu(_LngStr('menu_file'))
	$aMenuFile[1] = GUICtrlCreateMenuItem(_LngStr('menu_exit'), $aMenuFile[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuOper[0] = GUICtrlCreateMenu(_LngStr('menu_operations'))
	$aMenuOper[1] = GUICtrlCreateMenuItem(_LngStr('menu_tab_pack_firm'), $aMenuOper[0])
	GUICtrlSetState($aMenuOper[1], $GUI_CHECKED)
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuOper[2] = GUICtrlCreateMenuItem(_LngStr('menu_tab_unpack_firm'), $aMenuOper[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuOper[3] = GUICtrlCreateMenuItem(_LngStr('menu_tab_mboot_keys'), $aMenuOper[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuOper[4] = GUICtrlCreateMenuItem(_LngStr('menu_tab_crypt'), $aMenuOper[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuOper[5] = GUICtrlCreateMenuItem(_LngStr('menu_tab_uboot'), $aMenuOper[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuOper[6] = GUICtrlCreateMenuItem(_LngStr('menu_tab_adb'), $aMenuOper[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuOpt[0] = GUICtrlCreateMenu(_LngStr('menu_options'))
	$aMenuOpt[1] = GUICtrlCreateMenuItem(_LngStr('menu_lang'), $aMenuOpt[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuHelp[0] = GUICtrlCreateMenu(_LngStr('menu_help'))
	$aMenuHelp[1] = GUICtrlCreateMenuItem(_LngStr('menu_manual'), $aMenuHelp[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuHelp[2] = GUICtrlCreateMenuItem(_LngStr('menu_changelog'), $aMenuHelp[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuHelp[3] = GUICtrlCreateMenuItem(_LngStr('menu_forum'), $aMenuHelp[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aMenuHelp[4] = GUICtrlCreateMenuItem(_LngStr('menu_about'), $aMenuHelp[0])
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	#EndRegion
	#Region Tabs
	$aMainTabs[0] = GUICtrlCreateTab(0, 0, $MF_W, $MF_H -37)
	GUICtrlSetOnEvent($aMainTabs[0], '_GuiCtrl_Events')
	#Region Tab1
	$aMainTabs[1] = GUICtrlCreateTabItem(_LngStr('menu_tab_pack_firm'))
	GUICtrlCreateGroup(_LngStr('group_pck_mode'), 5, 25, 170, 55)
	$aTb1Ctrl[0] = GUICtrlCreateCombo('', 15, 45, 150, 23, $CBS_DROPDOWNLIST + $WS_VSCROLL)
	GUICtrlSetData($aTb1Ctrl[0], _LngStr('combo_pck_mode'))
	GUICtrlSendMsg($aTb1Ctrl[0], $CB_SETCURSEL, 1, 0)
	GUICtrlSendMsg($aTb1Ctrl[0], $CB_SETDROPPEDWIDTH, 150, 0)
	GUICtrlSetOnEvent($aTb1Ctrl[0], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	GUICtrlCreateGroup(_LngStr('group_inp_cfg'), 180, 25, 270, 55)
	$aTb1Ctrl[1] = GUICtrlCreateInput(_LngStr('tip_sel_cfg'), 190, 45, 225, 22, -1, -1)
	$aTb1Ctrl[2] = GUICtrlCreateButton('', 420, 44, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb1Ctrl[2], 'shell32.dll', 4, 0)
	GUICtrlSetOnEvent($aTb1Ctrl[2], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	$aTb1Ctrl[3] = GUICtrlCreateButton(_LngStr('btn_pack_firm'), 455, 30, 90, 50, BitOR($BS_FLAT, $BS_MULTILINE))
	GUICtrlSetOnEvent($aTb1Ctrl[3], '_GuiCtrl_Events')
	$aTb1Ctrl[28] = GUICtrlCreateCheckbox(_LngStr('cbox_add_comments'), 5, 85, 150, 16)
	GUICtrlSetOnEvent($aTb1Ctrl[28], '_GuiCtrl_Events')
	$aTb1Ctrl[29] = GUICtrlCreateInput('', 170, 85, 375, 18, -1, -1)
	GUICtrlSetState($aTb1Ctrl[29], $GUI_DISABLE)
	$aTb1Ctrl[4] = GUICtrlCreateRadio(_LngStr('radbtn_config'), 5, 105, 108)
	GUICtrlSetState($aTb1Ctrl[4], $GUI_CHECKED)
	GUICtrlSetOnEvent($aTb1Ctrl[4], '_GuiCtrl_Events')
	$aTb1Ctrl[5] = GUICtrlCreateRadio(_LngStr('radbtn_part_table'), 113, 105, 108)
	GUICtrlSetOnEvent($aTb1Ctrl[5], '_GuiCtrl_Events')
	$aTb1Ctrl[6] = GUICtrlCreateRadio(_LngStr('radbtn_imglist'), 221, 105, 108)
	GUICtrlSetOnEvent($aTb1Ctrl[6], '_GuiCtrl_Events')
	$aTb1Ctrl[7] = GUICtrlCreateRadio(_LngStr('radbtn_env'), 329, 105, 108)
	GUICtrlSetOnEvent($aTb1Ctrl[7], '_GuiCtrl_Events')
	$aTb1Ctrl[8] = GUICtrlCreateRadio(_LngStr('radbtn_script'), 437, 105, 108)
	GUICtrlSetOnEvent($aTb1Ctrl[8], '_GuiCtrl_Events')
	$aTb1Ctrl[9] = GUICtrlCreateLabel(_LngStr('lbl_dir_path'), 5, 140, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb1Ctrl[10] = GUICtrlCreateInput('', 115, 140, 430, 20)
	$aTb1Ctrl[11] = GUICtrlCreateLabel(_LngStr('lbl_ext_name'), 5, 165, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb1Ctrl[12] = GUICtrlCreateInput('', 115, 165, 235, 20)
	$aTb1Ctrl[13] = GUICtrlCreateLabel(_LngStr('lbl_int_name'), 5, 190, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb1Ctrl[14] = GUICtrlCreateInput('', 115, 190, 235, 20)
	$aTb1Ctrl[15] = GUICtrlCreateLabel(_LngStr('lbl_factory_init'), 5, 215, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb1Ctrl[16] = GUICtrlCreateInput('', 115, 215, 235, 20)
	$aTb1Ctrl[17] = GUICtrlCreateLabel(_LngStr('lbl_use_hex_prefix'), 360, 165, 80, 20, $SS_LEFTNOWORDWRAP)
	$aTb1Ctrl[18] = GUICtrlCreateCombo('', 445, 165, 100, 23, $CBS_DROPDOWNLIST)
	GUICtrlSetData($aTb1Ctrl[18], $aYesNo[0] & '|' & $aYesNo[1], $aYesNo[1])
	GUICtrlSendMsg($aTb1Ctrl[18], $CB_SETDROPPEDWIDTH, 100, 0)
	$aTb1Ctrl[19] = GUICtrlCreateLabel(_LngStr('lbl_type_crc'), 360, 190, 80, 20, $SS_LEFTNOWORDWRAP)
	$aTb1Ctrl[20] = GUICtrlCreateCombo('', 445, 190, 100, 23, $CBS_DROPDOWNLIST)
	GUICtrlSetData($aTb1Ctrl[20], '1|2|3', '1')
	GUICtrlSendMsg($aTb1Ctrl[20], $CB_SETDROPPEDWIDTH, 100, 0)
	$aTb1Ctrl[21] = GUICtrlCreateLabel(_LngStr('lbl_ram_addr'), 360, 215, 80, 20, $SS_LEFTNOWORDWRAP)
	$aTb1Ctrl[22] = GUICtrlCreateInput('', 445, 215, 100, 20)
	$aTb1Ctrl[23] = GUICtrlCreateListView(_LngStr('lv_part_table'), 0, 128, 550, 152, $LVS_REPORT, $LVS_EX_FULLROWSELECT)
	GUICtrlSendMsg($aTb1Ctrl[23], $LVM_SETCOLUMNWIDTH, 0, 200)
	GUICtrlSendMsg($aTb1Ctrl[23], $LVM_SETCOLUMNWIDTH, 1, 160)
	GUICtrlSendMsg($aTb1Ctrl[23], $LVM_SETCOLUMNWIDTH, 2, 160)
	GUICtrlSetState($aTb1Ctrl[23], $GUI_HIDE)
	$aTb1Ctrl[24] = GUICtrlCreateListView(_LngStr('lv_imglist'), 0, 128, 550, 152)
	_GUICtrlListView_SetExtendedListViewStyle($aTb1Ctrl[24], BitOR($LVS_EX_FULLROWSELECT, $LVS_EX_CHECKBOXES))
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 0, 120)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 1, 100)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 2, 50)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 3, 50)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 4, 60)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 5, 80)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 6, 100)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 7, 100)
	GUICtrlSendMsg($aTb1Ctrl[24], $LVM_SETCOLUMNWIDTH, 8, 100)
	GUICtrlSetState($aTb1Ctrl[24], $GUI_HIDE)
	$aTb1Ctrl[25] = GUICtrlCreateEdit('', 0, 128, 275, 152, BitOR($GUI_SS_DEFAULT_EDIT, $ES_NOHIDESEL, $ES_MULTILINE), $WS_EX_CLIENTEDGE)
	GUICtrlSendMsg($aTb1Ctrl[25], $EM_LIMITTEXT, -1, 0)
	GUICtrlSetTip($aTb1Ctrl[25], _LngStr('tip_preenv'))
	GUICtrlSetState($aTb1Ctrl[25], $GUI_HIDE)
	$aTb1Ctrl[26] = GUICtrlCreateEdit('', 275, 128, 275, 152, BitOR($GUI_SS_DEFAULT_EDIT, $ES_NOHIDESEL, $ES_MULTILINE), $WS_EX_CLIENTEDGE)
	GUICtrlSendMsg($aTb1Ctrl[26], $EM_LIMITTEXT, -1, 0)
	GUICtrlSetTip($aTb1Ctrl[26], _LngStr('tip_postenv'))
	GUICtrlSetState($aTb1Ctrl[26], $GUI_HIDE)
	$aTb1Ctrl[27] = GUICtrlCreateEdit('', 0, 128, 550, 152, BitOR($GUI_SS_DEFAULT_EDIT, $ES_NOHIDESEL, $ES_READONLY), $WS_EX_CLIENTEDGE)
	GUICtrlSendMsg($aTb1Ctrl[27], $EM_LIMITTEXT, -1, 0)
	GUICtrlSetState($aTb1Ctrl[27], $GUI_HIDE)
	GUICtrlCreateLabel('', 15, 285, 10, 10, $SS_LEFTNOWORDWRAP)
	GUICtrlSetBkColor(-1, 0x660000)
	GUICtrlCreateLabel(_LngStr('lbl_danger'), 30, 285, 65, 15, $SS_LEFTNOWORDWRAP)
	GUICtrlCreateLabel('', 100, 285, 10, 10, $SS_LEFTNOWORDWRAP)
	GUICtrlSetBkColor(-1, 0x000066)
	GUICtrlCreateLabel(_LngStr('lbl_file_not_found'), 115, 285, 100, 15, $SS_LEFTNOWORDWRAP)
	#EndRegion
	#Region Tab2
	$aMainTabs[2] = GUICtrlCreateTabItem(_LngStr('menu_tab_unpack_firm'))
	GUICtrlCreateGroup(_LngStr('group_unp_mode'), 5, 25, 170, 55)
	$aTb2Ctrl[0] = GUICtrlCreateCombo('', 15, 45, 150, 23, $CBS_DROPDOWNLIST + $WS_VSCROLL)
	GUICtrlSetData($aTb2Ctrl[0], _LngStr('combo_unp_mode'))
	GUICtrlSendMsg($aTb2Ctrl[0], $CB_SETCURSEL, 0, 0)
	GUICtrlSendMsg($aTb2Ctrl[0], $CB_SETDROPPEDWIDTH, 150, 0)
	GUICtrlSetOnEvent($aTb2Ctrl[0], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	GUICtrlCreateGroup(_LngStr('group_inp_firm'), 180, 25, 270, 55)
	$aTb2Ctrl[1] = GUICtrlCreateInput(_LngStr('tip_sel_firm'), 190, 45, 225, 22, -1, -1)
	$aTb2Ctrl[2] = GUICtrlCreateButton('', 420, 44, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb2Ctrl[2], 'shell32.dll', 4, 0)
	GUICtrlSetOnEvent($aTb2Ctrl[2], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	$aTb2Ctrl[3] = GUICtrlCreateButton(_LngStr('btn_unpack_firm'), 455, 30, 90, 50, BitOR($BS_FLAT, $BS_MULTILINE))
	GUICtrlSetOnEvent($aTb2Ctrl[3], '_GuiCtrl_Events')
	$aTb2Ctrl[4] = GUICtrlCreateRadio(_LngStr('radbtn_config'), 5, 84, 108)
	GUICtrlSetState($aTb2Ctrl[4], $GUI_CHECKED)
	GUICtrlSetOnEvent($aTb2Ctrl[4], '_GuiCtrl_Events')
	$aTb2Ctrl[5] = GUICtrlCreateRadio(_LngStr('radbtn_part_table'), 113, 84, 108)
	GUICtrlSetOnEvent($aTb2Ctrl[5], '_GuiCtrl_Events')
	$aTb2Ctrl[6] = GUICtrlCreateRadio(_LngStr('radbtn_imglist'), 221, 84, 108)
	GUICtrlSetOnEvent($aTb2Ctrl[6], '_GuiCtrl_Events')
	$aTb2Ctrl[7] = GUICtrlCreateRadio(_LngStr('radbtn_env'), 329, 84, 108)
	GUICtrlSetOnEvent($aTb2Ctrl[7], '_GuiCtrl_Events')
	$aTb2Ctrl[8] = GUICtrlCreateRadio(_LngStr('radbtn_script'), 437, 84, 108)
	GUICtrlSetOnEvent($aTb2Ctrl[8], '_GuiCtrl_Events')
	$aTb2Ctrl[9] = GUICtrlCreateLabel(_LngStr('lbl_dir_path'), 5, 120, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb2Ctrl[10] = GUICtrlCreateInput('', 115, 120, 430, 20)
	$aTb2Ctrl[11] = GUICtrlCreateLabel(_LngStr('lbl_ext_name'), 5, 145, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb2Ctrl[12] = GUICtrlCreateInput('', 115, 145, 235, 20)
	$aTb2Ctrl[13] = GUICtrlCreateLabel(_LngStr('lbl_int_name'), 5, 170, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb2Ctrl[14] = GUICtrlCreateInput('', 115, 170, 235, 20)
	$aTb2Ctrl[15] = GUICtrlCreateLabel(_LngStr('lbl_factory_init'), 5, 195, 110, 20, $SS_LEFTNOWORDWRAP)
	$aTb2Ctrl[16] = GUICtrlCreateInput('', 115, 195, 235, 20)
	$aTb2Ctrl[17] = GUICtrlCreateLabel(_LngStr('lbl_use_hex_prefix'), 360, 145, 80, 20, $SS_LEFTNOWORDWRAP)
	$aTb2Ctrl[18] = GUICtrlCreateCombo('', 445, 145, 100, 23, $CBS_DROPDOWNLIST)
	GUICtrlSetData($aTb2Ctrl[18], $aYesNo[0] & '|' & $aYesNo[1], $aYesNo[1])
	GUICtrlSendMsg($aTb2Ctrl[18], $CB_SETDROPPEDWIDTH, 100, 0)
	$aTb2Ctrl[19] = GUICtrlCreateLabel(_LngStr('lbl_type_crc'), 360, 170, 80, 20, $SS_LEFTNOWORDWRAP)
	$aTb2Ctrl[20] = GUICtrlCreateCombo('', 445, 170, 100, 23, $CBS_DROPDOWNLIST)
	GUICtrlSetData($aTb2Ctrl[20], '1|2|3', '1')
	GUICtrlSendMsg($aTb2Ctrl[20], $CB_SETDROPPEDWIDTH, 100, 0)
	$aTb2Ctrl[21] = GUICtrlCreateLabel(_LngStr('lbl_ram_addr'), 360, 195, 80, 20, $SS_LEFTNOWORDWRAP)
	$aTb2Ctrl[22] = GUICtrlCreateInput('', 445, 195, 100, 20)
	$aTb2Ctrl[23] = GUICtrlCreateListView(_LngStr('lv_part_table'), 0, 107, 550, 196, $LVS_REPORT, $LVS_EX_FULLROWSELECT)
	GUICtrlSendMsg($aTb2Ctrl[23], $LVM_SETCOLUMNWIDTH, 0, 200)
	GUICtrlSendMsg($aTb2Ctrl[23], $LVM_SETCOLUMNWIDTH, 1, 160)
	GUICtrlSendMsg($aTb2Ctrl[23], $LVM_SETCOLUMNWIDTH, 2, 160)
	GUICtrlSetState($aTb2Ctrl[23], $GUI_HIDE)
	$aTb2Ctrl[24] = GUICtrlCreateListView(_LngStr('lv_imglist'), 0, 107, 550, 196)
	_GUICtrlListView_SetExtendedListViewStyle($aTb2Ctrl[24], BitOR($LVS_EX_FULLROWSELECT, $LVS_EX_CHECKBOXES))
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 0, 120)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 1, 100)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 2, 50)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 3, 50)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 4, 60)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 5, 80)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 6, 100)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 7, 100)
	GUICtrlSendMsg($aTb2Ctrl[24], $LVM_SETCOLUMNWIDTH, 8, 100)
	GUICtrlSetState($aTb2Ctrl[24], $GUI_HIDE)
	$aTb2Ctrl[25] = GUICtrlCreateEdit('', 0, 107, 275, 196, BitOR($GUI_SS_DEFAULT_EDIT, $ES_NOHIDESEL, $ES_READONLY), $WS_EX_CLIENTEDGE)
	GUICtrlSendMsg($aTb2Ctrl[25], $EM_LIMITTEXT, -1, 0)
	GUICtrlSetTip($aTb2Ctrl[25], _LngStr('tip_preenv'))
	GUICtrlSetState($aTb2Ctrl[25], $GUI_HIDE)
	$aTb2Ctrl[26] = GUICtrlCreateEdit('', 275, 107, 275, 196, BitOR($GUI_SS_DEFAULT_EDIT, $ES_NOHIDESEL, $ES_READONLY), $WS_EX_CLIENTEDGE)
	GUICtrlSendMsg($aTb2Ctrl[26], $EM_LIMITTEXT, -1, 0)
	GUICtrlSetTip($aTb2Ctrl[26], _LngStr('tip_postenv'))
	GUICtrlSetState($aTb2Ctrl[26], $GUI_HIDE)
	$aTb2Ctrl[27] = GUICtrlCreateEdit('', 0, 107, 550, 196, BitOR($GUI_SS_DEFAULT_EDIT, $ES_NOHIDESEL, $ES_READONLY), $WS_EX_CLIENTEDGE)
	GUICtrlSendMsg($aTb2Ctrl[27], $EM_LIMITTEXT, -1, 0)
	GUICtrlSetState($aTb2Ctrl[27], $GUI_HIDE)
	#EndRegion
	#Region Tab3
	$aMainTabs[3] = GUICtrlCreateTabItem(_LngStr('menu_tab_mboot_keys'))
	GUICtrlCreateGroup(_LngStr('group_inp_mboot'), 5, 25, 445, 55)
	$aTb3Ctrl[0] = GUICtrlCreateInput(_LngStr('tip_sel_mboot'), 15, 45, 400, 22, -1, -1)
	$aTb3Ctrl[1] = GUICtrlCreateButton('', 420, 44, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb3Ctrl[1], 'shell32.dll', 4, 0)
	GUICtrlSetOnEvent($aTb3Ctrl[1], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	$aTb3Ctrl[2] = GUICtrlCreateButton(_LngStr('btn_extract_keys'), 455, 30, 90, 50, BitOR($BS_FLAT, $BS_MULTILINE))
	GUICtrlSetOnEvent($aTb3Ctrl[2], '_GuiCtrl_Events')
	$aTb3Ctrl[3] = GUICtrlCreateListView(_LngStr('lv_config'), 0, 85, 550, 217, $LVS_REPORT, $LVS_EX_FULLROWSELECT)
	GUICtrlSendMsg($aTb3Ctrl[3], $LVM_SETCOLUMNWIDTH, 0, 160)
	GUICtrlSendMsg($aTb3Ctrl[3], $LVM_SETCOLUMNWIDTH, 1, 380)
	#EndRegion
	#Region Tab4
	$aMainTabs[4] = GUICtrlCreateTabItem(_LngStr('menu_tab_crypt'))
	GUICtrlCreateGroup(_LngStr('group_sel_action'), 5, 25, 210, 55)
	$aTb4Ctrl[0] = GUICtrlCreateCombo('', 15, 45, 190, 23, $CBS_DROPDOWNLIST + $WS_VSCROLL)
	GUICtrlSetData($aTb4Ctrl[0], _LngStr('combo_dec_enc_img'))
	GUICtrlSendMsg($aTb4Ctrl[0], $CB_SETCURSEL, 0, 0)
	GUICtrlSendMsg($aTb4Ctrl[0], $CB_SETDROPPEDWIDTH, 190, 0)
	GUICtrlSetOnEvent($aTb4Ctrl[0], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	GUICtrlCreateGroup(_LngStr('group_inp_uboot'), 220, 25, 230, 55)
	$aTb4Ctrl[1] = GUICtrlCreateInput(_LngStr('tip_sel_enc_img'), 230, 45, 185, 22, -1, -1)
	$aTb4Ctrl[2] = GUICtrlCreateButton('', 420, 44, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb4Ctrl[2], 'shell32.dll', 4, 0)
	GUICtrlSetOnEvent($aTb4Ctrl[2], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	$aTb4Ctrl[3] = GUICtrlCreateButton(_LngStr('btn_decrypt_img'), 455, 30, 90, 50, BitOR($BS_FLAT, $BS_MULTILINE))
	GUICtrlSetOnEvent($aTb4Ctrl[3], '_GuiCtrl_Events')
	GUICtrlCreateGroup(_LngStr('group_def_keys'), 5, 85, 540, 105)
	GUICtrlCreateLabel(_LngStr('lbl_aes_key'), 15, 105, 100, 15, $SS_LEFTNOWORDWRAP)
	$aTb4Ctrl[4] = GUICtrlCreateInput(@ScriptDir & '\default_keys\AESboot.bin', 120, 105, 390, 22, -1, -1)
	$aTb4Ctrl[5] = GUICtrlCreateButton('', 515, 104, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb4Ctrl[5], 'shell32.dll', 4, 0)
	GUICtrlSetOnEvent($aTb4Ctrl[5], '_GuiCtrl_Events')
	GUICtrlCreateLabel(_LngStr('lbl_rsa_priv_key'), 15, 132, 100, 15, $SS_LEFTNOWORDWRAP)
	$aTb4Ctrl[6] = GUICtrlCreateInput(@ScriptDir & '\default_keys\RSAboot_priv.txt', 120, 132, 390, 22, -1, -1)
	GUICtrlSetState($aTb4Ctrl[6], $GUI_DISABLE)
	$aTb4Ctrl[7] = GUICtrlCreateButton('', 515, 131, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb4Ctrl[7], 'shell32.dll', 4, 0)
	GUICtrlSetState($aTb4Ctrl[7], $GUI_DISABLE)
	GUICtrlSetOnEvent($aTb4Ctrl[7], '_GuiCtrl_Events')
	GUICtrlCreateLabel(_LngStr('lbl_rsa_publ_key'), 15, 159, 100, 15, $SS_LEFTNOWORDWRAP)
	$aTb4Ctrl[8] = GUICtrlCreateInput(@ScriptDir & '\default_keys\RSAboot_pub.txt', 120, 159, 390, 22, -1, -1)
	GUICtrlSetState($aTb4Ctrl[8], $GUI_DISABLE)
	$aTb4Ctrl[9] = GUICtrlCreateButton('', 515, 158, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb4Ctrl[9], 'shell32.dll', 4, 0)
	GUICtrlSetState($aTb4Ctrl[9], $GUI_DISABLE)
	GUICtrlSetOnEvent($aTb4Ctrl[9], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	GUICtrlCreateGroup(_LngStr('group_cust_keys'), 5, 195, 540, 80)
	GUICtrlCreateLabel(_LngStr('lbl_aes_key'), 15, 215, 100, 15, $SS_LEFTNOWORDWRAP)
	$aTb4Ctrl[10] = GUICtrlCreateInput(@ScriptDir & '\keys\AESboot.bin', 120, 215, 390, 22, -1, -1)
	GUICtrlSetState($aTb4Ctrl[10], $GUI_DISABLE)
	$aTb4Ctrl[11] = GUICtrlCreateButton('', 515, 214, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb4Ctrl[11], 'shell32.dll', 4, 0)
	GUICtrlSetState($aTb4Ctrl[11], $GUI_DISABLE)
	GUICtrlSetOnEvent($aTb4Ctrl[11], '_GuiCtrl_Events')
	GUICtrlCreateLabel(_LngStr('lbl_rsa_publ_key'), 15, 242, 100, 15, $SS_LEFTNOWORDWRAP)
	$aTb4Ctrl[12] = GUICtrlCreateInput(@ScriptDir & '\keys\RSAboot_pub.txt', 120, 242, 390, 22, -1, -1)
	GUICtrlSetState($aTb4Ctrl[12], $GUI_DISABLE)
	$aTb4Ctrl[13] = GUICtrlCreateButton('', 515, 241, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb4Ctrl[13], 'shell32.dll', 4, 0)
	GUICtrlSetState($aTb4Ctrl[13], $GUI_DISABLE)
	GUICtrlSetOnEvent($aTb4Ctrl[13], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	GUICtrlCreateLabel(_LngStr('lbl_rsa_priv_used_always'), 5, 280, 540, 15, $SS_LEFTNOWORDWRAP)
	#EndRegion
	#Region Tab5
	$aMainTabs[5] = GUICtrlCreateTabItem(_LngStr('menu_tab_uboot'))
	GUICtrlCreateGroup(_LngStr('group_sel_action'), 5, 25, 170, 55)
	$aTb5Ctrl[0] = GUICtrlCreateCombo('', 15, 45, 150, 23, $CBS_DROPDOWNLIST + $WS_VSCROLL)
	GUICtrlSetData($aTb5Ctrl[0], _LngStr('combo_pck_unp_uboot'))
	GUICtrlSendMsg($aTb5Ctrl[0], $CB_SETCURSEL, 1, 0)
	GUICtrlSendMsg($aTb5Ctrl[0], $CB_SETDROPPEDWIDTH, 150, 0)
	GUICtrlSetOnEvent($aTb5Ctrl[0], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	GUICtrlCreateGroup(_LngStr('group_sel_uboot_or_cfg'), 180, 25, 270, 55)
	$aTb5Ctrl[1] = GUICtrlCreateInput(_LngStr('tip_sel_uboot_img'), 190, 45, 225, 22, -1, -1)
	$aTb5Ctrl[2] = GUICtrlCreateButton('', 420, 44, 22, 24, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb5Ctrl[2], 'shell32.dll', 4, 0)
	GUICtrlSetOnEvent($aTb5Ctrl[2], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	$aTb5Ctrl[3] = GUICtrlCreateButton(_LngStr('btn_unpack_img'), 455, 30, 90, 50, BitOR($BS_FLAT, $BS_MULTILINE))
	GUICtrlSetOnEvent($aTb5Ctrl[3], '_GuiCtrl_Events')
	$aTb5Ctrl[4] = GUICtrlCreateListView(_LngStr('lv_config'), 0, 85, 550, 217, $LVS_REPORT, $LVS_EX_FULLROWSELECT)
	GUICtrlSendMsg($aTb5Ctrl[4], $LVM_SETCOLUMNWIDTH, 0, 200)
	GUICtrlSendMsg($aTb5Ctrl[4], $LVM_SETCOLUMNWIDTH, 1, 320)
	#EndRegion
	#Region Tab6
	$aMainTabs[6] = GUICtrlCreateTabItem(_LngStr('menu_tab_adb'))
	GUICtrlCreateGroup(_LngStr('group_sel_ip'), 5, 25, 230, 55)
	$aTb6Ctrl[0] = GUICtrlCreateCombo(IniRead($aAppCfg[0], $aAppInfo[0], 'LastIP', ''), 10, 45, 115, 20, $CBS_DROPDOWNLIST + $WS_VSCROLL)
	GUICtrlSendMsg($aTb6Ctrl[0], $CB_SETDROPPEDWIDTH, 110, 0)
	$aTb6Ctrl[1] = GUICtrlCreateButton('', 132, 34, 20, 20, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb6Ctrl[1], 'shell32.dll', 23, 0)
	GUICtrlSetOnEvent($aTb6Ctrl[1], '_GuiCtrl_Events')
	$aTb6Ctrl[2] = GUICtrlCreateButton('', 132, 56, 20, 20, $BS_ICON + $BS_FLAT)
	GUICtrlSetImage($aTb6Ctrl[2], 'shell32.dll', 24, 0)
	GUICtrlSetOnEvent($aTb6Ctrl[2], '_GuiCtrl_Events')
	$aTb6Ctrl[3] = GUICtrlCreateButton(_LngStr('btn_connect'), 155, 34, 75, 20, $BS_FLAT)
	GUICtrlSetOnEvent($aTb6Ctrl[3], '_GuiCtrl_Events')
	$aTb6Ctrl[4] = GUICtrlCreateButton(_LngStr('btn_disconnect'), 155, 56, 75, 20, $BS_FLAT)
	GUICtrlSetOnEvent($aTb6Ctrl[4], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	GUICtrlCreateGroup(_LngStr('group_sel_action'), 240, 25, 305, 55)
	$aTb6Ctrl[5] = GUICtrlCreateCombo('', 250, 45, 210, 23, $CBS_DROPDOWNLIST + $WS_VSCROLL)
	GUICtrlSetData($aTb6Ctrl[5], _LngStr('combo_adb_cmds'))
	GUICtrlSendMsg($aTb6Ctrl[5], $CB_SETCURSEL, 0, 0)
	GUICtrlSendMsg($aTb6Ctrl[5], $CB_SETDROPPEDWIDTH, 200, 0)
	$aTb6Ctrl[6] = GUICtrlCreateButton(_LngStr('btn_send_cmd'), 470, 35, 70, 40, BitOR($BS_FLAT, $BS_MULTILINE))
	GUICtrlSetOnEvent($aTb6Ctrl[6], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	$aTb6Ctrl[7] = GUICtrlCreateListView('-|-|-', 0, 85, 550, 217, $LVS_REPORT, $LVS_EX_FULLROWSELECT)
	GUICtrlSendMsg($aTb6Ctrl[7], $LVM_SETCOLUMNWIDTH, 0, 200)
	GUICtrlSendMsg($aTb6Ctrl[7], $LVM_SETCOLUMNWIDTH, 1, 200)
	GUICtrlSendMsg($aTb6Ctrl[7], $LVM_SETCOLUMNWIDTH, 2, 120)
	#EndRegion
	GUICtrlCreateTabItem('')
	#EndRegion
	#Region StatusBar
	$iStBar = GUICtrlCreateLabel('', 2, $MF_H -37, $MF_W -5, 15, $SS_SUNKEN + $SS_LEFTNOWORDWRAP)
	GUICtrlSetFont($iStBar, 9, 400, 2, 'Calibri', 5)
	#EndRegion
	GUISetState(@SW_SHOW, $aGuiForm[0])
EndFunc

Func _LngForm($LF_LIST, $LF_W = 160, $LF_H = 110)
	GUISetState(@SW_DISABLE, $aGuiForm[0])
	$aGuiForm[1] = GUICreate(_LngStr('title_language'), $LF_W, $LF_H, -1, -1, -1, $WS_EX_APPWINDOW, $aGuiForm[0])
	GUISetFont(8.5, 400, 0, 'Segoe UI', $aGuiForm[1], 5)
	GUICtrlCreateGroup(_LngStr('group_language'), 5, 5, $LF_W -10, 55)
	$aLngCtrl[0] = GUICtrlCreateCombo('', 15, 25, $LF_W -30, 23, $CBS_DROPDOWNLIST + $WS_VSCROLL)
	GUICtrlSetData($aLngCtrl[0], $LF_LIST, $aAppInfo[2])
	GUICtrlSendMsg($aLngCtrl[0], $CB_SETDROPPEDWIDTH, $LF_W -30, 0)
	GUICtrlSetOnEvent($aLngCtrl[0], '_GuiCtrl_Events')
	GUICtrlCreateGroup('', -99, -99, 1, 1)
	$aLngCtrl[1] = GUICtrlCreateButton(_LngStr('btn_save'), $LF_W - 150, $LF_H - 30, 70, 24, $BS_FLAT)
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	$aLngCtrl[2] = GUICtrlCreateButton(_LngStr('btn_cancel'), $LF_W - 75, $LF_H - 30, 70, 24, $BS_FLAT)
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	GUISetOnEvent($GUI_EVENT_CLOSE, '_GuiCtrl_Events', $aGuiForm[1])
	GUISetState(@SW_SHOW, $aGuiForm[1])
	GUISwitch($aGuiForm[1])
EndFunc

Func _InfoForm($IF_TITLE, $IF_TXT, $IF_W = 350, $IF_H = 200)
	GUISetState(@SW_DISABLE, $aGuiForm[0])
	$aGuiForm[2] = GUICreate($IF_TITLE, $IF_W, $IF_H, -1, -1, -1, $WS_EX_APPWINDOW, $aGuiForm[0])
	GUISetFont(8.5, 400, 0, 'Segoe UI', $aGuiForm[2], 5)
	$aInfoCtrl[0] = GUICtrlCreateEdit($IF_TXT, 0, 0, $IF_W, $IF_H -35, $ES_AUTOVSCROLL + $WS_VSCROLL + $ES_NOHIDESEL + $ES_READONLY, $WS_EX_CLIENTEDGE)
	GUICtrlSendMsg($aInfoCtrl[0], $EM_LIMITTEXT, -1, 0)
	$aInfoCtrl[1] = GUICtrlCreateButton(_LngStr('btn_close'), $IF_W - 75, $IF_H - 30, 70, 24, $BS_FLAT)
	GUICtrlSetOnEvent(-1, '_GuiCtrl_Events')
	GUISetOnEvent($GUI_EVENT_CLOSE, '_GuiCtrl_Events', $aGuiForm[2])
	GUISetState(@SW_SHOW, $aGuiForm[2])
	GUISwitch($aGuiForm[2])
EndFunc

Func _LngStr($INP_TAG)
	Local $aLngStrVal = _StringBetween($aAppCfg[3], '<string id="' & $INP_TAG & '">', '</string>')
	If (@error = 0) Then Return $aLngStrVal[0]
EndFunc

Func _GuiCtrl_Events()
	Switch @GUI_CtrlId
		Case $GUI_EVENT_CLOSE, $aMenuFile[1], $aLngCtrl[2], $aInfoCtrl[1]; Close Parent or Child window
			Switch @GUI_WinHandle
				Case $aGuiForm[0]
					GUIDelete($aGuiForm[0])
					Exit
				Case Else
					GUISetState(@SW_ENABLE, $aGuiForm[0])
					GUISwitch($aGuiForm[0])
					GUIDelete(@GUI_WinHandle)
			EndSwitch
		Case $aLngCtrl[1]; Save Language
			Local $SelLng = GUICtrlRead($aLngCtrl[0])
			GUISetState(@SW_ENABLE, $aGuiForm[0])
			GUISwitch($aGuiForm[0])
			GUIDelete($aGuiForm[1])
			If Not ($SelLng = $aAppInfo[2]) Then
				_SaveSettings('Language', $SelLng); save lang
				GUIDelete($aGuiForm[0])
				_MainForm()
			EndIf
		Case $aMenuOper[1], $aMenuOper[2], $aMenuOper[3], $aMenuOper[4], $aMenuOper[5], $aMenuOper[6]; Switch MainTabs
			For $im = 1 To UBound($aMenuOper) -1
				If ($aMenuOper[$im] = @GUI_CtrlId) Then
					GUICtrlSetState($aMenuOper[$im], $GUI_CHECKED)
					GUICtrlSetState($aMainTabs[$im], $GUI_SHOW)
				Else
					GUICtrlSetState($aMenuOper[$im], $GUI_UNCHECKED)
				EndIf
			Next
		Case $aMenuOpt[1]; Show Language dialog
			Local $aLangArray = _FileListToArray($aAppCfg[1], '*.xml', 1), $sLangList = ''
			If IsArray($aLangArray) Then
				For $il = 1 To UBound($aLangArray) -1
					$sLangList &= StringReplace($aLangArray[$il], '.xml', '') & '|'
				Next
				$sLangList = StringTrimRight($sLangList, 1)
			EndIf
			_LngForm($sLangList)
		Case $aMenuHelp[1]; Show Manual
			If FileExists(@ScriptDir & '\bin\MStarBinTool-GUI_Manual.pdf') Then ShellExecute(@ScriptDir & '\bin\MStarBinTool-GUI_Manual.pdf')
		Case $aMenuHelp[2]; Show ChangeLog
			If FileExists(@ScriptDir & '\bin\ChangeLog.txt') Then ShellExecute(@ScriptDir & '\bin\ChangeLog.txt')
		Case $aMenuHelp[3]; Go to 4PDA forum
			ShellExecute('http://4pda.ru/forum/index.php?showtopic=847902&st=30#entry76686400')
		Case $aMenuHelp[4]; Show About
			MsgBox(4096, _LngStr('title_about'), StringFormat('%s v%s by Alexandr-Pessimist.\r\n\r\nGUI Tool based on "mstar-bin-tool" utility by dipcore.', $aAppInfo[0], $aAppInfo[1]), 0, $aGuiForm[0])
		Case $aMainTabs[0]; Switch Menu
			For $im = 1 To UBound($aMenuOper) -1
				If ($im = GUICtrlRead($aMainTabs[0]) +1) Then
					GUICtrlSetState($aMenuOper[$im], $GUI_CHECKED)
				Else
					GUICtrlSetState($aMenuOper[$im], $GUI_UNCHECKED)
				EndIf
			Next
		Case $aTb1Ctrl[0]; Swith Tab1 Combo
			Switch GUICtrlSendMsg($aTb1Ctrl[0], $CB_GETCURSEL, 0, 0)
				Case 0
					_GUICtrlListView_SetItemChecked($aTb1Ctrl[24], -1, True)
				Case Else
					_GUICtrlListView_SetItemChecked($aTb1Ctrl[24], -1, False)
			EndSwitch
		Case $aTb1Ctrl[2]; Select and read Firmware config
			GUICtrlSetData ($aTb1Ctrl[1], FileOpenDialog(_LngStr('dlg_sel_firm_cfg'), @ScriptDir & '\', 'XML file (*.xml)', 1 + 2, '', $aGuiForm[0]))
			If FileExists(GUICtrlRead($aTb1Ctrl[1])) Then
				_ClearTabCtrl(1)
				GUICtrlSetData($iStBar, _LngStr('msg_read_firm_cfg'))
				Local $sFileData = _ReadInpFile(GUICtrlRead($aTb1Ctrl[1]), 0, 0, 1)
				GUICtrlSetData($aTb1Ctrl[10], StringStripWS(_GetXmlStr($sFileData, $aFirmCfg[0]), 3)); DirPath
				GUICtrlSetData($aTb1Ctrl[12], StringStripWS(_GetXmlStr($sFileData, $aFirmCfg[1]), 3)); ExtName
				GUICtrlSetData($aTb1Ctrl[14], StringStripWS(_GetXmlStr($sFileData, $aFirmCfg[2]), 3)); IntName
				GUICtrlSetData($aTb1Ctrl[16], StringStripWS(_GetXmlStr($sFileData, $aFirmCfg[3]), 3)); FactoryInit
				GUICtrlSetData($aTb1Ctrl[22], StringStripWS(_GetXmlStr($sFileData, $aFirmCfg[6]), 3)); RamAddr
				If (StringStripWS(_GetXmlStr($sFileData, $aFirmCfg[4]), 3) = $aYesNo[0]) Then GUICtrlSendMsg($aTb1Ctrl[18], $CB_SETCURSEL, 0, 0); UseHexPrefix
				GUICtrlSendMsg($aTb1Ctrl[20], $CB_SETCURSEL, StringStripWS(_GetXmlStr($sFileData, $aFirmCfg[5]), 3) -1, 0); TypeCRC
				Local $aPTable = _GetXmlStr($sFileData, $aXmlTag[3], 1, 1); load UserPartTable
				If IsArray($aPTable) Then; show UserPartTable
					For $i = 0 To UBound($aPTable) -1
						GUICtrlCreateListViewItem(_GetXmlStr($aPTable[$i], $aPartTable[0], 2) & '|' & _GetXmlStr($aPTable[$i], $aPartTable[1], 2) & '|' & _GetXmlStr($aPTable[$i], $aPartTable[2], 2), $aTb1Ctrl[23])
					Next
				EndIf
				Local $aITable = _GetXmlStr($sFileData, $aXmlTag[5], 1, 1); load ImageList
				If IsArray($aITable) Then
					Local $FOutDir = GUICtrlRead($aTb1Ctrl[10])
					Local $IName, $ISize, $ILzo, $ISparse, $ISplit, $ISkip, $IType, $IPath, $IChunks
					For $i = 0 To (UBound($aITable) -1)
						$IName = _GetXmlStr($aITable[$i], $aImgList[0], 2)
						$IPath = _GetXmlStr($aITable[$i], $aImgList[7], 2)
						Switch FileExists($FOutDir & $IPath)
							Case 0; file not exists
								GUICtrlCreateListViewItem(StringFormat('%s|-|-|-|-|-|-|not_found!|-', $IName), $aTb1Ctrl[24])
								GUICtrlSetBkColor(-1, 0x000066)
								GUICtrlSetColor(-1, 0xFFFFFF)
							Case 1; file exists
								$ISize = FileGetSize($FOutDir & $IPath)
								$ILzo = _GetXmlStr($aITable[$i], $aImgList[2], 2)
								$ISparse = _GetXmlStr($aITable[$i], $aImgList[3], 2)
								$ISplit = _GetXmlStr($aITable[$i], $aImgList[4], 2)
								$ISkip = _GetXmlStr($aITable[$i], $aImgList[5], 2)
								$IType = _GetXmlStr($aITable[$i], $aImgList[6], 2)
								$IChunks = '1'
								If ($ISplit = $aYesNo[0]) Then
									If ($ISize > 157286400) Then $IChunks = Ceiling($ISize/157286400)
								EndIf
								Switch $IName
									Case $aRiskImg[0], $aRiskImg[1], $aRiskImg[2], $aRiskImg[3], $aRiskImg[4]
										GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s|%s', $IName, $ISize, $ILzo, $ISparse, $ISplit, $ISkip, $IType, $IPath, $IChunks), $aTb1Ctrl[24])
										GUICtrlSetBkColor(-1, 0x660000)
										GUICtrlSetColor(-1, 0xFFFFFF)
									Case Else
										GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s|%s', $IName, $ISize, $ILzo, $ISparse, $ISplit, $ISkip, $IType, $IPath, $IChunks), $aTb1Ctrl[24])
								EndSwitch
						EndSwitch
					Next
				EndIf
				Local $sPreEnv = _GetXmlStr($sFileData, $aXmlTag[7]); load PreEnv
				If (StringLen($sPreEnv) > 2) Then GUICtrlSetData($aTb1Ctrl[25], StringStripWS(StringReplace($sPreEnv, @LF, @CRLF), 3), 1)
				Local $sPostEnv = _GetXmlStr($sFileData, $aXmlTag[8]); load PostEnv
				If (StringLen($sPostEnv) > 2) Then GUICtrlSetData($aTb1Ctrl[26], StringStripWS(StringReplace($sPostEnv, @LF, @CRLF), 3), 1)
				If GUICtrlSendMsg($aTb1Ctrl[0], $CB_GETCURSEL, 0, 0) = 0 Then
					_GUICtrlListView_SetItemChecked($aTb1Ctrl[24], -1, True)
				Else
					_GUICtrlListView_SetItemChecked($aTb1Ctrl[24], -1, False)
				EndIf
				GUICtrlSetData($iStBar, _LngStr('msg_done'))
			EndIf
		Case $aTb1Ctrl[3]; Pack BIN Firmware
			If FileExists(GUICtrlRead($aTb1Ctrl[1])) Or (GUICtrlSendMsg($aTb1Ctrl[0], $CB_GETCURSEL, 0, 0) = 2) Then
				GUICtrlSetState($aTb1Ctrl[3], $GUI_DISABLE)
				GUICtrlSetData($iStBar, _LngStr('msg_init_config'))
				Local $Inp0File = GUICtrlRead($aTb1Ctrl[1]), $iComboState = GUICtrlSendMsg($aTb1Ctrl[0], $CB_GETCURSEL, 0, 0), $sComments = GUICtrlRead($aTb1Ctrl[29])
				Local $sFirmDirPath = GUICtrlRead($aTb1Ctrl[10]), $sIntName = GUICtrlRead($aTb1Ctrl[14]), $sFactoryInit = GUICtrlRead($aTb1Ctrl[16])
				Local $sHexPref = _Str2Num(GUICtrlRead($aTb1Ctrl[18])), $sTypeCrc = _Str2Num(GUICtrlRead($aTb1Ctrl[20])), $sRamAddr = _HexPrefCheck(GUICtrlRead($aTb1Ctrl[22]), $sHexPref)
				Local $sPreEnv = StringReplace(GUICtrlRead($aTb1Ctrl[25]), @LF, @CRLF), $sPostEnv = StringReplace(GUICtrlRead($aTb1Ctrl[26]), @LF, @CRLF)
				Local $sWrkDir = $sFirmDirPath & 'tmp\', $sFooter = '0x', $FirmScript = $sWrkDir & 'Script.sh', $FirmBin = $sWrkDir & 'Data.bin', $sExtName = GUICtrlRead($aTb1Ctrl[12])
				If (StringLen($sExtName) < 2) Then $sExtName = 'MstarUpgrade.bin'
				Local $NewFirmware = @ScriptDir & '\work\new-' & $sExtName
				GUICtrlSetData($aTb1Ctrl[27], StringFormat('# MSTAR FIRMWARE. Generated by MStarBinTool-GUI.\r\n# Build TIME: %s\r\n', _Now()), 1)
				If (BitAND(GUICtrlRead($aTb1Ctrl[28]), $GUI_CHECKED) = $GUI_CHECKED) And (StringLen($sComments) > 1) Then GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n', $sComments), 1); Comments line
				If (StringLen($sPreEnv) > 2) Then GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n%s\r\n', $aXmlTag[7], StringStripWS($sPreEnv, 3)), 1); PreEnv
				If ($iComboState < 2) Then; add Format & PartTable & Images
					Local $ImgSize, $ImgOffset, $InpImgPath, $OutImgPath, $OutImgChunkPath, $sForceErase
					DirRemove($sWrkDir, $DIR_REMOVE)
					DirCreate($sWrkDir)
					If ($iComboState = 0) Then; add Format & PartTable
						GUICtrlSetData($iStBar, _LngStr('msg_write_part_table'))
						Sleep(150)
						GUICtrlSetData($aTb1Ctrl[27], StringFormat('%s %s\r\n%s %s\r\n', $aMMC[0], $aMMC[1], $aMMC[0], $aMMC[2]), 1); Format EMMC
						If (StringLen($sFactoryInit) > 1) Then GUICtrlSetData($aTb1Ctrl[27], $sFactoryInit & @CRLF, 1); FactoryInit
						If (_GUICtrlListView_GetItemCount($aTb1Ctrl[23]) > 0) Then; PartTable
							For $i = 0 To (_GUICtrlListView_GetItemCount($aTb1Ctrl[23]) -1)
								Local $aBinParts = _GUICtrlListView_GetItemTextArray($aTb1Ctrl[23], $i)
								GUICtrlSetData($aTb1Ctrl[27], StringFormat('%s %s %s %s\r\n', $aMMC[0], $aMMC[3], $aBinParts[1], $aBinParts[2]), 1); Create parts
								If ($aBinParts[3] = $aYesNo[0]) Then
									If (_GUICtrlListView_FindText($aTb1Ctrl[24], $aBinParts[1]) = -1) Then $sForceErase &= StringFormat('# %s\r\n%s %s %s\r\n', $aBinParts[1], $aMMC[0], $aMMC[4], $aBinParts[1])
								EndIf
							Next
						Else
							DirRemove($sWrkDir, $DIR_REMOVE)
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_part_table_not_found'), 0, $aGuiForm[0])
							Exit
						EndIf
						If (StringLen($sForceErase) > 1) Then GUICtrlSetData($aTb1Ctrl[27], $sForceErase, 1); pre-erase empty parts
					EndIf
					If ($iComboState = 1) Then; add FactoryInit
						If (StringLen($sFactoryInit) > 1) Then GUICtrlSetData($aTb1Ctrl[27], $sFactoryInit & @CRLF, 1)
					EndIf
					For $i = 0 To (_GUICtrlListView_GetItemCount($aTb1Ctrl[24]) -1); add Images
						If _GUICtrlListView_GetItemChecked($aTb1Ctrl[24], $i) Then
							Local $aBinImg = _GUICtrlListView_GetItemTextArray($aTb1Ctrl[24], $i)
							$InpImgPath = $sFirmDirPath & $aBinImg[8]
							$OutImgPath = $sWrkDir & $aBinImg[8]
							$OutImgChunkPath = $sWrkDir & $aBinImg[1] & '1.img'
							Switch $aBinImg[6]; EmptySkip
								Case $aYesNo[0]; yes
									$aBinImg[6] = '1'
								Case $aYesNo[1]; no
									$aBinImg[6] = ''
							EndSwitch
							If Not FileExists($InpImgPath) Then
								DirRemove($sWrkDir, $DIR_REMOVE)
								MsgBox(4112, _LngStr('title_error'), _LngStr('msg_file_not_found') & $aBinImg[8], 0, $aGuiForm[0])
								Exit
							EndIf
							Switch $aBinImg[5]; ImgSplit
								Case $aYesNo[0]; Yes (partition type only)
									Local $ChunkOffset = 0, $ChunkSize = 157286400
									Switch $aBinImg[1]; if not risk, then ERASE part
										Case $aRiskImg[2], $aRiskImg[3], $aRiskImg[4]; risk parts
											GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n', $aBinImg[1]), 1)
										Case Else
											GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n%s %s %s\r\n', $aBinImg[1], $aMMC[0], $aMMC[4], $aBinImg[1]), 1)
									EndSwitch
									If ($aBinImg[4] = $aYesNo[0]) Then ; if ImgSparse
										GUICtrlSetData($iStBar, _LngStr('msg_sparse_img') & $aBinImg[8])
										_ConvSparse($InpImgPath, $OutImgPath, 1)
										GUICtrlSetData($iStBar, _LngStr('msg_split_sparse_img') & $aBinImg[8])
										_ConvSparse($OutImgPath, $sWrkDir & $aBinImg[1] & '_sparsechunk', 2)
										FileDelete($OutImgPath)
										Local $aSparseChunks = _FileListToArray($sWrkDir, $aBinImg[1] & '_sparsechunk.*', 1)
										If (@error = 4) Or Not IsArray($aSparseChunks) Then
											DirRemove($sWrkDir, $DIR_REMOVE)
											MsgBox(4112, _LngStr('title_error'), _LngStr('msg_file_not_found') & $aBinImg[1] & '_sparsechunk.*', 0, $aGuiForm[0])
											Exit
										EndIf
										$aBinImg[9] = $aSparseChunks[0]
									EndIf
									For $i1 = 1 To $aBinImg[9]; NumChunks
										$OutImgPath = $sWrkDir & $aBinImg[8]
										Switch $aYesNo[0]; copy lzo\sparse\normal
											Case $aBinImg[3]; ImgLZO
												GUICtrlSetData($iStBar, _LngStr('msg_lzo_chunk') & StringFormat('%s (%s|%s)', $aBinImg[8], $i1, $aBinImg[9]))
												$OutImgPath &= '.lzo'
												If ($i1 < $aBinImg[9]) Then
													_CopyPart($InpImgPath, $OutImgChunkPath, $ChunkOffset, $ChunkSize)
												Else
													_CopyPart($InpImgPath, $OutImgChunkPath, $ChunkOffset, FileGetSize($InpImgPath) - $ChunkOffset)
												EndIf
												_LzoTool($OutImgChunkPath, $OutImgPath, 1); compress part
												FileDelete($OutImgChunkPath)
											Case $aBinImg[4]; ImgSparse
												GUICtrlSetData($iStBar, _LngStr('msg_save_chunk') & StringFormat('%s (%s|%s)',$aBinImg[8], $i1, $aBinImg[9]))
												$OutImgPath = $sWrkDir & $aBinImg[1] & '_sparsechunk.' & ($i1 -1)
											Case Else; normal
												GUICtrlSetData($iStBar, _LngStr('msg_save_chunk') & StringFormat('%s (%s|%s)',$aBinImg[8], $i1, $aBinImg[9]))
												If ($i1 < $aBinImg[9]) Then
													_CopyPart($InpImgPath, $OutImgPath, $ChunkOffset, $ChunkSize)
												Else
													_CopyPart($InpImgPath, $OutImgPath, $ChunkOffset, FileGetSize($InpImgPath) - $ChunkOffset)
												EndIf
										EndSwitch
										$ImgSize = FileGetSize($OutImgPath)
										$ImgSize = _HexPrefCheck(Hex($ImgSize, 8), $sHexPref)
										$ImgOffset = 16384 + FileGetSize($FirmBin)
										$ImgOffset = _HexPrefCheck(Hex($ImgOffset, 8), $sHexPref)
										Switch $aYesNo[0]; write lzo\sparse\normal
											Case $aBinImg[3]; Lzo
												If ($i1 = 1) Then
													GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('%s %s %s %s %s\r\n%s %s %s %s %s %s', $aMMC[5], $sRamAddr, _
														$sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[11], $sRamAddr, $ImgSize, $aBinImg[1], $aBinImg[6]), 2) & @CRLF, 1)
												Else
													GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('%s %s %s %s %s\r\n%s %s %s %s %s %s', $aMMC[5], $sRamAddr, _
														$sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[12], $sRamAddr, $ImgSize, $aBinImg[1], $aBinImg[6]), 2) & @CRLF, 1)
												EndIf
											Case $aBinImg[4]; Sparse
												GUICtrlSetData($aTb1Ctrl[27], StringFormat('%s %s %s %s %s\r\n%s %s %s %s %s', $aMMC[5], $sRamAddr, _
													$sIntName, $ImgOffset, $ImgSize, $aMMC[10], $aMMC[0], $sRamAddr, $aBinImg[1], $ImgSize) & @CRLF, 1)
											Case Else; normal img
												If ($i1 = 1) Then
													GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('%s %s %s %s %s\r\n%s %s %s %s %s %s', $aMMC[5], $sRamAddr, _
														$sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[6], $sRamAddr, $aBinImg[1], $ImgSize, $aBinImg[6]), 2) & @CRLF, 1)
												Else
													GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('%s %s %s %s %s\r\n%s %s %s %s %s %s', $aMMC[5], $sRamAddr, _
														$sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[7], $sRamAddr, $aBinImg[1], $ImgSize, $aBinImg[6]), 2) & @CRLF, 1)
												EndIf
										EndSwitch
										$ChunkOffset += $ChunkSize
										GUICtrlSetData($iStBar, _LngStr('msg_align_chunk') & StringFormat('%s (%s|%s)', $aBinImg[8], $i1, $aBinImg[9]))
										_AlignFile($OutImgPath)
										GUICtrlSetData($iStBar, _LngStr('msg_append_chunk') & StringFormat('%s (%s|%s)', $aBinImg[8], $i1, $aBinImg[9]))
										_CopyPart($OutImgPath, $FirmBin); Append image to BinPart
										GUICtrlSetData($iStBar, _LngStr('msg_delete_tmp_files'))
										FileDelete($OutImgPath)
										Sleep(100)
									Next	
								Case $aYesNo[1]; No
									Switch $aYesNo[0]; Yes
										Case $aBinImg[3]; ImgLZO
											GUICtrlSetData($iStBar, _LngStr('msg_lzo_img') & $aBinImg[8])
											$OutImgPath &= '.lzo'
											_LzoTool($InpImgPath, $OutImgPath, 1)
										Case $aBinImg[4]; ImgSparse
											GUICtrlSetData($iStBar, _LngStr('msg_sparse_img') & $aBinImg[8])
											_ConvSparse($InpImgPath, $OutImgPath, 1)
										Case Else
											GUICtrlSetData($iStBar, _LngStr('msg_copy_img_to_tmp') & $aBinImg[8])
											_CopyPart($InpImgPath, $OutImgPath)
									EndSwitch
									$ImgSize = FileGetSize($OutImgPath)
									$ImgSize = _HexPrefCheck(Hex($ImgSize, 8), $sHexPref)
									$ImgOffset = 16384 + FileGetSize($FirmBin)
									$ImgOffset = _HexPrefCheck(Hex($ImgOffset, 8), $sHexPref)
									Switch $aBinImg[7]; ImgType
										Case $aImgType[0], $aImgType[1]; rboot, sboot
											Local $BootIdx = '1'; sboot
											If ($aBinImg[7] = $aImgType[0]) Then $BootIdx = '0'
											GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('# %s\r\n%s %s %s %s %s\r\n%s %s %s %s 0 %s %s', $aBinImg[1], _
												$aMMC[5], $sRamAddr, $sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[9], $BootIdx, $sRamAddr, $ImgSize, $aBinImg[6]), 2) & @CRLF, 1)
										Case $aImgType[2]; Partition
											Switch $aYesNo[0]; Yes
												Case $aBinImg[3]; Lzo
													GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('# %s\r\n%s %s %s %s %s\r\n%s %s %s\r\n%s %s %s %s %s %s', $aBinImg[1], $aMMC[5], _
														$sRamAddr, $sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[4], $aBinImg[1], $aMMC[0], $aMMC[11], $sRamAddr, $ImgSize, $aBinImg[1], $aBinImg[6]), 2) & @CRLF, 1)
												Case $aBinImg[4]; Sparse
													GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n%s %s %s %s %s\r\n%s %s %s\r\n%s %s %s %s %s', $aBinImg[1], $aMMC[5], _
														$sRamAddr, $sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[4], $aBinImg[1], $aMMC[10], $aMMC[0], $sRamAddr, $aBinImg[1], $ImgSize) & @CRLF, 1)
												Case Else; Normal (not lzo\sparse)
													Switch $aBinImg[1]; if not risk, then ERASE part
														Case $aRiskImg[2], $aRiskImg[3], $aRiskImg[4]; risk parts (without ERASE)
															GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('# %s\r\n%s %s %s %s %s\r\n%s %s %s %s %s %s', $aBinImg[1], $aMMC[5], _
																$sRamAddr, $sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[6], $sRamAddr, $aBinImg[1], $ImgSize, $aBinImg[6]), 2) & @CRLF, 1)
														Case Else; with ERASE
															GUICtrlSetData($aTb1Ctrl[27], StringStripWS(StringFormat('# %s\r\n%s %s %s %s %s\r\n%s %s %s\r\n%s %s %s %s %s %s', $aBinImg[1], $aMMC[5], _
																$sRamAddr, $sIntName, $ImgOffset, $ImgSize, $aMMC[0], $aMMC[4], $aBinImg[1], $aMMC[0], $aMMC[6], $sRamAddr, $aBinImg[1], $ImgSize, $aBinImg[6]), 2) & @CRLF, 1)
													EndSwitch
											EndSwitch
										Case $aImgType[3]; SecureInfo
											GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n%s %s %s %s %s\r\n%s %s %s\r\n', $aBinImg[1], _
												$aMMC[5], $sRamAddr, $sIntName, $ImgOffset, $ImgSize, $aMMC[14], $aBinImg[1], $sRamAddr), 1)
										Case $aImgType[4]; NuttxConfig
											GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n%s %s %s %s %s\r\n%s %s %s\r\n', $aBinImg[1], _
												$aMMC[5], $sRamAddr, $sIntName, $ImgOffset, $ImgSize, $aMMC[15], $aBinImg[1], $sRamAddr), 1)
									EndSwitch
									GUICtrlSetData($iStBar, _LngStr('msg_align_img') & $aBinImg[8])
									_AlignFile($OutImgPath)
									GUICtrlSetData($iStBar, _LngStr('msg_append_img') & $aBinImg[8])
									_CopyPart($OutImgPath, $FirmBin)
									GUICtrlSetData($iStBar, _LngStr('msg_delete_tmp_files'))
									FileDelete($OutImgPath)
							EndSwitch
							Sleep(150)
						EndIf
					Next
				EndIf
				If ($iComboState = 2) Then; add FactoryInit
					If (StringLen($sFactoryInit) > 1) Then GUICtrlSetData($aTb1Ctrl[27], $sFactoryInit & @CRLF, 1)
				EndIf
				If (StringLen($sPostEnv) > 2) Then GUICtrlSetData($aTb1Ctrl[27], StringFormat('# %s\r\n%s\r\n', $aXmlTag[8], StringStripWS($sPostEnv, 3)), 1); PostEnv
				If ($sTypeCrc = 3) Then; TypeCRC
					Local $sBinCRC32 = _ReverseBytes(_CRC32Calc($FirmBin))
					GUICtrlSetData($aTb1Ctrl[27], StringFormat('%s %s %s\r\n%s %s %s\r\n%s\r\n', $aMMC[17], $aMMC[24], $sBinCRC32, $aMMC[17], $aMMC[25], $sBinCRC32, $aMMC[28]), 1)
				EndIf
				GUICtrlSetData($aTb1Ctrl[27], '% <- this is end of file symbol' & @CRLF, 1)
				GUICtrlSetData($iStBar, _LngStr('msg_write_firm_script'))
				_WriteFile($FirmScript, StringStripCR(GUICtrlRead($aTb1Ctrl[27])), 1)
				_AlignFile($FirmScript, 16384); Align header to 16 kb
				GUICtrlSetData($iStBar, _LngStr('msg_append_firm_script'))
				_CopyPart($FirmScript, $NewFirmware); append header
				If FileExists($FirmBin) Then
					GUICtrlSetData($iStBar, _LngStr('msg_append_bin_part'))
					_CopyPart($FirmBin, $NewFirmware); append bin
				EndIf
				GUICtrlSetData($iStBar, _LngStr('msg_write_firm_footer'))
				If ($sTypeCrc = 3) Then $sFooter &= StringTrimLeft(_CRC32Calc($FirmBin), 2); BIN_CRC
				$sFooter &= StringTrimLeft(StringToBinary('12345678', 4), 2) & StringTrimLeft(_CRC32Calc($FirmScript), 2); MAGIC+HEADER_CRC
				If ($sTypeCrc = 1) Then $sFooter &= StringTrimLeft(_CRC32Calc($FirmBin), 2); BIN_CRC
				_WriteFile($NewFirmware, $sFooter)
				If ($sTypeCrc > 1) Then _WriteFile($NewFirmware, _CRC32Calc($NewFirmware)); CRC32 of HEADER+BIN+BIN_CRC+MAGIC+HEADER_CRC
				_WriteFile($NewFirmware, StringToBinary('# MSTAR FIRMWARE', 4)); first 16 bytes of header part
				GUICtrlSetData($iStBar, _LngStr('msg_delete_tmp_files'))
				FileDelete($FirmScript)
				FileDelete($FirmBin)
				DirRemove($sWrkDir, $DIR_REMOVE)
				GUICtrlSetState($aTb1Ctrl[3], $GUI_ENABLE)
				GUICtrlSetData ($aTb1Ctrl[1], '')
				_ClearTabCtrl(1)
				GUICtrlSetData($iStBar, _LngStr('msg_done'))
				MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_done'), 0, $aGuiForm[0])
			Else
				MsgBox(4112, _LngStr('title_error'), _LngStr('msg_firm_cfg_not_sel'), 0, $aGuiForm[0])
			EndIf
		Case $aTb1Ctrl[4], $aTb1Ctrl[5], $aTb1Ctrl[6], $aTb1Ctrl[7], $aTb1Ctrl[8]; switch to Cfg, PartTable, ImgList, EnvList, Script
			For $ir = 9 To 27
				GUICtrlSetState($aTb1Ctrl[$ir], $GUI_HIDE)
			Next
			For $ir = 4 To 8
				If ($aTb1Ctrl[$ir] = @GUI_CtrlId) Then
					Switch $ir
						Case 4; show Cfg
							For $ic = 9 To 22
								GUICtrlSetState($aTb1Ctrl[$ic], $GUI_SHOW)
							Next
						Case 5; show PartTable
							GUICtrlSetState($aTb1Ctrl[23], $GUI_SHOW)
						Case 6; show ImgList
							GUICtrlSetState($aTb1Ctrl[24], $GUI_SHOW)
						Case 7; show EnvList
							GUICtrlSetState($aTb1Ctrl[25], $GUI_SHOW)
							GUICtrlSetState($aTb1Ctrl[26], $GUI_SHOW)
						Case 8; show Script
							GUICtrlSetState($aTb1Ctrl[27], $GUI_SHOW)
					EndSwitch
				EndIf
			Next
		Case $aTb1Ctrl[28]; ON-OFF comments line
			Select
				Case BitAND(GUICtrlRead($aTb1Ctrl[28]), $GUI_CHECKED) = $GUI_CHECKED
					GUICtrlSetState($aTb1Ctrl[29], $GUI_ENABLE)
					GUICtrlSetData ($aTb1Ctrl[29], '')
				Case BitAND(GUICtrlRead($aTb1Ctrl[28]), $GUI_UNCHECKED) = $GUI_UNCHECKED
					GUICtrlSetData ($aTb1Ctrl[29], '')
					GUICtrlSetState($aTb1Ctrl[29], $GUI_DISABLE)
			EndSelect
		Case $aTb2Ctrl[0]; Swith Tab2 Combo
			Switch GUICtrlSendMsg($aTb2Ctrl[0], $CB_GETCURSEL, 0, 0)
				Case 0
					_GUICtrlListView_SetItemChecked($aTb2Ctrl[24], -1, True)
				Case Else
					_GUICtrlListView_SetItemChecked($aTb2Ctrl[24], -1, False)
			EndSwitch
		Case $aTb2Ctrl[2]; Select and read BIN Firmware
			GUICtrlSetData ($aTb2Ctrl[1], FileOpenDialog(_LngStr('dlg_sel_firm'), @ScriptDir & '\', 'BIN file (*.bin)', 1 + 2, '', $aGuiForm[0]))
			If FileExists(GUICtrlRead($aTb2Ctrl[1])) Then
				Local $Inp0File = GUICtrlRead($aTb2Ctrl[1]), $sFileData = _ReadInpFile($Inp0File, 0, 16384, 1)
				If (StringInStr($sFileData, '% <-') <> 0) Then
					_ClearTabCtrl(2)
					GUICtrlSetData($iStBar, _LngStr('msg_read_firm'))
					$sFileData = StringRegExpReplace($sFileData,'(% <-.*)\n(.*)', '')
					GUICtrlSetData($aTb2Ctrl[27], StringReplace($sFileData, @LF, @CRLF), 1)
					Local $FName = StringRegExpReplace($Inp0File, '^(?:.*\\)([^\\]*?)(?:\.[^.]+)?$', '\1')
					GUICtrlSetData($aTb2Ctrl[10], @ScriptDir & '\work\' & $FName & '\'); DirPath
					GUICtrlSetData($aTb2Ctrl[12], $FName & '.bin'); ExtName
					Local $aScriptLines = StringSplit(StringRegExpReplace($sFileData, '(#.*)\n', ''), @LF, 1), $ImgName, $ImgSize, $ImgOffset, $ImgSkip
					For $i = 1 To $aScriptLines[0]
						If (StringLen($aScriptLines[$i]) >= 2) Then
							Local $aLineArgv = StringSplit($aScriptLines[$i], ' ', 1)
							$ImgSkip = $aYesNo[1]
							Switch $aLineArgv[1]
								Case $aMMC[0]; mmc
									Switch $aLineArgv[2]
										Case $aMMC[3]; create
											GUICtrlCreateListViewItem(StringFormat('%s|%s|%s', $aLineArgv[3], $aLineArgv[4], $aYesNo[1]), $aTb2Ctrl[23])
											If (StringLeft($aLineArgv[3], 3) = 'xgi') Then GUICtrlSendMsg($aTb2Ctrl[20], $CB_SETCURSEL, 1, 0); TypeCRC=2
										Case $aMMC[4]; erase.p
											For $i1 = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[23]) -1)
												If (_GUICtrlListView_GetItemText($aTb2Ctrl[23], $i1, 0) = $aLineArgv[3]) Then
													_GUICtrlListView_SetItemText($aTb2Ctrl[23], $i1, $aYesNo[0], 2); ImgErase
													ExitLoop
												EndIf
											Next
										Case $aMMC[9]; write.boot
											If $aLineArgv[0] > 6 Then $ImgSkip = $aYesNo[0]
											Switch $aLineArgv[3]
												Case '0'; RomBoot
													$ImgName = 'rboot'
													GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s.img|%s,%s', $ImgName, $ImgSize, $aYesNo[1], $aYesNo[1], $aYesNo[1], $aYesNo[1], $aImgType[0], $ImgName, $ImgOffset, $ImgSize), $aTb2Ctrl[24])
												Case '1'; SecureBoot
													$ImgName = 'sboot'
													GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s.img|%s,%s', $ImgName, $ImgSize, $aYesNo[1], $aYesNo[1], $aYesNo[1], $aYesNo[1], $aImgType[1], $ImgName, $ImgOffset, $ImgSize), $aTb2Ctrl[24])
											EndSwitch
										Case $aMMC[6], $aMMC[7], $aMMC[8]; write.p, write.p.continue, write.p.cont
											If $aLineArgv[0] > 5 Then $ImgSkip = $aYesNo[0]
											Switch $aLineArgv[4]; ImgName
												Case $ImgName
													For $i1 = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[24]) -1)
														If (_GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 0) = $ImgName) Then
															_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, _GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 1) + $ImgSize, 1)
															_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, $aYesNo[0], 4); ImgSplit
															_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, _GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 8) & StringFormat(';%s,%s', $ImgOffset, $ImgSize), 8)
															ExitLoop
														EndIf
													Next
												Case Else
													$ImgName = $aLineArgv[4]
													GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s.img|%s,%s', $ImgName, $ImgSize, $aYesNo[1], $aYesNo[1], $aYesNo[1], $ImgSkip, $aImgType[2], $ImgName, $ImgOffset, $ImgSize), $aTb2Ctrl[24])
											EndSwitch
										Case $aMMC[11], $aMMC[12], $aMMC[13]; unlzo, unlzo.continue, unlzo.cont
											Switch $aLineArgv[5]; ImgName
												Case $ImgName
													For $i1 = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[24]) -1)
														If (_GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 0) = $ImgName) Then
															_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, _GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 1) + $ImgSize, 1)
															_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, $aYesNo[0], 4); ImgSplit
															_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, _GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 8) & StringFormat(';%s,%s', $ImgOffset, $ImgSize), 8)
															ExitLoop
														EndIf
													Next
												Case Else
													$ImgName = $aLineArgv[5]
													If $aLineArgv[0] > 5 Then $ImgSkip = $aYesNo[0]
													GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s.img|%s,%s', $ImgName, $ImgSize, $aYesNo[0], $aYesNo[1], $aYesNo[1], $ImgSkip, $aImgType[2], $ImgName, $ImgOffset, $ImgSize), $aTb2Ctrl[24])
											EndSwitch
									EndSwitch
								Case $aMMC[5]; filepartload
									If (StringMid($aLineArgv[4], 1, 1) <> '$') Then
										$ImgOffset = Dec(_HexPrefCheck($aLineArgv[4]), 0)
										$ImgSize = Dec(_HexPrefCheck($aLineArgv[5]), 0)
									EndIf
									GUICtrlSetData($aTb2Ctrl[14], $aLineArgv[3]); InternalName
									If (StringLeft($aLineArgv[2], 2) == '0x') Then
										GUICtrlSendMsg($aTb2Ctrl[18], $CB_SETCURSEL, 0, 0); UseHexPrefix=$aYesNo[0]
										GUICtrlSetData($aTb2Ctrl[22], StringTrimLeft($aLineArgv[2], 2)); RamAddress
									Else
										GUICtrlSetData($aTb2Ctrl[22], $aLineArgv[2]); RamAddress
									EndIf
								Case $aMMC[10]; sparse_write
									Switch $aLineArgv[4]; ImgName
										Case $ImgName
											For $i1 = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[24]) -1)
												If (_GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 0) = $ImgName) Then
													_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, _GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 1) + $ImgSize, 1)
													_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, $aYesNo[0], 4); ImgSplit
													_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, _GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 8) & StringFormat(';%s,%s', $ImgOffset, $ImgSize), 8)
													ExitLoop
												EndIf
											Next
										Case Else
											$ImgName = $aLineArgv[4]
											GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s.img|%s,%s', $ImgName, $ImgSize, $aYesNo[1], $aYesNo[0], $aYesNo[1], $aYesNo[1], $aImgType[2], $ImgName, $ImgOffset, $ImgSize), $aTb2Ctrl[24])
									EndSwitch
								Case $aMMC[14]; store_secure_info
									If ($aLineArgv[2] = $ImgName & 'Sign') Then
										For $i1 = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[24]) -1)
											If (_GUICtrlListView_GetItemText($aTb2Ctrl[24], $i1, 0) = $ImgName) Then
												_GUICtrlListView_SetItemText($aTb2Ctrl[24], $i1, $ImgName & '.aes', 7)
												ExitLoop
											EndIf
										Next
									EndIf
									$ImgName = $aLineArgv[2]
									GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s.img|%s,%s', $ImgName, $ImgSize, $aYesNo[1], $aYesNo[1], $aYesNo[1], $aYesNo[1], $aImgType[3], $ImgName, $ImgOffset, $ImgSize), $aTb2Ctrl[24])
								Case $aMMC[15]; store_nuttx_config
									$ImgName = $aLineArgv[2]
									GUICtrlCreateListViewItem(StringFormat('%s|%s|%s|%s|%s|%s|%s|%s.img|%s,%s', $ImgName, $ImgSize, $aYesNo[1], $aYesNo[1], $aYesNo[1], $aYesNo[1], $aImgType[4], $ImgName, $ImgOffset, $ImgSize), $aTb2Ctrl[24])
								Case $aMMC[16]; factory_init
									GUICtrlSetData($aTb2Ctrl[16], $aScriptLines[$i]); FactoryInit
								Case $aMMC[17]; setenv
									Switch $aLineArgv[2]
										Case $aMMC[18]; imageSize
											If ($aLineArgv[0] > 2) Then $ImgSize = Dec(_HexPrefCheck($aLineArgv[3]), 0)
										Case $aMMC[19]; imageOffset
											If ($aLineArgv[0] > 2) Then $ImgOffset = Dec(_HexPrefCheck($aLineArgv[3]), 0)
										Case $aMMC[20], $aMMC[21], $aMMC[22], $aMMC[23]; open_DDR_size, DDR, EMMC, dont_overwrite_init
											GUICtrlSetData($aTb2Ctrl[25], $aScriptLines[$i] & @CRLF, 1); PreEnv
										Case $aMMC[24], $aMMC[25]; CEnv_UpgradeCRC_Tmp, CEnv_UpgradeCRC_Val
											GUICtrlSendMsg($aTb2Ctrl[20], $CB_SETCURSEL, 2, 0); TypeCRC=3
										Case Else
											GUICtrlSetData($aTb2Ctrl[26], $aScriptLines[$i] & @CRLF, 1); PostEnv
									EndSwitch
								Case $aMMC[26], $aMMC[27]; printenv, cleanallenv
									; skip
								Case Else
									GUICtrlSetData($aTb2Ctrl[26], $aScriptLines[$i] & @CRLF, 1); PostEnv
							EndSwitch
						EndIf
					Next
					If GUICtrlSendMsg($aTb2Ctrl[0], $CB_GETCURSEL, 0, 0) = 0 Then
						_GUICtrlListView_SetItemChecked($aTb2Ctrl[24], -1, True)
					Else
						_GUICtrlListView_SetItemChecked($aTb2Ctrl[24], -1, False)
					EndIf
					GUICtrlSetData($iStBar, _LngStr('msg_done'))
				Else
					GUICtrlSetData ($aTb2Ctrl[1], '')
					MsgBox(4112, _LngStr('title_error'), _LngStr('msg_firm_file_wrong'), 0, $aGuiForm[0])
				EndIf
			EndIf
		Case $aTb2Ctrl[3]; Unpack BIN Firmware
			If FileExists(GUICtrlRead($aTb2Ctrl[1])) Then
				GUICtrlSetState($aTb2Ctrl[3], $GUI_DISABLE)
				Local $Inp0File = GUICtrlRead($aTb2Ctrl[1])
				GUICtrlSetData($iStBar, _LngStr('msg_save_firm_script_and_cfg'))
				Local $OutDir = GUICtrlRead($aTb2Ctrl[10]), $ScriptFile= $OutDir & 'Script.sh', $ConfigFile= $OutDir & 'Config.xml'
				If FileExists($ScriptFile) Then FileDelete($ScriptFile)
				If FileExists($ConfigFile) Then FileDelete($ConfigFile)
				Local $ConfigData = StringFormat('<?xml version="1.0" encoding="UTF-8"?>\n<!-- Generated by MStarBinTool-GUI -->\n<%s>\n\t<%s>\n' & _
					'\t\t<%s>%s</%s>\n\t\t<%s>%s</%s>\n\t\t<%s>%s</%s>\n\t\t<%s>%s</%s>\n\t\t<%s>%s</%s>\n\t\t<%s>%s</%s>\n\t\t<%s>%s</%s>\n\t</%s>\n', _
					$aXmlTag[0], $aXmlTag[1], $aFirmCfg[0], GUICtrlRead($aTb2Ctrl[10]), $aFirmCfg[0], $aFirmCfg[1], GUICtrlRead($aTb2Ctrl[12]), $aFirmCfg[1], _
					$aFirmCfg[2], GUICtrlRead($aTb2Ctrl[14]), $aFirmCfg[2], $aFirmCfg[3], GUICtrlRead($aTb2Ctrl[16]), $aFirmCfg[3], $aFirmCfg[4], GUICtrlRead($aTb2Ctrl[18]), _
					$aFirmCfg[4], $aFirmCfg[5], GUICtrlRead($aTb2Ctrl[20]), $aFirmCfg[5], $aFirmCfg[6], GUICtrlRead($aTb2Ctrl[22]), $aFirmCfg[6], $aXmlTag[1])
				If (_GUICtrlListView_GetItemCount($aTb2Ctrl[23]) > 0) Then; save PartList
					$ConfigData &= StringFormat('\t<%s>\n', $aXmlTag[2])
					For $i = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[23]) -1)
						Local $aPList = _GUICtrlListView_GetItemTextArray($aTb2Ctrl[23], $i)
						$ConfigData &= StringFormat('\t\t<%s %s="%s" %s="%s" %s="%s"/>\n', $aXmlTag[3], $aPartTable[0], $aPList[1], $aPartTable[1], $aPList[2], $aPartTable[2], $aPList[3])
					Next
					$ConfigData &= StringFormat('\t</%s>\n', $aXmlTag[2])
				Else
					$ConfigData &= StringFormat('\t<%s></%s>\n', $aXmlTag[2], $aXmlTag[2])
				EndIf
				If (_GUICtrlListView_GetItemCount($aTb2Ctrl[24]) > 0) Then; save ImgList
					$ConfigData &= StringFormat('\t<%s>\n', $aXmlTag[4])
					For $i = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[24]) -1); save first rboot, sboot, mboot, mbootbak, mpool
						Local $aIList = _GUICtrlListView_GetItemTextArray($aTb2Ctrl[24], $i)
						Switch $aIList[1]
							Case $aRiskImg[0], $aRiskImg[1], $aRiskImg[2], $aRiskImg[3], $aRiskImg[4]
								$ConfigData &= StringFormat('\t\t<%s %s="%s" %s="%s" %s="%s" %s="%s" %s="%s" %s="%s" %s="%s"/>\n', $aXmlTag[5], $aImgList[0], $aIList[1], _
									$aImgList[2], $aIList[3], $aImgList[3], $aIList[4], $aImgList[4], $aIList[5], $aImgList[5], $aIList[6], $aImgList[6], $aIList[7], $aImgList[7], $aIList[8])
						EndSwitch
					Next
					For $i = 0 To (_GUICtrlListView_GetItemCount($aTb2Ctrl[24]) -1); save other images
						Local $aIList = _GUICtrlListView_GetItemTextArray($aTb2Ctrl[24], $i)
						Switch $aIList[1]
							Case $aRiskImg[0], $aRiskImg[1], $aRiskImg[2], $aRiskImg[3], $aRiskImg[4]
								; skip
							Case Else
								$ConfigData &= StringFormat('\t\t<%s %s="%s" %s="%s" %s="%s" %s="%s" %s="%s" %s="%s" %s="%s"/>\n', $aXmlTag[5], $aImgList[0], $aIList[1], _
									$aImgList[2], $aIList[3], $aImgList[3], $aIList[4], $aImgList[4], $aIList[5], $aImgList[5], $aIList[6], $aImgList[6], $aIList[7], $aImgList[7], $aIList[8])
						EndSwitch
					Next
					$ConfigData &= StringFormat('\t</%s>\n', $aXmlTag[4])
				Else
					$ConfigData &= StringFormat('\t<%s></%s>\n', $aXmlTag[4], $aXmlTag[4])
				EndIf
				$ConfigData &= StringFormat('\t<%s>\n', $aXmlTag[6]); Environments
				Local $sPreEnv = StringStripCR(GUICtrlRead($aTb2Ctrl[25]))
				If (StringLen($sPreEnv) > 2) Then; save PreEnv
					$ConfigData &= StringFormat('\t\t<%s>\n%s\n\t\t</%s>\n', $aXmlTag[7], StringStripWS($sPreEnv, 3), $aXmlTag[7])
				Else
					$ConfigData &= StringFormat('\t\t<%s></%s>\n', $aXmlTag[7], $aXmlTag[7])
				EndIf
				Local $sPostEnv = StringStripCR(GUICtrlRead($aTb2Ctrl[26]))
				If (StringLen($sPostEnv) > 2) Then; save PostEnv
					$ConfigData &= StringFormat('\t\t<%s>\n%s\n\t\t</%s>\n', $aXmlTag[8], StringStripWS($sPostEnv, 3), $aXmlTag[8])
				Else
					$ConfigData &= StringFormat('\t\t<%s></%s>\n', $aXmlTag[8], $aXmlTag[8])
				EndIf
				$ConfigData &= StringFormat('\t</%s>\n</%s>', $aXmlTag[6], $aXmlTag[0]); Environments, Firmware
				_WriteFile($ConfigFile, $ConfigData, 1)
				_WriteFile($ScriptFile, GUICtrlRead($aTb2Ctrl[27]), 1)
				If (GUICtrlSendMsg($aTb2Ctrl[0], $CB_GETCURSEL, 0, 0) = 0) Then
					For $i = 0 To _GUICtrlListView_GetItemCount($aTb2Ctrl[24]) -1
						If _GUICtrlListView_GetItemChecked($aTb2Ctrl[24], $i) Then
							Local $aImgInfo = _GUICtrlListView_GetItemTextArray($aTb2Ctrl[24], $i)
							Switch $aImgInfo[3]; ImgLzo
								Case $aYesNo[1]; No
									Switch $aImgInfo[5]; ImgSplit
										Case $aYesNo[1]; No
											Switch $aImgInfo[4]; ImgSparse
												Case $aYesNo[1]; No
													Local $aLineArgv = StringSplit($aImgInfo[9], ',', 1)
													GUICtrlSetData($iStBar, _LngStr('msg_save_img') & $aImgInfo[8])
													_CopyPart($Inp0File, $OutDir & $aImgInfo[8], $aLineArgv[1], $aLineArgv[2])
												Case $aYesNo[0]; Yes
													Local $aLineArgv = StringSplit($aImgInfo[9], ',', 1)
													GUICtrlSetData($iStBar, _LngStr('msg_save_img') & $aImgInfo[8])
													_CopyPart($Inp0File, $OutDir & $aImgInfo[1] & '_sparse.img', $aLineArgv[1], $aLineArgv[2])
													GUICtrlSetData($iStBar, _LngStr('msg_unsparse_img') & $aImgInfo[8])
													_ConvSparse($OutDir & $aImgInfo[1] & '_sparse.img', $OutDir & $aImgInfo[8])
													FileDelete($OutDir & $aImgInfo[1] & '_sparse.img')
											EndSwitch
										Case $aYesNo[0]; Yes
											Switch $aImgInfo[4]; ImgSparse
												Case $aYesNo[1]; No
													Local $aLines = StringSplit($aImgInfo[9], ';', 1)
													For $i1 = 1 To $aLines[0]
														Local $aLineArgv = StringSplit($aLines[$i1], ',', 1)
														GUICtrlSetData($iStBar, _LngStr('msg_save_chunk') & StringFormat('%s (%s|%s)', $aImgInfo[8], $i1, $aLines[0]))
														_CopyPart($Inp0File, $OutDir & $aImgInfo[8], $aLineArgv[1], $aLineArgv[2])
														Sleep(100)
													Next
												Case $aYesNo[0]; Yes
													Local $aLines = StringSplit($aImgInfo[9], ';', 1)
													For $i1 = 1 To $aLines[0]
														Local $aLineArgv = StringSplit($aLines[$i1], ',', 1)
														GUICtrlSetData($iStBar, _LngStr('msg_save_chunk') & StringFormat('%s (%s|%s)', $aImgInfo[8], $i1, $aLines[0]))
														_CopyPart($Inp0File, $OutDir & $aImgInfo[1] & '_sparsechunk.' & ($i1-1), $aLineArgv[1], $aLineArgv[2])
														GUICtrlSetData($iStBar, _LngStr('msg_unsparse_chunk') & StringFormat('%s (%s|%s)', $aImgInfo[8], $i1, $aLines[0]))
														_ConvSparse($OutDir & $aImgInfo[1] & '_sparsechunk.' & ($i1-1), $OutDir & $aImgInfo[8])
													Next
													FileDelete($OutDir & $aImgInfo[1] & '_sparsechunk.*')
											EndSwitch
									EndSwitch
								Case $aYesNo[0]; Yes
									Switch $aImgInfo[5]; ImgSplit
										Case $aYesNo[1]
											Local $aLineArgv = StringSplit($aImgInfo[9], ',', 1)
											GUICtrlSetData($iStBar, _LngStr('msg_save_img') & $aImgInfo[8])
											_CopyPart($Inp0File, $OutDir & $aImgInfo[1] & '.lzo', $aLineArgv[1], $aLineArgv[2])
											GUICtrlSetData($iStBar, _LngStr('msg_unlzo_img') & $aImgInfo[8])
											_LzoTool($OutDir & $aImgInfo[1] & '.lzo', $OutDir & $aImgInfo[8])
											FileDelete($OutDir & $aImgInfo[1] & '.lzo')
										Case $aYesNo[0]
											Local $aLines = StringSplit($aImgInfo[9], ';', 1)
											For $i1 = 1 To $aLines[0]
												Local $aLineArgv = StringSplit($aLines[$i1], ',', 1)
												GUICtrlSetData($iStBar, _LngStr('msg_save_chunk') & StringFormat('%s (%s|%s)', $aImgInfo[8], $i1, $aLines[0]))
												_CopyPart($Inp0File, $OutDir & $aImgInfo[1] & '.lzo', $aLineArgv[1], $aLineArgv[2])
												GUICtrlSetData($iStBar, _LngStr('msg_unlzo_chunk') & StringFormat('%s (%s|%s)', $aImgInfo[8], $i1, $aLines[0]))
												_LzoTool($OutDir & $aImgInfo[1] & '.lzo', $OutDir & $aImgInfo[1] & '1.img')
												GUICtrlSetData($iStBar, _LngStr('msg_append_chunk') & StringFormat('%s (%s|%s)', $aImgInfo[8], $i1, $aLines[0]))
												_CopyPart($OutDir & $aImgInfo[1] & '1.img', $OutDir & $aImgInfo[8])
												FileDelete($OutDir & $aImgInfo[1] & '.lzo')
												FileDelete($OutDir & $aImgInfo[1] & '1.img')
												Sleep(100)
											Next
									EndSwitch
							EndSwitch
							Sleep(100)
						EndIf
					Next
				EndIf
				GUICtrlSetState($aTb2Ctrl[3], $GUI_ENABLE)
				GUICtrlSetData ($aTb2Ctrl[1], '')
				_ClearTabCtrl(2)
				GUICtrlSetData($iStBar, _LngStr('msg_done'))
				MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_done'), 0, $aGuiForm[0])
			Else
				MsgBox(4112, _LngStr('title_error'), _LngStr('msg_firm_not_sel'), 0, $aGuiForm[0])
			EndIf
		Case $aTb2Ctrl[4], $aTb2Ctrl[5], $aTb2Ctrl[6], $aTb2Ctrl[7], $aTb2Ctrl[8]; switch to Cfg, PartTable, ImgList, EnvList, Script
			For $ir = 9 To 27
				GUICtrlSetState($aTb2Ctrl[$ir], $GUI_HIDE)
			Next
			For $ir = 4 To 8
				If ($aTb2Ctrl[$ir] = @GUI_CtrlId) Then
					Switch $ir
						Case 4; show Cfg
							For $ic = 9 To 22
								GUICtrlSetState($aTb2Ctrl[$ic], $GUI_SHOW)
							Next
						Case 5; show PartTable
							GUICtrlSetState($aTb2Ctrl[23], $GUI_SHOW)
						Case 6; show ImgList
							GUICtrlSetState($aTb2Ctrl[24], $GUI_SHOW)
						Case 7; show EnvList
							GUICtrlSetState($aTb2Ctrl[25], $GUI_SHOW)
							GUICtrlSetState($aTb2Ctrl[26], $GUI_SHOW)
						Case 8; show Script
							GUICtrlSetState($aTb2Ctrl[27], $GUI_SHOW)
					EndSwitch
				EndIf
			Next
		Case $aTb3Ctrl[1]; Select and read MBOOT image
			GUICtrlSetData ($aTb3Ctrl[0], FileOpenDialog(_LngStr('dlg_sel_mboot'), @ScriptDir & '\', 'MBOOT image (*MBOOT*.img)', 1 + 2, '', $aGuiForm[0]))
			If FileExists(GUICtrlRead($aTb3Ctrl[0])) Then
				_GUICtrlListView_DeleteAllItems($aTb3Ctrl[3])
				Local $Inp0File = GUICtrlRead($aTb3Ctrl[0]), $aKBankParts[13][3] = [['Security ID', 16, '5345435552495459'], ['u32Num', 8, ''], ['u32Size', 8, ''], ['u8Signature', 512, ''], _
					['RSA boot public key (N)', 512, ''], ['RSA boot public key (E)', 8, ''], ['RSA upgrade public key (N)', 512, ''], ['RSA upgrade public key (E)', 8, ''], ['RSA image public key (N)', 512, ''], _
					['RSA image public key (E)', 8, ''], ['AES boot key', 32, ''], ['AES upgrade key', 32, ''], ['Magic ID', 32, '4D737461722E4B65792E42616E6B2E2E']]
				GUICtrlSetData($iStBar, _LngStr('msg_read_mboot'))
				Local $sFileData = _ReadInpFile($Inp0File, 138240)
				Local $aKeysChunk = _StringBetween($sFileData, $aKBankParts[0][2], $aKBankParts[12][2], $STR_ENDNOTSTART)
				If IsArray($aKeysChunk) And (@error = 0) Then
					For $i = 0 To UBound($aKeysChunk) - 1
						If (StringLen($aKeysChunk[$i]) = 2152) Then
							$sFileData = $aKeysChunk[$i]
							ExitLoop
						EndIf
					Next
				EndIf
				Switch StringLen($sFileData)
					Case 2152
						GUICtrlSetData($iStBar, _LngStr('msg_keybank_found'))
						Local $DataOffset = 1
						For $i =0 To 12
							Switch $i
								Case 0, 12
									GUICtrlCreateListViewItem($aKBankParts[$i][0] & '|' & BinaryToString('0x' & $aKBankParts[$i][2], 4), $aTb3Ctrl[3])
								Case 1, 2, 3
									$DataOffset += $aKBankParts[$i][1]
								Case Else
									GUICtrlCreateListViewItem($aKBankParts[$i][0] & '|' & StringMid($sFileData, $DataOffset, $aKBankParts[$i][1]), $aTb3Ctrl[3])
									$DataOffset += $aKBankParts[$i][1]
							EndSwitch
						Next
					Case Else
						GUICtrlSetData($iStBar, _LngStr('msg_keybank_not_found'))
						GUICtrlSetData ($aTb3Ctrl[0], '')
						MsgBox(4112, _LngStr('title_error'), _LngStr('msg_keybank_not_found'), 0, $aGuiForm[0])
				EndSwitch
				$sFileData = 0
			EndIf
		Case $aTb3Ctrl[2]; Save MStar keys
			If FileExists(GUICtrlRead($aTb3Ctrl[0])) Then
				GUICtrlSetState($aTb3Ctrl[2], $GUI_DISABLE)
				Local $sKeysDir = @ScriptDir & '\keys\'
				Local $sRBootPubN = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 1, 1), $sRBootPubE = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 2, 1)
				Local $sRUpgPubN = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 3, 1), $sRUpgPubE = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 4, 1)
				Local $sRImgPubN = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 5, 1), $sRImgPubE = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 6, 1)
				Local $sABootK = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 7, 1), $sAUpgK = _GUICtrlListView_GetItemText($aTb3Ctrl[3], 8, 1)
				If (StringLeft($sRBootPubE, 2) = '00') Then $sRBootPubE = StringTrimLeft($sRBootPubE, 2)
				If (StringLeft($sRUpgPubE, 2) = '00') Then $sRUpgPubE = StringTrimLeft($sRUpgPubE, 2)
				If (StringLeft($sRImgPubE, 2) = '00') Then $sRImgPubE = StringTrimLeft($sRImgPubE, 2)
				_WriteFile($sKeysDir & 'RSAboot_pub.txt', StringFormat('N = %s\nE = %s', $sRBootPubN, $sRBootPubE), 1); asTXT
				_WriteFile($sKeysDir & 'RSAupgrade_pub.txt', StringFormat('N = %s\nE = %s', $sRUpgPubN, $sRUpgPubE), 1); asTXT
				_WriteFile($sKeysDir & 'RSAimage_pub.txt', StringFormat('N = %s\nE = %s', $sRImgPubN, $sRImgPubE), 1); asTXT
				_WriteFile($sKeysDir & 'AESBoot.bin', '0x' & $sABootK, 1); asHEX
				_WriteFile($sKeysDir & 'AESupgrade.bin', '0x' & $sAUpgK, 1); asHEX
				GUICtrlSetState($aTb3Ctrl[2], $GUI_ENABLE)
				GUICtrlSetData ($aTb3Ctrl[0], '')
				_GUICtrlListView_DeleteAllItems($aTb3Ctrl[3])
				GUICtrlSetData($iStBar, _LngStr('msg_done'))
				MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_keys_extracted'), 0, $aGuiForm[0])
			Else
				MsgBox(4112, _LngStr('title_error'), _LngStr('msg_mboot_not_sel'), 0, $aGuiForm[0])
			EndIf
		Case $aTb4Ctrl[0]; Swith Tab4 Combo
			Switch GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0)
				Case 0, 2
					GUICtrlSetState($aTb4Ctrl[4], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[5], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[6], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[7], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[8], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[9], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[10], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[11], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[12], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[13], $GUI_DISABLE)
				Case 1, 3
					GUICtrlSetState($aTb4Ctrl[4], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[5], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[6], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[7], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[8], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[9], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[10], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[11], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[12], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[13], $GUI_DISABLE)
				Case 4
					GUICtrlSetState($aTb4Ctrl[4], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[5], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[6], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[7], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[8], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[9], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[10], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[11], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[12], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[13], $GUI_DISABLE)
				Case 5
					GUICtrlSetState($aTb4Ctrl[4], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[5], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[6], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[7], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[8], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[9], $GUI_DISABLE)
					GUICtrlSetState($aTb4Ctrl[10], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[11], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[12], $GUI_ENABLE)
					GUICtrlSetState($aTb4Ctrl[13], $GUI_ENABLE)
			EndSwitch
			If GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0) < 2 Then
				GUICtrlSetData($aTb4Ctrl[1], _LngStr('tip_sel_enc_img'))
				GUICtrlSetData ($aTb4Ctrl[3], _LngStr('btn_decrypt_img'))
			Else
				GUICtrlSetData($aTb4Ctrl[1], _LngStr('tip_sel_dec_img'))
				GUICtrlSetData ($aTb4Ctrl[3], _LngStr('btn_encrypt_img'))
			EndIf
		Case $aTb4Ctrl[2]; Select boot\recovery image
			Switch GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0)
				Case 0, 1; Decrypt
					GUICtrlSetData ($aTb4Ctrl[1], FileOpenDialog(_LngStr('dlg_sel_enc_uboot'), @ScriptDir & '\', 'UBOOT image (boot*.aes;recovery*.aes)', 1 + 2, '', $aGuiForm[0]))
				Case 2, 3, 4, 5; Encrypt
					GUICtrlSetData ($aTb4Ctrl[1], FileOpenDialog(_LngStr('dlg_sel_dec_uboot'), @ScriptDir & '\', 'UBOOT image (boot*.img;recovery*.img)', 1 + 2, '', $aGuiForm[0]))
			EndSwitch
			If FileExists(GUICtrlRead($aTb4Ctrl[1])) Then
				Local $Inp0File = GUICtrlRead($aTb4Ctrl[1])
				Switch GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0)
					Case 0, 1; Decrypt
						GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking'))
						If Not (_ReadInpFile($Inp0File, 0, 4) = '0x' & $auCfg[0][1]) Then
							GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking_ok'))
						Else
							GUICtrlSetData($iStBar, _LngStr('msg_uboot_already_dec'))
							GUICtrlSetData($aTb4Ctrl[1], '')
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_already_dec'), 0, $aGuiForm[0])
						EndIf
					Case 2, 3, 4, 5; Encrypt
						GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking'))
						If (_ReadInpFile($Inp0File, 0, 4) = '0x' & $auCfg[0][1]) Then
							GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking_ok'))
						Else
							GUICtrlSetData($iStBar, _LngStr('msg_uboot_already_enc'))
							GUICtrlSetData($aTb4Ctrl[1], '')
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_already_enc'), 0, $aGuiForm[0])
						EndIf
				EndSwitch
			EndIf
		Case $aTb4Ctrl[3]; Encrypt\Decrypt uboot image
			If FileExists(GUICtrlRead($aTb4Ctrl[1])) Then
				GUICtrlSetState($aTb4Ctrl[3], $GUI_DISABLE)
				Local $ImgPath, $AES_File, $ResRSA_pub
				Local $Inp0File = GUICtrlRead($aTb4Ctrl[1]), $DefAES = GUICtrlRead($aTb4Ctrl[4]), $DefRSA_priv = GUICtrlRead($aTb4Ctrl[6])
				Local $DefRSA_pub = GUICtrlRead($aTb4Ctrl[8]), $CustAES = GUICtrlRead($aTb4Ctrl[10]), $CustRSA_pub = GUICtrlRead($aTb4Ctrl[12])
				Local $ImgName = @ScriptDir & '\work\' & StringRegExpReplace($Inp0File, '^(?:.*\\)([^\\]*?)(?:\.[^.]+)?$', '\1'), $ImgSignName = $ImgName & 'Sign.img'
				If (GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0) < 2) Then
					$ImgPath = $ImgName & '.img';def_enc_ext
				Else
					$ImgPath = $ImgName & '.aes';def_enc_ext
				EndIf
				Switch GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0)
					Case 0, 1; Decrypt (default\custom keys)
						If (GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0) = 0) Then
							$AES_File = $DefAES
						Else
							$AES_File = $CustAES
						EndIf
						If FileExists($AES_File) Then
							GUICtrlSetData($iStBar, _LngStr('msg_decrypt_img'))
							_AesCrypt2($Inp0File, $ImgPath, $AES_File, 1)
							If FileExists($ImgPath) Then
								GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking'))
								If (_ReadInpFile($ImgPath, 0, 4) = '0x' & $auCfg[0][1]) Then
									GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking_ok'))
									MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_done'), 0, $aGuiForm[0])
								Else
									GUICtrlSetData($iStBar, _LngStr('msg_uboot_magic_not_found'))
									GUICtrlSetData($aTb4Ctrl[1], '')
									FileDelete($ImgPath)
								EndIf
							Else
								MsgBox(4112, _LngStr('title_error'), _LngStr('msg_error_decrypt_img'), 0, $aGuiForm[0])
							EndIf
						Else
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_aes_keyfile_not_found'), 0, $aGuiForm[0])
						EndIf
					Case 2, 3; Encrypt (default\custom keys)
						If (GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0) = 2) Then
							$AES_File = $DefAES
						Else
							$AES_File = $CustAES
						EndIf
						If FileExists($AES_File) Then
							GUICtrlSetData($iStBar, _LngStr('msg_encrypt_img'))
							_AesCrypt2($Inp0File, $ImgPath, $AES_File, 0)
							If FileExists($ImgPath) Then
								GUICtrlSetData($iStBar, _LngStr('msg_done'))
								MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_done'), 0, $aGuiForm[0])
							Else
								MsgBox(4112, _LngStr('title_error'), _LngStr('msg_error_encrypt_img'), 0, $aGuiForm[0])
							EndIf
						Else
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_aes_keyfile_not_found'), 0, $aGuiForm[0])
						EndIf
					Case 4, 5; Encrypt+Sign (default\custom keys)
						If (GUICtrlSendMsg($aTb4Ctrl[0], $CB_GETCURSEL, 0, 0) = 4) Then
							$AES_File = $DefAES
							$ResRSA_pub = $DefRSA_pub
						Else
							$AES_File = $CustAES
							$ResRSA_pub = $CustRSA_pub
						EndIf
						If FileExists($AES_File) And FileExists($DefRSA_priv) And FileExists($ResRSA_pub) Then
							GUICtrlSetData($iStBar, _LngStr('msg_align_img') & $Inp0File)
							_AlignImg($Inp0File)
							GUICtrlSetData($iStBar, _LngStr('msg_gen_sign_img'))
							_GenSignImg($Inp0File, $ImgSignName, $DefRSA_priv, $ResRSA_pub)
							If FileExists($ImgSignName) Then
								GUICtrlSetData($iStBar, _LngStr('msg_encrypt_img'))
								_AesCrypt2($Inp0File, $ImgPath, $AES_File, 0)
								If FileExists($ImgPath) Then
									GUICtrlSetData($iStBar, _LngStr('msg_done'))
									MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_done'), 0, $aGuiForm[0])
								Else
									MsgBox(4112, _LngStr('title_error'), _LngStr('msg_error_encrypt_img'), 0, $aGuiForm[0])
								EndIf
							Else
								MsgBox(4112, _LngStr('title_error'), _LngStr('msg_error_sign_img'), 0, $aGuiForm[0])
							EndIf
						Else
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_aes_rsa_keyfile_not_found'), 0, $aGuiForm[0])
						EndIf
				EndSwitch
				GUICtrlSetState($aTb4Ctrl[3], $GUI_ENABLE)
				GUICtrlSetData ($aTb4Ctrl[1], '')
				GUICtrlSetData($iStBar, _LngStr('msg_done'))
			Else
				MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_not_sel'), 0, $aGuiForm[0])
			EndIf
		Case $aTb4Ctrl[5]; Select default AES boot key file
			GUICtrlSetData ($aTb4Ctrl[4], FileOpenDialog(_LngStr('dlg_sel_def_aes_key'), @ScriptDir & '\default_keys\', 'AES key file (AES*.bin)', 1 + 2, '', $aGuiForm[0]))
		Case $aTb4Ctrl[7]; Select default RSA boot private key file
			GUICtrlSetData ($aTb4Ctrl[6], FileOpenDialog(_LngStr('dlg_sel_rsa_priv_key'), @ScriptDir & '\default_keys\', 'RSA key file (RSA*_priv.txt)', 1 + 2, '', $aGuiForm[0]))
		Case $aTb4Ctrl[9]; Select default RSA boot public key file
			GUICtrlSetData ($aTb4Ctrl[8], FileOpenDialog(_LngStr('dlg_sel_def_rsa_publ_key'), @ScriptDir & '\default_keys\', 'RSA key file (RSA*_pub.txt)', 1 + 2, '', $aGuiForm[0]))
		Case $aTb4Ctrl[11]; Select custom AES boot key file
			GUICtrlSetData ($aTb4Ctrl[10], FileOpenDialog(_LngStr('dlg_sel_cus_aes_key'), @ScriptDir & '\keys\', 'AES key file (AES*.bin)', 1 + 2, '', $aGuiForm[0]))
		Case $aTb4Ctrl[13]; Select custom RSA boot public key file
			GUICtrlSetData ($aTb4Ctrl[12], FileOpenDialog(_LngStr('dlg_sel_cus_rsa_publ_key'), @ScriptDir & '\keys\', 'RSA key file (RSA*_pub.txt)', 1 + 2, '', $aGuiForm[0]))
		Case $aTb5Ctrl[0]; Swith Tab5 Combo
			Switch GUICtrlSendMsg($aTb5Ctrl[0], $CB_GETCURSEL, 0, 0)
				Case 0
					GUICtrlSetData($aTb5Ctrl[1], _LngStr('tip_sel_uboot_cfg'))
					GUICtrlSetData ($aTb5Ctrl[3], _LngStr('btn_pack_img'))
				Case 1
					GUICtrlSetData($aTb5Ctrl[1], _LngStr('tip_sel_uboot_img'))
					GUICtrlSetData ($aTb5Ctrl[3], _LngStr('btn_unpack_img'))
			EndSwitch
		Case $aTb5Ctrl[2]; Select Uboot image\config
			Local $CurComboSel = GUICtrlSendMsg($aTb5Ctrl[0], $CB_GETCURSEL, 0, 0)
			If ($CurComboSel = 0) Then
				GUICtrlSetData ($aTb5Ctrl[1], FileOpenDialog(_LngStr('dlg_sel_uboot_cfg'), @ScriptDir & '\', 'INI file (*.ini)', 1 + 2, '', $aGuiForm[0]))
			Else
				GUICtrlSetData ($aTb5Ctrl[1], FileOpenDialog(_LngStr('dlg_sel_dec_uboot'), @ScriptDir & '\', 'UBOOT image (boot*.img;recovery*.img)', 1 + 2, '', $aGuiForm[0]))
			EndIf
			If FileExists(GUICtrlRead($aTb5Ctrl[1])) Then
				_ClearTabCtrl(5)
				Local $Inp0File = GUICtrlRead($aTb5Ctrl[1])
				GUICtrlSetData($iStBar, _LngStr('msg_read_uboot'))
				Switch $CurComboSel; read img or cfg
					Case 0; read ini
						Local $aUbIniCfg = IniReadSection($Inp0File, 'UBOOT')
						If IsArray($aUbIniCfg) Then
							For $i = 1 To UBound($aUbIniCfg) -1
								$aUbIniCfg[$i][1] = StringReplace($aUbIniCfg[$i][1], '"', '')
								GUICtrlCreateListViewItem(_LngStr($auCfg[$i-1][3]) & '|' & $aUbIniCfg[$i][1], $aTb5Ctrl[4])
							Next
							Switch $aUbIniCfg[1][1]; check MAGIC
								Case $auCfg[0][1]; correct
									GUICtrlSetData($iStBar, _LngStr('msg_uboot_magic_found'))
									If ($aUbIniCfg[8][1] = $auCfg[7][1]) And ($aUbIniCfg[9][1] = $auCfg[8][1]) And ($aUbIniCfg[10][1] = $auCfg[9][1]) And ($aUbIniCfg[11][1] = $auCfg[10][1]) Then
										Switch $aUbIniCfg[12][1]; IN
											Case $auCfg[14][0], $auCfg[14][1]
												GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking_ok'))
											Case Else
												GUICtrlSetData($iStBar, _LngStr('msg_uboot_int_name_not_math'))
												GUICtrlSetData($aTb5Ctrl[1], '')
										EndSwitch
									Else
										GUICtrlSetData($iStBar, _LngStr('msg_uboot_img_wrong'))
										GUICtrlSetData($aTb5Ctrl[1], '')
										_ClearTabCtrl(5)
										MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_img_wrong'), 0, $aGuiForm[0])
									EndIf
								Case Else; incorrect
									GUICtrlSetData($iStBar, _LngStr('msg_uboot_magic_not_found'))
									GUICtrlSetData ($aTb5Ctrl[1], '')
									_ClearTabCtrl(5)
									MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_magic_not_found'), 0, $aGuiForm[0])
							EndSwitch
						Else
							GUICtrlSetData($iStBar, _LngStr('msg_uboot_cfg_wrong'))
							GUICtrlSetData ($aTb5Ctrl[1], '')
							_ClearTabCtrl(5)
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_cfg_wrong'), 0, $aGuiForm[0])
						EndIf
					Case 1; read img
						Local $aUbCnk[15], $sFOP = FileOpen($Inp0File, 16)
						For $i = 0 To UBound($aUbCnk) -1
							$aUbCnk[$i] = StringTrimLeft(FileRead($sFOP, $auCfg[$i][0]), 2)
							Switch $i
								Case 2; BT
									$aUbCnk[$i] = int('0x' & Hex('0x' & $aUbCnk[$i]), 1); ! CONVERT LITTLE ENDIAN TO BIG ENDIAN, THEN INT OF VALUE
									$aUbCnk[$i] = _EPOCH_decrypt($aUbCnk[$i])
								Case 3, 12, 13
									$aUbCnk[$i] = int('0x' & Hex('0x' & $aUbCnk[$i]), 1); ! CONVERT LITTLE ENDIAN TO BIG ENDIAN, THEN INT OF VALUE
								Case 7; OS
									For $i1 = 0 To UBound($auOS) -1
										If ($auOS[$i1][0] = $aUbCnk[$i]) Then
											$aUbCnk[$i] = $auOS[$i1][1]
											ExitLoop
										EndIf
									Next
								Case 8; ARCH
									For $i1 = 0 To UBound($auArch) -1
										If ($auArch[$i1][0] = $aUbCnk[$i]) Then
											$aUbCnk[$i] = $auArch[$i1][1]
											ExitLoop
										EndIf
									Next
								Case 9; IT
									For $i1 = 0 To UBound($auType) -1
										If ($auType[$i1][0] = $aUbCnk[$i]) Then
											$aUbCnk[$i] = $auType[$i1][1]
											ExitLoop
										EndIf
									Next
								Case 10; CT
									For $i1 = 0 To UBound($auCmpr) -1
										If ($auCmpr[$i1][0] = $aUbCnk[$i]) Then
											$aUbCnk[$i] = $auCmpr[$i1][1]
											ExitLoop
										EndIf
									Next
								Case 11; IN
									$aUbCnk[$i] = BinaryToString('0x' & $aUbCnk[$i], 4)
							EndSwitch
							If ($i < 14) Then
								GUICtrlCreateListViewItem(_LngStr($auCfg[$i][3]) & '|' & $aUbCnk[$i], $aTb5Ctrl[4])
							Else
								GUICtrlCreateListViewItem(_LngStr($auCfg[$i][3]) & '|' & @ScriptDir & '\work\' & StringRegExpReplace($Inp0File, '^(?:.*\\)([^\\]*?)(?:\.[^.]+)?$', '\1'), $aTb5Ctrl[4])
							EndIf
						Next
						FileClose($sFOP)
						Switch $aUbCnk[0]; check MAGIC
							Case $auCfg[0][1]; correct
								GUICtrlSetData($iStBar, _LngStr('msg_uboot_magic_found'))
								If ($aUbCnk[7] = $auCfg[7][1]) And ($aUbCnk[8] = $auCfg[8][1]) And ($aUbCnk[9] = $auCfg[9][1]) And ($aUbCnk[10] = $auCfg[10][1]) Then
									Switch $aUbCnk[11]; IN
										Case $auCfg[14][0], $auCfg[14][1]
											GUICtrlSetData($iStBar, _LngStr('msg_uboot_checking_ok'))
										Case Else
											GUICtrlSetData($iStBar, _LngStr('msg_uboot_int_name_not_math'))
											GUICtrlSetData($aTb5Ctrl[1], '')
									EndSwitch
								Else
									GUICtrlSetData($iStBar, _LngStr('msg_uboot_img_wrong'))
									GUICtrlSetData($aTb5Ctrl[1], '')
									_ClearTabCtrl(5)
									MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_img_wrong'), 0, $aGuiForm[0])
								EndIf
							Case Else; incorrect
								GUICtrlSetData($iStBar, _LngStr('msg_uboot_magic_not_found'))
								GUICtrlSetData ($aTb5Ctrl[1], '')
								_ClearTabCtrl(5)
								MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_magic_not_found'), 0, $aGuiForm[0])
						EndSwitch
				EndSwitch
			EndIf
		Case $aTb5Ctrl[3]; Pack\Unpack Uboot
			If FileExists(GUICtrlRead($aTb5Ctrl[1])) Then
				GUICtrlSetState($aTb5Ctrl[3], $GUI_DISABLE)
				Local $Inp0File = GUICtrlRead($aTb5Ctrl[1])
				Switch GUICtrlSendMsg($aTb5Ctrl[0], $CB_GETCURSEL, 0, 0)
					Case 0; pack image
						Local $LoadAddr = _GUICtrlListView_GetItemText($aTb5Ctrl[4], 4, 1), $EntryPoint = _GUICtrlListView_GetItemText($aTb5Ctrl[4], 5, 1)
						Local $ImgName = _GUICtrlListView_GetItemText($aTb5Ctrl[4], 11, 1), $UnpDir = _GUICtrlListView_GetItemText($aTb5Ctrl[4], 14, 1)
						Local $KernelImg = $UnpDir & '\kernel.img', $RDiskImg = $UnpDir & '\ramdisk.img', $OutImg = 'boot-new.img'
						If (StringInStr($ImgName, 'recovery') <> 0) Then $OutImg = 'recovery-new.img'
						GUICtrlSetData($iStBar, _LngStr('msg_create_ramdisk'))
						_AndImgTool($RDiskImg, $UnpDir, 1)
						GUICtrlSetData($iStBar, _LngStr('msg_create_uboot'))
						Local $sCrRes = _MkUbootImage($UnpDir, $LoadAddr, $EntryPoint, $ImgName, $OutImg)
						FileDelete($RDiskImg)
						MsgBox(4160, _LngStr('title_notification'), $sCrRes, 0, $aGuiForm[0])
					Case 1; unpack image
						Local $UnpDir, $KernelImgSize, $RDiskImgSize, $UbootConfig
						For $i = 0 To (_GUICtrlListView_GetItemCount($aTb5Ctrl[4]) -1)
							Local $aImgCfg = _GUICtrlListView_GetItemTextArray($aTb5Ctrl[4], $i)
							$UbootConfig &= StringFormat('%s="%s"\n', $auCfg[$i][2], $aImgCfg[2])
							Switch $i
								Case 12
									$KernelImgSize = $aImgCfg[2]
								Case 13
									$RDiskImgSize = $aImgCfg[2]
								Case 14
									$UnpDir = $aImgCfg[2]
							EndSwitch
						Next
						Local $KernelImg = $UnpDir & '\kernel.img', $RDiskImg = $UnpDir & '\ramdisk.img'
						GUICtrlSetData($iStBar, _LngStr('msg_create_outdirs'))
						DirRemove($UnpDir, $DIR_REMOVE)
						DirCreate($UnpDir)
						Sleep(200)
						GUICtrlSetData($iStBar, _LngStr('msg_save_kernel'))
						_CopyPart($Inp0File, $KernelImg, 76, $KernelImgSize)
						GUICtrlSetData($iStBar, _LngStr('msg_save_ramdisk'))
						_CopyPart($Inp0File, $RDiskImg, 76 + $KernelImgSize, $RDiskImgSize)
						GUICtrlSetData($iStBar, _LngStr('msg_unpack_ramdisk'))
						_AndImgTool($RDiskImg, $UnpDir)
						GUICtrlSetData($iStBar, _LngStr('msg_save_uboot_cfg'))
						IniWriteSection($UnpDir & '\config.ini', 'UBOOT', $UbootConfig)
						FileDelete($RDiskImg)
				EndSwitch
				GUICtrlSetState($aTb5Ctrl[3], $GUI_ENABLE)
				GUICtrlSetData ($aTb5Ctrl[1], '')
				_ClearTabCtrl(5)
				GUICtrlSetData($iStBar, _LngStr('msg_done'))
				MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_done'), 0, $aGuiForm[0])
			Else
				MsgBox(4112, _LngStr('title_error'), _LngStr('msg_uboot_img_or_cfg_not_sel'), 0, $aGuiForm[0])
			EndIf
		Case $aTb6Ctrl[1]; Search devices IP
			GUICtrlSetState($aTb6Ctrl[1], $GUI_DISABLE)
			Local $aLocIPs[4] = [@IPAddress1, @IPAddress2, @IPAddress3, @IPAddress4]
			For $i = 0 To 3
				If (StringInStr($aLocIPs[$i], '192.168.') <> 0) Then $aAdbCfg[1] = $aLocIPs[$i]
			Next
			Switch $aAdbCfg[1]
				Case '0.0.0.0', '127.0.0.1'
					MsgBox(4112, _LngStr('title_error'), _LngStr('msg_get_ip_failed'), 0, $aGuiForm[0])
				Case Else
					Local $DevIPCombo, $TestIP, $CurIPMask = StringRegExpReplace($aAdbCfg[1], '(192).(168).([0-9]{1,3}).([0-9]{1,3})', '$1.$2.$3.')
					Switch Number(StringRegExpReplace($aAdbCfg[1], '(192).(168).([0-9]{1,3}).([0-9]{1,3})', '$4')); pool IP=40
						Case 1 To 40
							For $i = 2 To 40
								$TestIP = $CurIPMask & $i
								GUICtrlSetData($iStBar, _LngStr('msg_check_ip') & $TestIP)
								If Not ($aAdbCfg[1] = $TestIP) Then
									Ping($TestIP, 1000)
									If (@error = 0) Then $DevIPCombo &= $TestIP & '|'
								EndIf
							Next
						Case 41 To 80
							For $i = 41 To 80
								$TestIP = $CurIPMask & $i
								GUICtrlSetData($iStBar, _LngStr('msg_check_ip') & $TestIP)
								If Not ($aAdbCfg[1] = $TestIP) Then
									Ping($TestIP, 1000)
									If (@error = 0) Then $DevIPCombo &= $TestIP & '|'
								EndIf
							Next
						Case 81 To 120
							For $i = 81 To 120
								$TestIP = $CurIPMask & $i
								GUICtrlSetData($iStBar, _LngStr('msg_check_ip') & $TestIP)
								If Not ($aAdbCfg[1] = $TestIP) Then
									Ping($TestIP, 1000)
									If (@error = 0) Then $DevIPCombo &= $TestIP & '|'
								EndIf
							Next
						Case 121 To 160
							For $i = 121 To 160
								$TestIP = $CurIPMask & $i
								GUICtrlSetData($iStBar, _LngStr('msg_check_ip') & $TestIP)
								If Not ($aAdbCfg[1] = $TestIP) Then
									Ping($TestIP, 1000)
									If (@error = 0) Then $DevIPCombo &= $TestIP & '|'
								EndIf
							Next
						Case 161 To 200
							For $i = 161 To 200
								$TestIP = $CurIPMask & $i
								GUICtrlSetData($iStBar, _LngStr('msg_check_ip') & $TestIP)
								If Not ($aAdbCfg[1] = $TestIP) Then
									Ping($TestIP, 1000)
									If (@error = 0) Then $DevIPCombo &= $TestIP & '|'
								EndIf
							Next
						Case 201 To 240
							For $i = 201 To 240
								$TestIP = $CurIPMask & $i
								GUICtrlSetData($iStBar, _LngStr('msg_check_ip') & $TestIP)
								If Not ($aAdbCfg[1] = $TestIP) Then
									Ping($TestIP, 1000)
									If (@error = 0) Then $DevIPCombo &= $TestIP & '|'
								EndIf
							Next
						Case 241 To 255
							For $i = 241 To 255
								$TestIP = $CurIPMask & $i
								GUICtrlSetData($iStBar, _LngStr('msg_check_ip') & $TestIP)
								If Not ($aAdbCfg[1] = $TestIP) Then
									Ping($TestIP, 1000)
									If (@error = 0) Then $DevIPCombo &= $TestIP & '|'
								EndIf
							Next
					EndSwitch
					GUICtrlSetData($aTb6Ctrl[0], '')
					GUICtrlSetData($aTb6Ctrl[0], StringTrimRight($DevIPCombo, 1))
					GUICtrlSendMsg($aTb6Ctrl[0], $CB_SETCURSEL, 0, 0)
					GUICtrlSetData($iStBar, _LngStr('msg_done'))
			EndSwitch
			GUICtrlSetState($aTb6Ctrl[1], $GUI_ENABLE)
		Case $aTb6Ctrl[2]; Show Tip
			MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_connect_help'), 0, $aGuiForm[0])
		Case $aTb6Ctrl[3]; Connect device
			Switch $aAdbCfg[0]; check connect state
				Case '0'
					GUICtrlSetState($aTb6Ctrl[3], $GUI_DISABLE)
					Local $SelIP = GUICtrlRead($aTb6Ctrl[0])
					_SaveSettings('LastIP', $SelIP); save IP
					For $i = 15 To 1 Step -1
						GUICtrlSetData($iStBar, _LngStr('msg_connect_to_dev') & $i)
						_ADB('connect ' & $SelIP & ':5555')
						If (_ADB('get-state') = 'device') Then
							$aAdbCfg[0] = '1'
							ExitLoop
						EndIf
					Next
					Switch $aAdbCfg[0]
						Case '1'
							GUICtrlSetData($iStBar, _LngStr('msg_connected_dev') & _ADB('ro.product.model', 4))
						Case '0'
							MsgBox(4112, _LngStr('title_error'), _LngStr('msg_error_connect_dev'), 0, $aGuiForm[0])
					EndSwitch
					GUICtrlSetState($aTb6Ctrl[3], $GUI_ENABLE)
				Case '1'
					GUICtrlSetData($iStBar, _LngStr('msg_dev_already_con'))
			EndSwitch
		Case $aTb6Ctrl[4]; Disconnect device
			If ProcessExists('adb.exe') Or ($aAdbCfg[0] = '1') Then
				GUICtrlSetState($aTb6Ctrl[4], $GUI_DISABLE)
				_ADB('disconnect')
				_ADB('kill-server')
				ProcessClose('adb.exe')
				$aAdbCfg[0] = '0'
				GUICtrlSetData($iStBar, _LngStr('msg_dev_disconnected'))
				GUICtrlSetState($aTb6Ctrl[4], $GUI_ENABLE)
			Else
				GUICtrlSetData($iStBar, _LngStr('msg_dev_already_discon'))
			EndIf
		Case $aTb6Ctrl[6]; Send command
			Switch GUICtrlSendMsg($aTb6Ctrl[5], $CB_GETCURSEL, 0, 0)
				Case 0; Show PartTable
					_ClearTabCtrl(6)
					GUICtrlSetData($aTb6Ctrl[7], _LngStr('lv_adb_part_table'))
					_GUICtrlListView_SetExtendedListViewStyle($aTb6Ctrl[7], $LVS_EX_FULLROWSELECT)
					GUICtrlSetData($iStBar, _LngStr('msg_load_part_table'))
					Local $PartList1 = _ADB('cat /proc/partitions', 1, 1), $PartList2 = _ADB('ls -al /dev/block/platform/mstar_mci.0/by-name', 1, 1)
					$PartList1 = StringRegExpReplace(StringRegExpReplace($PartList1, '(\w+)\s(\w+)\s+#blocks\s+(\w+)', ''), '\s(\d+)\s+(\d+)\s+(\d+)\s(\w+)', '$4|$3')
					$PartList2 = StringRegExpReplace($PartList2, '(\w+)\s(\w+)\s+(\w+)\s+(\d+)-(\d+)-(\d+)\s(\d+):(\d+)\s(\w+)\s->\s/(\w+)/(\w+)/(\w+)', '$12|$9')
					Local $aPTable1 = StringSplit($PartList1, @LF, 1), $aPTable2 = StringSplit($PartList2, @LF, 1)
					If IsArray($aPTable1) And IsArray($aPTable2) Then
						Local $aPTable1Argv, $aPTable2Argv, $aDevPartTable[1][3] = [['Name', 'Block', 'Size']]
						For $i = 1 To $aPTable1[0]
							If (StringLen($aPTable1[$i]) > 2) Then
								$aPTable1Argv = StringSplit($aPTable1[$i], '|', 1)
								Switch $aPTable1Argv[1]
									Case 'mmcblk0'
										_ArrayAdd($aDevPartTable, StringFormat('All EMMC|%s|%s', $aPTable1Argv[1], $aPTable1Argv[2] * 1024))
									Case 'mmcblk0boot0'
										_ArrayAdd($aDevPartTable, StringFormat('rboot|%s|%s', $aPTable1Argv[1], $aPTable1Argv[2] * 1024))
									Case 'mmcblk0boot1'
										_ArrayAdd($aDevPartTable, StringFormat('sboot|%s|%s', $aPTable1Argv[1], $aPTable1Argv[2] * 1024))
									Case 'zram0'
										_ArrayAdd($aDevPartTable, StringFormat('zRam|%s|%s', $aPTable1Argv[1], $aPTable1Argv[2] * 1024))
									Case Else
										_ArrayAdd($aDevPartTable, StringFormat('0|%s|%s', $aPTable1Argv[1], $aPTable1Argv[2] * 1024))
								EndSwitch
							EndIf
						Next
						For $i = 1 To $aPTable2[0]
							If (StringLen($aPTable2[$i]) > 2) Then
								$aPTable2Argv = StringSplit($aPTable2[$i], '|', 1)
								For $i1 = 1 To UBound($aDevPartTable) - 1
									If ($aPTable2Argv[1] = $aDevPartTable[$i1][1]) Then $aDevPartTable[$i1][0] = $aPTable2Argv[2]
								Next
							EndIf
						Next
						For $i = 1 To UBound($aDevPartTable) - 1
							If (StringLen($aDevPartTable[$i][0]) > 2) Then
								GUICtrlCreateListViewItem(StringFormat('%s|%s|%s', $aDevPartTable[$i][0], $aDevPartTable[$i][1], $aDevPartTable[$i][2]), $aTb6Ctrl[7])
								Sleep(20)
							EndIf
						Next
						GUICtrlSetData($iStBar, _LngStr('msg_done'))
					Else
						GUICtrlSetData($iStBar, _LngStr('msg_error_load_part_table'))
					EndIf
				Case 1; Save PartTable
					Local $PartTableListFile = @ScriptDir & '\work\EmmcPartTable.xml'
					Local $PartTableList = StringFormat('<?xml version="1.0" encoding="UTF-8"?>\n<!-- Generated by MStarBinTool-GUI -->\n<%s>\n', $aXmlTag[9])
					For $i = 0 To _GUICtrlListView_GetItemCount($aTb6Ctrl[7]) -1
						Local $aEPT = _GUICtrlListView_GetItemTextArray($aTb6Ctrl[7], $i)
						If Not ($aEPT[1] = 'zRam') Then $PartTableList &= StringFormat('\t<%s %s="%s" %s="%s" %s="/dev/block/%s"/>\n', $aXmlTag[3], $aPartTable[0], $aEPT[1], $aPartTable[1], $aEPT[3], $aXmlTag[10], $aEPT[2])
					Next
					$PartTableList &= StringFormat('</%s>', $aXmlTag[9])
					_WriteFile($PartTableListFile, $PartTableList, 1)
					_ClearTabCtrl(6)
					MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_part_table_saved') & @CRLF & $PartTableListFile, 0, $aGuiForm[0])
				Case 2; Show All packages
					_ClearTabCtrl(6)
					GUICtrlSetData($aTb6Ctrl[7], _LngStr('lv_adb_app_list'))
					_GUICtrlListView_SetExtendedListViewStyle($aTb6Ctrl[7], BitOR($LVS_EX_FULLROWSELECT, $LVS_EX_CHECKBOXES))
					GUICtrlSetData($iStBar, _LngStr('msg_load_app_list'))
					Local $sAppList = StringReplace(_ADB('pm list packages -f', 1, 1), 'package:', '')
					$sAppList = StringRegExpReplace($sAppList, '(/*/.*/)(.*.apk)=(.*)', '$2|$3|$1')
					Local $aAppArray = StringSplit($sAppList, @LF, 1), $aAppArgv
					If IsArray($aAppArray) Then
						For $i = 1 To $aAppArray[0]
							If (StringLen($aAppArray[$i]) > 2) Then
								GUICtrlCreateListViewItem($aAppArray[$i], $aTb6Ctrl[7])
								Sleep(20)
							EndIf
						Next
						GUICtrlSetData($iStBar, _LngStr('msg_done'))
					Else
						GUICtrlSetData($iStBar, _LngStr('msg_error_load_app_list'))
					EndIf
				Case 3; Save package list
					Local $sAppListFile = @ScriptDir & '\work\AppList.xml'
					Local $sAppList = StringFormat('<?xml version="1.0" encoding="UTF-8"?>\n<!-- Generated by MStarBinTool-GUI -->\n<%s %s="%s">\n', _
						$aXmlTag[11], $aXmlTag[12], _GUICtrlListView_GetItemCount($aTb6Ctrl[7]))
					For $i = 0 To _GUICtrlListView_GetItemCount($aTb6Ctrl[7]) -1
						Local $aAppList = _GUICtrlListView_GetItemTextArray($aTb6Ctrl[7], $i)
						$sAppList &= StringFormat('\t<%s %s="%s" %s="%s%s"/>\n', $aXmlTag[13], $aXmlTag[14], $aAppList[2], $aImgList[7], $aAppList[3], $aAppList[1])
					Next
					$sAppList &= StringFormat('</%s>', $aXmlTag[11])
					_WriteFile($sAppListFile, $sAppList, 1)
					_ClearTabCtrl(6)
					MsgBox(4160, _LngStr('title_notification'), _LngStr('msg_app_list_saved') & @CRLF & $sAppListFile, 0, $aGuiForm[0])
			EndSwitch
	EndSwitch
EndFunc

Func _SaveSettings($INP_KEY, $INP_VAL)
	IniWrite($aAppCfg[0], $aAppInfo[0], $INP_KEY, '"' & $INP_VAL & '"')
EndFunc

Func _ClearTabCtrl($INP_TAB)
	Switch $INP_TAB
		Case 1
			Local $aClNum[9] = [10, 12, 14, 16, 22, 25, 26, 27, 29]; GUICtrlSetData
			For $ic = 0 To UBound($aClNum) -1
				GUICtrlSetData($aTb1Ctrl[$aClNum[$ic]], '')
			Next
			GUICtrlSendMsg($aTb1Ctrl[18], $CB_SETCURSEL, 1, 0)
			GUICtrlSendMsg($aTb1Ctrl[20], $CB_SETCURSEL, 0, 0)
			_GUICtrlListView_DeleteAllItems($aTb1Ctrl[23])
			_GUICtrlListView_DeleteAllItems($aTb1Ctrl[24])
		Case 2
			Local $aClNum[8] = [10, 12, 14, 16, 22, 25, 26, 27]; GUICtrlSetData
			For $ic = 0 To UBound($aClNum) -1
				GUICtrlSetData($aTb2Ctrl[$aClNum[$ic]], '')
			Next
			GUICtrlSendMsg($aTb2Ctrl[18], $CB_SETCURSEL, 1, 0)
			GUICtrlSendMsg($aTb2Ctrl[20], $CB_SETCURSEL, 0, 0)
			_GUICtrlListView_DeleteAllItems($aTb2Ctrl[23])
			_GUICtrlListView_DeleteAllItems($aTb2Ctrl[24])
		Case 3
			If _GUICtrlListView_GetItemCount($aTb3Ctrl[3]) > 0 Then _GUICtrlListView_DeleteAllItems($aTb3Ctrl[3])
		Case 5
			_GUICtrlListView_DeleteAllItems($aTb5Ctrl[4])
		Case 6
			GUICtrlSetData($aTb6Ctrl[7], '-|-|-')
			_GUICtrlListView_DeleteAllItems($aTb6Ctrl[7])
	EndSwitch
EndFunc

Func _Str2Num($INP_STR)
	Switch $INP_STR
		Case $aYesNo[0], '1'; Yes
			Return 1
		Case $aYesNo[1], '0'; No
			Return 0
		Case Else
			Return Number($INP_STR)
	EndSwitch
EndFunc

Func _ReadInpFile($INP_FILE, $INP_OFFSET = 0, $INP_SIZE = 0, $FMODE = 0); 0=asHEX, 1=asTXT(UTF-8)
	Local $sInpFOpen, $sInpFData
	Switch $FMODE
		Case 0
			$sInpFOpen = FileOpen($INP_FILE, 16)
		Case 1
			$sInpFOpen = FileOpen($INP_FILE, 256)
	EndSwitch
	If ($INP_OFFSET > 0) Then FileSetPos($sInpFOpen, $INP_OFFSET, $FILE_BEGIN)
	If ($INP_SIZE > 0) Then
		$sInpFData = FileRead($sInpFOpen, $INP_SIZE)
	Else
		$sInpFData = FileRead($sInpFOpen)
	EndIf
	FileClose($sInpFOpen)
	Return $sInpFData
EndFunc

Func _GetXmlStr($INP_FDATA, $INP_STR, $SMODE = 0, $RMODE = 0); SMODE: 0=FindBetwStr, 1=FindAfterStr, 2=FindAtribStr; RMODE: 0=String, 1=Array
	Local $aSrchArray[3][2] = [['<' & $INP_STR & '>', '</' & $INP_STR & '>'], ['<' & $INP_STR, '/>'], [$INP_STR & '="', '"']]; FindBetwStr, FindAfterStr, FindAtribStr
	Local $aXmlStrVal = _StringBetween($INP_FDATA, $aSrchArray[$SMODE][0], $aSrchArray[$SMODE][1])
	Switch @error
		Case 0; no error
			Switch $RMODE
				Case 0; return string
					Return StringStripWS($aXmlStrVal[0], 3)
				Case 1; return array
					Return $aXmlStrVal
			EndSwitch
		Case Else
			Return ''
	EndSwitch
EndFunc

Func _HexPrefCheck($INP_HEX, $FMODE = 0); 0=PrefixOFF, 1=PrefixON
	Local $PrefResult = $INP_HEX
	If StringLeft($PrefResult, 2) = '0x' Then $PrefResult = StringTrimLeft($PrefResult, 2)
	While StringLeft($PrefResult, 1) = '0'
		$PrefResult = StringTrimLeft($PrefResult, 1)
	WEnd
	If ($FMODE = 1) Then $PrefResult ='0x' & $PrefResult
	Return $PrefResult
EndFunc

Func _ReverseBytes($INP_HEX)
	Local $RevResult = $INP_HEX
	If StringLeft($RevResult, 2) = '0x' Then $RevResult = StringTrimLeft($RevResult, 2)
	If StringLen($RevResult) = 8 Then
		$RevResult = '0x' & StringMid($RevResult, 7, 2) & StringMid($RevResult, 5, 2) & StringMid($RevResult, 3, 2) & StringMid($RevResult, 1, 2)
	EndIf
	Return $RevResult
EndFunc

Func _WriteFile($INP_FILE, $INP_DATA, $FMODE = 0); 1=asTXT(UTF-8), 0=asHEX
	Local $InpOpen
	Switch $FMODE
		Case 0
			If FileExists($INP_FILE) Then
				$InpOpen = FileOpen($INP_FILE, 17); write to end of file
			Else
				$InpOpen = FileOpen($INP_FILE, 26); create and write file
			EndIf
		Case 1
			If FileExists($INP_FILE) Then
				$InpOpen = FileOpen($INP_FILE, 257); write to end of file
			Else
				$InpOpen = FileOpen($INP_FILE, 266); create and write file
			EndIf
	EndSwitch
	FileWrite($InpOpen, $INP_DATA)
	FileClose($InpOpen)
	Sleep(200)
EndFunc

Func _AlignFile($INP_FILE, $INP_ALSIZE = 4096)
	Local $InpOpen, $NumBytes, $TmpData
	Switch $INP_ALSIZE
		Case 4096
			$NumBytes = $INP_ALSIZE - Mod(FileGetSize($INP_FILE), $INP_ALSIZE)
			If $NumBytes > 0 Then
				$InpOpen = FileOpen($INP_FILE, 17); write to end of file
				For $i8 = 1 To $NumBytes
					FileWrite($InpOpen, Chr(12))
				Next
				FileClose($InpOpen)
			EndIf
		Case 16384
			$NumBytes = $INP_ALSIZE - FileGetSize($INP_FILE)
			$InpOpen = FileOpen($INP_FILE, 17); write to end of file
			For $i8 = 1 To $NumBytes
				FileWrite($InpOpen, Chr(12))
			Next
			FileClose($InpOpen)
		Case Else
			If ($INP_ALSIZE > 1024) Then
				$InpOpen = FileOpen($INP_FILE, 16)
				$TmpData = FileRead($InpOpen, 1024)
				FileClose($InpOpen)
				Sleep(200)
				$InpOpen = FileOpen($INP_FILE, 17); write to end of file
				$NumBytes = Ceiling($INP_ALSIZE/1024)
				For $i8 = 1 To $NumBytes - 1
					FileWrite($InpOpen, $TmpData)
					$INP_ALSIZE -= 1024
				Next
			EndIf
			For $i8 = 1 To $INP_ALSIZE
				FileWrite($InpOpen, Chr(0))
			Next
			FileClose($InpOpen)
	EndSwitch
	Sleep(200)
EndFunc

Func _CopyPart($INP_FILE, $OUT_FILE, $INP_OFFSET = 0, $INP_SIZE = 0, $BufSize = 16777216)
	Local $InpOpen, $OutOpen, $NChunks, $InpFSize
	$InpOpen = FileOpen($INP_FILE, 16)
	If FileExists($OUT_FILE) Then
		$OutOpen = FileOpen($OUT_FILE, 17); write to end of file
	Else
		$OutOpen = FileOpen($OUT_FILE, 26); create and write file
	EndIf
	If ($INP_SIZE = 0) Then
		If ($INP_OFFSET > 0) Then FileSetPos($InpOpen, $INP_OFFSET, $FILE_BEGIN)
		$InpFSize = FileGetSize($INP_FILE)
		If ($InpFSize > $BufSize) Then
			$NChunks = Ceiling($InpFSize/$BufSize)
			For $i9 = 1 To $NChunks - 1
				FileWrite($OutOpen, FileRead($InpOpen, $BufSize))
			Next
		EndIf
		FileWrite($OutOpen, FileRead($InpOpen))
	Else
		If ($INP_OFFSET > 0) Then FileSetPos($InpOpen, $INP_OFFSET, $FILE_BEGIN)
		If ($INP_SIZE > $BufSize) Then
			$NChunks = Ceiling($INP_SIZE/$BufSize)
			For $i9 = 1 To $NChunks - 1
				FileWrite($OutOpen, FileRead($InpOpen, $BufSize))
				$INP_SIZE -= $BufSize
			Next
		EndIf
		FileWrite($OutOpen, FileRead($InpOpen, $INP_SIZE))
	EndIf
	FileClose($InpOpen)
	FileClose($OutOpen)
	Sleep(200)
EndFunc

Func _ConvSparse($INP_FILE, $OUT_FILE, $FMODE = 0); 0=Simg2Img\MergeSimg2Img, 1=Img2Simg, 2=SplitSimg
	Switch $FMODE
		Case 0
			RunWait(StringFormat('%s /c %ssparse2img.exe %s %s', @ComSpec, $aAppCfg[2], $INP_FILE, $OUT_FILE), @ScriptDir, @SW_HIDE)
		Case 1
			RunWait(StringFormat('%s /c %simg2simg.exe %s %s', @ComSpec, $aAppCfg[2], $INP_FILE, $OUT_FILE), @ScriptDir, @SW_HIDE)
		Case 2
			RunWait(StringFormat('%s /c %ssimg2simg.exe %s %s 157286400', @ComSpec, $aAppCfg[2], $INP_FILE, $OUT_FILE), @ScriptDir, @SW_HIDE)
	EndSwitch
	Sleep(300)
EndFunc

Func _CRC32Calc($INP_FILE)
	If FileExists($INP_FILE) Then
		Local $a_hCall, $hFile, $hFileMappingObject, $pFile, $iBufferSize, $a_iCall
		$a_hCall = DllCall('kernel32.dll', 'hwnd', 'CreateFileW', 'wstr', $INP_FILE, 'dword', 0x80000000, 'dword', 3, 'ptr', 0, 'dword', 3, 'dword', 0, 'ptr', 0)
		$hFile = $a_hCall[0]
		$a_hCall = DllCall('kernel32.dll', 'ptr', 'CreateFileMappingW', 'hwnd', $hFile, 'dword', 0, 'dword', 2, 'dword', 0, 'dword', 0, 'ptr', 0)
		If @error Or Not $a_hCall[0] Then DllCall('kernel32.dll', 'int', 'CloseHandle', 'hwnd', $hFile)
		DllCall('kernel32.dll', 'int', 'CloseHandle', 'hwnd', $hFile)
		$hFileMappingObject = $a_hCall[0]
		$a_hCall = DllCall('kernel32.dll', 'ptr', 'MapViewOfFile', 'hwnd', $hFileMappingObject, 'dword', 4, 'dword', 0, 'dword', 0, 'dword', 0)
		If @error Or Not $a_hCall[0] Then DllCall('kernel32.dll', 'int', 'CloseHandle', 'hwnd', $hFileMappingObject)
		$pFile = $a_hCall[0]
		$iBufferSize = FileGetSize($INP_FILE)
		$a_iCall = DllCall('ntdll.dll', 'dword', 'RtlComputeCrc32', 'dword', 0, 'ptr', $pFile, 'int', $iBufferSize)
		If @error Or Not $a_iCall[0] Then
			DllCall('kernel32.dll', 'int', 'UnmapViewOfFile', 'ptr', $pFile)
			DllCall('kernel32.dll', 'int', 'CloseHandle', 'hwnd', $hFileMappingObject)
		EndIf
		DllCall('kernel32.dll', 'int', 'UnmapViewOfFile', 'ptr', $pFile)
		DllCall('kernel32.dll', 'int', 'CloseHandle', 'hwnd', $hFileMappingObject)
		Return StringMid(Binary($a_iCall[0]), 1, 10)
	Else
		Return Binary('0x00000000')
	EndIf
EndFunc

Func _EPOCH_decrypt($INP_INT_TIME)
    Local $iDayToAdd = Int($INP_INT_TIME / 86400)
    Local $iTimeVal = Mod($INP_INT_TIME, 86400)
    If $iTimeVal < 0 Then
        $iDayToAdd -= 1
        $iTimeVal += 86400
    EndIf
    Local $i_wFactor = Int((573371.75 + $iDayToAdd) / 36524.25)
    Local $i_xFactor = Int($i_wFactor / 4)
    Local $i_bFactor = 2442113 + $iDayToAdd + $i_wFactor - $i_xFactor
    Local $i_cFactor = Int(($i_bFactor - 122.1) / 365.25)
    Local $i_dFactor = Int(365.25 * $i_cFactor)
    Local $i_eFactor = Int(($i_bFactor - $i_dFactor) / 30.6001)
    Local $aDatePart[3]
    $aDatePart[2] = $i_bFactor - $i_dFactor - Int(30.6001 * $i_eFactor)
    $aDatePart[1] = $i_eFactor - 1 - 12 * ($i_eFactor - 2 > 11)
    $aDatePart[0] = $i_cFactor - 4716 + ($aDatePart[1] < 3)
    Local $aTimePart[3]
    $aTimePart[0] = Int($iTimeVal / 3600)
    $iTimeVal = Mod($iTimeVal, 3600)
    $aTimePart[1] = Int($iTimeVal / 60)
    $aTimePart[2] = Mod($iTimeVal, 60)
    Return StringFormat('%.2d.%.2d.%.2d %.2d:%.2d:%.2d', $aDatePart[2], $aDatePart[1], $aDatePart[0], $aTimePart[0], $aTimePart[1], $aTimePart[2])
EndFunc

Func _LzoTool($INP_FILE, $OUT_FILE, $FMODE = 0); 1=LZO, 0=unLZO
	Switch $FMODE
		Case 0
			RunWait(StringFormat('%s /c %slzop.exe -d -q -o %s %s', @ComSpec, $aAppCfg[2], $OUT_FILE, $INP_FILE), @ScriptDir, @SW_HIDE)
		Case 1
			RunWait(StringFormat('%s /c %slzop.exe -1 -q -o %s %s', @ComSpec, $aAppCfg[2], $OUT_FILE, $INP_FILE), @ScriptDir, @SW_HIDE)
	EndSwitch
	Sleep(200)
EndFunc

Func _AesCrypt2($INP_FILE, $OUT_FILE, $AES_KEY, $FMODE); 0=Encrypt, 1=Decrypt
	RunWait(StringFormat('%s /c %saescrypt2.exe %s %s %s %s', @ComSpec, $aAppCfg[2], $FMODE, $INP_FILE, $OUT_FILE, $AES_KEY), @ScriptDir, @SW_HIDE)
	Sleep(200)
EndFunc

Func _AlignImg($INP_FILE)
	RunWait(StringFormat('%s /c %salignment.exe %s', @ComSpec, $aAppCfg[2], $INP_FILE), @ScriptDir, @SW_HIDE)
	Sleep(200)
EndFunc

Func _GenSignImg($INP_FILE, $OUT_FILE, $RSA_PRIV_KEY, $RSA_PUBL_KEY)
	Local $pid_txt = ''
	Local $pid = Run(StringFormat('%s /c %sSubSecureInfoGen.exe %s %s %s %s 0 8 1 2097152 0 bin\\win32\\', @ComSpec, $aAppCfg[2], $OUT_FILE, $INP_FILE, $RSA_PRIV_KEY, $RSA_PUBL_KEY), @ScriptDir, @SW_HIDE, $STDIN_CHILD + $STDOUT_CHILD)
	While 1
		$pid_txt &= StdoutRead($pid)
		If @error Then ExitLoop
		If $pid_txt <> '' Then
			StdInWrite($pid, @CRLF)
			Sleep(1000)
		EndIf
	WEnd
	ProcessWaitClose($pid)
	Sleep(200)
	FileDelete(@ScriptDir & '\hash.bin')
EndFunc

Func _AndImgTool($INP_FILE, $INP_DIR, $FMODE = 0); 0-unpack_rdisk, 1-pack_rdisk
	Switch $FMODE
		Case 0
			RunWait(StringFormat('%s /c %sAndImgTool.exe %s %s', @ComSpec, $aAppCfg[2], $INP_FILE, $INP_DIR), @ScriptDir, @SW_HIDE)
		Case 1
			RunWait(StringFormat('%sAndImgTool.exe %s %s', $aAppCfg[2], $INP_DIR, $INP_FILE), @ScriptDir, @SW_HIDE)
	EndSwitch
	Sleep(300)
EndFunc

Func _MkUbootImage($INP_DIR, $INP_LA, $INP_EP, $INP_NAME, $OUT_IMG)
	Local $pid_txt = ''
	Local $pid = Run(StringFormat('%s /c %smkimage.exe -A arm -O linux -T multi -C none -a %s -e %s -d kernel.img:ramdisk.img -n "%s" ..\%s', @ComSpec, $aAppCfg[2], $INP_LA, $INP_EP, $INP_NAME, $OUT_IMG), $INP_DIR, @SW_HIDE, $STDOUT_CHILD)
	While 1
		$pid_txt &= StdoutRead($pid)
		If @error Then ExitLoop
	WEnd
	ProcessWaitClose($pid)
	Sleep(200)
	Return $pid_txt
EndFunc

Func _ADB($INP_CMD, $FMODE = 0, $RMODE = 0); FMODE (0=Adb,1=Shell,2=SU,3=BBox,4=GetProp), ReturnMODE (0=SingleLine,1=MultiLine)
	Local $PrId, $PrId_Txt = '', $Cmd_Line
	Switch $FMODE
		Case 0; ADB
			$Cmd_Line = StringFormat('%s /c %sadb.exe %s', @ComSpec, $aAppCfg[2], $INP_CMD)
		Case 1; Shell
			$Cmd_Line = StringFormat('%s /c %sadb.exe shell %s', @ComSpec, $aAppCfg[2], $INP_CMD)
		Case 2; SuperSU
			$Cmd_Line = StringFormat('%s /c %sadb.exe shell su -c "%s"', @ComSpec, $aAppCfg[2], $INP_CMD)
		Case 3; BusyBox
			$Cmd_Line = StringFormat('%s /c %sadb.exe shell busybox %s', @ComSpec, $aAppCfg[2], $INP_CMD)
		Case 4; GetProp
			$Cmd_Line = StringFormat('%s /c %sadb.exe shell getprop %s', @ComSpec, $aAppCfg[2], $INP_CMD)
	EndSwitch
	$PrId = Run($Cmd_Line, @ScriptDir, @SW_HIDE, $STDERR_CHILD + $STDOUT_CHILD)
	While 1
		$PrId_Txt &= StdoutRead($PrId)
		If @error Then ExitLoop
	WEnd
	ProcessWaitClose($PrId)
	Switch $RMODE
		Case 0; SingleLine
			Return StringStripCR(StringTrimRight($PrId_Txt, StringLen(@CRLF)))
		Case 1; MultiLine
			Return StringStripCR($PrId_Txt)
	EndSwitch
EndFunc

Func _ADB_SendFile($INP_FILE, $OUT_FILE, $FMODE = 0); 0=Send_file_to_TV, 1=Send_file_to_PC
	Switch $FMODE
		Case 0; Send file from PC to TV
			Return _ADB('push "' & $INP_FILE & '" "' & $OUT_FILE & '"')
		Case 1; Send file from TV to PC
			Return _ADB('pull "' & $INP_FILE & '" "' & $OUT_FILE & '"')
	EndSwitch
EndFunc

Func _ADB_Reboot($FMODE = 0)
	Switch $FMODE
		Case 0
			_ADB('reboot')
		Case 1
			_ADB('reboot bootloader')
		Case 2
			_ADB('reboot recovery')
	EndSwitch
EndFunc

#Region LOOP
While 1
	Sleep(1000)
WEnd
#EndRegion