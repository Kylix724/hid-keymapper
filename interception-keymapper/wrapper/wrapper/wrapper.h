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

extern "C"{
	__declspec(dllexport) BSTR get_hardware_id();
	__declspec(dllexport) void start_interception(BSTR hwid[], int key[], unsigned short val[], int length, int delay, int interrupt);
}
#endif