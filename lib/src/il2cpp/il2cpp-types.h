#pragma once
#include <cstdint>

typedef struct Il2CppClass Il2CppClass;

typedef struct Il2CppObject
{
	Il2CppClass* klass;
	void* monitor;
};

typedef struct Il2CppString : Il2CppObject
{
	const int length;
	const char chars[];
};

typedef uintptr_t il2cpp_array_size_t;
typedef struct Il2CppArrayBounds Il2CppArrayBounds;
typedef struct Il2CppArray : Il2CppObject
{
	Il2CppArrayBounds* bounds;
	il2cpp_array_size_t max_length;
	char vector[];
};

typedef struct System_Text_Encoding System_Text_Encoding;

typedef struct UnityEngine_AssetBundle UnityEngine_AssetBundle;

typedef enum class UnityEngine_ArchiveFileFormat : int
{
	Unknown = 0,
	Unity_Unencrypted = 1,
	MiHoYoFile_Encrypted = 2,
	Blob_Unencrypted = 3,
	MiHoYoFile_Unencrypted = 4
};

typedef enum class Newtonsoft_Json_Formatting : int
{
	None = 0,
	Indented = 1
};
