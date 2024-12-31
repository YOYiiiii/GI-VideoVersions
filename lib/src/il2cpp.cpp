#include <string>
#include <filesystem>
#include <Windows.h>

#include "il2cpp.h"
#include "utils.h"
#include "export.h"

using namespace il2cpp;

char* BaseAddress = reinterpret_cast<char*>(GetModuleHandle(0));
namespace il2cpp
{
#define DO_APP_FUNC(o, r, n, p) r (*n) p = reinterpret_cast<r(*)p>(BaseAddress + o)
#define DO_STATIC_VAR(o, r, n) r (*n) = reinterpret_cast<r*>(BaseAddress + o)
#define DO_STATIC_REF(o, r, n) r (*n) = reinterpret_cast<r*>(reinterpret_cast<char*>(*s_StaticFieldReferences) + o)
#include "il2cpp\il2cpp-offsets.h"
#undef DO_APP_FUNC
#undef DO_STATIC_VAR
#undef DO_STATIC_REF
}

void DumpVersionList(ULONG_PTR Parameter)
{
	if (!*MoleMole_VideoVersions_VersionBlkPath)
		il2cpp_runtime_class_init(*MoleMole_VideoVersions__Class);

	auto bundle = UnityEngine_AssetBundle_LoadFromFile(
		*MoleMole_VideoVersions_VersionBlkPath, 0, 0,
		UnityEngine_ArchiveFileFormat::MiHoYoFile_Encrypted);
	if (!bundle)
	{
		SetEvent(*(HANDLE*)Parameter);
		return;
	}

	size_t size = UnityEngine_MiHoYoBinData_BinFileLengthInBundleByHash(bundle, 0xBF0949CB);
	if (size <= 0)
	{
		SetEvent(*(HANDLE*)Parameter);
		return;
	}

	auto array = il2cpp_array_new(*System_Byte__Class, size);
	UnityEngine_MiHoYoBinData_ReadBinFileInBundleByHash(bundle, 0xBF0949CB, array, 0);
	UnityEngine_AssetBundle_Unload(bundle, true);

	((Il2CppArray**)Parameter)[1] = array;
	SetEvent(*(HANDLE*)Parameter);
}

void DumpVersionTagKeys(ULONG_PTR Parameter)
{
	if (!*MoleMole_VideoVersions__tagKeys)
	{
		SetEvent(*(HANDLE*)Parameter);
		return;
	}

	auto json = Newtonsoft_Json_JsonConvert_SerializeObject(
		*MoleMole_VideoVersions__tagKeys, Newtonsoft_Json_Formatting::None);
	
	auto bytes = System_Text_Encoding_GetBytes(
		System_Text_Encoding_get_Default(), json);

	((Il2CppArray**)Parameter)[1] = bytes;
	SetEvent(*(HANDLE*)Parameter);
}

std::string DumpInMainThread(PAPCFUNC callback)
{
	DWORD tid = GetProcessMainThreadId(GetCurrentProcessId());
	HANDLE hThread = OpenThread(THREAD_SET_CONTEXT, FALSE, tid);
	if (!hThread) return {};

	HANDLE hEvent = CreateEventA(0, FALSE, FALSE, 0);
	if (!hEvent) return {};

	void* param[] = { hEvent, 0 };
	QueueUserAPC(callback, hThread, (ULONG_PTR)&param);
	WaitForSingleObject(hEvent, INFINITE);
	CloseHandle(hEvent);
	CloseHandle(hThread);

	auto array = (Il2CppArray*)param[1];
	if (!array) return {};
	return std::string(array->vector, array->max_length);
}
