// wrapper.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "wrapper.h"

//void add_pair(char* hwid, int key, unsigned short val) {
//	if (find(known_devices.begin(), known_devices.end(), hwid) != known_devices.end())
//		shortcut_map[hwid][key].push_back(val);
//}
//
//__declspec(dllexport) void add_pair(char* hwid, int key, unsigned short val[]) {
//	if (find(known_devices.begin(), known_devices.end(), hwid) != known_devices.end()) {
//		for (int i = 0; i < sizeof(val); i++)
//			shortcut_map[hwid][key].push_back(val[i]);
//	}
//}
//
//__declspec(dllexport) void add_device(char* hwid) {
//	if (find(known_devices.begin(), known_devices.end(), hwid) == known_devices.end())
//		known_devices.push_back(hwid);
//}
//
//__declspec(dllexport) void remove_pair(char* hwid, int key) {
//	if (find(known_devices.begin(), known_devices.end(), hwid) != known_devices.end()) {
//		shortcut_map[hwid].erase(key);
//	}
//}
//
//__declspec(dllexport) void remove_device(char* hwid) {
//	shortcut_map.erase(hwid);
//	auto it = find(known_devices.begin(), known_devices.end(), hwid);
//	if (it != known_devices.end())
//		known_devices.erase(it);
//}
string ConvertWCSToMBS(const wchar_t* pstr, long wslen)
{
	int len = WideCharToMultiByte(CP_ACP, 0, pstr, wslen, NULL, 0, NULL, NULL);

	string dblstr(len, '\0');
	len = WideCharToMultiByte(CP_ACP, 0 /* no flags */,
		pstr, wslen /* not necessary NULL-terminated */,
		&dblstr[0], len,
		NULL, NULL /* no default char */);

	return dblstr;
}

string ConvertBSTRToMBS(BSTR bstr)
{
	int wslen = SysStringLen(bstr);
	return ConvertWCSToMBS((wchar_t*)bstr, wslen);
}




__declspec(dllexport) BSTR get_hardware_id() {
	InterceptionContext context;
	InterceptionDevice device;
	InterceptionKeyStroke stroke;

	SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);

	context = interception_create_context();

	interception_set_filter(context, interception_is_keyboard, INTERCEPTION_FILTER_KEY_UP);

	interception_receive(context, device = interception_wait(context), (InterceptionStroke*)&stroke, 1);

	if (stroke.code == 1)
		return NULL;
	wchar_t temp[500];
	size_t length = interception_get_hardware_id(context, device, temp, sizeof(temp));

	//interception_send(context, device, (InterceptionStroke*)&stroke, 1);
	interception_destroy_context(context);

	if (length > 0 && length < sizeof(temp)) {
		BSTR bstr = SysAllocString(temp);
		return bstr;

	}
	return NULL;
}

__declspec(dllexport) void start_interception(BSTR hwid[], int key[], unsigned short val[], int length) {
	InterceptionContext context;
	InterceptionDevice device;
	InterceptionKeyStroke stroke;

	bool found = false;
	
	map<wstring, map<int, vector<unsigned short>>> shortcut_map;
	int offset = 0;
	for(int i = 0; i < length; i++) {
		assert(bs != nullptr);
		wstring ws(hwid[i], SysStringLen(hwid[i]));
		printf("%ws %d\n", ws.c_str(), key[i]);
		for(int j = offset; j < offset+16; j++){
			if(val[j] != 0)
				shortcut_map[ws][(key[i])].push_back(val[j]);
		}
		offset += 16;
	}

	SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);

	context = interception_create_context();

	interception_set_filter(context, interception_is_keyboard, INTERCEPTION_FILTER_KEY_ALL);

	while (interception_receive(context, device = interception_wait(context), (InterceptionStroke *)&stroke, 1) > 0)
	{
		if (stroke.code == 1) break;
		found = false;
		wchar_t hardware_id[500];
		size_t length = interception_get_hardware_id(context, device, hardware_id, sizeof(hardware_id));
		printf("%d \n %ws \n", length, hardware_id);
		map<wstring, map<int, vector<unsigned short>>>::iterator mapIter = shortcut_map.find(wstring(hardware_id));
		if (length > 0 && length < sizeof(hardware_id) && mapIter != shortcut_map.end()) {
			map<int, vector<unsigned short>>::iterator it = mapIter->second.begin();
			while (it != mapIter->second.end()) {
				if (stroke.code == it->first) {
					found = true;
					vector<unsigned short>::iterator it2 = it->second.begin();
					while (it2 != it->second.end()) {
						if (*it2 > 1000) {
							stroke.code = *it2 - 1000;
							if (stroke.state < 2) {
								stroke.state += 2;
							}
						}
						else {
							stroke.code = *it2;
							if (stroke.state > 1) {
								stroke.state -= 2;
							}
						}
						printf("shortcut: %d %d %d\n", stroke.code, stroke.state, stroke.information);
						interception_send(context, device, (InterceptionStroke *)&stroke, 1);
						Sleep(20);

						it2++;
					}
					break;
				}
				it++;
			}
			

			if (!found) {
				printf("keypress: %d %d %d\n", stroke.code, stroke.state, stroke.information);
				interception_send(context, device, (InterceptionStroke *)&stroke, 1);
			}
		}
		else { 
			printf("rejected keypress: device not in list of linked devices: %ws %d, %d, %d\n", hardware_id, stroke.code, stroke.state, stroke.information);
			interception_send(context, device, (InterceptionStroke *)&stroke, 1);
		}
	}

	interception_destroy_context(context);
}


