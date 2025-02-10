#ifndef DO_APP_FUNC
#define DO_APP_FUNC(offset, return_type, name, params)
#endif
#ifndef DO_STATIC_VAR
#define DO_STATIC_VAR(offset, type, name)
#endif
#ifndef DO_STATIC_REF
#define DO_STATIC_REF(offset, type, name)
#endif

DO_APP_FUNC(0x467D40, Il2CppArray*, il2cpp_array_new, (Il2CppClass* elementTypeInfo, il2cpp_array_size_t length));
DO_APP_FUNC(0x6AEC0, void, il2cpp_runtime_class_init, (Il2CppClass* klass));

DO_APP_FUNC(0x1012F470, System_Text_Encoding*, System_Text_Encoding_get_Default, ());
DO_APP_FUNC(0x1012FF80, Il2CppArray*, System_Text_Encoding_GetBytes, (System_Text_Encoding* __this, Il2CppString* s));

DO_APP_FUNC(0x10781350, UnityEngine_AssetBundle*, UnityEngine_AssetBundle_LoadFromFile, (Il2CppString* path, uint32_t crc, uint64_t offset, UnityEngine_ArchiveFileFormat format));
DO_APP_FUNC(0x10781580, void, UnityEngine_AssetBundle_Unload, (UnityEngine_AssetBundle* __this, bool unloadAllLoadedObjects));
DO_APP_FUNC(0x10874A70, int, UnityEngine_MiHoYoBinData_BinFileLengthInBundleByHash, (UnityEngine_AssetBundle* bundle, int path));
DO_APP_FUNC(0x10874A80, int, UnityEngine_MiHoYoBinData_ReadBinFileInBundleByHash, (UnityEngine_AssetBundle* bundle, int path, Il2CppArray* datas, int offset));

DO_APP_FUNC(0x107F84F0, Il2CppString*, Newtonsoft_Json_JsonConvert_SerializeObject, (Il2CppObject* value, Newtonsoft_Json_Formatting formatting));

DO_STATIC_VAR(0x41F6AB8, Il2CppObject**, s_StaticFieldReferences);
DO_STATIC_VAR(0x3F500D0, Il2CppClass*, System_Byte__Class);
DO_STATIC_VAR(0x42CBC10, Il2CppClass*, MoleMole_VideoVersions__Class);

DO_STATIC_REF(0x3B568, Il2CppObject*, MoleMole_VideoVersions__tagKeys);
DO_STATIC_REF(0x3B560, Il2CppString*, MoleMole_VideoVersions_VersionBlkPath);
