#pragma once

#ifndef _WRAPPER_H_
	#define _WRAPPER_H_

#include <stdlib.h>
#include <memory.h>
#include <tchar.h>
#include <string>
#include "library/interception.h"
#include <iostream>
#include <map>
#include <vector>
#include <fstream>
#include <comutil.h>
#include <assert.h>

using namespace std;

//void add_pair(char* hwid, int key, unsigned short val);

struct Cut{
	char* hwid;
	int key;
	unsigned short* val;
};

extern "C"{
	//__declspec(dllexport) void add_pair(char* hwid, int key, unsigned short val[]);
	//__declspec(dllexport) void add_device(char* hwid);
	//__declspec(dllexport) void remove_pair(char* hwid, int key);
	//__declspec(dllexport) void remove_device(char* hwid);
	__declspec(dllexport) BSTR get_hardware_id();
	__declspec(dllexport) void start_interception(BSTR hwid[], int key[], unsigned short val[], int length);
	//__declspec(dllexport) void test_cut(Cut c);
}
#endif