#pragma once
#include "pipe\PipeClient.h"

class PipeMessage
{
public:
	static bool HandleMessage(PipeClient& client, DWORD msg);

private:
	enum
	{
		MsgListDump = 0xAE0DFA5B,
		MsgKeyDump = 0x7367339B
	};

	static bool HandleListDump(PipeClient& client);
	static bool HandleKeyDump(PipeClient& client);
};
