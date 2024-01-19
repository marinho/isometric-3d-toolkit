shader_type spatial;
render_mode unshaded, shadows_disabled, cull_disabled, depth_draw_opaque;

/**
 * Important note: the lines about color below rely on using vertex color attribute. If that attribute isn't imported
 * with the mesh, the meshes will be displayed with no color.
 - 
 * In order to solve this, one way is to bake color information in Blender, by doing the following:
 * - in Blender
 *   - keep all meshes in the root of the main collection - preferably merge them into a single Mesh
 *   - in each mesh, open the Inspector "Data" tab, Color Attributes section, add "Color" (keep defaults)
 *   - go to Inspector "Render" tab
 *	   - Render Engine: Cycles
 *     - Device: GPU Compute
 *     - Sampling: Render -> uncheck "Noise Threshold"
 *     - Bake: Bake Type -> Diffuse Color
 *     - Bake: Contributions -> uncheck "Direct" and "Indirect"
 *     - Bake: Output -> Target -> change to "Active Color Attribute"
 *	   - Click on "Bake"
 *   - Save and export using your favourite format (I use .blend file directly)
 * - in Godot
 *   - select the model file in the FileSystem panel
 *   - in the Import tab, scroll down to "Blender" section
 *   - check "Color" in "Meshes" section
 *   - (re)import the model
 */

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