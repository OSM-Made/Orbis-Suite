#include "stdafx.h"
#include "AppDatabase.h"
#include "sqlite3.h"
#include <UserServiceExt.h>

sqlite3* AppDatabase::OpenDatabase()
{
	sqlite3* db;

	auto res = sqlite3_open("/system_data/priv/mms/app.db", &db);
	if (res != SQLITE_OK)
	{
		Logger::Error("OpenDatabase(): Failed because %s\n", sqlite3_errmsg(db));
		return nullptr;
	}
	else
	{
		return db;
	}
}

const char* AppDatabase::GetText(sqlite3_stmt* stmt, int column)
{
	auto res = sqlite3_column_text(stmt, column);
	if (res != 0)
	{
		return (const char*)res;
	}

	return "";
}

int AppDatabase::GetVersion()
{
	int result = -1;
	auto db = OpenDatabase();
	if (db == nullptr)
	{
		return -1;
	}

	// Prepare statement.
	sqlite3_stmt* stmt;
	auto res = sqlite3_prepare(db, "SELECT * FROM tbl_version WHERE category='sync_server'", -1, &stmt, NULL);
	if (res != SQLITE_OK)
	{
		Logger::Error("sqlite3_prepare(): Failed because %s\n", sqlite3_errmsg(db));
		sqlite3_close(db);
		return false;
	}

	res = sqlite3_step(stmt);

	if (res == SQLITE_ROW)
	{
		result = sqlite3_column_int(stmt, 1);

		// Release resources.
		sqlite3_finalize(stmt);
		sqlite3_close(db);

		return result;
	}

	if (res != SQLITE_DONE) {
		printf("GetVersion(): Res %d Error: %s\n", res, sqlite3_errmsg(db));
	}

	// Release resources.
	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return result;
}

bool AppDatabase::GetApps(std::vector<AppInfo>& Apps)
{
	int res;

	auto db = OpenDatabase();
	if (db == nullptr)
	{
		return false;
	}

	// Get the current user id.
	int ForegroundAccountId;
	sceUserServiceGetForegroundUser(&ForegroundAccountId);

	// build statement.
	char query[0x200];
	snprintf(query, sizeof(query), "SELECT * FROM tbl_appbrowse_0%i", ForegroundAccountId);

	// Prepare statement.
	sqlite3_stmt* stmt;
	res = sqlite3_prepare(db, query, -1, &stmt, NULL);
	if (res != SQLITE_OK)
	{
		Logger::Error("sqlite3_prepare(): Failed because %s\n", sqlite3_errmsg(db));
		sqlite3_close(db);
		return false;
	}

	// Execute step until all rows are read.
	while ((res = sqlite3_step(stmt)) == SQLITE_ROW)
	{
		AppInfo info;
		strcpy(info.TitleId, GetText(stmt, 0));
		strcpy(info.ContentId, GetText(stmt, 1));
		strcpy(info.TitleName, GetText(stmt, 2));
		strcpy(info.MetaDataPath, GetText(stmt, 3));
		strcpy(info.LastAccessTime, GetText(stmt, 4));
		info.Visible = sqlite3_column_int(stmt, 8);
		info.SortPriority = sqlite3_column_int(stmt, 9);
		info.DispLocation = sqlite3_column_int(stmt, 12);
		info.CanRemove = sqlite3_column_int(stmt, 13) == 1;
		strcpy(info.Category, GetText(stmt, 14));
		info.ContentSize = sqlite3_column_int(stmt, 22);
		strcpy(info.InstallDate, GetText(stmt, 23));
		strcpy(info.UICategory, GetText(stmt, 25));

		Apps.push_back(info);
	}

	if (res != SQLITE_DONE) {
		printf("GetApps(): Res %d Error: %s\n", res, sqlite3_errmsg(db));
	}

	// Release resources.
	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return true;
}

bool AppDatabase::GetAppInfoString(const char* TitleId, char* Out, size_t OutSize, const char* Key)
{
	int res;

	auto db = OpenDatabase();
	if (db == nullptr)
	{
		return false;
	}

	// build statement.
	char query[0x200];
	snprintf(query, sizeof(query), "SELECT * FROM tbl_appinfo WHERE titleId='%s' AND key='%s'", TitleId, Key);

	// Prepare statement.
	sqlite3_stmt* stmt;
	res = sqlite3_prepare(db, query, -1, &stmt, NULL);
	if (res != SQLITE_OK)
	{
		Logger::Error("sqlite3_prepare(): Failed because %s\n", sqlite3_errmsg(db));
		sqlite3_close(db);
		return false;
	}

	res = sqlite3_step(stmt);

	if (res == SQLITE_ROW)
	{
		strlcpy(Out, GetText(stmt, 2), OutSize);

		// Release resources.
		sqlite3_finalize(stmt);
		sqlite3_close(db);

		return true;
	}

	if (res != SQLITE_DONE) {
		printf("GetAppInfoString(): Res %d Error: %s\n", res, sqlite3_errmsg(db));
	}

	// Release resources.
	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return false;
}

bool AppDatabase::SetVisibility(const char* TitleId, VisibilityType Visibility)
{
	int res;

	auto db = OpenDatabase();
	if (db == nullptr)
	{
		return false;
	}

	// Get the current user id.
	int ForegroundAccountId;
	sceUserServiceGetForegroundUser(&ForegroundAccountId);

	// build statement.
	char query[0x200];
	snprintf(query, sizeof(query), "UPDATE tbl_appbrowse_0%i SET visible=%i WHERE titleId='%s'", ForegroundAccountId, Visibility, TitleId);

	// Execute Statement.
	res = sqlite3_exec(db, query, nullptr, nullptr, nullptr);
	if (res != SQLITE_OK)
	{
		Logger::Error("sqlite3_exec(): Failed because %s\n", sqlite3_errmsg(db));
		sqlite3_close(db);
		return false;
	}

	// Release resources.
	sqlite3_close(db);

	return true;
}

AppDatabase::VisibilityType AppDatabase::GetVisibility(const char* TitleId)
{
	int res;

	auto db = OpenDatabase();
	if (db == nullptr)
	{
		return VisibilityType::VT_NONE;
	}

	// Get the current user id.
	int ForegroundAccountId;
	sceUserServiceGetForegroundUser(&ForegroundAccountId);

	// build statement.
	char query[0x200];
	snprintf(query, sizeof(query), "SELECT * FROM tbl_appbrowse_0%i WHERE titleId='%s'", ForegroundAccountId, TitleId);

	// Prepare statement.
	sqlite3_stmt* stmt;
	res = sqlite3_prepare(db, query, -1, &stmt, NULL);
	if (res != SQLITE_OK)
	{
		Logger::Error("sqlite3_prepare(): Failed because %s\n", sqlite3_errmsg(db));
		sqlite3_close(db);
		return VisibilityType::VT_NONE;
	}

	res = sqlite3_step(stmt);

	if (res == SQLITE_ROW)
	{
		auto result = (VisibilityType)sqlite3_column_int(stmt, 8);

		// Release resources.
		sqlite3_finalize(stmt);
		sqlite3_close(db);

		return result;
	}

	if (res != SQLITE_DONE) {
		printf("GetAppInfoString(): Res %d Error: %s\n", res, sqlite3_errmsg(db));
	}

	// Release resources.
	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return VisibilityType::VT_NONE;
}

AppDatabase::AppDatabase()
{

}

AppDatabase::~AppDatabase()
{

}