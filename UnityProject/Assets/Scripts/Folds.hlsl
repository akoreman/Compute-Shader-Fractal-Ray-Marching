#ifndef FOLDS_DEFINED
#define FOLDS_DEFINED

// Apply space transformations to the position vectors of rays.

float3 ApplyScaleTranslate(float3 position, float scale, float3 offset)
{
	return position * scale + offset;
}

float3 ApplySierpinskiFold(float3 position)
{
	if (position.x + position.y < 0) position.xy = -position.yx;
	if (position.x + position.z < 0) position.xz = -position.zx;
	if (position.y + position.z < 0) position.zy = -position.yz;

	return position;
}

float3 ApplyMengerFold(float3 position)
{
	//if (position.x < position.y) { position.xy = position.yx; }
	//if (position.x < position.z) { position.xz = position.zx; }
	//if (position.y < position.z) { position.zy = position.yz; }

	
	float a = min(position.x - position.y, 0);
	position.x -= a;
	position.y += a;

	a = min(position.x - position.z, 0);
	position.x -= a;
	position.z += a;

	a = min(position.y - position.z, 0);
	position.y -= a;
	position.z += a;
	

	return position;
}

float3 ApplyBoxFold(float3 position, float3 r)
{
	position.x = ClipToRange(position.x, r) * 2 - position.x;
	position.y = ClipToRange(position.y, r) * 2 - position.y;
	position.z = ClipToRange(position.z, r) * 2 - position.z;


	return position;
}

float3 ApplySphereFold(float3 position, float minr, float maxr)
{
	float rSquared = dot(position, position);
	return position * max(maxr / max(minr, rSquared), 1.0);
}


float3 ApplyAbsX(float3 position)
{
	position.x = abs(position.x);
	return position;
}

float3 ApplyAbsY(float3 position)
{
	position.y = abs(position.y);
	return position;
}

float3 ApplyAbsZ(float3 position)
{
	position.z = abs(position.z);
	return position;
}

float3 ApplyModX(float3 position, float m)
{
	position.x = position.x % m;
	return position;
}

float3 ApplyModY(float3 position, float m)
{
	position.y = position.y % m;
	return position;
}

float3 ApplyModZ(float3 position, float m)
{
	position.z = position.z % m;
	return position;
}

float3 ApplyRotX(float3 position, float angle)
{
	float s = sin(angle);
	float c = cos(angle);

	position.y = c * position.y + s * position.z;
	position.z = c * position.z - s * position.y;

	return position;
}

float3 ApplyRotY(float3 position, float angle)
{
	float s = sin(angle);
	float c = cos(angle);

	position.x = c * position.x - s * position.z;
	position.z = c * position.z + s * position.x;

	return position;
}

float3 ApplyRotZ(float3 position, float angle)
{
	float s = sin(angle);
	float c = cos(angle);

	position.x = c * position.x + s * position.y;
	position.y = c * position.y - s * position.x;

	return position;
}

float3 ApplyNFold(float3 position, float3 n, float d)
{
	position -= 2.0 * min(0.0, dot(position, n) - d) * n;

	return position;
}

#endif