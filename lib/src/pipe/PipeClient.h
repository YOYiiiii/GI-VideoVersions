#pragma once
#include <string>
#include "PipeTransfer.h"

class PipeClient : public PipeTransfer
{
public:
	inline PipeClient() = default;
	inline PipeClient(const char* pipeName) { ConnectTo(pipeName); }
	inline ~PipeClient() { Disconnect(); }

	bool ConnectTo(const char* pipeName);
	bool WaitForConnect(int timeout);
	bool Disconnect() override;

private:
	std::string m_pipeName;
};
