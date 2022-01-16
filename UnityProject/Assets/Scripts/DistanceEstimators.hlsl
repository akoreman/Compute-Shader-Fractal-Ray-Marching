#ifndef DE_DEFINED
#define DE_DEFINED

// Distance Estimators for three primitives.

float ApplyDESphere(float3 Position, float r, float3 c)
{
	float3 distanceVector = Position - c;
	return max(0, (length(distanceVector) - r));
}

float ApplyDETetra(float3 Position, float d, float3 c)
{
	Position = Position - c;

	float md1 = max(-Position.x - Position.y - Position.z, Position.x + Position.y - Position.z);
	float md2 = max(-Position.x + Position.y + Position.z, Position.x - Position.y + Position.z);
	float md = max(md1, md2);

	return (md - d) / sqrt(3);
}

float ApplyDEBox(float3 Position, float3 s, float3 c)
{
	float3 a = abs(Position - c) - s;
	float md = min(max(max(a.x, a.y), a.z), 0.0);

	a.x = max(a.x, 0.0);
	a.y = max(a.y, 0.0);
	a.z = max(a.z, 0.0);

	return md + length(a);
}

#endif