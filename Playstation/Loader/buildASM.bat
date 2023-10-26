set indir=%1
set outdir=%2

for %%f in (%indir%\*.s) do (
	orbis-clang -m64 -nodefaultlibs -nostdlib -c -o %outdir%\%%~nf.o %%~nf.s
)

pause