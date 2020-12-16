# -*- mode: python ; coding: utf-8 -*-

block_cipher = None


a = Analysis(['PolyLine_Extraction.py'],
             pathex=['E:\\Assignment\\3_2\\SW\\2020-2-OSSP1-AMBB-7\\PolyLine_Link\\PolyLine_Link\\py'],
             binaries=[],
             datas=[],
             hiddenimports=['xlrd'],
             hookspath=[],
             runtime_hooks=[],
             excludes=[],
             win_no_prefer_redirects=False,
             win_private_assemblies=False,
             cipher=block_cipher,
             noarchive=False)
pyz = PYZ(a.pure, a.zipped_data,
             cipher=block_cipher)
exe = EXE(pyz,
          a.scripts,
          a.binaries,
          a.zipfiles,
          a.datas,
          [],
          name='PolyLine_Extraction',
          debug=False,
          bootloader_ignore_signals=False,
          strip=False,
          upx=True,
          upx_exclude=[],
          runtime_tmpdir=None,
          console=True )
