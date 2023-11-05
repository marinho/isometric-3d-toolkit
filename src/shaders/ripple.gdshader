shader_type spatial;

#include "shapes.gdshaderinc"
#include "trigonometry.gdshaderinc"

uniform float speed = .3;
uniform float radius = .45;
uniform float stroke = 0.05;
uniform vec3 color: source_color = vec3(1., 1., 0.);
uniform bool inwards = false;
uniform float alphaExtrapolation = 1.;

float drawSmoothLineCircle(vec2 p, float d, float lineWidth, float minimum, vec2 uv)
{
    float dist = distance(p, uv);
    float half_stroke = lineWidth * .5;
    float d1 = d - half_stroke;
    float d2 = d + half_stroke;
    float diff = abs(dist - d);
    return (dist >= d1 && dist <= d2) ? smoothstep(1., minimum, diff / half_stroke) : .0;
}

float pulseCircle(vec2 p, float timeSpeed, vec2 uv) {
    float pulse = fract(timeSpeed);
    return drawSmoothLineCircle(p, radius * pulse, stroke, .0, uv) * (1.-pulse);
}

void fragment() {
    ALBEDO = color;
    ALPHA = .0;
    
	float speedDirection = speed * (inwards ? -1. : 1.);
    ALPHA = pulseCircle(CENTER, (TIME - 1.2) * speedDirection, UV)
          + pulseCircle(CENTER, (TIME - 0.4) * speedDirection, UV) 
          + pulseCircle(CENTER, (TIME + 0.4) * speedDirection, UV) 
          + pulseCircle(CENTER, (TIME + 1.2) * speedDirection, UV);
		
	ALPHA *= alphaExtrapolation;
}
