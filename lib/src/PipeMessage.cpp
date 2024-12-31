#include "PipeMessage.h"
#include "il2cpp.h"

bool PipeMessage::HandleMessage(PipeClient& client, DWORD msg)
{
	DWORD rsp = ~msg;
	client.Write((char*)&rsp, 4, 0);
	switch (msg)
	{
	case MsgListDump:
		return HandleListDump(client);

	case MsgKeyDump:
		return HandleKeyDump(client);
	}
}

bool PipeMessage::HandleListDump(PipeClient& client)
{
	auto str = DumpInMainThread(DumpVersionList);
	int size = str.size();
	return client.Write((char*)&size, 4, 0)
		&& client.Write(str.c_str(), size, 0);
}

bool PipeMessage::HandleKeyDump(PipeClient& client)
{
	auto str = DumpInMainThread(DumpVersionTagKeys);
	int size = str.size();
	return client.Write((char*)&size, 4, 0)
		&& client.Write(str.c_str(), size, 0);
}
