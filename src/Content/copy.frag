#version 130
uniform vec2 u_resolution;

uniform sampler2D image;

void main() 
{
	vec2 uv = gl_FragCoord.xy / u_resolution;
	gl_FragColor = texture(image, uv);
}