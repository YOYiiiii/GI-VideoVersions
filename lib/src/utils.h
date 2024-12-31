#pragma once
#include <string>
#include <Windows.h>

#define NT_SUCCESS(x) ((NTSTATUS)(x) >= 0)

std::string MD5Hash(const char* data, size_t length);

std::string ReadFileAllBytes(const char* fileName);
DWORD GetProcessMainThreadId(DWORD processId);
