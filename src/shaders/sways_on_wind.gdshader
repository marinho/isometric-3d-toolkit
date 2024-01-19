shader_type spatial;
render_mode unshaded, shadows_disabled, cull_disabled, depth_draw_opaque;

uniform float sway_strength = 0.1;
uniform float sway_height = 0.5;

varying vec4 color;

void vertex() {
	float positionedY = smoothstep(sway_height, 1.0, VERTEX.y);
	float deviation = sway_strength * sin(TIME) * positionedY;
	VERTEX.x += deviation;
	color = COLOR;
}

void fragment() {
	ALBEDO = color.rgb;
//	ALPHA = color.a; // this line was causing an overlapping issue with water behind
}