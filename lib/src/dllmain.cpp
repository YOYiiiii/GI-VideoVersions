#include <filesystem>
#include <Windows.h>

#include "pipe\PipeClient.h"
#include "PipeMessage.h"
#include "utils.h"
#include "exports.h"

static HANDLE hThread;
static int heartbeat = 1;
static bool flag = true;

static DWORD WINAPI OnProcAttach(LPVOID lpThreadParameter)
{
	char buffer[MAX_PATH];
	GetModuleFileName(0, buffer, MAX_PATH);
	std::filesystem::path filename(buffer);
	std::string fileBytes = ReadFileAllBytes(filename.string().c_str());
	if (_stricmp(filename.filename().string().c_str(), GetGenshinProcName()) ||
		_stricmp(MD5Hash(fileBytes.data(), fileBytes.size()).c_str(), GetGenshinHash()))
		FreeLibraryAndExitThread((HMODULE)lpThreadParameter, 0);

	PipeClient client(GetPipeName());
	if (!client.WaitForConnect(3000))
		FreeLibraryAndExitThread((HMODULE)lpThreadParameter, 0);
	while (flag)
	{
		DWORD msg, len;
		if (!client.Read((char*)&msg, 4, (int*)&len) && len == 4 ||
			!PipeMessage::HandleMessage(client, msg))
			break;
	}
	client.Disconnect();
	FreeLibraryAndExitThread((HMODULE)lpThreadParameter, 0);
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD dwReason, LPVOID lpReserved)
{
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		hThread = CreateThread(0, 0, &OnProcAttach, hModule, 0, 0);
		break;

	case DLL_PROCESS_DETACH:
		flag = false;
		if (WaitForSingleObject(hThread, 3000) == WAIT_TIMEOUT)
			return FALSE;
		CloseHandle(hThread);
		break;
	}
	return TRUE;
}
