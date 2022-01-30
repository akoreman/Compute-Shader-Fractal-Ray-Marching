#ifndef FRACTALS_DEFINED
#define FRACTALS_DEFINED

#define PI 3.14159265358979

// Combine the space folds with the distance estimators to create fractal recipes.

///////////// FUNCTIONAL FRACTALS //////////////////////

float InfiniteShapes(float3 position)
{
	position = ApplyAbsX(position);
	position = ApplyModX(position, 2);
	position = ApplyAbsY(position);
	position = ApplyModY(position, 2);
	position = ApplyAbsZ(position);
	position = ApplyModZ(position, 2);

	//return ApplyDETetra(position, 0.5, float3(1, 1, 1));
	return ApplyDESphere(position, 0.5, float3(1, 1, 1));
	//return ApplyDEBox(position, float3(.5, .5, .5), float3(1, 1, 1));
}

float SierpinskiTetrahedron(float3 position)
{
	//// SCALE HAS TO BE 2 ////////////
	float scale = 2;
	//////////////////////////////////

	int n = 0;

	while (n < 9) 
	{
		position = ApplySierpinskiFold(position);
		position = ApplyScaleTranslate(position, scale, float3(-1, -1, -1));
		n++;
	}

	//float Distance = ApplyDESphere(position, 1.0, float3(0, 0, 0));
	float Distance = ApplyDETetra(position, 1.0, float3(0, 0, 0));
	//float Distance = ApplyDEBox(position, float3(1,1,1), float3(1,1,1));

	return Distance * pow(scale, -float(n));
}

float MengerBox(float3 position)
{
	//// SCALE HAS TO BE 3 ////////////
	float scale = 3;
	//////////////////////////////////

	int n = 0;

	while (n < 8) 
	{
		n++;

		position = ApplyAbsX(position);
		position = ApplyAbsY(position);
		position = ApplyAbsZ(position);

		position = ApplyMengerFold(position);

		position = ApplyScaleTranslate(position, scale, float3(-2, -2, 0));

		position = ApplyNFold(position, float3(0, 0, -1), -1);
	}

	float Distance = ApplyDEBox(position, float3(2, 2, 2), float3(0, 0, 0));
	//float Distance = ApplyDESphere(position, 2.0, float3(0, 0, 0));
	//float Distance = ApplyDETetra(position, 2.0, float3(0, 0, 0));

	return Distance * pow(scale, -float(n));
}



///////////////////////////////////////////////////////////////////////////////


///////////////// WORK IN PROGRESS /////////////////////////////////////////////

// Fractal recipe from https://github.com/HackerPoet/PySpace .
float TreePlanet(float3 position)
{
	int n = 0;

	float scale = 1.3;

	while (n < 40) 
	{
		n++;

		position = ApplyRotY(position,0.44);

		position = ApplyAbsX(position);
		position = ApplyAbsY(position);
		position = ApplyAbsZ(position);

		position = ApplyMengerFold(position);
		
		position = ApplyScaleTranslate(position, scale, float3(-2, -4.8, 0));

		position = ApplyNFold(position, float3(0, 0, -1), 0);
		
	}

	return ApplyDEBox(position, float3(4.8, 4.8, 4.8), float3(0, 0, 0)) * pow(scale, -float(n));
	//return ApplyDEBox(position, float3(1, 1, 1), float3(0, 0, 0)) * pow(scale, -float(n));

}

float SnowStadium(float3 position)
{
	int n = 0;
	float scale = 1.57;

	while (n < 40)
	{
		n++;

		position = ApplyRotY(position, 3.33);
		position = ApplySierpinskiFold(position);
		position = ApplyRotX(position, 0.15);
		position = ApplyMengerFold(position);
		position = ApplyScaleTranslate(position, scale, float3(-6.61f, .4f, -2.32f));
	}

	return ApplyDEBox(position, float3(4.8, 4.8, 4.8), float3(0, 0, 0)) * pow(scale, -float(n));
}




#endif