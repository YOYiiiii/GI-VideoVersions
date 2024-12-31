#include "PipeTransfer.h"

bool PipeTransfer::Read(char* buffer, int length, int* bytesRead) const
{
	return IsConnected() && ReadFile(m_hPipe,
		buffer, length, (LPDWORD)bytesRead, 0);
}

bool PipeTransfer::Write(const char* buffer, int length, int* bytesWritten) const
{
	return IsConnected() && WriteFile(m_hPipe,
		buffer, length, (LPDWORD)bytesWritten, 0);
}

bool PipeTransfer::Peek(char* buffer, int length, int* bytesRead) const
{
	int TotalBytesAvail, BytesLeftThisMessage;
	return IsConnected() && PeekNamedPipe(m_hPipe, buffer, length,
		(LPDWORD)bytesRead, (LPDWORD)&TotalBytesAvail, (LPDWORD)&BytesLeftThisMessage);
}

int PipeTransfer::ReadByte() const
{
	int buffer;
	return Read((char*)&buffer, 1, 0)
		? buffer : -1;
}

void PipeTransfer::WriteByte(char byte) const
{
	Write((char*)&byte, 1, 0);
}

int PipeTransfer::PeekByte() const
{
	int buffer;
	return Peek((char*)&buffer, 1, 0)
		? buffer : -1;
}

constexpr bool PipeTransfer::IsConnected() const
{
	return m_hPipe != INVALID_HANDLE_VALUE;
}
