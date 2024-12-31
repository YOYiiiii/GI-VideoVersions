#pragma once

extern "C" __declspec(dllexport)
inline const char* GetGenshinProcName()
{
	return "YuanShen.exe";
}

extern "C" __declspec(dllexport)
inline const char* GetGenshinVersion()
{
	return "CNRel_5.3.0";
}

extern "C" __declspec(dllexport)
inline const char* GetGenshinHash()
{
	return "eee020bb5847f3ffc4dc464577567f3b";
}

extern "C" __declspec(dllexport)
inline const char* GetPipeName()
{
	return "GenshinVideoVersions";
}
