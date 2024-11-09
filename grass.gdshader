shader_type spatial;
render_mode cull_disabled;

//color and color 2 creates grass colour gradients
uniform vec3 color: source_color;
uniform vec3 color2: source_color;
//Sampler2D noise creates random colour gradients in the grass
uniform sampler2D noise;
//noise scale changes noise texture
uniform float noiseScale = 20.0;
//worldPosition lets us project noiset texture onto our grass
varying vec3 worldPosition;

void vertex() {
	// Make worldPosition equal to local space
	worldPosition = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
}

void fragment() {
	// 3D noiselevel used for color mixing
	vec3 noiseLevel = texture(noise, worldPosition.xz / noiseScale).rgb;
	//blends color and color2 along the UV.y coordinate for vertical gradient
	//Use fastnoiselite to add texture to color
	ALBEDO = mix(color,color2,UV.y) * mix(color,color2,noiseLevel.r);
	//Inverts Normal vector for lighting to display properly
	if(!FRONT_FACING){
		NORMAL = -NORMAL;
	}
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
