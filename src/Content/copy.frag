#version 130
uniform vec2 u_resolution;

uniform sampler2D image;

out vec4 fragColor;
void main() 
{
	vec2 uv = gl_FragCoord.xy / u_resolution;
	fragColor = texture(image, uv);
}