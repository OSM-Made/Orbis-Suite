#pragma once
#include "stdafx.h"
#include "sqlite3.h"

class AppDatabase
{
public:
	enum VisibilityType
	{
		VT_NONE,
		VT_VISIBLE,
		VT_INVISIBLE,
	};

	enum DispLocation
	{
		DL_NONE = 0,
		DL_CONTENTAREA = 1,
		DL_TV_VIDEO = 2,
		DL_LIBRARY = 4,
		DL_UNK = 5,
	};

	struct AppInfo
	{
		char TitleId[10];
		char ContentId[100];
		char TitleName[200];
		char MetaDataPath[100];
		char LastAccessTime[100];
		int Visible;
		int SortPriority;
		int DispLocation;
		bool CanRemove;
		char Category[10];
		int ContentSize;
		char InstallDate[100];
		char UICategory[10];
	};

	static int GetVersion();
	static bool GetApps(std::vector<AppInfo>& Apps);
	static bool GetAppInfoString(const char* TitleId, char* Out, size_t OutSize, const char* Key);
	static bool SetVisibility(const char* TitleId, VisibilityType Visibility);
	static VisibilityType GetVisibility(const char* TitleId);

	AppDatabase();
	~AppDatabase();

private:
	static sqlite3* OpenDatabase();
	static const char* GetText(sqlite3_stmt* stmt, int column);
};