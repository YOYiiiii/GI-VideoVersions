#include "PipeClient.h"

bool PipeClient::ConnectTo(const char* pipeName)
{
	if (IsConnected())
		return false;

	m_pipeName = R"(\\.\pipe\)";
	m_pipeName += pipeName;
	return true;
}

bool PipeClient::WaitForConnect(int timeout)
{
	if (IsConnected())
		return false;

	if (!WaitNamedPipe(m_pipeName.c_str(), timeout))
		return false;

	m_hPipe = CreateFile(m_pipeName.c_str(),
		GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, 0, 0);
	return IsConnected();
}

bool PipeClient::Disconnect()
{
	if (IsConnected())
	{
		CloseHandle(m_hPipe);
		m_hPipe = INVALID_HANDLE_VALUE;
	}
	return true;
}
