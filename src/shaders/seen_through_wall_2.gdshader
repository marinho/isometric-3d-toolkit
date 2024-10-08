/*
	アウトラインシルエット Pass #2 シェーダー by あるる（きのもと 結衣）
	Outline Silhouette Pass #2 Shader by Yui Kinomoto @arlez80

	Source: https://godotshaders.com/shader/outline-silhouette-shader/

	MIT License
*/

/**
 * 1. Create a Shader Material with this shader
 * 2. set the material as Next Pass of the material with seen_through_wall_1
 * 3. set Next Pass to 126
 */

shader_type spatial;
render_mode unshaded, cull_disabled;

uniform vec4 color : source_color = vec4( 0.0, 0.0, 0.0, 1.0 );
uniform float bias = -1.0;
uniform float thickness = 0.02;
uniform sampler2D DEPTH_TEXTURE : hint_depth_texture, filter_linear_mipmap;

varying mat4 camera_matrix;

void vertex( )
{
	VERTEX += NORMAL * thickness;
	camera_matrix = INV_VIEW_MATRIX;
}

void fragment( )
{
	vec4 screen_pixel_vertex = vec4( vec3( SCREEN_UV, textureLod( DEPTH_TEXTURE, SCREEN_UV, 0.0 ).x ) * 2.0 - 1.0, 1.0 );
	vec4 screen_pixel_coord = INV_PROJECTION_MATRIX * screen_pixel_vertex;
	screen_pixel_coord.xyz /= screen_pixel_coord.w;
	float depth = -screen_pixel_coord.z;

	float z = -VERTEX.z;

	ALBEDO = color.rgb;
	ALPHA = color.a * float( depth < z + bias );
	DEPTH = 0.00001;
}