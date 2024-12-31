#include <sstream>
#include <iomanip>

#include <Windows.h>
#include <TlHelp32.h>
#include "utils.h"

#pragma comment(lib, "bcrypt.lib")

std::string MD5Hash(const char* data, size_t length)
{
	std::string result;
	BCRYPT_ALG_HANDLE hashAlg;
	if (!NT_SUCCESS(BCryptOpenAlgorithmProvider(&hashAlg, BCRYPT_MD5_ALGORITHM, 0, 0)))
		return result;

	byte hash[16]{};
	if (NT_SUCCESS(BCryptHash(hashAlg, 0, 0, (byte*)data, length, hash, 16)))
	{
		std::stringstream ss;
		ss << std::hex << std::setfill('0');
		for (int i = 0; i < 16; i++)
			ss << std::setw(2) << (int)hash[i];
		result = ss.str();
	}

	BCryptCloseAlgorithmProvider(hashAlg, 0);
	return result;
}

std::string ReadFileAllBytes(const char* fileName)
{
	std::string result;
	HANDLE hFile = CreateFile(
		fileName, GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, 0, 0);
	if (!hFile) return result;

	DWORD sizeh;
	DWORD sizel = GetFileSize(hFile, &sizeh);
	size_t size = ((size_t)sizeh << 32) | sizel;

	result.resize(size);
	if (!ReadFile(hFile, result.data(), size, 0, 0))
		result.clear();
	CloseHandle(hFile);
	return result;
}

DWORD GetProcessMainThreadId(DWORD processId)
{
	HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, processId);
	if (hSnapshot == INVALID_HANDLE_VALUE)
		return 0;

	THREADENTRY32 entry{ .dwSize = sizeof(THREADENTRY32) };
	if (Thread32First(hSnapshot,&entry))
		do {
			if (entry.th32OwnerProcessID == processId)
			{
				CloseHandle(hSnapshot);
				return entry.th32ThreadID;
			}
		} while (Thread32Next(hSnapshot, &entry));

	CloseHandle(hSnapshot);
	return 0;
}
