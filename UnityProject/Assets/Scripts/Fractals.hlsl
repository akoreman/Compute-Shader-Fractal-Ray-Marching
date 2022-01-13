#ifndef FRACTALS_DEFINED
#define FRACTALS_DEFINED


///////////// FUNCTIONAL FRACTALS //////////////////////

float infiniteShapes(float3 Position)
{
	Position = ApplyAbsX(Position);
	Position = ApplyModX(Position, 2);
	Position = ApplyAbsY(Position);
	Position = ApplyModY(Position, 2);
	Position = ApplyAbsZ(Position);
	Position = ApplyModZ(Position, 2);

	//return ApplyDETetra(Position, 0.5, float3(1, 1, 1));
	return ApplyDESphere(Position, 0.5, float3(1, 1, 1));
	//return ApplyDEBox(Position, float3(.5, .5, .5), float3(1, 1, 1));
}

float sierpinskiTetrahedron(float3 Position)
{
	//// SCALE HAS TO BE 2 ////////////
	float Scale = 2;
	//////////////////////////////////

	int n = 0;

	while (n < 11) {
		Position = ApplySierpinskiFold(Position);
		Position = ApplyScaleTranslate(Position, Scale, float3(-1, -1, -1));
		n++;
	}

	//float Distance = ApplyDESphere(Position, 1.0, float3(0, 0, 0));
	float Distance = ApplyDETetra(Position, 1.0, float3(0, 0, 0));
	//float Distance = ApplyDEBox(Position, float3(1,1,1), float3(1,1,1));

	return Distance * pow(Scale, -float(n));
}

float mengerBox(float3 Position)
{
	//// SCALE HAS TO BE 3 ////////////
	float Scale = 3;
	//////////////////////////////////

	int n = 0;

	while (n < 8) {
		n++;
		Position = ApplyAbsX(Position);
		Position = ApplyAbsY(Position);
		Position = ApplyAbsZ(Position);

		Position = ApplyMengerFold(Position);

		Position = ApplyScaleTranslate(Position, 3, float3(-2, -2, 0));

		Position = ApplyNFold(Position, float3(0, 0, -1), -1);
	}

	float Distance = ApplyDEBox(Position, float3(2, 2, 2), float3(0, 0, 0));

	return Distance * pow(Scale, -float(n));
}



///////////////////////////////////////////////////////////////////////////////


///////////////// WORK IN PROGRESS /////////////////////////////////////////////

float butterweedHills(float3 Position)
{
	int n = 0;

	float Scale = 1.5;

	while (n < 30)
	{
		n++;
		Position = ApplyAbsX(Position);
		Position = ApplyAbsY(Position);
		Position = ApplyAbsZ(Position);

		Position = ApplyScaleTranslate(Position, Scale, float3(-1, -0.5, -0.2));

		Position = ApplyRotX(Position, 3.61);
		Position = ApplyRotY(Position, 2.03);
	}

	return ApplyDESphere(Position, 1, float3(0, 0, 0));// *pow(Scale, -float(n));
}

float mandelBox(float3 Position)
{
	int n = 0;
	float Scale = 1.5;

	while (n < 3) {
		n++;
		Position = ApplyBoxFold(Position, 1);
		Position = ApplySphereFold(Position, .5, 1);
	}

	//return ApplyDESphere(Position, 1, float3(1, 1, 1)) * pow(Scale, -float(n));
	return ApplyDEBox(Position, float3(6, 6, 6), float3(1, 1, 1)) * pow(Scale, -float(n));

}

float treePlanet(float3 Position)
{
	int n = 0;
	int Iterations = 3;
	float Scale = 3.0;

	while (n < 1) {
		n++;

		Position = ApplyRotY(Position,0.44);

		Position = ApplyAbsX(Position);
		Position = ApplyAbsY(Position);
		Position = ApplyAbsZ(Position);

		Position = ApplyMengerFold(Position);

		Position = ApplyScaleTranslate(Position, 1.3,float3(-2,-4.8,0));

		Position = ApplyNFold(Position, float3(0, 0, -1), 0);
	}

	return ApplyDEBox(Position, float3(4.8, 4.8, 4.8), float3(0, 0, 0)) * pow(Scale, -float(n));

}
#endif