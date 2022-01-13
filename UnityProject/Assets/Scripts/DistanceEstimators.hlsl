#ifndef DE_DEFINED
#define DE_DEFINED

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
	float md = min(max(max(Position.x, Position.y), Position.z),0);

	return md + length(max(Position, 0));
}
#endif