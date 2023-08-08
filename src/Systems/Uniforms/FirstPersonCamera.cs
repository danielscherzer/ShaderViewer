using OpenTK.Mathematics;

namespace ShaderViewer.Systems.Uniforms;

public static class FirstPersonCamera
{
	public static void Update(in Vector3 movement, ref Vector3 position, float heading, float tilt)
	{
		Matrix3 CalcRotationMatrix()
		{
			Matrix3 rotX = Matrix3.CreateRotationX(-tilt);
			Matrix3 rotY = Matrix3.CreateRotationY(heading);
			return rotX * rotY;
		}

		Matrix3 rotation = CalcRotationMatrix();
		position += Vector3.TransformRow(movement, rotation);
	}
}