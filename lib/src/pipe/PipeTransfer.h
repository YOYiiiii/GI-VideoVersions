#pragma once
#include <Windows.h>

class PipeTransfer
{
public:
	inline constexpr PipeTransfer()
		: m_hPipe(INVALID_HANDLE_VALUE) { }

	bool Read(char* buffer, int length, int* bytesRead) const;
	bool Write(const char* buffer, int length, int* bytesWritten) const;
	bool Peek(char* buffer, int length, int* bytesRead) const;

	int ReadByte() const;
	void WriteByte(char byte) const;
	int PeekByte() const;

	constexpr bool IsConnected() const;
	virtual bool Disconnect() = 0;

protected:
	HANDLE m_hPipe;

private:
	PipeTransfer(const PipeTransfer&) = delete;
	PipeTransfer(PipeTransfer&&) = delete;
	PipeTransfer& operator=(const PipeTransfer&) = delete;
	PipeTransfer& operator=(PipeTransfer&&) = delete;
};
