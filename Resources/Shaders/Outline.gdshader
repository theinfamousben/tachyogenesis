shader_type canvas_item;

uniform bool enabled = true;
uniform vec4 outline_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);
uniform float outline_width : hint_range(0.0, 10.0) = 1.0;

void fragment() {
	vec4 col = texture(TEXTURE, UV);
	
	if (enabled) {
		vec2 size = TEXTURE_PIXEL_SIZE * outline_width;
		
		float alpha = col.a;
		alpha += texture(TEXTURE, UV + vec2(size.x, 0.0)).a;
		alpha += texture(TEXTURE, UV + vec2(-size.x, 0.0)).a;
		alpha += texture(TEXTURE, UV + vec2(0.0, size.y)).a;
		alpha += texture(TEXTURE, UV + vec2(0.0, -size.y)).a;
		
		if (col.a < 1.0 && alpha > 0.0) {
			COLOR = outline_color;
		} else {
			COLOR = col;
		}
	} else {
		COLOR = col;
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
