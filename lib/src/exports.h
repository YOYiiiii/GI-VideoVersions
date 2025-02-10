#pragma once
#include <Windows.h>

extern "C" __declspec(dllexport)
inline const char* GetGenshinProcName()
{
	return "YuanShen.exe";
}

extern "C" __declspec(dllexport)
inline const char* GetGenshinVersion()
{
	return "CNRel_5.4.0";
}

extern "C" __declspec(dllexport)
inline const char* GetGenshinHash()
{
	return "5a57b3c787190ad012ab53f1fb79d65d";
}

extern "C" __declspec(dllexport)
inline const char* GetPipeName()
{
	return "GenshinVideoVersions";
}
