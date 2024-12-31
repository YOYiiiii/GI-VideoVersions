#pragma once

extern "C" __declspec(dllexport)
inline const char* GetGenshinProcName()
{
	return "YuanShen.exe";
}

extern "C" __declspec(dllexport)
inline const char* GetGenshinVersion()
{
	return "CNRel_5.2.0";
}

extern "C" __declspec(dllexport)
inline const char* GetGenshinHash()
{
	return "98ce2d5a9d409074fb59b4ed8479f0b9";
}

extern "C" __declspec(dllexport)
inline const char* GetPipeName()
{
	return "GenshinVideoVersions";
}
