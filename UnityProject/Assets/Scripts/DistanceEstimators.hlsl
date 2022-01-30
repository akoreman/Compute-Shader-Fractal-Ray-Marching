#ifndef DE_DEFINED
#define DE_DEFINED

// Distance Estimators for three primitives.

float ApplyDESphere(float3 position, float r, float3 c)
{
	float3 distanceVector = position - c;
	return max(0, (length(distanceVector) - r));
}

float ApplyDETetra(float3 position, float d, float3 c)
{
	position = position - c;

	float partialOne = max(-position.x - position.y - position.z, position.x + position.y - position.z);
	float partialTwo = max(-position.x + position.y + position.z, position.x - position.y + position.z);
	float joint = max(partialOne, partialTwo);

	return (joint - d) / sqrt(3);
}

float ApplyDEBox(float3 position, float3 s, float3 c)
{
	float3 a = abs(position - c) - s;
	float joint = min(max(max(a.x, a.y), a.z), 0.0);

	a.x = max(a.x, 0.0);
	a.y = max(a.y, 0.0);
	a.z = max(a.z, 0.0);

	return joint + length(a);
}

#endif