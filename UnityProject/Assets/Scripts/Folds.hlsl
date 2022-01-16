#ifndef FOLDS_DEFINED
#define FOLDS_DEFINED

// Apply space transformations to the position vectors of rays.

float3 ApplyScaleTranslate(float3 Position, float Scale, float3 Offset)
{
	return Position * Scale + Offset;
}

float3 ApplySierpinskiFold(float3 Position)
{
	if (Position.x + Position.y < 0) Position.xy = -Position.yx; 
	if (Position.x + Position.z < 0) Position.xz = -Position.zx; 
	if (Position.y + Position.z < 0) Position.zy = -Position.yz;	

	return Position;
}

float3 ApplyMengerFold(float3 Position)
{
	//if (Position.x < Position.y) { Position.xy = Position.yx; }
	//if (Position.x < Position.z) { Position.xz = Position.zx; }
	//if (Position.y < Position.z) { Position.zy = Position.yz; }

	
	float a = min(Position.x - Position.y, 0);
	Position.x -= a;
	Position.y += a;

	a = min(Position.x - Position.z, 0);
	Position.x -= a;
	Position.z += a;

	a = min(Position.y - Position.z, 0);
	Position.y -= a;
	Position.z += a;
	

	return Position;
}

float3 ApplyBoxFold(float3 Position, float3 r)
{
	Position.x = ClipToRange(Position.x, r) * 2 - Position.x;
	Position.y = ClipToRange(Position.y, r) * 2 - Position.y;
	Position.z = ClipToRange(Position.z, r) * 2 - Position.z;


	return Position;
}

float3 ApplySphereFold(float3 Position, float minR, float maxR)
{
	float rSquared = dot(Position, Position);
	return Position * max(maxR / max(minR, rSquared), 1.0);
}


float3 ApplyAbsX(float3 Position)
{
	Position.x = abs(Position.x);
	return Position;
}

float3 ApplyAbsY(float3 Position)
{
	Position.y = abs(Position.y);
	return Position;
}

float3 ApplyAbsZ(float3 Position)
{
	Position.z = abs(Position.z);
	return Position;
}

float3 ApplyModX(float3 Position, float m)
{
	Position.x = Position.x % m;
	return Position;
}

float3 ApplyModY(float3 Position, float m)
{
	Position.y = Position.y % m;
	return Position;
}

float3 ApplyModZ(float3 Position, float m)
{
	Position.z = Position.z % m;
	return Position;
}

float3 ApplyRotX(float3 Position, float Angle)
{
	float s = sin(Angle);
	float c = cos(Angle);

	Position.y = c * Position.y + s * Position.z;
	Position.z = c * Position.z - s * Position.y;

	return Position;
}

float3 ApplyRotY(float3 Position, float Angle)
{
	float s = sin(Angle);
	float c = cos(Angle);

	Position.x = c * Position.x - s * Position.z;
	Position.z = c * Position.z + s * Position.x;

	return Position;
}

float3 ApplyRotZ(float3 Position, float Angle)
{
	float s = sin(Angle);
	float c = cos(Angle);

	Position.x = c * Position.x + s * Position.y;
	Position.y = c * Position.y - s * Position.x;

	return Position;
}

float3 ApplyNFold(float3 Position, float3 n, float d)
{
	Position -= 2.0 * min(0.0, dot(Position, n) - d) * n;

	return Position;
}

#endif