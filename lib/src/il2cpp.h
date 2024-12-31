#pragma once
#include <string>

namespace il2cpp
{
#include "il2cpp\il2cpp-types.h"
#define DO_APP_FUNC(o, r, n, p) extern r (*n) p
#define DO_STATIC_VAR(o, r, n) extern r (*n)
#define DO_STATIC_REF(o, r, n) extern r (*n)
#include "il2cpp\il2cpp-offsets.h"
#undef DO_APP_FUNC
#undef DO_STATIC_VAR
#undef DO_STATIC_REF
}

void DumpVersionList(ULONG_PTR Parameter);
void DumpVersionTagKeys(ULONG_PTR Parameter);
std::string DumpInMainThread(PAPCFUNC callback);
