#ifndef UTIL_DEFINED
#define UTIL_DEFINED

float ClipToRange(float v, float r)
{
	if (v > r)
	{
		return r;
	}

	if (v < -r)
	{
		return -r;
	}

	return v;
}


#endif